using Realtime.Ortc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aci.KeepYourDistance.ViewControllers
{
    public class MainMenuViewController : MonoBehaviour
    {
        private const string ChannelIn = "ACI_KYD";
        private const string ChannelOut = "ACI_KYD_OUT";

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
            ConsoleMessageViewController vc = m_Factory.Create(message);
            m_Messages.Add(vc);
            m_ScrollRect.verticalNormalizedPosition = 0f;
        }

        private void OnSubscribedToChannel(string channel)
        {
            Debug.Log($"Successfully subscribed to channel: {channel}");
        }

        public void StartApplication()
        {
            m_OrtcClient.Send(ChannelIn, "Start");
        }

        public void StopApplication()
        {
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