using UnityEngine;

public class cameraConstraint : MonoBehaviour
{
    public CanvasGroup warningMessage;

    public LoadingOverlay loadingOverlay;

    private void Start()
    {
        loadingOverlay.FadeOutStart();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            loadingOverlay.FadeIn();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainCamera")
        {
            loadingOverlay.FadeOut();
        }
    }
}