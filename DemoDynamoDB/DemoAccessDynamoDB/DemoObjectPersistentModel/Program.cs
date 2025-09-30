using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoObjectPersistentModel
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DDBOperation operation = new DDBOperation();
            await operation.CRUDOperations();

            Console.WriteLine("All operations have been finished sucessfully!");
            Console.ReadKey();
        }
    }
}
