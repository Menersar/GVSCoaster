using UnityEngine;
namespace Dreamteck.Splines.Examples
{
    public class stopCart : MonoBehaviour
    {
        public RollerCoaster rc;
        public bool firstRound = false;

        private void OnTriggerEnter(Collider other)
        {
            if (firstRound)
            {
                if (other.tag == "Cart")
                {
                    rc.stopCart();
                    rc.minSpeed = 0f;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!firstRound)
            {
                if (other.tag == "Cart")
                {
                    firstRound = true;
                }

            }

        }

    }
}