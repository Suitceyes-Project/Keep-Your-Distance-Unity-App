using Realtime.Ortc;
using UnityEngine;

namespace Aci.KeepYourDistance.ViewControllers
{
    public class MainMenuViewController : MonoBehaviour
    {
        private const string Channel = "ACI_KYD";

        private IOrtcClient m_OrtcClient;

        [Zenject.Inject]
        private void Construct(IOrtcClient ortcClient)
        {
            m_OrtcClient = ortcClient;
        }

        public void HelloWorld()
        {
            m_OrtcClient.Send(Channel, "Hello World");
        }
    }
}
