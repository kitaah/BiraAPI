using System.ComponentModel.DataAnnotations.Schema;

namespace BiraAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }

    }
}
