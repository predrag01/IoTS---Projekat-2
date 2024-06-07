import paho.mqtt.client as paho
import json
import datetime

sensor = "sensor/weather-data"
analytics= "analytics/weather-data"

def on_connect(client, userdata, flags, rc):
    print("Connected with result code " + str(rc))
    client.subscribe(sensor + "/+")

def on_message(client, userdatqa, msg):
    if msg.topic.startswith(sensor):
        data = json.loads(msg.payload)
        print("Message received from sensor:", data)

        if "AirTemperature" in data:
            if data["AirTemperature"] > 30:
                data["EventType"] = "HighTemperatureAlarm"

                json_data = json.dumps(data)

                measurement_id = data["MeasurementId"]
                client.publish(analytics + "/" + str(measurement_id), payload=json_data, qos=1)

            elif data["AirTemperature"] < 10:
                data["EventType"] = "LowTemperatureAlarm"

                json_data = json.dumps(data)

                measurement_id = data["MeasurementId"]
                client.publish(analytics + "/" + str(measurement_id), payload=json_data, qos=1)



def on_publish(client, userdata, mid):
    print("Message published")

def start_analytics():
    client = paho.Client()

    client.on_connect = on_connect
    client.on_message = on_message
    client.on_publish = on_publish
    client.connect("mosquitto", 8883, 60)
    client.loop_forever()

if __name__ == "__main__":
    start_analytics()