using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using UnityEngine;
using Zenject;

namespace Aci.KeepYourDistance.Client
{
    public class MqttInstaller : MonoInstaller<MqttInstaller>
    {
        [SerializeField]
        private string[] _topics;

        public override void InstallBindings()
        {
            MqttFactory factory = new MqttFactory();
            IMqttClient mqttClient = factory.CreateMqttClient();
            IMqttClientOptions options = new MqttClientOptionsBuilder()
                                            .WithClientId("KeepYourDistance_App")
                                            .WithTcpServer("mqtt.ably.io", 8883)
                                            .WithCredentials("GIn8xA.lwvRbg", "mmkCycAbnYQvmZxS")
                                            .WithTls()
                                            .WithCleanSession()
                                            .Build();

            Container.Bind<string[]>().FromInstance(_topics).AsSingle();
            Container.Bind<IMqttClient>().FromInstance(mqttClient).AsSingle();
            Container.Bind<IMqttClientOptions>().FromInstance(options).AsSingle();
            Container.BindInterfacesAndSelfTo<MqttClientService>().AsSingle().NonLazy();
        }
    }
}