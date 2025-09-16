﻿using Amazon;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example4ObjectLevelOps
{

    public static class Helper
    {
        public readonly static IAmazonS3 s3Client;

        static Helper()
        {
            s3Client = GetS3Client();
        }

        private static IAmazonS3 GetS3Client()
        {
            string awsAccessKey = ConfigurationManager.AppSettings["accessId"];
            string awsSecretKey = ConfigurationManager.AppSettings["secretKey"];
            return new AmazonS3Client(awsAccessKey, awsSecretKey, RegionEndpoint.USEast1);
        }
    }
}
