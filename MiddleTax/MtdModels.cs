using System;
using System.ComponentModel.DataAnnotations;

namespace MiddleTax
{
    public class Obligation
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Due { get; set; }
        public string Status { get; set; }
        public string PeriodKey { get; set; }
        public string? Received { get; set; }
    }
    public class GetObligation
    {
        [Required]
        [Display(Name = "vrn")]
        [StringLength(9, ErrorMessage = "VAT registration number, is a 9 digit number.")]
        public string Vrn {get; set;}
        [Display(Name = "from")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime From {get; set;}
        [Display(Name = "from")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime to {get; set;}
        [Display(Name = "status")]
        [StringLength(1, ErrorMessage = "Obligation status to return O= Open, F= Fulfilled. Omit status to retrieve all obligations.")]
        public string Status {get; set;}
    }
    public class Returns
    {
        [Required]
        [StringLength(4, ErrorMessage = "Max and min length is 4, The ID code the obligation belongs to, is alpha numeric and occasionally the format includes #")]
        public string periodKey { get; set; }
        [Required]
        public double vatDueSales { get; set
            {
                value = Math.Round(value, 2);
            } 
        }
        [Required]
        [Display(Name = "VAT Due Acquisitions")]
        public double vatDueAcquisitions { get; set; }
        [Required]
        [Display(Name = "Total VAT due")]
        public double totalVatDue { get; set; }
        [Required]
        [Display(Name = "VAT Reclaimed Current Period")]
        public double vatReclaimedCurrPeriod { get; set; }
        [Required]
        [Display(Name = "Net VAT due")]
        public double netVatDue { get; set; }
        [Required]
        [Display(Name = "Total Value Sales Ex. VAT")]
        public int totalValueSalesExVAT { get; set; }
        [Required]
        [Display(Name = "Total Value Purchases Ex. VAT")]
        public int totalValuePurchasesExVAT { get; set; }
        [Required]
        [Display(Name = "Total Value Foods Supplied Ex. VAT")]
        public int totalValueGoodsSuppliedExVAT { get; set; }
        [Required]
        [Display(Name = "total Acquisitions Ex. VAT")]
        public int totalAcquisitionsExVAT
        {
            get; set
          {
                value = Math.Round(value, 2, MidpointRounding.ToZero);
        } }
        [Required]
        [Display(Name = "Finalised")]
        public bool finalised { get; set; }
    }
    public class ReturnReturns
    {
        public string periodKey { get; set; }
        public double vatDueSales { get; set; }
        public double vatDueAcquisitions { get; set; }
        public double totalVatDue { get; set; }
        public double vatReclaimedCurrPeriod { get; set; }
        public double netVatDue { get; set; }
        public int totalValueSalesExVAT { get; set; }
        public int totalValuePurchasesExVAT { get; set; }
        public int totalValueGoodsSuppliedExVAT { get; set; }
        public int totalAcquisitionsExVAT { get; set; }
        public bool finalised { get; set; }
    }
}
