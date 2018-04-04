using NLog;
using NLog.Config;
using NLog.Targets;
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
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Training\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;

            new Program().LoadCsv();   
        }

        public void LoadCsv() 
        {
            using (var reader = new StreamReader(@"C:\Work\Training\SupportBank\DodgyTransactions2015.csv"))
            {
                List<Transaction> transactions = new List<Transaction>();

                var i = true;

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');
             
                    if (i)
                    {
                        i = false;
                        continue; //continue breaks 
                    }
                    var currentRow = Transaction.CreateIfPossible(values[0], values[1], values[2], values[3], values[4]);
                    if(currentRow != null)
                    {
                       transactions.Add(currentRow);
                    }
                }
                Console.WriteLine("Type 'List All' to list all transactions");
                string nameInputByUser = Console.ReadLine().ToLower();

                if(nameInputByUser == "list all")
                {
                    var outPutOfDictionary = ListAll(transactions);
                    PrintResult(outPutOfDictionary);
                }
                else
                {
                    var outputListAccount = ListAccount(transactions, nameInputByUser);
                    PrintAccount(outputListAccount);
                }
                Console.ReadLine();
            }
        }

        public Dictionary<string, double> ListAll(List<Transaction> dataRow)
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

        public List<Transaction> ListAccount(List<Transaction> allTransactions, string userName)
        {
            //uses LINQ which provides the where method to filter a list based on a condition passed to it - expressed using an anonymous function.
            var filtered = allTransactions.Where(transaction => transaction.FromName == userName || transaction.ToName == userName).ToList();
            if(!filtered.Any())
            {
                Console.WriteLine("Sorry " + userName + " is not recognised");
            }
            return filtered;
        }

        public void PrintResult(Dictionary<string, double> outPutOfDictionary)
        {
            foreach(var key in outPutOfDictionary.Keys)
            {
                Console.WriteLine(key);
                Console.WriteLine(outPutOfDictionary[key]);
            }
        }

        public void PrintAccount(List<Transaction> transactionsForAccount)
        {
            foreach(var transaction in transactionsForAccount)
            {
                Console.WriteLine(transaction.ToString());
            }
        }
    }
}

