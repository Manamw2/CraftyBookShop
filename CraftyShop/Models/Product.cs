using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftyShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [StringLength(30)]
        public string ISBN { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Author { get; set; } = string.Empty;
        [Required]
        [DisplayName("List Price")]
        public double ListPrice { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        [DisplayName("Price For 10+")]
        public double Price10 { get; set; }        
        [Required]
        [Range(0, double.MaxValue)]
        [DisplayName("Price For 20+")]
        public double Price20 { get; set; }
        [DisplayName("Category")]
        public int CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        public Category? Category { get; set; }
        [ValidateNever]
        [DisplayName("Image")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
