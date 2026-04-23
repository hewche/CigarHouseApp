using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CigarHouseApp.Models;

public partial class CigarhouseContext : DbContext
{
    public CigarhouseContext()
    {
    }

    public CigarhouseContext(DbContextOptions<CigarhouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accessory> Accessories { get; set; }

    public virtual DbSet<AccessoryInfo> AccessoryInfos { get; set; }

    public virtual DbSet<BestProduct> BestProducts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<BrandProduct> BrandProducts { get; set; }

    public virtual DbSet<Cigar> Cigars { get; set; }

    public virtual DbSet<CigarInfo> CigarInfos { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<DeliveryProduct> DeliveryProducts { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsView> NewsViews { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderBasic> OrderBasics { get; set; }

    public virtual DbSet<OrderContent> OrderContents { get; set; }

    public virtual DbSet<OrderDelivery> OrderDeliveries { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Paymentmethod> Paymentmethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Usercart> Usercarts { get; set; }

    public virtual DbSet<Userfavorite> Userfavorites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=cigarhouse; Username=postgres; Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accessory>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("accessory_pkey");

            entity.ToTable("accessory");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.Material)
                .HasMaxLength(100)
                .HasColumnName("material");

            entity.HasOne(d => d.Product).WithOne(p => p.Accessory)
                .HasForeignKey<Accessory>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accessory_product_id_fkey");
        });

        modelBuilder.Entity<AccessoryInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("accessory_info");

            entity.Property(e => e.BrandName)
                .HasMaxLength(50)
                .HasColumnName("brand_name");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.Material)
                .HasMaxLength(100)
                .HasColumnName("material");
            entity.Property(e => e.Price)
                .HasPrecision(8, 3)
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<BestProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("best_products");

            entity.Property(e => e.AvgRating).HasColumnName("avg_rating");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ReviewCount).HasColumnName("review_count");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("brand_pkey");

            entity.ToTable("brand");

            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<BrandProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("brand_products");

            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.BrandName)
                .HasMaxLength(50)
                .HasColumnName("brand_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Price)
                .HasPrecision(8, 3)
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
        });

        modelBuilder.Entity<Cigar>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("cigar_pkey");

            entity.ToTable("cigar");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.FlavorProfile)
                .HasMaxLength(200)
                .HasColumnName("flavor_profile");
            entity.Property(e => e.RingGauge)
                .HasMaxLength(20)
                .HasColumnName("ring_gauge");
            entity.Property(e => e.Strength)
                .HasMaxLength(50)
                .HasColumnName("strength");
            entity.Property(e => e.Vitola)
                .HasMaxLength(100)
                .HasColumnName("vitola");

            entity.HasOne(d => d.Product).WithOne(p => p.Cigar)
                .HasForeignKey<Cigar>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cigar_product_id_fkey");
        });

        modelBuilder.Entity<CigarInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("cigar_info");

            entity.Property(e => e.BrandName)
                .HasMaxLength(50)
                .HasColumnName("brand_name");
            entity.Property(e => e.FlavorProfile)
                .HasMaxLength(200)
                .HasColumnName("flavor_profile");
            entity.Property(e => e.Price)
                .HasPrecision(8, 3)
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RingGauge)
                .HasMaxLength(20)
                .HasColumnName("ring_gauge");
            entity.Property(e => e.Strength)
                .HasMaxLength(50)
                .HasColumnName("strength");
            entity.Property(e => e.Vitola)
                .HasMaxLength(100)
                .HasColumnName("vitola");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("country_pkey");

            entity.ToTable("country");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .HasColumnName("country_name");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("delivery_pkey");

            entity.ToTable("delivery");

            entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.DeliveryLocationFrom).HasColumnName("delivery_location_from");
            entity.Property(e => e.DeliveryLocationTo).HasColumnName("delivery_location_to");

            entity.HasOne(d => d.DeliveryLocationFromNavigation).WithMany(p => p.DeliveryDeliveryLocationFromNavigations)
                .HasForeignKey(d => d.DeliveryLocationFrom)
                .HasConstraintName("delivery_delivery_location_from_fkey");

            entity.HasOne(d => d.DeliveryLocationToNavigation).WithMany(p => p.DeliveryDeliveryLocationToNavigations)
                .HasForeignKey(d => d.DeliveryLocationTo)
                .HasConstraintName("delivery_delivery_location_to_fkey");
        });

        modelBuilder.Entity<DeliveryProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("delivery_products");

            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");
            entity.Property(e => e.DeliveryLocationFrom).HasColumnName("delivery_location_from");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("news_pkey");

            entity.ToTable("news");

            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.NewsAuthor)
                .HasMaxLength(100)
                .HasColumnName("news_author");
            entity.Property(e => e.NewsText)
                .HasMaxLength(300)
                .HasColumnName("news_text");

            entity.HasOne(d => d.Author).WithMany(p => p.News)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("news_author_id_fkey");
        });

        modelBuilder.Entity<NewsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("news_view");

            entity.Property(e => e.AuthorFullname)
                .HasMaxLength(100)
                .HasColumnName("author_fullname");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(100)
                .HasColumnName("author_name");
            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.NewsText)
                .HasMaxLength(300)
                .HasColumnName("news_text");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
            entity.Property(e => e.PaymentMethod).HasColumnName("payment_method");
            entity.Property(e => e.PersonEmail)
                .HasMaxLength(100)
                .HasColumnName("person_email");
            entity.Property(e => e.PersonName)
                .HasMaxLength(300)
                .HasColumnName("person_name");
            entity.Property(e => e.PersonPhone)
                .HasMaxLength(20)
                .HasColumnName("person_phone");
            entity.Property(e => e.TotalCost)
                .HasPrecision(9, 3)
                .HasColumnName("total_cost");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("orders_order_status_id_fkey");

            entity.HasOne(d => d.PaymentMethodNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentMethod)
                .HasConstraintName("orders_payment_method_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<OrderBasic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("order_basic");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Customer)
                .HasMaxLength(100)
                .HasColumnName("customer");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Payment)
                .HasMaxLength(50)
                .HasColumnName("payment");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");
        });

        modelBuilder.Entity<OrderContent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("order_contents");

            entity.Property(e => e.BrandName)
                .HasMaxLength(50)
                .HasColumnName("brand_name");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.PricePerUnit)
                .HasPrecision(8, 3)
                .HasColumnName("price_per_unit");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");
        });

        modelBuilder.Entity<OrderDelivery>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("order_delivery");

            entity.Property(e => e.Customer)
                .HasMaxLength(100)
                .HasColumnName("customer");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");
            entity.Property(e => e.DeliveryLocationFrom).HasColumnName("delivery_location_from");
            entity.Property(e => e.DeliveryLocationTo).HasColumnName("delivery_location_to");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("order_item_pkey");

            entity.ToTable("order_item");

            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_item_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("order_item_product_id_fkey");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("order_status_pkey");

            entity.ToTable("order_status");

            entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Paymentmethod>(entity =>
        {
            entity.HasKey(e => e.PaymentmethodId).HasName("paymentmethod_pkey");

            entity.ToTable("paymentmethod");

            entity.Property(e => e.PaymentmethodId).HasColumnName("paymentmethod_id");
            entity.Property(e => e.PaymentName)
                .HasMaxLength(50)
                .HasColumnName("payment_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CostProduct)
                .HasPrecision(8, 3)
                .HasColumnName("cost_product");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");
            entity.Property(e => e.Image)
                .HasMaxLength(300)
                .HasColumnName("image");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("product_brand_id_fkey");

            entity.HasOne(d => d.CountryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Country)
                .HasConstraintName("product_country_fkey");

            entity.HasOne(d => d.Delivery).WithMany(p => p.Products)
                .HasForeignKey(d => d.DeliveryId)
                .HasConstraintName("product_delivery_id_fkey");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("product_reviews");

            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .HasColumnName("author");
            entity.Property(e => e.BrandName)
                .HasMaxLength(50)
                .HasColumnName("brand_name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductName)
                .HasMaxLength(200)
                .HasColumnName("product_name");
            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.ReviewText)
                .HasMaxLength(300)
                .HasColumnName("review_text");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("review_pkey");

            entity.ToTable("review");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReviewText)
                .HasMaxLength(300)
                .HasColumnName("review_text");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("review_product_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("review_user_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Birthday)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birthday");
            entity.Property(e => e.Cart).HasColumnName("cart");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Favorites).HasColumnName("favorites");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Image)
                .HasMaxLength(100)
                .HasColumnName("image");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("user_roles");

            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Usercart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("usercart_pkey");

            entity.ToTable("usercart");

            entity.HasIndex(e => e.UserId, "usercart_user_id_key").IsUnique();

            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Usercart)
                .HasForeignKey<Usercart>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usercart_user_id_fkey");

            entity.HasMany(d => d.Products).WithMany(p => p.Carts)
                .UsingEntity<Dictionary<string, object>>(
                    "Itemcart",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("itemcart_product_id_fkey"),
                    l => l.HasOne<Usercart>().WithMany()
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("itemcart_cart_id_fkey"),
                    j =>
                    {
                        j.HasKey("CartId", "ProductId").HasName("itemcart_pkey");
                        j.ToTable("itemcart");
                        j.IndexerProperty<int>("CartId").HasColumnName("cart_id");
                        j.IndexerProperty<int>("ProductId").HasColumnName("product_id");
                    });
        });

        modelBuilder.Entity<Userfavorite>(entity =>
        {
            entity.HasKey(e => e.FavoritesId).HasName("userfavorites_pkey");

            entity.ToTable("userfavorites");

            entity.HasIndex(e => e.UserId, "userfavorites_user_id_key").IsUnique();

            entity.Property(e => e.FavoritesId).HasColumnName("favorites_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Userfavorite)
                .HasForeignKey<Userfavorite>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userfavorites_user_id_fkey");

            entity.HasMany(d => d.Products).WithMany(p => p.Favorites)
                .UsingEntity<Dictionary<string, object>>(
                    "Itemfavorite",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("itemfavorites_product_id_fkey"),
                    l => l.HasOne<Userfavorite>().WithMany()
                        .HasForeignKey("FavoritesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("itemfavorites_favorites_id_fkey"),
                    j =>
                    {
                        j.HasKey("FavoritesId", "ProductId").HasName("itemfavorites_pkey");
                        j.ToTable("itemfavorites");
                        j.IndexerProperty<int>("FavoritesId").HasColumnName("favorites_id");
                        j.IndexerProperty<int>("ProductId").HasColumnName("product_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
