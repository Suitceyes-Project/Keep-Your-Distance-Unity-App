using Realtime.Ortc;
using System;
using UnityEngine;
using Zenject;

namespace Aci.KeepYourDistance.Client
{
    public class OrtClientInitialization : IInitializable, IDisposable
    {
        private const string Endpoint = "http://ortc-developers.realtime.co/server/2.1";
        private const string EndpointSSL = "https://ortc-developers.realtime.co/server/ssl/2.1";
        private const string UserName = "ACI";

        private readonly string m_ApplicationKey;
        private readonly string m_AuthorizationToken;
        private readonly IOrtcClient m_Client;

        public OrtClientInitialization(IOrtcClient client, string applicationKey, string authToken)
        {
            m_ApplicationKey = applicationKey;
            m_AuthorizationToken = authToken;
            m_Client = client;
        }

        public void Initialize()
        {
            m_Client.OnConnected += OnConnected;
            m_Client.OnDisconnected += OnDisconnected;
            m_Client.OnReconnected += OnReconnecting;
            m_Client.OnReconnected += OnReconnected;           

            m_Client.ConnectionMetadata = UserName;
            m_Client.HeartbeatActive = false;

            m_Client.ClusterUrl = Endpoint;
            m_Client.Connect(m_ApplicationKey, m_AuthorizationToken);
        }

        private void OnConnected()
        {
            Debug.Log("Connected to RealTime Framework");
        }

        private void OnDisconnected()
        {
            Debug.Log("Disconnected from RealTime Framework");
        }

        private void OnReconnecting()
        {
            Debug.Log("Reconnecting to RealTime Framework");
        }

        private void OnReconnected()
        {
            Debug.Log("Reconnected to RealTime Framework");
        }

        public void Dispose()
        {
            m_Client.Disconnect();
            m_Client.OnConnected -= OnConnected;
            m_Client.OnDisconnected -= OnDisconnected;
            m_Client.OnReconnected -= OnReconnecting;
            m_Client.OnReconnected -= OnReconnected;
        }
    }
}