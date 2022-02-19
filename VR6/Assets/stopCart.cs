using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dreamteck.Splines.Examples
{
    public class stopCart : MonoBehaviour
    {
        public RollerCoaster rc;
        public bool firstRound = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Cart")// && rc.brakeRemoved)
            {
                if (!firstRound)
                {
                    firstRound = true;
                }
                else
                {
                    rc.stop = true;
                    // Debug.Log("hoooi");
                    //   rc.RemoveBrake();
                    //   rc.AddForce(10f);
                    rc.speed = 0;
                    rc.minSpeed = 0;
                    rc.stop = true;
                }
            }

        }
    }
}