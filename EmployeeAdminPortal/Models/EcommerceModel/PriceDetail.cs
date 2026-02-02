namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class PriceDetail
    {
        public decimal BasePrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FinalPrice { get; set; }
        public string Currency { get; set; } = "TK";
    }
}
