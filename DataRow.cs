using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    class DataRow
    {
        public string Date;
        public string FromName;
        public string ToName;
        public string Narrative;
        public double MoneyOwed;

        public DataRow(string Date, string FromName, string ToName, string Narrative, string MoneyOwed)
        {
            //'this' is an instance of the class that we are in (so this calls the Date class above not the argument)
            this.Date = Date;
            this.FromName = FromName;
            this.ToName = ToName;
            this.Narrative = Narrative;
            this.MoneyOwed = Convert.ToDouble(MoneyOwed);
        }
    }
}
