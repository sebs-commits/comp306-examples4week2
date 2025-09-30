using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace DemoObjectPersistentModel
{
    public class DDBOperation
    {
        AmazonDynamoDBClient client;
        DynamoDBContext context;
        Amazon.Runtime.BasicAWSCredentials credentials;

        public DDBOperation()
        {
            credentials = new Amazon.Runtime.BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast2);
            context = new DynamoDBContext(client);
        }

        public async Task CRUDOperations()
        {
            Student student = new Student
            {
                Id = "999",
                FirstName = "Cindy",
                LastName = "Patel"
            };

            // insert the student object to DynamoDB Table Student
            await context.SaveAsync(student);

            // Retrieve the student.
            Student studentRetrieved = await context.LoadAsync<Student>(student.Id);

            // Update few properties.
            studentRetrieved.FirstName = "updated firstname";
            await context.SaveAsync(studentRetrieved);

            // Retrieve the updated person. This time add the optional ConsistentRead parameter using DynamoDBContextConfig object.
            Student updatedStudent = await context.LoadAsync<Student>(student.Id, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });

           await context.DeleteAsync<Student>("300111222");
            // Try to retrieve deleted student. It should return null.
            Student deletedStudent = await context.LoadAsync<Student>("999", new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            if (deletedStudent == null)
                Console.WriteLine("Student has been deleted");

            //ProductCatalog table
            Book myBook = new Book
            {
                ISBN = "999-000001",
                Title = "AWS Certified Developer Guide: architecture  ",
                BookAuthors = new List<string> { "Tong Kim", "Cindy Smith" },
                CoverPage = "The cover page"
            };
            await context.SaveAsync(myBook);
        }
    }
}
