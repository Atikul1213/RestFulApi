﻿using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.EcommerceModel
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Product Category is required")]
        public string Category { get; set; }
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000")]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be a negative value")]
        public int Stock { get; set; }
    }
}
