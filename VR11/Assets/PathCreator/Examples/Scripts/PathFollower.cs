using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        private float defaultSpeed;
        float distanceTravelled;

        public float lokaleRotaionX;

        public bool changeDirection = false;

        public float minimalSpeed = 2;
        public float maximalSpeed = 50;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;

                defaultSpeed = speed;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                float lokaleRotaionX = transform.localEulerAngles.x;
                lokaleRotaionX = (lokaleRotaionX > 180) ? lokaleRotaionX - 360 : lokaleRotaionX;
                //  if (transform.localEulerAngles.x == 0)
                //  {
                //      speed = defaultSpeed;
                //     lokaleRotaionX = (transform.localEulerAngles.x);
                // } else  {#
                if (!changeDirection) {
                    speed = speed + (lokaleRotaionX) * .5f*Time.deltaTime;
                } else {
                    speed = speed - (lokaleRotaionX) * 1 * Time.deltaTime;

                }
                //lokaleRotaionX = (transform.localEulerAngles.x);
                if (speed <= minimalSpeed)
                {
                    speed = minimalSpeed;
                } else if (speed >= maximalSpeed)
                {
                    speed = maximalSpeed;
                }


                    //  }
                    if (!changeDirection)
                {
                    distanceTravelled += speed * Time.deltaTime;
                }
                else
                {
                    distanceTravelled -= speed * Time.deltaTime;

                }
                //distanceTravelled -= speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}