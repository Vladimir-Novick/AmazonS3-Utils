using Newtonsoft.Json;
using System;
using System.IO;

////////////////////////////////////////////////////////////////////////////
//	Copyright 2017 : Vladimir Novick    https://www.linkedin.com/in/vladimirnovick/  
//        
//             https://github.com/Vladimir-Novick/AmazonS3-Utils
//
//    NO WARRANTIES ARE EXTENDED. USE AT YOUR OWN RISK. 
//
// To contact the author with suggestions or comments, use  :vlad.novick@gmail.com
//
////////////////////////////////////////////////////////////////////////////

namespace SGCombo.AmazonS3
{
    public class AmazonS3Config
    {


        public static AmazonS3ConfigData GetConfigData { get; set;  }



        public static void GetConfiguration(String configFile = "AmazonConfig.json")
        {
            String pathToTheFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + configFile;

            GetConfigData = new AmazonS3ConfigData();
            using (StreamReader file = File.OpenText(pathToTheFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                GetConfigData = (AmazonS3ConfigData)serializer.Deserialize(file, typeof(AmazonS3ConfigData));
            }

        }

  
    }
}
