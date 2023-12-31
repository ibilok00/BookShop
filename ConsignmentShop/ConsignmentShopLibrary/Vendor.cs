﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsignmentShopLibrary
{
    public class Vendor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Commission { get; set; }
        public decimal PaymentDue { get; set; }

        public Vendor()
        {
            Commission = .5;
        }
        public string Display
        {
            get
            {
                return string.Format("{0} {1} - {2}", FirstName, LastName, 
                    string.Format(new CultureInfo("en-US"), "{0:C}", PaymentDue));
            }
        }
    }
}
