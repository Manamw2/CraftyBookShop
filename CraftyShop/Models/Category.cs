using System.ComponentModel.DataAnnotations;

namespace CraftyShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [Range(1, 100)]
        public int DisplayOrder { get; set; }
    }
}
