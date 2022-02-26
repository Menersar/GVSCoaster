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
            if (firstRound )// && rc.brakeRemoved)
            {
                if (other.tag == "Cart")
                {
                   // firstRound = true;
                
                
                
                   // rc.stop = true;
                    rc.stopCart();
                    // Debug.Log("hoooi");
                    //   rc.RemoveBrake();
                    //   rc.AddForce(10f);
                   // rc.speed = 0;
                   // rc.minSpeed = 0;
                   // rc.stop = true;
                }
            }

        }

        private void OnTriggerExit (Collider other)
        {
            if (!firstRound)
            {
                if (other.tag == "Cart")// && rc.brakeRemoved)
                   {
                
                    firstRound = true;
                }
               
            }

        }

    }
}