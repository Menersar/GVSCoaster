using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.InputSystem.XR;
using UnityEngine.SpatialTracking;

public class EnterCart : MonoBehaviour
{
    public GameObject cart;
    public GameObject otheroni;
    public GameObject exitButton;
    public ConnectionManager conMan;

    public TrackedPoseDriver trackedPoseDriver;

   // public GameState gs;

    //  public Roller cart;
    private void LateUpdate()
    {          //  trackedPoseDriver = FindObjectOfType<TrackedPoseDriver>();

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

            // cart.GetComponent<RollerCoaster>()
            conMan.setPlayerOnCart(true);
            //Debug.Log("Player on the Cart!");
           // otheroni = other.gameObject;
            // other.transform.SetParent(cart.transform);
            otheroni.transform.position = cart.transform.position;
            otheroni.transform.rotation = cart.transform.rotation;

            Camera.main.transform.localPosition = new Vector3(0,0,0);
            Camera.main.transform.localRotation = new Quaternion(0,0,0,1);
            //UnityEngine.XR.InputTracking.disablePositionalTracking = true;
            if (!GameState.Instance.GetMovable())
            {
                trackedPoseDriver.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
            }
           // VRDe
            exitButton.SetActive(true);

            //  other.GetComponent<XR>
        }
    }
}
