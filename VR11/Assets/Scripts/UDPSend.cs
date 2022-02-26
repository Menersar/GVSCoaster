//Inspired by this thread: https://forum.unity.com/threads/simple-udp-implementation-send-read-via-mono-c.15900/
//thanks OP la1n
//thanks MattijsKneppers for letting me know that I also need to lock my queue while enqueuing
//adapted during projects according to my needs

using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend
{
    public string IP { get; private set; }
    public int sourcePort { get; private set; } //sometimes we need to define the source port, since some devices only accept messages coming from a predefined sourceport.
    public int remotePort { get; private set; }

    IPEndPoint remoteEndPoint;

    Thread receiveThread;

    // udpclient object
    public UdpClient client;

    // public
    // public string IP = "127.0.0.1"; default local
    public int port = 10000; // define > init

    // infos
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!

    public bool newdatahereboys = false;

    public void init(string IPAdress, int RemotePort, int SourcePort = -1) //if sourceport is not set, its being chosen randomly by the system
    {
        IP = IPAdress;
        sourcePort = SourcePort;
        remotePort = RemotePort;

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), remotePort);
        if (sourcePort <= -1)
        {
            client = new UdpClient();
            Debug.Log("Sending to " + IP + " : " + remotePort);
        }
        else
        {
            client = new UdpClient(sourcePort);
            Debug.Log("Sending to " + IP + " : " + remotePort + " from Source Port: " + sourcePort);
        }

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    private void ReceiveData()
    {


        //client = sender.client;
        while (true)
        {

            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);



                // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                string text = Encoding.UTF8.GetString(data);

                // Den abgerufenen Text anzeigen.

                Debug.Log(text);
                //!
                //   if (lastReceivedUDPPacket != text)
                // {
                //!
                newdatahereboys = true;
                //!
                //} else
                //{
                //   newdatahereboys = false;
                //}
                //!

                //PlayerPrefs.SetString("ReceivedData",text);

                // latest UDPpacket
                lastReceivedUDPPacket = text;

                // ....
                allReceivedUDPPackets = allReceivedUDPPackets + text;

            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
    }

    // sendData in different ways. Can be extended accordingly
    public void sendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);

        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public void sendInt32(Int32 myInt)
    {
        try
        {
            byte[] data = BitConverter.GetBytes(myInt);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }
    public void sendInt32Array(Int32[] myInts)
    {
        try
        {
            byte[] data = new byte[myInts.Length * sizeof(Int32)];
            Buffer.BlockCopy(myInts, 0, data, 0, data.Length);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }
    public void sendInt16Array(Int16[] myInts)
    {
        try
        {
            byte[] data = new byte[myInts.Length * sizeof(Int16)];
            Buffer.BlockCopy(myInts, 0, data, 0, data.Length);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }


    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }

    public void ClosePorts()
    {

        Debug.Log("closing receiving UDP on port: " + port);

        if (receiveThread != null)
        {
            Debug.Log("aborting receiveThread.");
            receiveThread.Abort();
        }
        if (client != null)
        {
            Debug.Log("closing client.");
            client.Close();
        }

    }

}