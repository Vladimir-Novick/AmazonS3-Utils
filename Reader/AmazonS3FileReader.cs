using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.IO.Compression;

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
    public class AmazonS3FileReader
    {
        private AmazonS3Client GetAmazonS3Client()
        {
            AmazonS3Client _client = new AmazonS3Client(
                    AmazonS3Config.GetConfigData.AmazonS3AccessKey,
                    AmazonS3Config.GetConfigData.AmazonS3SecretKey,
                    new Amazon.S3.AmazonS3Config { ServiceURL = AmazonS3Config.GetConfigData.AmazonS3ServiceURL }
            );
            return _client;
        }

        /// <summary>
        ///   Download new/Updated AmazonS3 files and return AmazonS3 keys 
        /// </summary>
        /// <returns></returns>
        public  List<String> FileDownloader()
        {

            List<String> ret = new List<String>();

            try
            {

                using (AmazonS3Client s3Client = GetAmazonS3Client()) {
                    try
                    {
                        ListObjectsRequest request = new ListObjectsRequest();
                        request.BucketName = AmazonS3Config.GetConfigData.AmazonS3BucketName;
                        Task<ListObjectsResponse> task = s3Client.ListObjectsAsync(request);
                        task.Wait();

                        ListObjectsResponse response = task.Result;

                        List<AmazonS3ConfigDataKeys> keys = AmazonS3Config.GetConfigData.Keys;

                        foreach (S3Object item in response.S3Objects)
                        {
                            AmazonS3ConfigDataKeys itemF = keys.FirstOrDefault(o => o.AmazonS3Key == item.Key) ;
                            try
                            {
                                if (itemF != null)
                                {

                                    ReadAmazonFile(s3Client, item, itemF);
                                }
                                ret.Add(item.Key); 
                            }
                            catch (Exception) { }

                          
                        }



                    } catch ( Exception )
                    {

                    }
                }

            }
            catch (Exception ) { }
            return ret ;
        }

        private void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        private void ReadAmazonFile(AmazonS3Client s3Client,S3Object item, AmazonS3ConfigDataKeys configKey)
        {
            String[] k = item.Key.Split("/");
            String filename = configKey.LocalFileName;

            FileInfo fileInfo = new FileInfo(filename);

            if ((!fileInfo.Exists) || (fileInfo.LastWriteTime < item.LastModified))
            {
                try
                {
                    Task<GetObjectResponse> taskGetObjectResponse = s3Client.GetObjectAsync(AmazonS3Config.GetConfigData.AmazonS3BucketName, item.Key);
                    taskGetObjectResponse.Wait();
                    using (GetObjectResponse getObjRespone = taskGetObjectResponse.Result)
                    {
                        using (Stream responseStream = getObjRespone.ResponseStream)
                        {
                            using (Stream fileStream = File.Create(filename))
                            {
                                CopyStream(responseStream, fileStream);
                                fileStream.Flush();
                                fileStream.Close();
                            }
                        }
                    }
                    File.SetCreationTime(filename, item.LastModified);
                    File.SetLastWriteTime(filename, item.LastModified);

                    string ext = Path.GetExtension(filename).ToLower();
                    if (ext == ".zip")
                    {
                        String strTargetFolder = configKey.LocalFileName;
                       
                        String strFolder = Path.GetDirectoryName(strTargetFolder);
                        ZipFile.ExtractToDirectory(filename, strFolder, true);
                    }
                }
                catch (Exception ) {
                }
            }
        }
    }
}
