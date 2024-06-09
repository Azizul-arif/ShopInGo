using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Priced { get; set; }

        [ForeignKey("Category")]
        public int catID {  get; set; }
        public Category?Category { get; set; }

        public ICollection<ProductImages>?ProductImages { get; set; }

    }
}
