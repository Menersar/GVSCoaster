using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCart : MonoBehaviour
{
    public GameObject eingang;
    public ConnectionManager conMan;
    //  public Roller cart;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



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
}
