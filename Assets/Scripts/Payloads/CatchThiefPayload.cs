using UnityEngine;

namespace Aci.KeepYourDistance.Payloads
{
    [System.Serializable]
    public class CatchThiefPayload : IPayload
    {
        [SerializeField]
        public string FunctionName => "CatchThief";
    }
}