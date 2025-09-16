using Amazon.S3.Transfer;
using Amazon.S3;

namespace Example4ObjectLevelOps
{
    internal class Program
    {
        private const string bucketName = "flowers4li";
        private const string filePath = @"c:\temp\Helper.cs";
        private const string keyName = "uploadedbyapplication";
        private const string fileName = "uploadedbyapp";
        static async Task Main(string[] args)
        {
            //await UploadFileAsync();
            await DownloadFile(bucketName,fileName);
        }


        private static async Task DownloadFile(string bucketName, string fileName)
        {
            var pathAndFileName = $"C:\\temp\\{fileName}";

            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = fileName,
                FilePath = pathAndFileName
            };

            using (var transferUtility = new TransferUtility(Helper.s3Client))
            {
                await transferUtility.DownloadAsync(downloadRequest);
            }
        }


        private static async Task UploadFileAsync()
        {
            try
            {
                var fileTransferUtility = new TransferUtility(Helper.s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                //await fileTransferUtility.UploadAsync(filePath, bucketName);
                //Console.WriteLine("Upload 1 completed");

                // Option 2. Specify object key name explicitly.
                await fileTransferUtility.UploadAsync(filePath, bucketName, keyName);
                Console.WriteLine("Upload 2 completed");

                //// Option 3. Upload data from a type of System.IO.Stream.
                //using (var fileToUpload = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                //{
                //    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, keyName);
                //}
                //Console.WriteLine("Upload 3 completed");

                //// Option 4. Specify advanced settings.
                //var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                //{
                //    BucketName = bucketName,
                //    FilePath = filePath,
                //    StorageClass = S3StorageClass.StandardInfrequentAccess,
                //    PartSize = 6291456, // 6 MB.
                //    Key = keyName,
                //    CannedACL = S3CannedACL.PublicRead
                //};
                //fileTransferUtilityRequest.Metadata.Add("date", "2020Fall");
                //fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                //await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                //Console.WriteLine("Upload 4 completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}