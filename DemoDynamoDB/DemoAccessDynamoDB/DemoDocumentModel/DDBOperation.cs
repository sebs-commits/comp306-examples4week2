using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDocumentModel
{
    public class DDBOperation
    {
        AmazonDynamoDBClient client;
       // Amazon.DynamoDBv2.DataModel.DynamoDBContext context;
        Amazon.Runtime.BasicAWSCredentials credentials;
        Table userTable;

        public DDBOperation(string tableName)
        {
            credentials = new Amazon.Runtime.BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            userTable = Table.LoadTable(client, tableName, DynamoDBEntryConversion.V2); //load the metadata of the table
        }

        public async Task InsertAsync()
        {  
            Document newUser = new Document();
            newUser["Id"] = "demo4Sec001@gmail.com";
            newUser["Name"] = "Yin Li";
            newUser["Address"] = "941 Progress Ave, Scarborough, Ontario, Canada";
            newUser["Active"] = true;
            newUser["Interests"] = new List<String> { "Yoga", "Running", "Golfing", "Playing Piano"};

            Document skills = new Document();
            skills["C#"] = 10;
            skills["Java"] = 12;
            skills["Python"] = 4;
            newUser["Skills"] = skills;

            await userTable.PutItemAsync(newUser);
        }

        public async Task LoadItemAsync(string key)
        {
            Document loadedUser = await userTable.GetItemAsync(key);

            Console.WriteLine(loadedUser.ToJsonPretty());
        }

        public async Task DeleteItemAsyc(string Key)
        {
            await userTable.DeleteItemAsync(Key);
        }

        public async Task DeleteItemAsyc(Document doc)
        {
            await userTable.DeleteItemAsync(doc);
        }
    }
}
