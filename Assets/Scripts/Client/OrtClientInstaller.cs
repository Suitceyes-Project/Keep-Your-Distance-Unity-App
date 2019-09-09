using Realtime.Ortc;
using UnityEngine;
using Zenject;

namespace Aci.KeepYourDistance.Client
{
    public class OrtClientInstaller : MonoInstaller<OrtClientInstaller>
    {
        [SerializeField]
        private string m_ApplicationKey;

        [SerializeField]
        private string m_AuthorizationToken;

        public override void InstallBindings()
        {
            Container.Bind<IOrtcClient>().FromInstance(OrtcFactory.Create()).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<OrtClientInitialization>().AsSingle().WithArguments(m_ApplicationKey, m_AuthorizationToken).NonLazy();
        }
    }
}