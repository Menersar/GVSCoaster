using UnityEngine;
using System.Collections;
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

    public bool doNotMovePlayer = false;


    public bool sendRotByMQTT = false;

    public int rotationValue = 0;
    public int maxRot = 0;
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
        }
        else
        {
            cartGVSToggleText.text = "GVS:\nOFF";
        }
    }

    public void setPlayerOnCart(bool isOnCart)
    {
        playerOnCart = isOnCart;
        doNotMovePlayer = true;
    }

    public bool isPlayerOnCart()
    {
        return playerOnCart;
    }

    public bool isGvsOnCartEnabled()
    {
        return cartGvs;
    }

    public void buttonPressedToEnterCart()
    {

    }

    public void onButton3Pressed()
    {
        if (readyToFire)
        {
            readyToFire = false;

            m2MqttUnityTest.sendString("T2");

            StartCoroutine(setTimerTimeout(2));
        }
    }

    public void onButton4Pressed()
    {
        if (readyToFire)
        {
            readyToFire = false;

            m2MqttUnityTest.sendString("T3");

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
            }
            else if (rot < 0)
            {
                rot = -10;
            }
        }

        rotationValue = Mathf.RoundToInt(rot);
        if (rotationValue > maxRot)
        {
            maxRot = rotationValue;
        }
        m2MqttUnityTest.analogForceChange(new Vector2(rotationValue, 0), new Vector2(0, 0));
    }

    public void leaveCart()
    {
        doNotMovePlayer = true;
        setPlayerOnCart(false);

        currentPlayer.transform.position = eingang.position;
        currentPlayer.transform.rotation = eingang.transform.rotation;
    }
}