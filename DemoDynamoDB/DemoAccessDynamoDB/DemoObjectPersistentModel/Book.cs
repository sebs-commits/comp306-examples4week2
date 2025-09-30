﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoObjectPersistentModel
{
    [DynamoDBTable("ProductCatalog")]
    public class Book
    {
        [DynamoDBHashKey]
        public string ISBN { get; set; }

        public string Title { get; set; }
        [DynamoDBProperty("Authors")]
        public List<string> BookAuthors { get; set; }

        [DynamoDBIgnore]
        public string CoverPage { get; set; }
    }
}
