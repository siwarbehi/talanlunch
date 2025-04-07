using Microsoft.EntityFrameworkCore;
using TalanLunch.Domain.Entities;
using TalanLunch.Domain.Enums;

namespace TalanLunch.Infrastructure.Data
{
    public class TalanLunchDbContext : DbContext
    {
        public TalanLunchDbContext(DbContextOptions<TalanLunchDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishRating> DishRatings { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuDish> MenuDishes { get; set; }
        public DbSet<OrderDish> OrderDishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the relationship between User and DishRating
            modelBuilder.Entity<DishRating>()
                .HasKey(dr => dr.RatingId);

            // One-to-many relationship: A User can have many DishRatings
            modelBuilder.Entity<DishRating>()
                .HasOne(dr => dr.User)
                .WithMany(u => u.DishRatings)
                .HasForeignKey(dr => dr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the relationship between User and Order (One-to-many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the join table OrderDish (Many-to-many relationship between Order and Dish)
            modelBuilder.Entity<OrderDish>()
                .HasKey(od => new { od.OrderId, od.DishId });

            // Clé composite pour la table de jointure MenuDish
            modelBuilder.Entity<MenuDish>()
                .HasKey(md => new { md.MenuId, md.DishId });

            // Relation Menu ↔ MenuDish
            modelBuilder.Entity<MenuDish>()
                .HasOne(md => md.Menu)
                .WithMany(m => m.MenuDishes)
                .HasForeignKey(md => md.MenuId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<MenuDish>()
                .HasOne(md => md.Dish)
                .WithMany(d => d.MenuDishes)
                .HasForeignKey(md => md.DishId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<MenuDish>()
                .HasIndex(md => new { md.MenuId, md.DishId })
                .IsUnique();



            // Ensure that UserRole is stored as a string in the database (instead of an integer)
            modelBuilder.Entity<User>()
                .Property(u => u.UserRole)
                .HasConversion(
                    v => v.ToString(),  // Convert enum to string
                    v => (UserRole)Enum.Parse(typeof(UserRole), v) // Convert string back to enum
                );
        }
    }
}
