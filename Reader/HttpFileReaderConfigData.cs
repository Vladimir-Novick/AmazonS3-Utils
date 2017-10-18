using System.Collections.Generic;
using System.Runtime.Serialization;

////////////////////////////////////////////////////////////////////////////
//	Copyright 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//        
//             https://github.com/Vladimir-Novick/CSharp-Utility-Classes
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////

namespace HttpFileReader.Config
{
    [DataContract(Namespace = "")]
    public class HttpFileReaderConfigData
    {
        [DataMember]
        public string AmazonS3AccessKey { get; set; }

        [DataMember]
        public string AmazonS3SecretKey { get; set; }

        [DataMember]

        public string TargetFolder { get; set; }

        [DataMember]

        public string BucketName { get; set; }

        [DataMember]

        public string ServiceURL { get; set; }


        [DataMember]

        public List<string> Keys { get; set; }

    }
}
