using declutter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace declutter.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Entry> Entries { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call the base method to configure Identity tables
            base.OnModelCreating(builder);

            //Configuring table names for PostgreSQL convention (lowercase snake_case) ---
            builder.Entity<ApplicationUser>().ToTable("users");
            builder.Entity<Entry>().ToTable("entries");
            builder.Entity<Tag>().ToTable("tags");

            // Configure Identity framework tables for PostgreSQL
            builder.Entity<IdentityRole>().ToTable("roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("user_claims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("user_logins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("role_claims");
            builder.Entity<IdentityUserToken<string>>().ToTable("user_tokens");

            // Configure column names for ApplicationUser properties
            builder.Entity<ApplicationUser>().Property(u => u.Id).HasColumnName("id");
            builder.Entity<ApplicationUser>().Property(u => u.UserName).HasColumnName("username");
            builder.Entity<ApplicationUser>().Property(u => u.NormalizedUserName).HasColumnName("normalized_username");
            builder.Entity<ApplicationUser>().Property(u => u.Email).HasColumnName("email");
            builder.Entity<ApplicationUser>().Property(u => u.NormalizedEmail).HasColumnName("normalized_email");
            builder.Entity<ApplicationUser>().Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
            builder.Entity<ApplicationUser>().Property(u => u.PasswordHash).HasColumnName("password_hash");
            builder.Entity<ApplicationUser>().Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            builder.Entity<ApplicationUser>().Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            builder.Entity<ApplicationUser>().Property(u => u.PhoneNumber).HasColumnName("phone_number");
            builder.Entity<ApplicationUser>().Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            builder.Entity<ApplicationUser>().Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            builder.Entity<ApplicationUser>().Property(u => u.LockoutEnd).HasColumnName("lockout_end");
            builder.Entity<ApplicationUser>().Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
            builder.Entity<ApplicationUser>().Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");
            builder.Entity<ApplicationUser>().Property(u => u.CreatedAt).HasColumnName("created_at");

            // Configure the many-to-many relationship between Entry and Tag
            builder.Entity<Entry>()
                .HasMany(e => e.Tags)
                .WithMany(t => t.Entries)
                .UsingEntity(j => j.ToTable("entry_tags"));

            // Configure the one-to-many relationship between ApplicationUser and Entry
            builder.Entity<Entry>()
                .HasOne(e => e.Author)
                .WithMany(u => u.Entries)
                .HasForeignKey(e => e.AuthorId);

            // Configure column names for Entry properties
            builder.Entity<Entry>().Property(e => e.Id).HasColumnName("id");
            builder.Entity<Entry>().Property(e => e.Title).HasColumnName("title");
            builder.Entity<Entry>().Property(e => e.Content).HasColumnName("content");
            builder.Entity<Entry>().Property(e => e.CreatedAt).HasColumnName("created_at");
            builder.Entity<Entry>().Property(e => e.AuthorId).HasColumnName("author_id");

            // Further configurations for properties
            builder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // Configure column names for Tag properties
            builder.Entity<Tag>().Property(t => t.Id).HasColumnName("id");
            builder.Entity<Tag>().Property(t => t.Name).HasColumnName("name");

            // Configure validation attributes for Entry properties
            builder.Entity<Entry>().Property(e => e.Title).IsRequired().HasMaxLength(200);
            builder.Entity<Entry>().Property(e => e.Content).IsRequired();
            builder.Entity<Entry>().Property(e => e.CreatedAt).IsRequired();
            builder.Entity<Entry>().Property(e => e.AuthorId).IsRequired();

            // Seed initial data for Tags
            builder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "Personal" },
                new Tag { Id = 2, Name = "Work" },
                new Tag { Id = 3, Name = "Ideas" },
                new Tag { Id = 4, Name = "Learning" },
                new Tag { Id = 5, Name = "Health" }
            );
        }
    }
    }
