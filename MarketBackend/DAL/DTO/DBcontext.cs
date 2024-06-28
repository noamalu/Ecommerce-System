using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MarketBackend.DAL.DTO
{
    public class DBcontext : DbContext
    {
        private static DBcontext _instance = null;
        public static string DbPath;
        public static string DbPathRemote;
        public static string DbPathLocal;
        public static bool LocalMode = true;

        public virtual DbSet<MemberDTO> Members { get; set; }
        public virtual DbSet<StoreDTO> Stores { get; set; }
        public virtual DbSet<RoleDTO> Roles { get; set; }

        public virtual DbSet<RuleDTO> Rules { get; set; }
        public virtual DbSet<ShoppingCartDTO> ShoppingCarts { get; set; }
        public virtual DbSet<BasketDTO> Baskets { get; set; }
        public virtual DbSet<BasketItemDTO> BasketItems { get; set; }
        public virtual DbSet<ProductDTO> Products { get; set; }
        public virtual DbSet<PurchaseDTO> Purchases { get; set; }
        public virtual DbSet<EventDTO> Events { get; set; }

        public override void Dispose()
        {
            Members.ExecuteDelete();
            Stores.ExecuteDelete();
            Roles.ExecuteDelete();
            ShoppingCarts.ExecuteDelete();
            Baskets.ExecuteDelete();
            BasketItems.ExecuteDelete();
            Purchases.ExecuteDelete();
            Products.ExecuteDelete();
            Rules.ExecuteDelete();
            SaveChanges();
            _instance = new DBcontext();
        }

        private static object _lock = new object();

        public static DBcontext GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DBcontext();
                    }
                }
            }
            return _instance;
        }
        public DBcontext()
        {
            DbPathLocal = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MarketDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;Application Intent=ReadWrite;MultiSubnetFailover=False";
    
            DbPathRemote = "Server=tcp:market-db-server.database.windows.net,1433;Initial Catalog=MarketDB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";";
            if (LocalMode)
                DbPath = DbPathLocal;
            else
                DbPath = DbPathRemote;
        }
        public static void SetLocalDB()
        {
            LocalMode = true;
            DbPath = DbPathLocal;
        }
        public static void SetRemoteDB()
        {
            LocalMode = false;
            DbPath = DbPathRemote;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                  optionsBuilder.UseSqlServer($"{DbPath}"); // Use DbPath to configure the database connection
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){


            modelBuilder.Entity<RuleDTO>()
                .HasDiscriminator<string>("Discriminator") // Specify the discriminator property name
                .HasValue<CompositeRuleDTO>("CompositeRule"); // Set the default discriminator value for the base class
            modelBuilder.Entity<RuleDTO>()
                .HasDiscriminator<string>("Discriminator") // Specify the discriminator property name
                .HasValue<SimpleRuleDTO>("SimpleRule"); // Set the default discriminator value for the base class
            modelBuilder.Entity<RuleDTO>()
                .HasDiscriminator<string>("Discriminator") // Specify the discriminator property name
                .HasValue<QuantityRuleDTO>("QuantityRule");
            modelBuilder.Entity<RuleDTO>()
                .HasDiscriminator<string>("Discriminator") // Specify the discriminator property name
                .HasValue<TotalPriceRuleDTO>("TotalPriceRule");

            modelBuilder.Entity<PolicyDTO>()
                .HasDiscriminator<string>("Discriminator") // Specify the discriminator property name
                .HasValue<DiscountPolicyDTO>("DiscountPolicy"); // Set the default discriminator value for the base class
            modelBuilder.Entity<PolicyDTO>()
                .HasDiscriminator<string>("Discriminator") // Specify the discriminator property name
                .HasValue<DiscountCompositePolicyDTO>("CompositeDiscountPolicy"); // Set the default discriminator value for the base class
            modelBuilder.Entity<PolicyDTO>()
                .HasDiscriminator<string>("Discriminator") // Specify the discriminator property name
                .HasValue<PurchasePolicyDTO>("PurchasePolicy");


            modelBuilder.Entity<PolicySubjectDTO>()
                .HasOne<ProductDTO>(s => s.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<RuleSubjectDTO>()
                .HasOne<ProductDTO>(s => s.Product)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PolicyDTO>()
                .HasOne<PolicySubjectDTO>(p => p.PolicySubject)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RuleDTO>()
                .HasOne<RuleSubjectDTO>(p => p.Subject)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberDTO>()
                .HasMany<ShoppingCartHistoryDTO>(m => m.OrderHistory)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<MemberDTO>()
                .HasMany<RoleDTO>(m => m.Roles)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberDTO>()
                .HasMany<MessageDTO>(s => s.Alerts)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoleDTO>()
                .HasKey(r => new { r.storeId, r.userName });

            modelBuilder.Entity<ShoppingCartHistoryDTO>()
                .HasMany<BasketDTO>(s => s._baskets)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCartHistoryDTO>()
                .HasMany<ProductDTO>(s => s._products)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCartDTO>()
                .HasMany<BasketDTO>(s => s.Baskets)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BasketDTO>()
                .HasMany<BasketItemDTO>(b => b.BasketItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseDTO>()
                .HasOne<BasketDTO>(p => p.Basket)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

        } 
    }
}