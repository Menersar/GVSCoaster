using UnityEngine;
using UnityEngine.SpatialTracking;

public class playerCollissionCheck : MonoBehaviour
{
    public ConnectionManager cm;
    public TrackedPoseDriver trackedPoseDriver;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && cm.doNotMovePlayer)
        {
            cm.doNotMovePlayer = false;

            trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
        }
    }
}