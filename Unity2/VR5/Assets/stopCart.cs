using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dreamteck.Splines.Examples
{
    public class stopCart : MonoBehaviour
    {
        public RollerCoaster rc;


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Cart" && rc.brakeRemoved)
            {
                // Debug.Log("hoooi");
                //   rc.RemoveBrake();
                //   rc.AddForce(10f);
                rc.speed = 0;
                rc.minSpeed = 0;
            }

        }
    }
}