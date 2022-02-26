using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class cameraConstraint : MonoBehaviour
{
   // public ConnectionManager cm;
    public CanvasGroup warningMessage;
  //  public bool showWarningMessage = false;

   // public float fadeSpeed = 5f;
    public LoadingOverlay loadingOverlay;
    //  public OVRBoundary boundary;
    // public XRInputSubsystem xrInputSubsystem;

    // public GameObject wallMarker;
    //Check if the boundary is configured
    // bool configured = OVRManager.boundary.GetConfigured();

    private void Start()
    {
        loadingOverlay.FadeOutStart();
        //LoadingOverlay overlay = GameObject.Find("LoadingOverlay").gameObject.GetComponent<LoadingOverlay>();
        //        xrInputSubsystem.TrySetTrackingOriginMode();
        //    xrInputSubsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
        //   xrInputSubsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Unbounded);

        //  bool v = XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
        // boundary.SetVisible(true);
        /*     if (configured)
             {
                 //Grab all the boundary points. Setting BoundaryType to OuterBoundary is necessary
                 Vector3[] boundaryPoints = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.OuterBoundary);

                 //Generate a bunch of tall thin cubes to mark the outline
                 foreach (Vector3 pos in boundaryPoints)
                 {
                     Instantiate(wallMarker, pos, Quaternion.identity);
                 }
             }*/

    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
          //  loadingOverlay.
            //  StartCoroutine(FadeMessage(true));
            loadingOverlay.FadeIn();
          //  loadingOverlay.FadeOut();

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            //  loadingOverlay.
            //  StartCoroutine(FadeMessage(true));
         //   loadingOverlay.FadeOut();
              loadingOverlay.FadeOut();

        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera" && cm.isPlayerOnCart())
        {
            showWarningMessage = true;
            // warningMessage.alpha = Vector3.Distance(Camera.main.transform.position, this.transform.position);
            StartCoroutine(FadeMessage());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera" && cm.isPlayerOnCart())
        {
            showWarningMessage = false;

            //warningMessage.alpha = Vector3.Distance(Camera.main.transform.position, this.transform.position);
            StartCoroutine(FadeMessage());

        }
    }
    */

    /*
    public IEnumerator FadeMessage(bool _showmessage)
    {
        if (_showmessage)
        {
            while (warningMessage.alpha < 1) {
                warningMessage.alpha += fadeSpeed * Time.deltaTime;
                if (warningMessage.alpha > 1)
                {
                    warningMessage.alpha = 1;
                }
                yield return null;
            }
        } else
        {
            while (warningMessage.alpha > 0)
            {
                warningMessage.alpha -= fadeSpeed * Time.deltaTime;
                if (warningMessage.alpha < 0)
                {
                    warningMessage.alpha = 0;
                }
                yield return null;
            }
        }

        
    }
    */

}
