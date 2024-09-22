using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models
{
	public class OrderHeader
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "User ID is required.")]
		[MaxLength(450, ErrorMessage = "User ID cannot exceed 450 characters.")]
		public string? ApplicationUserId { get; set; }

		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser? ApplicationUser { get; set; }

		[Required(ErrorMessage = "Order date is required.")]
		public DateTime OrderDate { get; set; }

		[Required(ErrorMessage = "Shipping date is required.")]
		public DateTime ShippingDate { get; set; }

		[Required(ErrorMessage = "Order total is required.")]
		public decimal? OrderTotal { get; set; }

		public string? OrderStatus { get; set; }
		public string? PaymentStatus { get; set; }

		[Required(ErrorMessage = "Payment Type is required.")]
		[MaxLength(45, ErrorMessage = "Payment Type cannot exceed 45 characters.")]
		public string? PaymentType { get; set; }

		public string? TrackingNumber { get; set; }
		public string? Carrier { get; set; }

		public DateTime PaymentDate { get; set; }
		public DateOnly PaymentDueDate { get; set; }

		public string? PaymentIntentId { get; set; }

		[Required(ErrorMessage = "Phone number is required.")]
		[MaxLength(10, ErrorMessage = "Phone number cannot exceed 10 characters.")]
		public string? PhoneNumber { get; set; }

		[Required(ErrorMessage = "Street address is required.")]
		[MaxLength(40, ErrorMessage = "Street address cannot exceed 40 characters.")]
		public string? StreetAddress { get; set; }

		[Required(ErrorMessage = "City is required.")]
		[MaxLength(40, ErrorMessage = "City cannot exceed 40 characters.")]
		public string? City { get; set; }

		[Required(ErrorMessage = "State is required.")]
		[MaxLength(40, ErrorMessage = "State cannot exceed 40 characters.")]
		public string? State { get; set; }

		[Required(ErrorMessage = "Postal code is required.")]
		[MaxLength(40, ErrorMessage = "Postal code cannot exceed 40 characters.")]
		public string? PostalCode { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(40, ErrorMessage = "Name cannot exceed 40 characters.")]
		public string? Name { get; set; }
	}

}
