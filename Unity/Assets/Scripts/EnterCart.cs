using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCart : MonoBehaviour
{
    public GameObject cart;
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
            conMan.setPlayerOnCart(true);
            Debug.Log("Player on the Cart!");

            other.transform.SetParent(cart.transform);
            other.transform.position = cart.transform.position;
            other.transform.rotation = cart.transform.rotation;
        }
    }
}
