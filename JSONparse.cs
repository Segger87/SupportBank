using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    class JSONparse
    {
        private string date;
        private string fromAccount;
        private string toAccount;
        private string narrative;
        private double amount;

        public string Date { get => date; set => date = value; }
        public string FromAccount { get => fromAccount; set => fromAccount = value; }
        public string ToAccount { get => toAccount; set => toAccount = value; }
        public string Narrative { get => narrative; set => narrative = value; }
        public double Amount { get => amount; set => amount = value; }

        public static void DeserializeJSON()
        {
            using (StreamReader r = new StreamReader(@"C:\Work\Training\SupportBank\Transactions2013.json"))
            {
                string json = r.ReadToEnd();
                List<JSONparse> items = JsonConvert.DeserializeObject<List<JSONparse>>(json);

                dynamic array = JsonConvert.DeserializeObject(json);
                foreach (var item in array)
                {
                    Console.WriteLine("{0} {1} {2} {3} {4}", item.Date, item.FromAccount, item.ToAccount, item.Narrative, item.Amount);
                }
                Console.ReadLine();
            }
        }
    }
}
