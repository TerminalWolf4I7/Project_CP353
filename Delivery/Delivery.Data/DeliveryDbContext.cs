using Delivery.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Data
{
    /// <summary>
    /// DeliveryDbContext คือศูนย์กลางของ Data Layer
    /// ทำหน้าที่เชื่อมต่อกับฐานข้อมูล PostgreSQL ผ่าน Entity Framework Core (EF Core)
    ///
    /// หน้าที่หลัก:
    /// - map Entity ↔ ตารางใน database
    /// - จัดการ CRUD ผ่าน EF Core
    /// - กำหนด relationship ของข้อมูล
    /// - ควบคุม schema/model ของระบบ
    ///
    /// Architecture:
    /// API Controller
    /// ↓
    /// Service / Business Logic
    /// ↓
    /// DeliveryDbContext
    /// ↓
    /// PostgreSQL Database
    /// </summary>
    public class DeliveryDbContext : DbContext
    {
        // Constructor รับ DbContextOptions จาก DI container
        //
        // options จะถูก config จาก Program.cs
        // เช่น:
        // - Connection String
        // - PostgreSQL provider
        // - Logging
        // - Migration settings
        //
        // Side effects:
        // - ผูก DbContext นี้เข้ากับ database configuration ของระบบ
        public DeliveryDbContext(
            DbContextOptions<DeliveryDbContext> options)
            : base(options)
        {
        }

        // =====================================================
        // DBSETS
        // =====================================================

        // DbSet แต่ละตัวเปรียบเสมือน "ตาราง" ใน database
        //
        // ใช้สำหรับ:
        // - Query ข้อมูล
        // - Insert
        // - Update
        // - Delete
        //
        // EF Core จะ map object ↔ row อัตโนมัติ

        // ตารางผู้ใช้งานทั้งหมดของระบบ
        public DbSet<User> Users => Set<User>();

        // ตารางร้านอาหาร
        public DbSet<Restaurant> Restaurants => Set<Restaurant>();

        // ตาราง Rider
        public DbSet<Rider> Riders => Set<Rider>();

        // ตารางเมนูอาหาร
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();

        // ตารางตะกร้าของลูกค้า
        public DbSet<Cart> Carts => Set<Cart>();

        // ตารางรายการอาหารภายในตะกร้า
        public DbSet<CartItem> CartItems => Set<CartItem>();

        // ตารางออเดอร์
        public DbSet<Order> Orders => Set<Order>();

        // ตารางรายการอาหารภายในออเดอร์
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        /// <summary>
        /// ใช้กำหนด schema และ relationship ของ database ผ่าน Fluent API
        ///
        /// หน้าที่หลัก:
        /// - กำหนด Primary Key
        /// - กำหนด Foreign Key
        /// - กำหนด Relationship
        /// - กำหนด Index
        /// - กำหนด auto increment
        ///
        /// EF Core จะใช้ข้อมูลส่วนนี้สร้าง migration/schema
        /// </summary>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================================================
            // USER ENTITY
            // =====================================================

            // ตาราง Users
            modelBuilder.Entity<User>(entity =>
            {
                // กำหนด Primary Key
                entity.HasKey(e => e.UserId);

                // ใช้ PostgreSQL identity column (auto increment)
                entity.Property(e => e.UserId)
                      .UseIdentityAlwaysColumn();
            });

            // =====================================================
            // RESTAURANT ENTITY
            // =====================================================

            // ตาราง Restaurants
            modelBuilder.Entity<Restaurant>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.RestaurantId);

                // Auto increment id
                entity.Property(e => e.RestaurantId)
                      .UseIdentityAlwaysColumn();

                // กำหนดให้ UserId unique
                //
                // หมายความว่า:
                // user 1 คนมีร้านได้เพียงร้านเดียว
                entity.HasIndex(e => e.UserId)
                      .IsUnique();

                // Relationship:
                // Restaurant → User
                //
                // 1 User สามารถมี Restaurants collection
                // Restaurant แต่ละตัวจะมี User เจ้าของ 1 คน
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Restaurants)
                      .HasForeignKey(e => e.UserId);
            });

            // =====================================================
            // RIDER ENTITY
            // =====================================================

            // ตาราง Riders
            modelBuilder.Entity<Rider>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.RiderId);

                // Auto increment id
                entity.Property(e => e.RiderId)
                      .UseIdentityAlwaysColumn();

                // Rider 1 คนต่อ User 1 คน
                entity.HasIndex(e => e.UserId)
                      .IsUnique();

                // Relationship:
                // Rider → User
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Riders)
                      .HasForeignKey(e => e.UserId);
            });

            // =====================================================
            // MENU ITEM ENTITY
            // =====================================================

            // ตารางเมนูอาหารของร้าน
            modelBuilder.Entity<MenuItem>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.ItemId);

                // Auto increment id
                entity.Property(e => e.ItemId)
                      .UseIdentityAlwaysColumn();

                // Relationship:
                // Restaurant 1 ร้านมีเมนูได้หลายรายการ
                entity.HasOne(e => e.Restaurant)
                      .WithMany(r => r.MenuItems)
                      .HasForeignKey(e => e.RestaurantId);
            });

            // =====================================================
            // CART ENTITY
            // =====================================================

            // ตารางตะกร้าของลูกค้า
            modelBuilder.Entity<Cart>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.CartId);

                // Auto increment id
                entity.Property(e => e.CartId)
                      .UseIdentityAlwaysColumn();

                // Relationship:
                // Cart → User
                //
                // ลูกค้า 1 คนสามารถมีหลาย cart ได้
                // (เช่น หลายร้าน)
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Carts)
                      .HasForeignKey(e => e.UserId);

                // Relationship:
                // Cart → Restaurant
                //
                // Cart นี้เป็นของร้านไหน
                entity.HasOne(e => e.Restaurant)
                      .WithMany(r => r.Carts)
                      .HasForeignKey(e => e.RestaurantId);
            });

            // =====================================================
            // CART ITEM ENTITY
            // =====================================================

            // ตารางรายการอาหารใน cart
            modelBuilder.Entity<CartItem>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.CartItemId);

                // Auto increment id
                entity.Property(e => e.CartItemId)
                      .UseIdentityAlwaysColumn();

                // Relationship:
                // Cart 1 ใบมีหลาย CartItems
                entity.HasOne(e => e.Cart)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(e => e.CartId);

                // Relationship:
                // CartItem อ้างถึง MenuItem
                entity.HasOne(e => e.MenuItem)
                      .WithMany(m => m.CartItems)
                      .HasForeignKey(e => e.ItemId);
            });

            // =====================================================
            // ORDER ENTITY
            // =====================================================

            // ตาราง Orders
            modelBuilder.Entity<Order>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.OrderId);

                // Auto increment id
                entity.Property(e => e.OrderId)
                      .UseIdentityAlwaysColumn();

                // Relationship:
                // Order → User
                //
                // ลูกค้าที่สร้างออเดอร์
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(e => e.UserId);

                // Relationship:
                // Order → Restaurant
                //
                // ร้านที่รับ order นี้
                entity.HasOne(e => e.Restaurant)
                      .WithMany(r => r.Orders)
                      .HasForeignKey(e => e.RestaurantId);

                // Relationship:
                // Order → Rider
                //
                // Rider ที่รับงานนี้
                //
                // RiderId เป็น nullable ได้
                // เพราะช่วงแรก order อาจยังไม่มี Rider รับ
                entity.HasOne(e => e.Rider)
                      .WithMany(r => r.Orders)
                      .HasForeignKey(e => e.RiderId);
            });

            // =====================================================
            // ORDER ITEM ENTITY
            // =====================================================

            // ตารางรายการอาหารภายใน order
            modelBuilder.Entity<OrderItem>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.OrderItemId);

                // Auto increment id
                entity.Property(e => e.OrderItemId)
                      .UseIdentityAlwaysColumn();

                // Relationship:
                // Order 1 ใบมีหลาย OrderItems
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(e => e.OrderId);

                // Relationship:
                // OrderItem อ้างถึง MenuItem
                entity.HasOne(e => e.MenuItem)
                      .WithMany(m => m.OrderItems)
                      .HasForeignKey(e => e.ItemId);
            });
        }
    }
}
