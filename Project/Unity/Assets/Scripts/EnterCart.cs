using UnityEngine;
using UnityEngine.SpatialTracking;

public class EnterCart : MonoBehaviour
{
    public GameObject cart;
    public GameObject otheroni;
    public GameObject exitButton;
    public ConnectionManager conMan;

    public TrackedPoseDriver trackedPoseDriver;

    private void LateUpdate()
    {
        if (conMan.isPlayerOnCart()) // && !conMan.playerLeft)
        {
            otheroni.transform.position = cart.transform.position;
            otheroni.transform.rotation = cart.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            conMan.setPlayerOnCart(true);

            otheroni.transform.position = cart.transform.position;
            otheroni.transform.rotation = cart.transform.rotation;

            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
            Camera.main.transform.localRotation = new Quaternion(0, 0, 0, 1);

            if (!GameState.Instance.GetMovable())
            {
                trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
            }

            exitButton.SetActive(true);
        }
    }
}