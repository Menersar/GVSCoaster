using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dreamteck.Splines.Examples
{
    public class boostCart : MonoBehaviour
    {
        public RollerCoaster rc;


    private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Cart")
            {
               // Debug.Log("hoooi");
                rc.RemoveBrake();
                rc.AddForce(20f);
             //   rc.AddBrake(10f);
            }

        }
    }
}