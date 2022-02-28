<h3 align="center">GVS Coaster</h3>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/github_username/repo_name">
    <img src="logo/logo2_1.png" alt="Logo" width="100" height="100">
  </a>

<h3 align="center">project_title</h3>


</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#respository-structure">Respository Structure</a>
    </li>
    <li>
      <a href="#setup-for-mqtt-connection">Setup for MQTT Connection</a>
    </li>
  </ol>
</details>

## Respository Structure

### Build (Android APK):
- Found under \Build.

### Documentation and Video:
- Found under \Documentation.

### Unity project files:
- Found under \Sources\Unity.

### Arduino (ESP) files:
- Found under \Sources\Arduino.

### Libraries used for the Arduino (ESP) project:
- Found under \Project\Arduino\libraries.
- To use them, copy these into folder C:\Users\USERNAME\Documents\Arduino\libraries.

### Eagle Schematic and Board:
- Found under \Sources\Eagle.


## Setup for MQTT Connection (for the Galvanic Vestibular Stimulation(GVS))

### Required (for connection with local MQTT-Broker): 
- Install Mosquitto MQTT broker on a local PC (found under \Project\_Mosquitto MQTT Broker).
- Replace the "mosquitto.conf" in the mosquitto installation folder with the "mosquitto.conf" file (found under \Project\_Mosquitto MQTT Broker).
- Run the Command Prompt as administrator and type "net start mosquitto" to start the mosquitto MQTT broker locally ("net stop mosquitto" to stop).

### Nice to have for monitoring of the MQTT messages:
- Install MQTT.fx (found under \Project\_MQTT fx).
- Click on the options icon, if installed on the same computer as the MQTT broker, set the Broker Adress to "127.0.0.1" and the Port to "1883".
- Click on Connect.
- Now you are able to Subscribe and Publish to Topics.
- For this project the two topics used are "M2MQTT_Unity/test/ESPXY/outTopic" (messages from the ESP) and "M2MQTT_Unity/test/ESPXY/inTopic" (messages from Unity / the application).