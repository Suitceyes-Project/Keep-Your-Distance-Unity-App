using MQTTnet;
using System;
using System.Threading.Tasks;

namespace Aci.KeepYourDistance.Client
{
    public interface IMqttClientService
    {
        void AddMessageHandler(string topic, Action<MqttApplicationMessage> handler);

        void RemoveMessageHandler(string topic, Action<MqttApplicationMessage> handler);

        Task PublishAsync(string topic, string payload = null);
    }
}