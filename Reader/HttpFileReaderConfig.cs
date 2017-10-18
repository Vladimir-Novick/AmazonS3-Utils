using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    public class HttpFileReaderConfig
    {

        // CrawlerConfig.GetConfigData

        public static HttpFileReaderConfigData GetConfigData { get; set;  }



        public static void GetConfiguration(String configFile = "config.json")
        {
            String pathToTheFile = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + configFile;

            GetConfigData = new HttpFileReaderConfigData();
            using (StreamReader file = File.OpenText(pathToTheFile))
            {
                JsonSerializer serializer = new JsonSerializer();
                GetConfigData = (HttpFileReaderConfigData)serializer.Deserialize(file, typeof(HttpFileReaderConfigData));
            }

        }

  
    }
}
