using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greiferButton : MonoBehaviour
{
    public greiferArm greiferArm;
 


    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("button touched1");

        if (other.name == "RightBaseController" )
        {
          //  Debug.Log("button touched");
            greiferArm.toggleOpenTheArm();
           
        }
            
    }

    
    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("button touched1");

        if (other.name == "RightBaseController")
        {
          //  Debug.Log("button touched false");
            greiferArm.toggleOpenTheArm();

        }

    }
}
