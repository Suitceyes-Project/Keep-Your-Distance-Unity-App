using Realtime.Ortc;
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
            m_Factory.Create(message);
            m_ScrollRect.verticalNormalizedPosition = 1f;
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
    }
}