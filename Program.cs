using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().LoadCsv();   
        }

        public void LoadCsv() 
        {
            using (var reader = new StreamReader(@"C:\Work\Training\SupportBank\Transactions2014.csv"))
            {
                List<DataRow> dataRows = new List<DataRow>();

                var i = true;

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');
             
                    if (i)
                    {
                        i = false;
                        continue;
                    }
                    var currentRow = new DataRow(values[0], values[1], values[2], values[3], values[4]);
                    dataRows.Add(currentRow);
                }
                Console.WriteLine("Type 'List All' to list all transactions");
                string userList = Console.ReadLine().ToLower();

                Dictionary<string, double> outPutOfDictionary;

                if(userList == "list all")
                {
                    outPutOfDictionary = ListAll(dataRows);
                    PrintResult(outPutOfDictionary);
                }  
                Console.ReadLine();
            }
        }

        public Dictionary<string, double> ListAll(List<DataRow> dataRow)
        {
            var myDictionary = new Dictionary<string, double>();

            foreach (var data in dataRow)
            {
                if(myDictionary.ContainsKey(data.FromName))
                {
                    myDictionary[data.FromName] += data.MoneyOwed;
                } else
                {
                    myDictionary.Add(data.FromName, data.MoneyOwed);
                }
                if (myDictionary.ContainsKey(data.ToName))
                {
                    myDictionary[data.ToName] -= data.MoneyOwed;
                }
                else
                {
                    myDictionary.Add(data.ToName, -data.MoneyOwed);
                }
            }
            return myDictionary;
        }

        public void PrintResult(Dictionary<string, double> outPutOfDictionary)
        {
            foreach(var key in outPutOfDictionary.Keys)
            {
                Console.WriteLine(key);
                Console.WriteLine(outPutOfDictionary[key]);
            }
        }
    }
}

