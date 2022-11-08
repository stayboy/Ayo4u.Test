﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFA.Medicals.Web.Ui.Shared.Models.Sales
{
    public class SalesImportDTO
    {
        public string Region { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
        public string StoreId { get; set; }
        public string TransactionHour { get; set; }
        public string TransactionDate { get; set; }
        public int TransactionId { get; set; }
        public string ProductGroup { get; set; }
        public string Product { get; set; }
        public double Amount { get; set; }
        public int PromotionId { get; set; }
        public int CustomerInfo { get; set; }
        public string PaymentType { get; set; }

    }
}
