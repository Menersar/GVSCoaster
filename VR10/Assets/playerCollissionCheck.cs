using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem.XR;
using UnityEngine.SpatialTracking;



public class playerCollissionCheck : MonoBehaviour
{
  //  public bool playerInside;
    public ConnectionManager cm;
    public TrackedPoseDriver trackedPoseDriver;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && cm.doNotMovePlayer)
        {
            // playerInside = true;
            cm.doNotMovePlayer = false;
           // if (!GameState.Instance.GetMovable())
         //   {
                trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
          //  }
        }
    }

    
}
