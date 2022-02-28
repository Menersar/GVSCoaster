#include <FS.h>                   //this needs to be first, or it all crashes and burns...

#include <arduino-timer.h>
#include <ESP8266WiFi.h>
#include <PubSubClient.h>
#include <WiFiManager.h>        //https://github.com/tzapu/WiFiManager
#include <ArduinoJson.h>

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

const int PACKET_SIZE = 256;
char packetBuffer[PACKET_SIZE];
auto timer = timer_create_default();
bool firing = false;

const char *topic = "MQTTUnityEsp/GVSCoaster/inTopic";
const char *outTopic = "MQTTUnityEsp/GVSCoaster/outTopic";
//const char *mqtt_username = "mqttuser";
//const char *mqtt_password = "mqttpassword";

//define your default values here, if there are different values in config.json, they are overwritten.
char mqtt_broker[40] = "192.168.178.20";
char mqtt_port[6] = "1883";

char wifi_name[32] = "";
char wifi_password[64] = "";

WiFiClient espClient;
PubSubClient client(espClient);

//flag for saving data
bool shouldSaveConfig = false;

bool resetEsp = true;

//callback notifying us of the need to save config
void saveConfigCallback () {
  //Serial.println("Should save config");
  shouldSaveConfig = true;
}

void coast() {
  //Serial.println( "Soft stop (coast)..." );
  digitalWrite( MOTOR_A_DIR, LOW );
  digitalWrite( MOTOR_A_PWM, LOW );
}

void brake() {
  //Serial.println( "Hard stop (brake)..." );
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
  float z = sqrt(x * x + y * y);

  // angle in radians
  float rad = acos(abs(x) / z);

  // handle NaN
  if (isnan(rad) == true) {
    rad = 0;
  }

  // degrees
  float angle = rad * 180 / PI;
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
    timer.in(shockTime * 1000, taser_stop);
    firing = true;
  }
}

void configModeCallback (WiFiManager *myWiFiManager) {
  //Serial.println("Entered config mode");
  Serial.println(WiFi.softAPIP());
  //if you used auto generated SSID, print it
  Serial.println(myWiFiManager->getConfigPortalSSID());
}

void setup() {

  // Set software serial baud to 115200;
  Serial.begin(115200);

  //WiFi.disconnect(true);

  //clean FS, for testing
  // SPIFFS.format();

  //read configuration from FS json
  //Serial.println("mounting FS...");

  if (SPIFFS.begin()) {
    //Serial.println("mounted file system");
    if (SPIFFS.exists("/config.json")) {
      //file exists, reading and loading
      //Serial.println("reading config file");
      File configFile = SPIFFS.open("/config.json", "r");
      if (configFile) {
        //Serial.println("opened config file");
        size_t size = configFile.size();
        // Allocate a buffer to store contents of the file.
        std::unique_ptr<char[]> buf(new char[size]);

        configFile.readBytes(buf.get(), size);

#ifdef ARDUINOJSON_VERSION_MAJOR >= 6
        DynamicJsonDocument json(1024);
        auto deserializeError = deserializeJson(json, buf.get());
        serializeJson(json, Serial);
        if ( ! deserializeError ) {
#else
        DynamicJsonBuffer jsonBuffer;
        JsonObject& json = jsonBuffer.parseObject(buf.get());
        json.printTo(Serial);
        if (json.success()) {
#endif
          //Serial.println("\nparsed json");
          strcpy(mqtt_broker, json["mqtt_broker"]);
          strcpy(mqtt_port, json["mqtt_port"]);
          strcpy(wifi_name, json["wifi_name"]);
          strcpy(wifi_password, json["wifi_password"]);
        } else {
          //Serial.println("failed to load json config");
        }
        configFile.close();
      }
    }
  } else {
    //Serial.println("failed to mount FS");
  }
  //end read

  // The extra parameters to be configured (can be either global or just in the setup)
  // After connecting, parameter.getValue() will get you the configured value
  // id/name placeholder/prompt default length
  WiFiManagerParameter custom_mqtt_broker("broker", "Here a server IP", mqtt_broker, 40);
  WiFiManagerParameter custom_mqtt_port("port", "Here a broker port", mqtt_port, 6);

  WiFiManagerParameter custom_wifi_name("s", "SSID", wifi_name, 32);
  WiFiManagerParameter custom_wifi_password("p", "password", wifi_password, 64, "type='password'");

  //WiFiManager
  //Local intialization. Once its business is done, there is no need to keep it around
  WiFiManager wifiManager;
  //reset settings - for testing
  //wifiManager.resetSettings();

  // wifiManager.setClass("invert"); // dark theme

  //set callback that gets called when connecting to previous WiFi fails, and enters Access Point mode
  wifiManager.setAPCallback(configModeCallback);

  wifiManager.setSaveConfigCallback(saveConfigCallback);

  //  WiFi.printDiag(Serial);
  //Serial.println("SAVED: " + (String)wifiManager.getWiFiIsSaved() ? "YES" : "NO");
  //Serial.println("SSID: " + (String)wifiManager.getSSID());
  //Serial.println("PASS: " + (String)wifiManager.getPassword());

  //add all your parameters here
  wifiManager.addParameter(&custom_wifi_name);
  wifiManager.addParameter(&custom_wifi_password);

  WiFiManagerParameter custom_text("<br/>");
  wifiManager.addParameter(&custom_text);

  wifiManager.addParameter(&custom_mqtt_broker);
  wifiManager.addParameter(&custom_mqtt_port);

  if (resetEsp) {
    if (!wifiManager.startConfigPortal("ESP8266 WiFiManager", "password")) {
      //Serial.println("failed to connect and hit timeout");
      //reset and try again, or maybe put it to deep sleep
      ESP.restart();
      delay(1000);
    }
    resetEsp = false;
  }

  //fetches ssid and pass and tries to connect
  //if it does not connect it starts an access point with the specified name
  //here  "AutoConnectAP"
  //and goes into a blocking loop awaiting configuration
  // if(!wifiManager.autoConnect("esp8266 mischiantis test")) {
    //Serial.println("failed to connect and hit timeout");
      //reset and try again, or maybe put it to deep sleep
  //  ESP.restart();
  //  delay(1000);
  //  }

  //if you get here you have connected to the WiFi
  //Serial.println(F("WIFIManager connected!"));

 //Serial.print(F("IP --> "));
  //Serial.println(WiFi.localIP());
  //Serial.print(F("GW --> "));
  //Serial.println(WiFi.gatewayIP());
  //Serial.print(F("SM --> "));
  //Serial.println(WiFi.subnetMask());

  //Serial.print(F("DNS 1 --> "));
  //Serial.println(WiFi.dnsIP(0));

  //Serial.print(F("DNS 2 --> "));
  //Serial.println(WiFi.dnsIP(1));

  //read updated parameters

  strcpy(wifi_password, wifiManager.getPassword().c_str());
  strcpy(wifi_name, wifiManager.getSSID().c_str());

  strcpy(mqtt_broker, custom_mqtt_broker.getValue());
  strcpy(mqtt_port, custom_mqtt_port.getValue());

  //save the custom parameters to FS
  if (shouldSaveConfig) {
    //Serial.println("saving config");
#ifdef ARDUINOJSON_VERSION_MAJOR >= 6
    DynamicJsonDocument json(1024);
#else
    DynamicJsonBuffer jsonBuffer;
    JsonObject& json = jsonBuffer.createObject();
#endif
    json["wifi_password"] = wifi_password;
    json["wifi_name"] = wifi_name;
    json["mqtt_broker"] = mqtt_broker;
    json["mqtt_port"] = mqtt_port;

    File configFile = SPIFFS.open("/config.json", "w");
    if (!configFile) {
      //Serial.println("failed to open config file for writing");
    }

#ifdef ARDUINOJSON_VERSION_MAJOR >= 6
    serializeJson(json, Serial);
    serializeJson(json, configFile);
#else
    json.printTo(Serial);
    json.printTo(configFile);
#endif
    configFile.close();
    //end save
  }

  //Serial.println("local ip");
  //Serial.println(WiFi.localIP());

  //Serial.print(F("wifiname --> "));
  //Serial.println(wifi_name);
  //Serial.print(F("wifipassword --> "));
  //Serial.println(wifi_password);
  //Serial.print(F("broker --> "));
  //Serial.println(mqtt_broker);
  //Serial.print(F("port --> "));
  //Serial.println(mqtt_port);

  //connecting to a mqtt broker
  client.setServer(mqtt_broker, atoi(mqtt_port));
  client.setCallback(callback);
  while (!client.connected()) {
    String client_id = "esp8266-client2-";
    client_id += String(WiFi.macAddress());
    //Serial.printf("The client %s connects to the public mqtt broker\n", client_id.c_str());
    //    if (client.connect(client_id.c_str(), mqtt_username, mqtt_password)) {
    if (client.connect(client_id.c_str())) {
      //Serial.println("Public emqx mqtt broker connected");
    } else {
      //Serial.print("failed with state ");
      //Serial.print(client.state());
      //Serial.print(mqtt_broker);
      //Serial.print(mqtt_broker);
      delay(2000);
    }
  }
  // publish and subscribe
  client.publish(topic, "hello emqx");
  client.subscribe(topic);

  pinMode( MOTOR_A_DIR, OUTPUT );
  pinMode( MOTOR_A_PWM, OUTPUT );
  coast();

  //Serial.println("start udp read");
}
int rlen, val_V = 0, val_S = 128;
int  x, y;
bool debug = false;

void loop() {
  client.loop();
}

void callback(char *topic, byte *payload, unsigned int length) {

  if (length > 0) {

    //Serial.print("Message arrived in topic: ");
    //Serial.println(topic);
    //Serial.print("Message:");
    for (int i = 0; i < length; i++) {
      //Serial.print((char) payload[i]);
      packetBuffer[i] = (char) payload[i];
    }
    //Serial.println();
    //Serial.println("-----------------------");

    packetBuffer[length] = 0;

    //Serial.printf("UDP packet contents: %s\n", packetBuffer);
    if (debug) {
      // Allocate the correct amount of memory for the payload copy
      byte* p = (byte*)malloc(length);
      // Copy the payload to the new buffer
      memcpy(p, payload, length);
      client.publish(outTopic, p, length);
      // Free the memory
      free(p);
    }

    char *val;
    val = strtok(packetBuffer, ":");
    //Serial.println("VAL: " + *val[0]);
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
        if (debug) {
          //udp.write("Debug ON\n");
          // udp.write("Debug ON");
          client.publish(outTopic, "Debug ON");
          //Serial.print("Debug ON");

        } else {
          //udp.write("Debug OFF\n");
          // udp.write("Debug OFF");
          client.publish(outTopic, "Debug OFF");
          //Serial.print("Debug OFF");
          //udp.endPacket();
        }
      }
      val = strtok(NULL, ":");
    }
    //x = atof(packetBuffer);

    //calcTankDrive(x,y);
    yield();
    //Serial.printf("L: %d, R: %d\n",LeftMotorOutput,RightMotorOutput);
    int mapx = map(x, -100, 100, -1023, 1023);
    //Serial.printf("x_val: %d\n", mapx);
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
  }
  timer.tick();
}
