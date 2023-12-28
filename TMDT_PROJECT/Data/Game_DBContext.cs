using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TMDT_PROJECT.Data
{
    public partial class Game_DBContext : DbContext
    {
        public Game_DBContext()
        {
        }

        public Game_DBContext(DbContextOptions<Game_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Developer> Developers { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<GameCategory> GameCategories { get; set; } = null!;
        public virtual DbSet<GameImage> GameImages { get; set; } = null!;
        public virtual DbSet<Library> Libraries { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-B0GT31Q\\MSSQLServer1; Database=Game_DB;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.AdminId)
                    .HasMaxLength(50)
                    .HasColumnName("AdminID");

                entity.Property(e => e.AdName).HasMaxLength(255);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.IsActive).HasMaxLength(255);

                entity.Property(e => e.IsHide)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.PassWord).HasMaxLength(255);

                entity.Property(e => e.RandomKey).HasMaxLength(10);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CateId);

                entity.Property(e => e.CateId)
                    .HasMaxLength(50)
                    .HasColumnName("CateID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.CateName).HasMaxLength(255);

                entity.Property(e => e.Logo).HasMaxLength(255);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("Client");

                entity.Property(e => e.Uid)
                    .HasMaxLength(50)
                    .HasColumnName("UID");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.IsActive).HasMaxLength(10);

                entity.Property(e => e.IsHide).HasMaxLength(10);

                entity.Property(e => e.PassWord).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(12);

                entity.Property(e => e.RandomKey).HasMaxLength(10);

                entity.Property(e => e.UserName).HasMaxLength(255);
            });

            modelBuilder.Entity<Developer>(entity =>
            {
                entity.HasKey(e => e.DevId);

                entity.ToTable("Developer");

                entity.Property(e => e.DevId)
                    .HasMaxLength(50)
                    .HasColumnName("Dev_ID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.Developer1)
                    .HasMaxLength(255)
                    .HasColumnName("Developer");

                entity.Property(e => e.IsHide).HasMaxLength(10);

                entity.Property(e => e.Logo).HasMaxLength(255);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("Game");

                entity.Property(e => e.GameId)
                    .HasMaxLength(50)
                    .HasColumnName("GameID");

                entity.Property(e => e.Alias).HasMaxLength(255);

                entity.Property(e => e.DevId)
                    .HasMaxLength(50)
                    .HasColumnName("DevID");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.EndDis)
                    .HasColumnType("date")
                    .HasColumnName("endDis");

                entity.Property(e => e.GameName).HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .HasMaxLength(10)
                    .HasColumnName("isActive");

                entity.Property(e => e.PublisherId)
                    .HasMaxLength(50)
                    .HasColumnName("PublisherID");

                entity.Property(e => e.ReleaseDate).HasColumnType("date");

                entity.Property(e => e.StartDis)
                    .HasColumnType("date")
                    .HasColumnName("startDis");

                entity.Property(e => e.Thumbnail).HasMaxLength(255);

                entity.Property(e => e.Video).HasMaxLength(255);

                entity.HasOne(d => d.Dev)
                    .WithMany(p => p.GameDevs)
                    .HasForeignKey(d => d.DevId)
                    .HasConstraintName("FK_Game_Developer");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.GamePublishers)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_Game_Developer1");
            });

            modelBuilder.Entity<GameCategory>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.CateId });

                entity.ToTable("Game_Categories");

                entity.Property(e => e.GameId)
                    .HasMaxLength(50)
                    .HasColumnName("GameID");

                entity.Property(e => e.CateId)
                    .HasMaxLength(50)
                    .HasColumnName("CateID");

                entity.Property(e => e.Note).HasMaxLength(255);

                entity.HasOne(d => d.Cate)
                    .WithMany(p => p.GameCategories)
                    .HasForeignKey(d => d.CateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Categories_Categories");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameCategories)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Categories_Game");
            });

            modelBuilder.Entity<GameImage>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.ImageId });

                entity.ToTable("Game_Image");

                entity.Property(e => e.GameId)
                    .HasMaxLength(50)
                    .HasColumnName("GameID");

                entity.Property(e => e.ImageId)
                    .HasMaxLength(50)
                    .HasColumnName("ImageID");

                entity.Property(e => e.ImageName).HasMaxLength(255);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameImages)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Image_Game");
            });

            modelBuilder.Entity<Library>(entity =>
            {
                entity.HasKey(e => new { e.Uid, e.GameId });

                entity.Property(e => e.Uid)
                    .HasMaxLength(50)
                    .HasColumnName("UID");

                entity.Property(e => e.GameId)
                    .HasMaxLength(50)
                    .HasColumnName("GameID");

                entity.Property(e => e.FeedBack).HasMaxLength(255);

                entity.Property(e => e.IsLiked)
                    .HasMaxLength(10)
                    .HasColumnName("isLiked");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Libraries)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Libraries_Game");

                entity.HasOne(d => d.UidNavigation)
                    .WithMany(p => p.Libraries)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Libraries_Client");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(50)
                    .HasColumnName("OrderID");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(50)
                    .HasColumnName("PaymentID");

                entity.Property(e => e.Uid)
                    .HasMaxLength(50)
                    .HasColumnName("UID");

                entity.HasOne(d => d.UidNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Client");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.GameId });

                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(50)
                    .HasColumnName("OrderID");

                entity.Property(e => e.GameId)
                    .HasMaxLength(50)
                    .HasColumnName("GameID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Game");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => new { e.GameId, e.Uid });

                entity.ToTable("ShoppingCart");

                entity.Property(e => e.GameId)
                    .HasMaxLength(50)
                    .HasColumnName("GameID");

                entity.Property(e => e.Uid)
                    .HasMaxLength(50)
                    .HasColumnName("UID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShoppingCart_Game");

                entity.HasOne(d => d.UidNavigation)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShoppingCart_Client");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
