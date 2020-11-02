using Aci.KeepYourDistance.Payloads;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;
using MQTTnet;
using Aci.KeepYourDistance.Client;

namespace Aci.KeepYourDistance.ViewControllers
{
    public class MainMenuViewController : MonoBehaviour
    {
        private const string Me = "Smartphone";

        private IMqttClientService m_MqttClient;
        private ConsoleMessageViewController.Factory m_Factory;

        [SerializeField]
        private ScrollRect m_ScrollRect;
        private List<ConsoleMessageViewController> m_Messages = new List<ConsoleMessageViewController>();

        [Zenject.Inject]
        private void Construct(IMqttClientService mqttClientService, ConsoleMessageViewController.Factory factory)
        {
            m_MqttClient = mqttClientService;
            m_Factory = factory;
        }

        private void OnEnable()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            m_MqttClient.AddMessageHandler("suitceyes/kyd/debug", OnDebugMessageReceived);
        }

        private void OnDisable()
        {
            m_MqttClient.RemoveMessageHandler("suitceyes/kyd/debug", OnDebugMessageReceived);
        }

        private void OnDebugMessageReceived(MqttApplicationMessage obj)
        {
            AddMessageToConsole("Pi", Convert.ToBase64String(obj.Payload));
        }

        private void AddMessageToConsole(string sender, string message)
        {
            ConsoleMessageViewController vc = m_Factory.Create(sender, message);
            m_Messages.Add(vc);
            m_ScrollRect.verticalNormalizedPosition = 0f;
        }

        private void OnSubscribedToChannel(string channel)
        {
            Debug.Log($"Successfully subscribed to channel: {channel}");
        }

        public void CatchThief()
        {
            AddMessageToConsole(Me, "Sending catch thief signal.");
            
            m_MqttClient.PublishAsync("suitceyes/kyd/CatchThief");
        }

        public void SetProgress(int progress)
        {
            AddMessageToConsole(Me, $"Setting progress to: {progress} %.");
            m_MqttClient.PublishAsync("suitceyes/kyd/SetProgress", JsonConvert.SerializeObject(new SetProgressPayload()
            {
                Value = progress
            }));
        }

        public void StartApplication()
        {
            AddMessageToConsole(Me, "Sending start application signal.");
            m_MqttClient.PublishAsync("suitceyes/kyd/start");
        }

        public void StopApplication()
        {
            AddMessageToConsole(Me, "Sending stop application signal.");
            m_MqttClient.PublishAsync("suitceyes/kyd/stop");
        }

        public void Clear()
        {
            for (int i = 0; i < m_Messages.Count; i++)
                Destroy(m_Messages[i].gameObject);

            m_Messages.Clear();
        }
    }
}