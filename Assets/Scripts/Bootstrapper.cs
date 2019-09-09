using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aci.KeepYourDistance
{
    public class Bootstrapper : MonoBehaviour
    {
        private async void Start()
        {
#if !UNITY_EDITOR

            await SceneManager.LoadSceneAsync("scn_UI", LoadSceneMode.Additive);                      
#endif

            if (!Debug.isDebugBuild)
                Debug.unityLogger.logEnabled = false;

        }
    }
}