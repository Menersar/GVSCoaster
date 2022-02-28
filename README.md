# esp-unity-mqtt
 
Required: 
- Install Mosquitto MQTT broker on a local PC (found under \Project\_Mosquitto MQTT Broker).
- Replace the "mosquitto.conf" in the mosquitto installation folder with the "mosquitto.conf" file (found under \Project\_Mosquitto MQTT Broker).
- Run the Command Prompt as administrator and type "net start mosquitto" to start the mosquitto MQTT broker locally ("net stop mosquitto" to stop).

Nice to have for monitoring the MQTT messages:
- Install MQTT.fx (found under \Project\_MQTT fx).
- Click on the options icon, if installed on the same computer as the MQTT broker, set the Broker Adress to "127.0.0.1" and the Port to "1883".
- Click on Connect.
- Now you are able to Subscribe and Publish to Topics.
- For this project the two topics used are "M2MQTT_Unity/test/ESPXY/outTopic" (messages from the ESP) and "M2MQTT_Unity/test/ESPXY/inTopic" (messages from Unity / the application).

Unity project files:
- Found under \Project\Unity.

Arduino (ESP) files:
- Found under \Project\Arduino.

Libraries used in the Arduino (ESP) project:
- Found under \Project\Arduino\libraries.
- To use them, copy these into folder C:\Users\USERNAME\Documents\Arduino\libraries.

Build (Android APK):
- Found under \Build.

Documentation and Video:
- Found under \Documentation.

Eagle Schematic and Board:
- Found under \Project\Eagle.