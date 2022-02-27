using System.Collections;
using UnityEngine;
namespace Dreamteck.Splines.Examples
{
    public class slowCart : MonoBehaviour
    {
        public RollerCoaster rc;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Cart")
            {
                rc.AddBrake(rc.speed);

                StartCoroutine(BrakeUntilSlow());
            }
        }

        private IEnumerator BrakeUntilSlow()
        {
            while (!rc.stop)
            {
                if (rc.speedPercent <= .1 && !rc.brakeRemoved)
                {
                    rc.RemoveBrake();
                    rc.minSpeed = 2f;
                    rc.maxSpeed = 2f;
                    rc.speed = 2f;
                }
                yield return null;
            }
        }
    }
}