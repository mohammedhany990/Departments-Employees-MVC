
using System.ComponentModel.DataAnnotations;
using System;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Maximum lenth of name 50")]
        [MinLength(3, ErrorMessage = "Minimum lenth of name 3")]
        public string Name { get; set; }

        [Range(22, 60, ErrorMessage = "Age must be between 22 and 60")]
        public int? age { get; set; }

        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,20}-[a-zA-Z]{4,20}-[a-zA-Z]{4,20}$",
            ErrorMessage = "Address must be in format '123-Street-City-Country'")]
        public string Address { get; set; }

        [Range(4000, 10000, ErrorMessage = "Salary must be in range 4000 : 1000")]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress(ErrorMessage = "Enter the email in the correct format")]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public string ImageName { get; set; }

        public IFormFile Image { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
