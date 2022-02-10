/*
The MIT License (MIT)

Copyright (c) 2018 Giovanni Paolo Vigano'

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using TMPro;


/// <summary>
/// Examples for the M2MQTT library (https://github.com/eclipse/paho.mqtt.m2mqtt),
/// </summary>
//namespace M2MqttUnity.Examples
//{
    /// <summary>
    /// Script for testing M2MQTT with a Unity UI
    /// </summary>
    public class M2MqttUnityTest : M2MqttUnityClient
    {

        Vector2 lastForce;
        public int powerLevel = 10;
        public int lastPowerLevel;
        private bool debugMessagesEnabled = true;
        
        private int receivedMessagegNumber = 0;
        private int sentMessagegNumber = 0;
        public TMP_Text receivedMessage;
        public TMP_Text sentMessage;
        private bool connectionEstablished = false;

        public ConnectionManager connMan;



        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        public bool autoTest = false;
        [Header("User Interface")]
       /* public InputField consoleInputField;
        public Toggle encryptedToggle;
        public InputField addressInputField;
        public InputField portInputField;
        public Button connectButton;
        public Button disconnectButton;
        public Button testPublishButton;
        public Button clearButton;*/
        public string topic1 = "M2MQTT_Unity/test/ESPXY";
        public string topicOut = "M2MQTT_Unity/test/ESPXY/outTopic";
        public bool setToZero = true;

        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;

        public String paceroni;

    public GameObject slider;
    public GameObject powerLevelSlider;
    public GameObject powerLevelText;

    public GameObject statusIndicator;

    public TMP_InputField brokerAdressInputField;
    public TMP_InputField brokerPortInputField;

    public Button manualConnect;
    public Button manualDisconnect;

    public void TestPublish()
        {
            client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
            AddUiMessage("Test message published.");
        }

        public void SetBrokerAddress(string brokerAddress)
        {
        /*
        
            if (addressInputField && !updateUI)
            {
                this.brokerAddress = brokerAddress;
            }
        */
        }

        public void SetBrokerPort(string brokerPort)
        {
        /*
            if (portInputField && !updateUI)
            {
                int.TryParse(brokerPort, out this.brokerPort);
            }
        */
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        public void SetUiMessage(string msg)
        {/*
            if (consoleInputField != null)
            {
                consoleInputField.text = msg;
                updateUI = true;
            }
        */
        }

        public void AddUiMessage(string msg)
        {
        /*
            if (consoleInputField != null)
            {
                consoleInputField.text += msg + "\n";
                updateUI = true;
            }
        */
        }

    public void connectMQT()
    {


        if (!connectionEstablished)
        {
            this.brokerAddress = brokerAdressInputField.text;
            int.TryParse(brokerPortInputField.text, out this.brokerPort);
            base.Connect();
            sendString("A" + this.brokerAddress + ":" + "P" + this.brokerPort);
        }
    }

    public void disconnectMQT()
    {
        if (connectionEstablished)
        {
            connectionEstablished = false;
            statusIndicator.GetComponent<RawImage>().color = Color.red;
            Disconnect();
            manualConnect.interactable = true;
            manualDisconnect.interactable = false;
        }


     
    }

    protected override void OnConnecting()
        {
            base.OnConnecting();
            SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            manualConnect.interactable = false;
            manualDisconnect.interactable = true;

            base.OnConnected();
            SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            connectionEstablished = true;
            statusIndicator.GetComponent<RawImage>().color = Color.green;

            
            sendString("A" + this.brokerAddress + ":" + "P" + this.brokerPort);





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
            AddUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
        manualConnect.interactable = true;
        manualDisconnect.interactable = false;
        AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            AddUiMessage("CONNECTION LOST!");
        }

        private void UpdateUI()
        {
        /*
            if (client == null)
            {
                if (connectButton != null)
                {
                    connectButton.interactable = true;
                    disconnectButton.interactable = false;
                    testPublishButton.interactable = false;
                }
            }
            else
            {
                if (testPublishButton != null)
                {
                    testPublishButton.interactable = client.IsConnected;
                }
                if (disconnectButton != null)
                {
                    disconnectButton.interactable = client.IsConnected;
                }
                if (connectButton != null)
                {
                    connectButton.interactable = !client.IsConnected;
                }
            }
            if (addressInputField != null && connectButton != null)
            {
                addressInputField.interactable = connectButton.interactable;
                addressInputField.text = brokerAddress;
            }
            if (portInputField != null && connectButton != null)
            {
                portInputField.interactable = connectButton.interactable;
                portInputField.text = brokerPort.ToString();
            }
            if (encryptedToggle != null && connectButton != null)
            {
                encryptedToggle.interactable = connectButton.interactable;
                encryptedToggle.isOn = isEncrypted;
            }
            if (clearButton != null && connectButton != null)
            {
                clearButton.interactable = connectButton.interactable;
            }
            updateUI = false;
        */
        }

        protected override void Start()
        {
            this.brokerAddress = brokerAdressInputField.text;
           // this.brokerPort = int.Parse(brokerPortInputField.text);
            int.TryParse(brokerPortInputField.text, out this.brokerPort);

            SetUiMessage("Ready.");
            updateUI = true;
            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            
            if (topic == this.topicOut)
            {
                string msg = System.Text.Encoding.UTF8.GetString(message);
                Debug.Log("Received: " + msg);
                StoreMessage(msg);

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
                statusIndicator.GetComponent<RawImage>().color = Color.red;
                Disconnect();
                }
            }
        }




        public void analogForceChange(Vector2 inForce, Vector2 inAnalog)
        {
        if (connectionEstablished) {
            if (Mathf.Abs(inForce.x) == 0 && !setToZero)
            {
                sendZeros();
                setToZero = true;
                lastForce = inForce;
            }
            else
            {
                if (Mathf.Abs(inForce.y - lastForce.y) > 0.1 || Mathf.Abs(inForce.x - lastForce.x) > 0.1 || lastPowerLevel != powerLevel)
                {
                    setToZero = false;

                    var pac = "Y" + Mathf.RoundToInt(inForce.y * powerLevel).ToString() + ":X" + Mathf.RoundToInt(inForce.x * powerLevel).ToString();
                    // Debug.Log(pac);
                    paceroni = pac;
                    client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes(pac), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                    lastForce = inForce;
                    lastPowerLevel = powerLevel;
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

    public void setPowerLevel()
    {
        // int sliderValue = Mathf.RoundToInt(powerLevelSlider.GetComponent<Slider>().value);
        powerLevel = Mathf.RoundToInt(powerLevelSlider.GetComponent<Slider>().value);
        powerLevelText.GetComponent<TMP_Text>().text = "Power Level: " + powerLevel;
        Debug.Log("Power Level: " + powerLevel);

        // sliderChanged();

    }

    public void enableDebugMessages(bool enabledMessages)
    {
        debugMessagesEnabled = enabledMessages;
    }
    public void sendString(string stringToSend)
    {
        if (debugMessagesEnabled)
        {
            clearMessages();

            sentMessage.text = sentMessage.text + "D" + "\n";
            sentMessagegNumber++;
        }
        client.Publish(topic1, System.Text.Encoding.UTF8.GetBytes(stringToSend), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        Debug.Log("Sent String: " + stringToSend);

    }



    private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            AddUiMessage("Received--: " + msg);
        }

        protected override void Update()
        {
            
            base.Update(); // call ProcessMqttEvents()


            //  DecodeMessage();

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            if (updateUI)
            {
                UpdateUI();
            }
        }

        private void OnDestroy()
        {
        //statusIndicator.GetComponent<RawImage>().color = Color.red;
        sendZeros();
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
