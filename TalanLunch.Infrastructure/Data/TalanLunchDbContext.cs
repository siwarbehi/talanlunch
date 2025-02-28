using Microsoft.EntityFrameworkCore;
using TalanLunch.core.Domain.Entities;
using TalanLunch.Core.Domain.Entities;

namespace TalanLunch.Core.Domain
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
            // Configuring the relationship between User and Order (One-to-many
            modelBuilder.Entity<Order>()
           .HasOne(o => o.User)
           .WithMany(u => u.Orders)
           .HasForeignKey(o => o.UserId)
           .OnDelete(DeleteBehavior.Cascade);
            // Configuring the join table OrderDish (Many-to-many relationship between Order and Dish)
            modelBuilder.Entity<OrderDish>()
                .HasKey(od => new { od.OrderId, od.DishId });

            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDishes)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Dish)
                .WithMany(d => d.OrderDishes)
                .HasForeignKey(od => od.DishId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configuring the join table MenuDish (Many-to-many relationship between Menu and Dish)
            modelBuilder.Entity<MenuDish>()
            .HasKey(md => new { md.MenuId, md.DishId });

            modelBuilder.Entity<MenuDish>()
                .HasOne(md => md.Menu)
                .WithMany(m => m.MenuDishes)
                .HasForeignKey(md => md.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuDish>()
                .HasOne(md => md.Dish)
                .WithMany(d => d.MenuDishes)
                .HasForeignKey(md => md.DishId)
                .OnDelete(DeleteBehavior.Cascade);





        }
    }
}
