
using Examples4BucketOps;



BucketOps op = new BucketOps();
op.GetBucketList();

Console.WriteLine("delete a bucket");

op.CreateBucket("bucketcreatedprogrammatically");
Helper.s3Client.DeleteBucketAsync("bucketcreatedprogrammatically");

Console.WriteLine();

op.GetBucketList();
Console.ReadLine();