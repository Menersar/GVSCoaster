using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


using TMPro;

public class ConnectionManager : MonoBehaviour
{
    // Elektrodenanordnung und -wirkung
    // Galvanic%20Vestibular%20Stimulation%20Applied%20to%20Flight%20Training.pdf
    // + nach vorne, - nach hinten (R vorne, L hinten)
    // + nach rechts, - nach links (R rechts, L links)



    public GameObject statusIndicator;
    public Transform eingang;

    public bool readyToFire = true;



    private bool cartGvs = true;
    public bool playerOnCart = false;
   // public bool playerLeft = true;
    public bool doNotMovePlayer = false;


    public bool sendRotByMQTT = false;

    public int rotationValue = 0;
    public int lastRotationValue = 0;

    public M2MqttUnityTest m2MqttUnityTest;

    public GameObject currentPlayer;
    public GameObject cart;

    public TMP_Text cartGVSToggleText;

    public void toggleCartGvs()
    {
        cartGvs = !cartGvs;
        if (cartGvs)
        {
            cartGVSToggleText.text = "GVS:\nON";
        } else
        {
            cartGVSToggleText.text = "GVS:\nOFF";
        }
    }

    public void setPlayerOnCart(bool isOnCart)
    {
        playerOnCart = isOnCart;
       // playerLeft = !isOnCart;
    }
    
    public bool isPlayerOnCart()
    {
       return playerOnCart;
    }

    public bool isGvsOnCartEnabled ()
    {
        return cartGvs;
    }

    public void buttonPressedToEnterCart()
    {
        // cart.GetComponent<RollerCoaster>()
       // setPlayerOnCart(true);
        //Debug.Log("Player on the Cart!");

        //currentPlayer.transform.SetParent(cart.transform);
        //currentPlayer.transform.position = cart.transform.position;
        //currentPlayer.transform.rotation = cart.transform.rotation;
       // cart.GetComponent<spli>
    }
    public void onButton3Pressed()
    {
        if (readyToFire)
        {
            readyToFire = false;


            m2MqttUnityTest.sendString("T2");

            //	$Timer.start()
            StartCoroutine(setTimerTimeout(2));
        }
    }

    public void onButton4Pressed()
    {
        if (readyToFire)
        {
            readyToFire = false;

            m2MqttUnityTest.sendString("T3");

            //	$Timer.start()
            StartCoroutine(setTimerTimeout(3));
        }
    }

    public void onDebugButton2Pressed()
    {

        m2MqttUnityTest.sendString("D");

    }

    IEnumerator setTimerTimeout(int secs)
    {
        yield return new WaitForSeconds(secs);
        onTimerTimeout();
    }

    public void onTimerTimeout()
    {
        readyToFire = true;
    }




    public void cartRotationChanged(float rot)
    {
        rot *= .1f;

        if (Mathf.Abs(rot) > 10)
        {
            if (rot > 0)
            {
                rot = 10;
            } else if (rot < 0)
            {
                rot = -10;
            }
        }

        rotationValue = Mathf.RoundToInt(rot);
        m2MqttUnityTest.analogForceChange(new Vector2(rotationValue, 0), new Vector2(0, 0));


    }


    
 
    public void leaveCart()
    {
        doNotMovePlayer = true;
         setPlayerOnCart(false);
       // playerLeft = true;


        StartCoroutine(waitUntilPlayerOnStart());
        // 
        


    }

    private IEnumerator waitUntilPlayerOnStart()
    {
        currentPlayer.transform.position = eingang.position;
        currentPlayer.transform.rotation = eingang.transform.rotation;

        while (currentPlayer.transform.position != eingang.position) yield return null;
        //Debug.Log("left!");
        doNotMovePlayer = true;

        // playerLeft = true;
    }


}