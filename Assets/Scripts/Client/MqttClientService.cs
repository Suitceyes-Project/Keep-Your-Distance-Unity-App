using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Aci.KeepYourDistance.Client
{
    public class MqttClientService : IInitializable, IDisposable, IMqttClientService
    {
        private IMqttClient _mqttClient;
        private IMqttClientOptions _options;
        private MqttTopicFilter _topicFilter;
        private Dictionary<string, Action<MqttApplicationMessage>> _handlers = new Dictionary<string, Action<MqttApplicationMessage>>();

        public MqttClientService(IMqttClient mqttClient, IMqttClientOptions options, string[] topics)
        {
            _mqttClient = mqttClient;
            _options = options;
            MqttTopicFilterBuilder topicFilterBuilder = new MqttTopicFilterBuilder();
            for (int i = 0; i < topics.Length; i++)
                topicFilterBuilder.WithTopic(topics[i]);
            _topicFilter = topicFilterBuilder.Build();
        }

        public async void Initialize()
        {
            _mqttClient.UseConnectedHandler(OnMqttConnected);
            _mqttClient.UseDisconnectedHandler(OnMqttDisconnected);
            _mqttClient.UseApplicationMessageReceivedHandler(OnApplicationMessageReceived);

            try
            {
                Debug.Log("Connecting to MQTT Server...");
                await _mqttClient.ConnectAsync(_options, CancellationToken.None);
            }
            catch(Exception e)
            {
                Debug.LogError("Connection to MQTT Broker failed!");
                Debug.LogException(e);
            }

        }

        private Task OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs arg)
        {
#if UNITY_EDITOR
            Debug.Log("Received MQTT Message.");
#endif
            if (_handlers.TryGetValue(arg.ApplicationMessage.Topic, out Action<MqttApplicationMessage> handler))
                handler?.Invoke(arg.ApplicationMessage);

            return Task.CompletedTask;
        }

        private async Task OnMqttDisconnected(MqttClientDisconnectedEventArgs arg)
        {
            Debug.Log("MQTT: Disconnected from server. Trying to reconnect...");

            await Task.Delay(TimeSpan.FromSeconds(5));
            try
            {
                await _mqttClient.ConnectAsync(_options, CancellationToken.None);
            }
            catch(Exception e)
            {
                Debug.LogError("Connection to MQTT Broker failed!");
                Debug.LogException(e);
            }
        }

        private async Task OnMqttConnected(MqttClientConnectedEventArgs arg)
        {
            Debug.Log($"Mqtt connected with result: {arg.AuthenticateResult.ResultCode}");

            if(arg.AuthenticateResult.ResultCode == MqttClientConnectResultCode.Success)
                await _mqttClient.SubscribeAsync(_topicFilter);
        }

        public void Dispose()
        {
            _mqttClient.DisconnectedHandler = null;
            _mqttClient.DisconnectAsync();
        }

        public void AddMessageHandler(string topic, Action<MqttApplicationMessage> handler)
        {
            _handlers.Add(topic, handler);
        }

        public void RemoveMessageHandler(string topic, Action<MqttApplicationMessage> handler)
        {
            _handlers.Remove(topic);
        }

        public async Task PublishAsync(string topic, string payload = null)
        {
            await _mqttClient.PublishAsync(topic, payload);
        }
    }
}