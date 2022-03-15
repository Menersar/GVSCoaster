using UnityEngine;
using System.Collections.Generic;

namespace Dreamteck.Splines.Examples
{
    public class RollerCoaster : MonoBehaviour
    {
        [System.Serializable]
        public class CoasterSound
        {
            public float startPercent = 0f;
            public float endPercent = 1f;
            public AudioSource source;
            public float startPitch = 1f;
            public float endPitch = 1f;
        }

        public float speed = 10f;
        public float minSpeed = 1f;
        public float maxSpeed = 20f;
        public float frictionForce = 0.1f;
        public float gravityForce = 1f;
        public float slopeRange = 60f;
        SplineFollower follower;
        public AnimationCurve speedGain;
        public AnimationCurve speedLoss;
        public float brakeSpeed = 0f;
        public float brakeReleaseSpeed = 0f;

        private float brakeTime = 0f;
        private float brakeForce = 0f;
        private float addForce = 0f;

        public CoasterSound[] sounds;
        public AudioSource brakeSound;
        public AudioSource boostSound;
        public AudioSource stopSound;
        public float soundFadeLength = 0.15f;

        public float zRoatation;
        public float lastZRoatation;

        public float speedPercent;

        public ConnectionManager conMan;

        public bool brakeRemoved = false;
        public bool stop = false;

        public GameObject exitButton;

        public bool canManuallyDrive = true;

        // Use this for initialization
        void Start()
        {
            follower = GetComponent<SplineFollower>();
            follower.onEndReached += OnEndReached;
        }

        void OnEndReached(double last)
        {
            //Detect when the wagon has reached the end of the spline
            List<SplineComputer> computers = new List<SplineComputer>();
            List<int> connections = new List<int>();
            List<int> connected = new List<int>();
            follower.spline.GetConnectedComputers(computers, connections, connected, 1.0, follower.direction, true); //Get the avaiable connected computers at the end of the spline
            if (computers.Count == 0) return;
            //Do not select computers that are not connected at the first point so that we don't reverse direction
            for (int i = 0; i < computers.Count; i++)
            {
                if (connected[i] != 0)
                {
                    computers.RemoveAt(i);
                    connections.RemoveAt(i);
                    connected.RemoveAt(i);
                    i--;
                    continue;
                }
            }
            float distance = follower.CalculateLength(0.0, follower.result.percent); //Get the excess distance after looping
            follower.spline = computers[Random.Range(0, computers.Count)]; //Change the spline computer to the new spline
            follower.SetDistance(distance); //Set the excess distance along the new spline
        }

        // Update is called once per frame
        void Update()
        {
            if (conMan.isGvsOnCartEnabled() && conMan.isPlayerOnCart())
            {
                zRoatation = transform.localEulerAngles.z;
                zRoatation = (zRoatation > 180) ? zRoatation - 360 : zRoatation;

               // conMan.cartRotationChanged(-zRoatation * 2);
                conMan.cartRotationChanged(-zRoatation * 1.4f);
            }

            //  if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
            float dot = Vector3.Dot(this.transform.forward, Vector3.down);
            float dotPercent = Mathf.Lerp(-slopeRange / 90f, slopeRange / 90f, (dot + 1f) / 2f);
            speed -= Time.deltaTime * frictionForce * (1f - brakeForce);
            if (stop)
            {
                speed = 0;
            }
            float speedAdd = 0f;
            speedPercent = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
            if (dotPercent > 0f)
            {
                speedAdd = gravityForce * dotPercent * speedGain.Evaluate(speedPercent) * Time.deltaTime;
            }
            else
            {
                speedAdd = gravityForce * dotPercent * speedLoss.Evaluate(1f - speedPercent) * Time.deltaTime;
            }
            speed += speedAdd * (1f - brakeForce);
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            if (stop)
            {
                speed = 0;
            }
            if (addForce > 0f)
            {
                float lastAdd = addForce;
                addForce = Mathf.MoveTowards(addForce, 0f, Time.deltaTime * 30f);
                speed += lastAdd - addForce;
                if (stop)
                {
                    speed = 0;
                }
            }
            follower.followSpeed = speed;
            follower.followSpeed *= (1f - brakeForce);
            if (brakeTime > Time.time) brakeForce = Mathf.MoveTowards(brakeForce, 1f, Time.deltaTime * brakeSpeed);
            else brakeForce = Mathf.MoveTowards(brakeForce, 0f, Time.deltaTime * brakeReleaseSpeed);

            speedPercent = Mathf.Clamp01(speed / maxSpeed) * (1f - brakeForce);
            for (int i = 0; i < sounds.Length; i++)
            {
                if (speedPercent < sounds[i].startPercent - soundFadeLength || speedPercent > sounds[i].endPercent + soundFadeLength)
                {
                    if (sounds[i].source.isPlaying) sounds[i].source.Pause();
                    continue;
                }
                else if (!sounds[i].source.isPlaying) sounds[i].source.UnPause();
                float volume = 1f;
                if (speedPercent < sounds[i].startPercent + soundFadeLength) volume = Mathf.InverseLerp(sounds[i].startPercent, sounds[i].startPercent + soundFadeLength, speedPercent);
                else if (speedPercent > sounds[i].endPercent) volume = Mathf.InverseLerp(sounds[i].endPercent + soundFadeLength, sounds[i].endPercent, speedPercent);
                float pitchPercent = Mathf.InverseLerp(sounds[i].startPercent, sounds[i].endPercent, speedPercent);
                sounds[i].source.volume = volume;
                sounds[i].source.pitch = Mathf.Lerp(sounds[i].startPitch, sounds[i].endPitch, pitchPercent);
            }

        }

        public void AddBrake(float time)
        {
            if (canManuallyDrive)
            {
                brakeTime = Time.time + time;
                if (!stop)
                {
                    brakeSound.Stop();
                    brakeSound.Play();
                }
                else
                {
                    stopSound.Stop();
                    stopSound.Play();
                }
                brakeRemoved = false;
            }
        }

        public void AddBrakeAutomatic(float time)
        {
            //if (canManuallyDrive)
            //{
                brakeTime = Time.time + time;
                if (!stop)
                {
                    brakeSound.Stop();
                    brakeSound.Play();
                }
                else
                {
                    stopSound.Stop();
                    stopSound.Play();
                }
                brakeRemoved = false;
            //}
        }

        public void RemoveBrake()
        {
            if (canManuallyDrive) {
                stop = false;

                brakeTime = 0f;
                brakeRemoved = true;

                exitButton.SetActive(false);
            }
        }

        public void RemoveBrakeAutomatic()
        {
           // if (canManuallyDrive)
           // {
                stop = false;

                brakeTime = 0f;
                brakeRemoved = true;

                exitButton.SetActive(false);
           // }
        }

        public void AddForce(float amount)
        {
            if (canManuallyDrive)
            {
                addForce = amount;
                boostSound.Stop();
                boostSound.Play();
                minSpeed = 5f;
            }
        }

        public void stopCart()
        {
            stop = true;
            stopSound.Stop();
            stopSound.Play();
            speed = 0;
            minSpeed = 0;
            maxSpeed = 40f;
            exitButton.SetActive(true);

            canManuallyDrive = true;
        }
    }
}