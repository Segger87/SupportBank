using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SupportBank
{
    class JSONparse
    {
        public static List<Transaction> LoadJSON(string filePath)
        {
            List<Transaction> transactions = new List<Transaction>();
            using (StreamReader reader = new StreamReader(@filePath))
            {
                return (List<Transaction>)new JsonSerializer().Deserialize(reader, typeof(List<Transaction>));
            }
        }
    }
}
