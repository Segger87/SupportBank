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
            LoadCsv();

        }

        public static void LoadCsv() 
        {
           

            using (var reader = new StreamReader(@"C:\Work\Training\SupportBank\Transactions2014.csv"))
            {
                List<DataRows> dataRow = new List<DataRows>();

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');
                  
                    DataRows myRows = new DataRows(values[0], values[1], values[2], values[3], values[4]);

                    dataRow.Add(myRows);

                    Console.WriteLine();
                }

                foreach (var data in dataRow)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4}",data.Date, data.FromName, data.ToName, data.Narrative, data.MoneyOwed);
                }
                Console.ReadLine();
            }
        }
    }
}
