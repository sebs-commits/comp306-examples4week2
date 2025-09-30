using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessDynamoDB
{
    public class DDBOperations
    {
        AmazonDynamoDBClient client;
        BasicAWSCredentials credentials; 
        string tableName = "MyAppsTableCreatedByCode";

        public DDBOperations()
        {
            credentials= new BasicAWSCredentials(
                                 ConfigurationManager.AppSettings["accessId"], 
                                 ConfigurationManager.AppSettings["secretKey"]);

            client = new AmazonDynamoDBClient(credentials,Amazon.RegionEndpoint.USEast2);


    }


        public async Task GetAllTables()
        {
            ListTablesResponse response = await client.ListTablesAsync();

            if (response.TableNames.Count > 0)
                foreach (string name in response.TableNames)
                    Console.WriteLine(name);
        }


        public async Task CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName="Id",
                        AttributeType="N"
                    },
                     new AttributeDefinition
                    {
                        AttributeName="UserName",
                        AttributeType="S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName="Id",
                        KeyType="HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName="UserName",
                        KeyType="RANGE"
                    }
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 2,
                    WriteCapacityUnits = 1
                }
            };

            try
            {
                var response = await client.CreateTableAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) Console.WriteLine("Table created successfully");
                
            }
            catch (InternalServerErrorException iee)
            {
                Console.WriteLine("An error occurred on the server side " + iee.Message);
            }
            catch (LimitExceededException lee)
            {
                Console.WriteLine("you are creating a table with one or more secondary indexes+ " + lee.Message);
            }
        }

        public async Task InsertItem()
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName=tableName,
                Item= new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue{N="1" } },
                    { "UserName", new AttributeValue{S="Yin" } }
                }
            };

            try
            {
                var response = await client.PutItemAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)  Console.WriteLine("Item added successfully!");
            }
            catch (InternalServerErrorException iee)
            { Console.WriteLine("An error occurred on the server side " + iee.Message); }

            catch (ResourceNotFoundException ex)
            { Console.WriteLine("The operation tried to access a nonexistent table or index. "); }
        }

        public async Task GetItem()
        {
            GetItemRequest request = new GetItemRequest
            {
                TableName=tableName,
                Key=new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue{ N= "1"} },
                    { "UserName", new AttributeValue{S="Yin" } }
                }
            };

            try
            {
                var response = await client.GetItemAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (response.Item.Count > 0)
                    {
                        Console.WriteLine("Item(s) retrieved successfully");
                        foreach (var item in response.Item)
                            Console.WriteLine($"Key: {item.Key}, Value: {item.Value.S}{item.Value.N}");
                    }
                }
            }
            catch (InternalServerErrorException iee)
            { Console.WriteLine("An error occurred on the server side " + iee.Message); }

            catch (ResourceNotFoundException ex)
            { Console.WriteLine("The operation tried to access a nonexistent table or index. "); }
        }

        public async Task DeleteItem()
        {
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName=tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue{N="1" } },
                    { "UserName", new AttributeValue{S="Yin" } }
                }
            };

            var response = await client.DeleteItemAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Item deleted successfully");
                await GetItem();
            }
        }

        public async Task DescribeTable()
        {
            DescribeTableRequest request = new DescribeTableRequest { TableName=tableName};

            var response = await client.DescribeTableAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"TableArn: {response.Table.TableArn}");
            }
        }

        public async Task DeleteTable()
        {
            var response = await client.DeleteTableAsync(tableName);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Table has been deleted!");
                Console.WriteLine($"Table status: {response.TableDescription.TableStatus.Value}");
            }
                    
        }

        public async Task BackupTable()
        {
            CreateBackupRequest request = new CreateBackupRequest
            {
                BackupName="BK001",
                TableName=tableName
            };

            var response = await client.CreateBackupAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Backup created successfully");
                Console.WriteLine($"Backup BackupArn: {response.BackupDetails.BackupArn}");
                Console.WriteLine($"Backup BackupCreationDateTime: {response.BackupDetails.BackupCreationDateTime}");
                Console.WriteLine($"Backup BackupStatus: {response.BackupDetails.BackupStatus}");
                Console.WriteLine($"Backup BackupSizeBytes: {response.BackupDetails.BackupSizeBytes}");

            }
        }
    }
}
