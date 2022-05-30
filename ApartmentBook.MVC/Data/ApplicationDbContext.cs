using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Payments.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApartmentBook.MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ShadowPropertiesSetup();
            return base.SaveChangesAsync(true, cancellationToken);
        }

        public override int SaveChanges()
        {
            ShadowPropertiesSetup();
            return base.SaveChanges();
        }

        private void ShadowPropertiesSetup()
        {
            ChangeTracker.DetectChanges();

            ChangeTracker.Entries()
                .Where(e => e.Metadata.FindProperty("CreationDate") != null && e.Metadata.FindProperty("LastModificationDate") != null)
                .ToList()
                .ForEach(e =>
                {
                    e.Property("LastModificationDate").CurrentValue = DateTimeOffset.UtcNow;

                    if (e.State == EntityState.Added)
                    {
                        e.Property("CreationDate").CurrentValue = DateTimeOffset.UtcNow;
                    }
                });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entities = modelBuilder.Model.GetEntityTypes()
                .Where(x => x.ClrType.Name != "IdentityUser")
                .ToList();

            entities.ForEach(entity =>
            {
                entity.AddProperty("CreationDate", typeof(DateTimeOffset));
                entity.AddProperty("LastModificationDate", typeof(DateTimeOffset));
            });
            base.OnModelCreating(modelBuilder);
            BuildModelForApplicationUsers(modelBuilder);
            BuildModelForApartments(modelBuilder);
            BuildModelForPayment(modelBuilder);
        }

        private static void BuildModelForApplicationUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users")
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Apartments)
                .WithOne(a => a.User);
        }

        private static void BuildModelForApartments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartment>()
                .ToTable("Apartments")
                .HasKey(a => new { a.Id });

            modelBuilder.Entity<Apartment>()
                .HasMany(a => a.Payments)
                .WithOne(p => p.Apartment);
        }

        private static void BuildModelForPayment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .ToTable("Payments")
                .HasKey(a => new { a.Id });

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Apartment)
                .WithMany(a => a.Payments);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Type)
                .HasConversion(new EnumToStringConverter<PaymentType>());
        }
    }
}