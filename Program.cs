using Newtonsoft.Json;
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
        public List<Transaction> transactions = new List<Transaction>();
        static void Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Training\SupportBank.log", Layout = @"${longdate} ${level} Located at: ${callsite} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;

            new Program().runProgram();
        }

        public void runProgram()
        {
            Console.WriteLine("Enter your file type: ");
            string filePath = Console.ReadLine();

            string extension = Path.GetExtension(filePath);

            switch (extension)
            {
                case ".csv":
                    transactions = LoadCsv(filePath);
                    UserInput();
                    break;
                case ".json":
                    transactions = JSONparse.LoadJSON(filePath);
                    UserInput();
                    break;
            }
        }

        public List<Transaction> LoadCsv(string filePath)
        {
            var transactions = new List<Transaction>();
            using (var reader = new StreamReader(@filePath))
            {
                var skipTheFirstRow = true;

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');

                    if (skipTheFirstRow)
                    {
                        skipTheFirstRow = false;
                        continue; //continue breaks the loop and executes the while loop again
                    }

                    var currentRow = Transaction.CreateIfPossible(values[0], values[1], values[2], values[3], values[4]);

                    if (currentRow != null)
                    {
                        transactions.Add(currentRow);
                    }
                }
            }
            return transactions;
        }

        public void UserInput()
        {
            Console.WriteLine("Search for a user or type 'List All' to list all transactions");
            string nameInputByUser = Console.ReadLine().ToLower();

            if (nameInputByUser == "list all")
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

        public Dictionary<string, double> ListAll(List<Transaction> dataRow)
        {
            var myDictionary = new Dictionary<string, double>();

            foreach (var data in dataRow)
            {
                //collates all the key values to one line (i.e all users called 'Sam' are collated and their money Owed is added up.
                if (myDictionary.ContainsKey(data.FromAccount))
                {
                    myDictionary[data.FromAccount] += data.Amount;
                }
                else
                {
                    myDictionary.Add(data.FromAccount, data.Amount);
                }
                if (myDictionary.ContainsKey(data.ToAccount))
                {
                    myDictionary[data.ToAccount] -= data.Amount;
                }
                else
                {
                    myDictionary.Add(data.ToAccount, -data.Amount);
                }
            }
            return myDictionary;
        }

        public List<Transaction> ListAccount(List<Transaction> allTransactions, string userName)
        {
            //uses LINQ which provides the where method to filter a list based on a condition passed to it - expressed using an anonymous function.
            var filtered = allTransactions.Where(transaction => transaction.FromAccount == userName || transaction.ToAccount == userName).ToList();
            if (!filtered.Any())
            {
                Console.WriteLine("Sorry " + userName + " is not recognised");
            }
            return filtered;
        }

        public void PrintResult(Dictionary<string, double> outPutOfDictionary)
        {
            foreach (var key in outPutOfDictionary.Keys)
            {
                Console.WriteLine(key);
                Console.WriteLine(outPutOfDictionary[key]);
            }
        }

        public void PrintAccount(List<Transaction> transactionsForAccount)
        {
            foreach (var transaction in transactionsForAccount)
            {
                Console.WriteLine(transaction.ToString());
            }
        }
    }
}

