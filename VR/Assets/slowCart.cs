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

                StartCoroutine(BrakeUntilSlow());
               // while(!rc.stop)
                //{
               //     if (rc.speed <= 5)
             //       {
               //         rc.RemoveBrake();
                //        }
               // }
               // rc.RemoveBrake();
            }

        }

        private IEnumerator BrakeUntilSlow ()
        {
            while (!rc.stop)
            {
                Debug.Log("braking");
                if (rc.speedPercent <= .1 && !rc.brakeRemoved)
                {
                    Debug.Log("stop braking");

                    rc.RemoveBrake();
                    rc.minSpeed = 5f;
                    rc.maxSpeed = 5f;
                    rc.speed = 5f;
                }
                yield return null;
            } 
                
        }
    }
}