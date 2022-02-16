using System.Collections;
using System.Collections.Generic;
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
                // Debug.Log("hoooi");
             //   rc.RemoveBrake();
             //   rc.AddForce(10f);
                   rc.AddBrake(rc.speed);
            }

        }
    }
}