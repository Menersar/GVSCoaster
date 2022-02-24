using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCart : MonoBehaviour
{
    public GameObject eingang;
    public ConnectionManager conMan;
    public GameObject player;
    public GameObject exitButton;

    //  public Roller cart;
    // Start is called before the first frame update

    public void leaveCart ()
    {
        conMan.leaveCart();
       // conMan.setPlayerOnCart(false);
        // cart.GetComponent<RollerCoaster>()
        //conMan.setPlayerOnCart(false);
        //  Debug.Log("Player left the Cart!");

        //  player.transform.SetParent(null);
       // player.transform.localPosition = eingang.transform.position;
       // player.transform.rotation = eingang.transform.rotation;
       // conMan.playerLeft = true;
       // if(player.transform.position == eingang.transform.position)
       // {
        //    Debug.Log(player.transform.position + "::::::::::::::::::::::" + eingang.transform.position);
             
        //}
     
        
          // exitButton.SetActive(false);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // cart.GetComponent<RollerCoaster>()
            conMan.setPlayerOnCart(false);
            Debug.Log("Player left the Cart!");

            other.transform.SetParent(null);
            other.transform.position = eingang.transform.position;
            other.transform.rotation = eingang.transform.rotation;
        }
    }
    */

}
