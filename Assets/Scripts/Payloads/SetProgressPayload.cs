namespace Aci.KeepYourDistance.Payloads
{
    [System.Serializable]
    public class SetProgressPayload : IPayload
    {
        public int Value { get; set; }
    }
}