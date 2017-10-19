using System.Runtime.Serialization;

namespace SGCombo.AmazonS3
{
    public class AmazonS3ConfigDataKeys
    {

        [DataMember]
        public string LocalFileName { get; set; }

        [DataMember]
        public string AmazonS3Key { get; set; }

    }
}
