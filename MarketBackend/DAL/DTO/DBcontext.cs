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

        public virtual DbSet<MemberDTO> Clients { get; set; }
        public virtual DbSet<StoreDTO> Stores { get; set; }
        public virtual DbSet<RoleDTO> Roles { get; set; }
        public virtual DbSet<ShoppingCartDTO> ShoppingCarts { get; set; }
        public virtual DbSet<BasketDTO> Baskets { get; set; }
        public virtual DbSet<BasketItemDTO> BasketItems { get; set; }
        public virtual DbSet<ProductDTO> Products { get; set; }
        public virtual DbSet<PurchaseDTO> Purchases { get; set; }

        public override void Dispose()
        {
            Clients.ExecuteDelete();
            Stores.ExecuteDelete();
            Roles.ExecuteDelete();
            ShoppingCarts.ExecuteDelete();
            Baskets.ExecuteDelete();
            BasketItems.ExecuteDelete();
            Purchases.ExecuteDelete();
            Products.ExecuteDelete();
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


        
    }
}