using System.ComponentModel.DataAnnotations;

namespace ECommerce.Data
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25)]
        public string? CategoryName { get; set; }
        public int ParentID { get; set; }
    }
}
