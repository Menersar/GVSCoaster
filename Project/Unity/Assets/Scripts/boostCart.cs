using UnityEngine;
namespace Dreamteck.Splines.Examples
{
    public class boostCart : MonoBehaviour
    {
        public RollerCoaster rc;
        public float forceToAdd = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Cart")
            {
                rc.RemoveBrake();
                rc.AddForce(forceToAdd);
                rc.minSpeed = 5f;
            }
        }
    }
}