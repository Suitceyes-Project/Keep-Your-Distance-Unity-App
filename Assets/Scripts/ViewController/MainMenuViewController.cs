using Aci.KeepYourDistance.Payloads;
using Realtime.Ortc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Aci.KeepYourDistance.ViewControllers
{
    public class MainMenuViewController : MonoBehaviour
    {
        private const string ChannelIn = "ACI_KYD";
        private const string ChannelOut = "ACI_KYD_OUT";
        private const string Me = "Smartphone";

        private IOrtcClient m_OrtcClient;
        private ConsoleMessageViewController.Factory m_Factory;

        [SerializeField]
        private ScrollRect m_ScrollRect;
        private List<ConsoleMessageViewController> m_Messages = new List<ConsoleMessageViewController>();

        [Zenject.Inject]
        private void Construct(IOrtcClient ortcClient, ConsoleMessageViewController.Factory factory)
        {
            m_OrtcClient = ortcClient;
            m_Factory = factory;
        }

        private void OnEnable()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            m_OrtcClient.OnConnected += OnConnected;
            m_OrtcClient.OnSubscribed += OnSubscribedToChannel;
        }

        private void OnConnected()
        {
            m_OrtcClient.Subscribe(ChannelOut, true, OnMessageReceived);
        }

        private void OnDisable()
        {
            m_OrtcClient.OnConnected -= OnConnected;
            m_OrtcClient.OnSubscribed -= OnSubscribedToChannel;
            m_OrtcClient.Unsubscribe(ChannelOut);
        }

        private void OnMessageReceived(string channel, string message)
        {
            AddMessageToConsole("Pi", message);
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
            //m_OrtcClient.Send(ChannelIn, "catch_thief");
            
            string json = JsonConvert.SerializeObject(new CatchThiefPayload());
            m_OrtcClient.Send(ChannelIn, json);
        }

        public void SetProgress(int progress)
        {
            AddMessageToConsole(Me, $"Setting progress to: {progress} %.");
            m_OrtcClient.Send(ChannelIn, JsonConvert.SerializeObject(new SetProgressPayload()
            {
                Value = progress
            }));
        }

        public void StartApplication()
        {
            AddMessageToConsole(Me, "Sending start application signal.");
            m_OrtcClient.Send(ChannelIn, "Start");
        }

        public void StopApplication()
        {
            AddMessageToConsole(Me, "Sending stop application signal.");
            m_OrtcClient.Send(ChannelIn, "Stop");
        }

        public void Clear()
        {
            for (int i = 0; i < m_Messages.Count; i++)
                Destroy(m_Messages[i].gameObject);

            m_Messages.Clear();
        }
    }
}