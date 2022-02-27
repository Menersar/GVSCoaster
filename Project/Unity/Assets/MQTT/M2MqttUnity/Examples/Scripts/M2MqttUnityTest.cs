/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

Changed by Maite-Aileen Brandt

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt.Messages;
using TMPro;
using System.IO;


/// <summary>
/// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),
/// </summary>

/// <summary>
/// Script for testing M2MQTT with a Unity UI
/// </summary>
/// 
public class M2MqttUnityTest : M2MqttUnityClient
{
    Vector2 lastForce;
    public int powerLevel = 10;
    //  public int lastPowerLevel;
    private bool debugMessagesEnabled = true;

    private int receivedMessagegNumber = 0;
    private int sentMessagegNumber = 0;
    public TMP_Text receivedMessage;
    public TMP_Text sentMessage;
    public bool connectionEstablished = false;

    public ConnectionManager connMan;

    [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
    public bool autoTest = false;
    [Header("User Interface")]

    public string topic1 = "M2MQTT_Unity/test/ESPXY";
    public string topicOut = "M2MQTT_Unity/test/ESPXY/outTopic";
    public bool setToZero = true;

    public String paceroni;

    public GameObject statusIndicator2;

    public TMP_Text brokerAdressText;
    public TMP_Text brokerPortText;
    public int currentMQTTText = 0;

    public GameObject manualConnect;
    public GameObject manualDisconnect;

    public TMP_Text debugMessagesTextIndicator;

    private static float HDRGlowIntensity = 2.416924f;

    private bool MQTTSettingsVisible = false;
    private bool settingsVisible = false;

    public GameObject MQTTSettingsToDisable;
    public GameObject graphicsSettingsToDisable;
    public GameObject soundSettingsToDisable;
    public OptionsManager optionsManager;

    public void toggleMQTTSettings()
    {
        MQTTSettingsVisible = !MQTTSettingsVisible;
        MQTTSettingsToDisable.SetActive(MQTTSettingsVisible);
    }

    public void toggleSettingsButtons()
    {
        settingsVisible = !settingsVisible;
        if (settingsVisible == false)
        {
            if (currentMQTTText == 0)
            {
                brokerAdressText.fontStyle = FontStyles.Normal;
            }
            else
            {
                brokerPortText.fontStyle = FontStyles.Normal;
            }
            optionsManager.settingsEnabled = false;
        }
        MQTTSettingsToDisable.SetActive(settingsVisible);
        graphicsSettingsToDisable.SetActive(settingsVisible);
        soundSettingsToDisable.SetActive(settingsVisible);

        if (settingsVisible == true)
        {
            optionsManager.settingsEnabled = true;
            optionsManager.refreshButtons();

            if (currentMQTTText == 0)
            {
                brokerAdressText.fontStyle = FontStyles.Underline;
            }
            else
            {
                brokerPortText.fontStyle = FontStyles.Underline;
            }
        }
    }

    public void toggleCurrentMQTTText()
    {
        if (currentMQTTText == 0)
        {
            currentMQTTText = 1;
            brokerAdressText.fontStyle = FontStyles.Normal;
            brokerPortText.fontStyle = FontStyles.Underline;
        }
        else
        {
            currentMQTTText = 0;
            brokerAdressText.fontStyle = FontStyles.Underline;
            brokerPortText.fontStyle = FontStyles.Normal;
        }
    }

    public void typeNumberText(string inputText)
    {
        // 0 stands for the address text, 1 for the port text
        if (currentMQTTText == 0)
        {
            // deleting text
            if (inputText != "")
            {
                brokerAdressText.text += inputText;
            }
            else
            {
                brokerAdressText.text = brokerAdressText.text.Remove(brokerAdressText.text.Length - 1);
            }

        }
        else if (currentMQTTText == 1)
        {
            if (inputText != "")
            {
                brokerPortText.text += inputText;
            }
            else
            {
                brokerPortText.text = brokerAdressText.text.Remove(brokerPortText.text.Length - 1);
            }

        }
    }

    public void TestPublish()
    {
        client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        //Debug.Log("Test message published");
    }

    public void startLocalMQTTBroker()
    {
        string path = Path.Combine(Application.streamingAssetsPath) + "MosquittoInstallationFile/mosquitto/mosquitto.exe";

        Application.OpenURL(path); //Open the folder in explorer
    }

    public void installLocalMosquittoMQTTBroker()
    {
        string path = Path.Combine(Application.streamingAssetsPath) + "MosquittoInstallationFile/mosquittoInstallWindows.exe";

        Application.OpenURL(path); //Open the folder in explorer
    }

    public void SetEncrypted(bool isEncrypted)
    {
        this.isEncrypted = isEncrypted;
    }

    public void connectMQT()
    {
        if (!connectionEstablished)
        {
            // get address and port information from the text fields in the scene
            this.brokerAddress = brokerAdressText.text;
            int.TryParse(brokerPortText.text, out this.brokerPort);

            base.Connect();
        }
    }

    public void disconnectMQT()
    {
        if (connectionEstablished)
        {
            sendZeros();

            Disconnect();
        }
    }

    protected override void OnConnecting()
    {
        base.OnConnecting();
    }

    protected override void OnConnected()
    {
        base.OnConnected();

        connectionEstablished = true;
        statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_BaseColor", Color.green);
        statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", Color.green * HDRGlowIntensity);

        optionsManager.ChangeMQTTSettings();

        if (connectionEstablished)
        {
            sendZeros();
        }

        if (autoTest)
        {
            TestPublish();
        }
    }

    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { topic1, topicOut }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { topic1, topicOut });
    }

    protected override void OnConnectionFailed(string errorMessage)
    {
        connectionEstablished = false;
        statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_BaseColor", Color.red);
        statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", Color.red * HDRGlowIntensity);
    }

    protected override void OnDisconnected()
    {
        connectionEstablished = false;
        statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_BaseColor", Color.red);
        statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", Color.red * HDRGlowIntensity);
    }

    protected override void OnConnectionLost()
    {
        connectionEstablished = false;
        statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_BaseColor", Color.red);
        statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", Color.red * HDRGlowIntensity);
    }

    protected override void Start()
    {
        // get address and port information from the SaveManager
        this.brokerAddress = SaveManager.Instance.state.MQTTAdress;
        int.TryParse(SaveManager.Instance.state.MQTTPort, out this.brokerPort);

        base.Start();
        powerLevel = SaveManager.Instance.state.powerLevel;
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        if (topic == this.topicOut)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);

            clearMessages();
            receivedMessage.text = receivedMessage.text + msg + "\n";
            receivedMessagegNumber++;
        }
        if (topic == this.topic1)
        {
            if (autoTest)
            {
                autoTest = false;
                connectionEstablished = false;
                statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_BaseColor", Color.red);
                statusIndicator2.GetComponent<MeshRenderer>().materials[0].SetColor("_EmissionColor", Color.red * HDRGlowIntensity);

                Disconnect();
            }
        }
    }

    public void analogForceChange(Vector2 inForce, Vector2 inAnalog)
    {
        if (connectionEstablished)
        {
            if (Mathf.Abs(inForce.x) == 0 && !setToZero)
            {
                sendZeros();
                setToZero = true;
                lastForce = inForce;
            }
            else
            {
                if (Mathf.Abs(inForce.y - lastForce.y) > 0.1 || Mathf.Abs(inForce.x - lastForce.x) > 0.1)
                {
                    setToZero = false;

                    var pac = "Y" + Mathf.RoundToInt(inForce.y * powerLevel).ToString() + ":X" + Mathf.RoundToInt(inForce.x * powerLevel).ToString();
                    paceroni = pac;
                    client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes(pac), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                    lastForce = inForce;

                    if (debugMessagesEnabled)
                    {
                        clearMessages();
                        sentMessage.text = sentMessage.text + "Sent X: " + Mathf.RoundToInt(inForce.x * powerLevel).ToString() + "\n";
                        sentMessagegNumber++;
                    }
                }
            }
        }
    }

    public void sendZeros()
    {
        string pac = "Y" + 0.ToString() + ":X" + 0.ToString();
        client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes(pac), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        if (debugMessagesEnabled)
        {
            clearMessages();
            sentMessage.text = sentMessage.text + pac + "\n";
            sentMessagegNumber++;
        }
        clearMessages();
        sentMessage.text = sentMessage.text + "Force x and y to ZERO\n";
        sentMessagegNumber++;
    }

    public void clearMessages()
    {
        if (receivedMessagegNumber >= 10)
        {
            receivedMessage.text = "Received:\n";
            receivedMessagegNumber = 0;
        }
        if (sentMessagegNumber >= 10)
        {
            sentMessage.text = "Sent:\n";
            sentMessagegNumber = 0;
        }
    }

    public void onClearButtonPressed()
    {
        receivedMessage.text = "Received:\n";
        receivedMessagegNumber = 0;


        sentMessage.text = "Sent:\n";
        sentMessagegNumber = 0;
    }

    public void increasePowerLevel()
    {
        optionsManager.increasePowerLevel();
        powerLevel = GameState.Instance.GetPowerLevel();
    }

    public void decreasePowerLevel()
    {
        optionsManager.decreasePowerLevel();
        powerLevel = GameState.Instance.GetPowerLevel();
    }

    public void enableDebugMessages(bool enabledMessages)
    {
        debugMessagesEnabled = enabledMessages;
    }

    public void toggleDebugMessages()
    {
        debugMessagesEnabled = !debugMessagesEnabled;
        if (debugMessagesEnabled)
        {
            debugMessagesTextIndicator.text = "ON";
        }
        else
        {
            debugMessagesTextIndicator.text = "OFF";
        }
    }

    public void sendString(string stringToSend)
    {
        if (debugMessagesEnabled)
        {
            clearMessages();

            sentMessage.text = sentMessage.text + stringToSend + "\n";
            sentMessagegNumber++;
        }
        client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes(stringToSend), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
    }

    private void OnDestroy()
    {
       // if (connectionEstablished)
       // {
       //     sendZeros();
       // }
        Disconnect();
        connectionEstablished = false;
    }

    private void OnValidate()
    {
        if (autoTest)
        {
            autoConnect = true;
        }
    }
}