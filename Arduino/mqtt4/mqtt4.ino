#include <FS.h>                   //this needs to be first, or it all crashes and burns...




/* 
 *  Mind Control Server - for ESP8266
 *  
 *  
 * License - GPL v3
 * (C) 2021 Copyright - Gene Ruebsamen
 * ruebsamen.gene@gmail.com
 */

#include <ESP8266WiFi.h>
#include <WiFiUdp.h>
#include <arduino-timer.h>
#include <ESP8266WebServer.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <WiFiManager.h>
#include <ArduinoJson.h>




 
//needed for library
#include <DNSServer.h>
#include "WiFiManager.h"          //https://github.com/tzapu/WiFiManager







#define DDRIVE_MIN -100 //The minimum value x or y can be.
#define DDRIVE_MAX 100  //The maximum value x or y can be.
#define MOTOR_MIN_PWM -1023 //The minimum value the motor output can be.
#define MOTOR_MAX_PWM 1023 //The maximum value the motor output can be.

// wired connections
#define L9110_A_IA 0 // D7 --> Motor B Input A --> MOTOR A +
#define L9110_A_IB 2 // D6 --> Motor B Input B --> MOTOR A -
 
// functional connections
#define MOTOR_A_PWM L9110_A_IA // Motor A PWM Speed
#define MOTOR_A_DIR L9110_A_IB // Motor A Direction
 
// the actual values for "fast" and "slow" depend on the motor
#define PWM_SLOW 50  // arbitrary slow speed PWM duty cycle
#define DIR_DELAY 1000 // brief delay for abrupt motor changes

#define MIN_THRESHOLD 50

//const char ssid[]="ssid_here";
//const char pass[]="password_here";
char  replyPacket[] = "Hi there! Got the message :-)";  // a reply string to send back
WiFiUDP udp;
unsigned int localPort = 10000;
const int PACKET_SIZE = 256;
char packetBuffer[PACKET_SIZE];
int status = WL_IDLE_STATUS;
int prev_S=128;
auto timer = timer_create_default();
bool firing = false;


// STA Hausnetzt
//const char* wifi_network_ssid     = "UB";
//const char* wifi_network_password =  "32046899250562104274";

// AP Wlan Netz Sensor
//const char *soft_ap_ssid          = "ESP-Sensor";
//const char *soft_ap_password      = "password";



// WiFi
//const char *ssid = "UB";
//const char *password = "32046899250562104274";

// MQTT Broker
//const char *mqtt_broker = "broker.emqx.io";
//const char *mqtt_broker = "broker.hivemq.com";
//const char *mqtt_broker = "192.168.178.20";
//const char *mqtt_broker = "192.168.1.10";
//IPAddress serveroni(192, 168, 178, 20);

const char *topic = "M2MQTT_Unity/test/ESPXY";
const char *outTopic = "M2MQTT_Unity/test/ESPXY/outTopic";
const char *mqtt_username = "mqttuser";
const char *mqtt_password = "mqttpassword";
//const int mqtt_port = 1883;



//define your default values here, if there are different values in config.json, they are overwritten.
char mqtt_broker[40] = "192.168.178.20";
char mqtt_port[6] = "1883";
//char blynk_token[34] = "YOUR_BLYNK_TOKEN";


WiFiClient espClient;
PubSubClient client(espClient);


ESP8266WebServer server(80);  // Server Instanz

//unsigned int localPort = 4211;  // local port to listen on
//unsigned int slavePort = 4210;

//WiFiUDP Udp;                  // UDP Instanz








//flag for saving data
bool shouldSaveConfig = false;

//callback notifying us of the need to save config
void saveConfigCallback () {
  Serial.println("Should save config");
  shouldSaveConfig = true;
}




void coast() {
  //Serial.println( "Soft stop (coast)..." );
  digitalWrite( MOTOR_A_DIR, LOW );
  digitalWrite( MOTOR_A_PWM, LOW );
}

void brake() {
  Serial.println( "Hard stop (brake)..." );
  digitalWrite( MOTOR_A_DIR, HIGH );
  digitalWrite( MOTOR_A_PWM, HIGH );
}

int LeftMotorOutput;
int RightMotorOutput;

void calcTankDrive(float x, float y)
{
  float rawLeft;
  float rawRight;
  float RawLeft;
  float RawRight;

  // first compute angle in deg
  // first hypotenuse
  float z = sqrt(x*x+y*y);

  // angle in radians
  float rad = acos(abs(x) / z);

  // handle NaN
  if (isnan(rad) == true) {
    rad = 0;
  }

  // degrees
  float angle = rad * 180/ PI;
  // Now angle indicates the measure of turn
  // Along a straight line, with an angle o, the turn co-efficient is same
  // this applies for angles between 0-90, with angle 0 the co-eff is -1
  // with angle 45, the co-efficient is 0 and with angle 90, it is 1

  float tcoeff = -1 + (angle / 90) * 2;
  float turn = tcoeff * abs(abs(y) - abs(x));
  turn = round(turn * 100) / 100;

  // And max of y or x is the movement
  float mov = _max(abs(y), abs(x));

  // First and third quadrant
  if ((x >= 0 && y >= 0) || (x < 0 && y < 0))
  {
    rawLeft = mov; rawRight = turn;
  }
  else
  {
    rawRight = mov; rawLeft = turn;
  }

  // Reverse polarity
  if (y < 0) {
    rawLeft = 0 - rawLeft;
    rawRight = 0 - rawRight;
  }

  // Update the values
  RawLeft = rawLeft;
  RawRight = rawRight;

  // Map the values onto the defined rang
  LeftMotorOutput = map(rawLeft, DDRIVE_MIN, DDRIVE_MAX, MOTOR_MIN_PWM, MOTOR_MAX_PWM);
  RightMotorOutput = map(rawRight, DDRIVE_MIN, DDRIVE_MAX, MOTOR_MIN_PWM, MOTOR_MAX_PWM);
}

bool taser_stop(void *) {
  firing = false;
  //digitalWrite(TASERPIN, LOW);
  return true; // repeat? true
}

void taser(int shockTime) {
  if (!firing) {
    //digitalWrite(TASERPIN, HIGH);
    timer.in(shockTime*1000,taser_stop);
    firing = true;
  }
}










void configModeCallback (WiFiManager *myWiFiManager) {
  Serial.println("Entered config mode");
  Serial.println(WiFi.softAPIP());
  //if you used auto generated SSID, print it
  Serial.println(myWiFiManager->getConfigPortalSSID());
}











void setup() {


    // Set software serial baud to 115200;
  Serial.begin(115200);













  //read configuration from FS json
  Serial.println("mounting FS...");

  if (SPIFFS.begin()) {
    Serial.println("mounted file system");
    if (SPIFFS.exists("/config.json")) {
      //file exists, reading and loading
      Serial.println("reading config file");
      File configFile = SPIFFS.open("/config.json", "r");
      if (configFile) {
        Serial.println("opened config file");
        size_t size = configFile.size();
        // Allocate a buffer to store contents of the file.
        std::unique_ptr<char[]> buf(new char[size]);

        configFile.readBytes(buf.get(), size);
        DynamicJsonBuffer jsonBuffer;
        JsonObject& json = jsonBuffer.parseObject(buf.get());
        json.printTo(Serial);
        if (json.success()) {
          Serial.println("\nparsed json");

          strcpy(mqtt_broker, json["mqtt_broker"]);
          strcpy(mqtt_port, json["mqtt_port"]);
      //    strcpy(blynk_token, json["blynk_token"]);

        } else {
          Serial.println("failed to load json config");
        }
      }
    }
  } else {
    Serial.println("failed to mount FS");
  }
  //end read
















 // The extra parameters to be configured (can be either global or just in the setup)
  // After connecting, parameter.getValue() will get you the configured value
  // id/name placeholder/prompt default length
  WiFiManagerParameter custom_mqtt_broker("broker", "Here a server IP", mqtt_broker, 40);
  WiFiManagerParameter custom_mqtt_port("port", "Here a broker port", mqtt_port, 6);
 // WiFiManagerParameter custom_blynk_token("blynk", "Here a blynk token", blynk_token, 32);






  //WiFiManager
  //Local intialization. Once its business is done, there is no need to keep it around
  WiFiManager wifiManager;
  //reset settings - for testing
  //wifiManager.resetSettings();
 
  //set callback that gets called when connecting to previous WiFi fails, and enters Access Point mode
  wifiManager.setAPCallback(configModeCallback);

wifiManager.setSaveConfigCallback(saveConfigCallback);






 //add all your parameters here
  wifiManager.addParameter(&custom_mqtt_broker);
  wifiManager.addParameter(&custom_mqtt_port);
 // wifiManager.addParameter(&custom_blynk_token);

  

  
 
  //fetches ssid and pass and tries to connect
  //if it does not connect it starts an access point with the specified name
  //here  "AutoConnectAP"
  //and goes into a blocking loop awaiting configuration
  if(!wifiManager.autoConnect("esp8266 mischiantis test")) {
    Serial.println("failed to connect and hit timeout");
    //reset and try again, or maybe put it to deep sleep
    ESP.reset();
    delay(1000);
  } 
 
  //if you get here you have connected to the WiFi
  Serial.println(F("WIFIManager connected!"));
 
  Serial.print(F("IP --> "));
  Serial.println(WiFi.localIP());
  Serial.print(F("GW --> "));
  Serial.println(WiFi.gatewayIP());
  Serial.print(F("SM --> "));
  Serial.println(WiFi.subnetMask());
 
  Serial.print(F("DNS 1 --> "));
  Serial.println(WiFi.dnsIP(0));
 
  Serial.print(F("DNS 2 --> "));
  Serial.println(WiFi.dnsIP(1));





//read updated parameters
  strcpy(mqtt_broker, custom_mqtt_broker.getValue());
  strcpy(mqtt_port, custom_mqtt_port.getValue());
 // strcpy(blynk_token, custom_blynk_token.getValue());


   //save the custom parameters to FS
  if (shouldSaveConfig) {
    Serial.println("saving config");
    DynamicJsonBuffer jsonBuffer;
    JsonObject& json = jsonBuffer.createObject();
    json["mqtt_broker"] = mqtt_broker;
    json["mqtt_port"] = mqtt_port;
 //   json["blynk_token"] = blynk_token;

    File configFile = SPIFFS.open("/config.json", "w");
    if (!configFile) {
      Serial.println("failed to open config file for writing");
    }

    json.printTo(Serial);
    json.printTo(configFile);
    configFile.close();
    //end save
  }

  Serial.println("local ip");
  Serial.println(WiFi.localIP());
 
  Serial.print(F("broker --> "));
  Serial.println(mqtt_broker);
  Serial.print(F("port --> "));
  Serial.println(mqtt_port);
//  Serial.print(F("blynk --> "));
 // Serial.println(blynk_token);





/*

  
  // connecting to a WiFi network
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
      delay(500);
      Serial.println("Connecting to WiFi..");
      }
  Serial.println("Connected to the WiFi network");
  */
  //connecting to a mqtt broker
  client.setServer(mqtt_broker, atoi(mqtt_port));
  client.setCallback(callback);
  while (!client.connected()) {
      String client_id = "esp8266-client2-";
      client_id += String(WiFi.macAddress());
      Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
  //    if (client.connect(client_id.c_str(), mqtt_username, mqtt_password)) {
      if (client.connect(client_id.c_str())) {
          Serial.println("Public emqx mqtt broker connected");
      } else {
          Serial.print("failed with state ");
          Serial.print(client.state());
          Serial.print(mqtt_broker);
          Serial.print(mqtt_broker);
          delay(2000);
      }
  }
  // publish and subscribe
  client.publish(topic, "hello emqx");
  client.subscribe(topic);



  pinMode( MOTOR_A_DIR, OUTPUT );
  pinMode( MOTOR_A_PWM, OUTPUT );
  coast();

 // Serial.println("start udp read");
}
int rlen,val_V=0,val_S=128;
int  x,y;
bool debug = false;

void loop() {

  
  client.loop();
  


}





void callback(char *topic, byte *payload, unsigned int length) {


if (length > 0) {
  
  Serial.print("Message arrived in topic: ");
  Serial.println(topic);
  Serial.print("Message:");
  for (int i = 0; i < length; i++) {
      Serial.print((char) payload[i]);
      packetBuffer[i] = (char) payload[i];
  }
  Serial.println();
  Serial.println("-----------------------");

 packetBuffer[length] = 0;



/*
// Allocate the correct amount of memory for the payload copy
  byte* p = (byte*)malloc(length);
  // Copy the payload to the new buffer
  memcpy(p,payload,length);
  client.publish(outTopic, p, length);
  // Free the memory
  free(p);
*/




  //rlen = udp.parsePacket();

 // if (rlen) {
    //Serial.printf("Received %d bytes from %s, port %d\n", rlen, udp.remoteIP().toString().c_str(), udp.remotePort());
  
   // int len = udp.read(packetBuffer, 255);


    //Serial.printf("UDP packet contents: %s\n", packetBuffer);
    if (debug) {
        // Allocate the correct amount of memory for the payload copy
        byte* p = (byte*)malloc(length);
        // Copy the payload to the new buffer
        memcpy(p,payload,length);
     //   p[length] = _;
     client.publish(outTopic, "_________________");
        client.publish(outTopic, p, length);
             client.publish(outTopic, "_________________");

        // Free the memory
        free(p);
      }



    char *val;
    val = strtok(packetBuffer,":");
   // Serial.println("VAL: " + *val[0]);
    while (val != NULL) {
      if (val[0] == 'X') {
        x = atoi(&val[1]);
        //client.publish(outTopic, "_X: " + (char)x);
        //client.publish(topic, "X");
      }
      else if (val[0] == 'Y') {
        y = atoi(&val[1]);
      //  client.publish(outTopic, "_Y: " + (char)y);
      }
      else if (val[0] == 'T') {
        // tail
        int num = atoi(&val[1]);
        taser(num);
        //udp.beginPacket(udp.remoteIP(), udp.remotePort());
        //udp.write("fire taser");
        client.publish(outTopic, "fire taser");
        //udp.endPacket();
        //yield();
      }
      else if (val[0] == 'D') {
        //udp.beginPacket(udp.remoteIP(), udp.remotePort());
        debug = !debug;
        if (debug){
          //udp.write("Debug ON\n");
         // udp.write("Debug ON");
           client.publish(outTopic, "Debug ON");
           Serial.print("Debug ON");

        }else {
          //udp.write("Debug OFF\n");
         // udp.write("Debug OFF");
         client.publish(outTopic, "Debug OFF");
         Serial.print("Debug OFF");
        //udp.endPacket();
      }
      }
      /*
      else if (val[0] == 'A') {
        //x = atoi(&val[1]);
        //client.publish(outTopic, "_X: " + (char)x);
        //client.publish(topic, "X");
        //mqtt_broker = atoi(&val[1]);
        // Allocate the correct amount of memory for the payload copy
        // Copy the payload to the new buffer
        //mqtt_broker = &(char) payload[1];
        mqtt_broker = &val[1];
                
        //for (int i = 2; i < length; i++) {
         // mqtt_broker = mqtt_broker + (char) payload[i];
          
           //mqtt_broker 
        //}
        Serial.println();
        Serial.println("New MQTT Broker Address:");
        Serial.println(mqtt_broker);

        if (client.connected()) {

          Serial.println("Esp Client starts to disconnect from mqtt broker");
        //  client.unsubscribe(topic);
         client.disconnect();

          while (client.connected()) {
            Serial.println("Waiting for client to disconnect from mqtt broker");
            delay(2000);
          }
          //connecting to a mqtt broker
          client.setServer(mqtt_broker, mqtt_port);
          client.setCallback(callback);
          while (!client.connected()) {
              String client_id = "esp8266-client2-";
              client_id += String(WiFi.macAddress());
              Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
          //    if (client.connect(client_id.c_str(), mqtt_username, mqtt_password)) {
              if (client.connect(client_id.c_str())) {
                  Serial.println("Public emqx mqtt broker connected");
              } else {
                  Serial.print("failed with state ");
                  Serial.print(client.state());
                  delay(2000);
              }
          }
          // publish and subscribe
          client.publish(topic, "hello emqx");
          client.subscribe(topic);
        }




        
      
       // Serial.print(val[1]);
      }*/
     // else if (val[0] == 'P') {
       // y = atoi(&val[1]);
      //  client.publish(outTopic, "_Y: " + (char)y);
     // mqtt_port = (atoi(&val[1])).toInt();
    //  }
      val = strtok(NULL,":");
    }
    //x = atof(packetBuffer);

    //calcTankDrive(x,y);
    yield();  
    //Serial.printf("L: %d, R: %d\n",LeftMotorOutput,RightMotorOutput);
    int mapx = map(x, -100, 100, -1023, 1023);
    Serial.printf("x_val: %d\n",mapx);
    if (mapx > MIN_THRESHOLD) {
      digitalWrite( MOTOR_A_DIR, LOW ); // direction = forward (HIGH)
      analogWrite( MOTOR_A_PWM, mapx ); // PWM speed = slow      
    } else if (mapx < MIN_THRESHOLD) {
      analogWrite( MOTOR_A_DIR, -mapx ); // direction = forward (HIGH)
      digitalWrite( MOTOR_A_PWM, LOW ); // PWM speed = slow            
    } else {
      digitalWrite( MOTOR_A_DIR, LOW ); // direction = forward (HIGH)
      digitalWrite( MOTOR_A_PWM, LOW ); // PWM speed = slow                  
    }      
 // }

}
  timer.tick();






  
}
