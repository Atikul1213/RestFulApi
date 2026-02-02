namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class CustomerProfile
    {
        public int CustomerId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int LoyaltyPoints { get; set; }
        public int TotalOrdersPlaced { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? RegisteredAt { get; set; }
        public Customer Customer { get; set; } = null!;
    }
}
