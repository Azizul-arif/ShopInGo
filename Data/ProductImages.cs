using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Data
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string?ImagePath { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        [ForeignKey("Product")]
       public int ProductId { get; set; }
        public Product?Product { get; set; }

    }
}
