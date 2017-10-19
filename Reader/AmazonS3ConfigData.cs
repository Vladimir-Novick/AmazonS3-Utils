using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SGCombo.AmazonS3
{
    [DataContract(Namespace = "")]
    public class AmazonS3ConfigData
    {
        [DataMember]
        public string AmazonS3AccessKey { get; set; }

        [DataMember]
        public string AmazonS3SecretKey { get; set; }

        [DataMember]

        public string AmazonS3BucketName { get; set; }

        [DataMember]

        public string AmazonS3ServiceURL { get; set; }

        [DataMember]
        public int AmazonS3DeltaTime { get; set; }

        [DataMember]
        public List<AmazonS3ConfigDataKeys> Keys { get; set; }



    }
}
