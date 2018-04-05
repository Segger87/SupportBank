using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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

        public void ExportXML()
        {
            List<Transaction> transactions = new List<Transaction>();

            XmlSerializer ser = new XmlSerializer(typeof(List<Transaction>));
            // write
            using (var stream = File.Create(@"C:\Work\Training\SupportBank\ExportTest.xml"))
            {
               ser.Serialize(stream, transactions); // your instance
            }
        }
    }
}
