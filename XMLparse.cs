using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SupportBank
{
    class XMLparse
    {
        public static List<Transaction> LoadXML(string filePath)
        {
            List<Transaction> transactions = new List<Transaction>();
 
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            foreach (XmlNode node in doc.DocumentElement)
            {
                var Date = node.Attributes[0].InnerText;

                foreach(XmlNode child in node.ChildNodes)
                {
                    var Narrative = node.ChildNodes[0].InnerText;
                    var MoneyOwed = node.ChildNodes[1].InnerText;
                    var FromAccount = node.ChildNodes[2].ChildNodes[0].InnerText;
                    var ToAccount = node.ChildNodes[2].ChildNodes[1].InnerText;

                    var CreateIfPossible = Transaction.CreateIfPossible(Date, FromAccount, ToAccount, Narrative, MoneyOwed);
                    transactions.Add(CreateIfPossible);
                }
            }
            return transactions;
        }
    }
}
