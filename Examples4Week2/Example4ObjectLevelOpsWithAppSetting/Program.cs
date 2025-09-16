using Amazon.S3.Transfer;

namespace Example4ObjectLevelOpsWithAppSetting
{
    internal class Program
    {
        private const string bucketName = "flowers4li";
        private const string filePath = @"c:\temp\Helper.cs";
        private const string keyName = "uploadedbyapplication";
        private const string fileName = "uploadedbyapp";
        static async Task Main(string[] args)
        {
            await DownloadFile(bucketName, fileName);
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
    }
}