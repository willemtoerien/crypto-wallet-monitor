using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Wallet.Api.DataAccess
{
    public partial class WalletDbContext : DbContext
    {
        public WalletDbContext()
        {
        }

        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=wallet-model;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Purpose).HasMaxLength(300);

                entity.Property(e => e.TransactionAt).HasColumnType("datetime");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.TransactionFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Users_From");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.TransactionToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_Users_To");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
