using System.Runtime.Serialization;

namespace MSI_LED_Tool
{
    [DataContract]
    public class LedSettings
    {
        [DataMember]
        public AnimationType AnimationType { get; set; }

        [DataMember]
        public int R { get; set; }

        [DataMember]
        public int G { get; set; }

        [DataMember]
        public int B { get; set; }
    }
}
