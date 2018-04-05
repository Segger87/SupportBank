using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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
                case ".xml":
                    transactions = XMLparse.LoadXML(filePath);
                    UserInput();
                    break;
                default:
                    Console.WriteLine("Sorry " + filePath + " is not a valid file path");
                    runProgram();
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
                var exportMyDamnUsers = PrintResult(outPutOfDictionary);
                ExportFile(exportMyDamnUsers);
            }
            else
            {
                var outputListAccount = ListAccount(transactions, nameInputByUser);
                var exportMyDamnFile = PrintAccount(outputListAccount);
                ExportFile(exportMyDamnFile);
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
                runProgram();
            }
            return filtered;
        }

        public List<Transaction> PrintResult(Dictionary<string, double> outPutOfDictionary)
        {
            var list = new List<Transaction>();
            foreach (var key in outPutOfDictionary.Keys)
            {
                Console.WriteLine(key);
                Console.WriteLine(outPutOfDictionary[key]);
            }
            return list;
        }

        public List<Transaction> PrintAccount(List<Transaction> transactionsForAccount)
        {
            var list = new List<Transaction>();
            foreach (var transaction in transactionsForAccount)
            {
                Console.WriteLine(transaction.ToString());
                list.Add(transaction);
            }
            return list;
        }

        public void ExportFile(List<Transaction> outputOfPrintAccount)
        {
            var sBuilder = new StringBuilder();

            Console.WriteLine("What file type would you like to export to? (.csv/.json/.xml)");
            string fileType = Console.ReadLine();

            sBuilder.AppendLine("Date, Name From, Name To, Narrative, Amount");

            foreach (var output in outputOfPrintAccount)
            {
                sBuilder.AppendLine(output.ToCSVOutput());
            }
            switch (fileType)
            {
                case ".json":
                    var jArray = GenerateJSON(outputOfPrintAccount);
                    File.WriteAllText(@"C:\Work\Training\SupportBank\ExportTest.json", jArray.ToString());
                    Console.WriteLine("File has been created");
                    break;
                case ".csv":
                    File.WriteAllText(@"C:\Work\Training\SupportBank\ExportTest.csv", sBuilder.ToString());
                    Console.WriteLine("File has been created");
                    break;
                case ".xml":
                    var xml = XMLdata(outputOfPrintAccount);
                    File.WriteAllText(@"C:\Work\Training\SupportBank\ExportTest.xml", xml.ToString());
                    break;
                default:
                    Console.WriteLine("Sorry, " + fileType + " is not an acceptable format");
                    UserInput();
                    break;
            }
        }

        private JArray GenerateJSON(List<Transaction> transactions)
        {
            return new JArray(
                transactions.Select(
                    transaction => new JObject
                    {
                        { "Date", transaction.Date },
                        { "FromAccount", transaction.FromAccount },
                        { "ToAccount", transaction.ToAccount },
                        { "Narrative", transaction.Narrative },
                        { "Amount", transaction.Amount }
                    }
                )
            );
        }
        private XElement XMLdata(List<Transaction> transactions)
        {
            var xml = new XElement("TransactionList", 
                transactions.Select(transaction => new XElement
                    ("TransactionList",
                    new XAttribute("Amount", transaction.Amount),
                    new XAttribute("Narrative", transaction.Narrative),
                    new XAttribute("ToAccount", transaction.ToAccount),
                    new XAttribute("FromAccount", transaction.FromAccount),
                    new XAttribute("Date", transaction.Date)
                    )));
            return xml;
        }
    }
}
