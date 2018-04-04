﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportBank
{
    class Transaction
    {
        public string Date;
        public string FromName;
        public string ToName;
        public string Narrative;
        public double MoneyOwed;

        public Transaction(string Date, string FromName, string ToName, string Narrative, string MoneyOwed)
        {
            //'this' is an instance of the class that we are in (so this calls the Date class above not the argument)
            this.Date = Date;
            this.FromName = FromName.ToLower();
            this.ToName = ToName.ToLower();
            this.Narrative = Narrative;
            this.MoneyOwed = Convert.ToDouble(MoneyOwed);
        }

        public override string ToString()
        {
            //override will over write the original objects method which is defined in the 'Object' class
            return Date + " " + FromName + " " + ToName + " " + Narrative + " " + MoneyOwed;
        }
    }
}