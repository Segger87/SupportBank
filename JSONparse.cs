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


//have a file type method that accepts a user file location - if it is a json file run the json method if it is a csv file run the csv method