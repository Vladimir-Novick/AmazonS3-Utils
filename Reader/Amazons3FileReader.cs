using Amazon.S3;
using Amazon.S3.Model;
using HttpFileReader.Config;
using System;
using System.Collections.Generic;
using Utils;
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

namespace HttpFileReader
{
    public class AmazonS3FileReader
    {
        public AmazonS3Client GetAmazonS3Client()
        {
            AmazonS3Client _client = new AmazonS3Client(
                    HttpFileReaderConfig.GetConfigData.AmazonS3AccessKey,
                    HttpFileReaderConfig.GetConfigData.AmazonS3SecretKey,
                    new AmazonS3Config { ServiceURL = HttpFileReaderConfig.GetConfigData.ServiceURL }
            );
            return _client;
        }


        public  List<S3Object> FileDownloader()
        {

            List<S3Object> ret = new List<S3Object>();

            try
            {

                using (AmazonS3Client s3Client = GetAmazonS3Client()) {
                    try
                    {
                        List<S3Object> chekS3Objects = new List<S3Object>();

                        ListObjectsRequest request = new ListObjectsRequest();
                        request.BucketName = HttpFileReaderConfig.GetConfigData.BucketName;
                        Task<ListObjectsResponse> task = s3Client.ListObjectsAsync(request);
                        task.Wait();

                        ListObjectsResponse response = task.Result;

                        List<String> keys = HttpFileReaderConfig.GetConfigData.Keys;

                        foreach (S3Object item in response.S3Objects)
                        {
                            String itemF = keys.FirstOrDefault(o => o == item.Key) ;
                            if (itemF != null)
                            {
                                chekS3Objects.Add(item);
                                ReadAmasonFile(s3Client,item);
                            }
                        }


                        var fileNameListOfFiles = HttpFileReaderConfig.GetConfigData.TargetFolder + "ListObjects.json";
                        var logWriter = System.IO.File.CreateText(fileNameListOfFiles);
                        logWriter.WriteLine(chekS3Objects.ToJson());
                        logWriter.Dispose();
                    } catch ( Exception ex)
                    {

                    }
                }

            }
            catch (Exception ex) { }
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

        private void ReadAmasonFile(AmazonS3Client s3Client,S3Object item)
        {
            String[] k = item.Key.Split("/");
            String filename = HttpFileReaderConfig.GetConfigData.TargetFolder + k[k.Length - 1];

            FileInfo fileInfo = new FileInfo(filename);

            if ((!fileInfo.Exists) || (fileInfo.LastWriteTime < item.LastModified))
            {
                try
                {
                    Task<GetObjectResponse> taskGetObjectResponse = s3Client.GetObjectAsync(HttpFileReaderConfig.GetConfigData.BucketName, item.Key);
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
                        ZipFile.ExtractToDirectory(filename, HttpFileReaderConfig.GetConfigData.TargetFolder,true);
                    }
                }
                catch (Exception ex) {
                }
            }
        }
    }
}
