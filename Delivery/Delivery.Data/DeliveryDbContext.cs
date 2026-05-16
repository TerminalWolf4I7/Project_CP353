using Delivery.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Data
{
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
        public DbSet<Rider> Riders => Set<Rider>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // users
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).UseIdentityAlwaysColumn();
            });

            // restaurants
            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasKey(e => e.RestaurantId);
                entity.Property(e => e.RestaurantId).UseIdentityAlwaysColumn();
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Restaurants)
                      .HasForeignKey(e => e.UserId);
            });

            // riders
            modelBuilder.Entity<Rider>(entity =>
            {
                entity.HasKey(e => e.RiderId);
                entity.Property(e => e.RiderId).UseIdentityAlwaysColumn();
                entity.HasIndex(e => e.UserId).IsUnique();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Riders)
                      .HasForeignKey(e => e.UserId);
            });

            // menu_items
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.ItemId);
                entity.Property(e => e.ItemId).UseIdentityAlwaysColumn();
                entity.HasOne(e => e.Restaurant)
                      .WithMany(r => r.MenuItems)
                      .HasForeignKey(e => e.RestaurantId);
            });

            // carts
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.CartId);
                entity.Property(e => e.CartId).UseIdentityAlwaysColumn();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Carts)
                      .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Restaurant)
                      .WithMany(r => r.Carts)
                      .HasForeignKey(e => e.RestaurantId);
            });

            // cart_items
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.CartItemId);
                entity.Property(e => e.CartItemId).UseIdentityAlwaysColumn();
                entity.HasOne(e => e.Cart)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(e => e.CartId);
                entity.HasOne(e => e.MenuItem)
                      .WithMany(m => m.CartItems)
                      .HasForeignKey(e => e.ItemId);
            });

            // orders
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).UseIdentityAlwaysColumn();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Restaurant)
                      .WithMany(r => r.Orders)
                      .HasForeignKey(e => e.RestaurantId);
                entity.HasOne(e => e.Rider)
                      .WithMany(r => r.Orders)
                      .HasForeignKey(e => e.RiderId);
            });

            // order_items
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.OrderItemId).UseIdentityAlwaysColumn();
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.OrderId);
                entity.HasOne(e => e.MenuItem)
                      .WithMany(m => m.OrderItems)
                      .HasForeignKey(e => e.ItemId);
            });
        }
    }
}
