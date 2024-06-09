using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories {  get; set; }
        public DbSet<Color>Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductImages>ProductImages { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? PicPath { get; set; }
        [NotMapped]
        public IFormFile? ProfilePic { get; set; }
    }
}
