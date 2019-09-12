using UnityEngine;
using Zenject;

namespace Aci.KeepYourDistance.ViewControllers
{
    public class ConsoleWindowInstaller : MonoInstaller<ConsoleWindowInstaller>
    {
        [SerializeField]
        private GameObject m_MessagePrefab;

        [SerializeField]
        private Transform m_ParentTransform;

        public override void InstallBindings()
        {
            Container.BindFactory<string, ConsoleMessageViewController, ConsoleMessageViewController.Factory>().
                      FromComponentInNewPrefab(m_MessagePrefab).UnderTransform(m_ParentTransform);
        }
    }
}