using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterCart : MonoBehaviour
{
    public GameObject cart;
    public GameObject otheroni;
    public ConnectionManager conMan;
    //  public Roller cart;

    private void LateUpdate()
    {
        if (conMan.isPlayerOnCart())
        {
            otheroni.transform.position = cart.transform.position;
            otheroni.transform.rotation = cart.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // cart.GetComponent<RollerCoaster>()
            conMan.setPlayerOnCart(true);
            //Debug.Log("Player on the Cart!");
           // otheroni = other.gameObject;
            // other.transform.SetParent(cart.transform);
            otheroni.transform.position = cart.transform.position;
            otheroni.transform.rotation = cart.transform.rotation;
            

          //  other.GetComponent<XR>
        }
    }
}
