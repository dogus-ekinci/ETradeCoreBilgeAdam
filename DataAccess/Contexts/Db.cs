#nullable disable       // ? işareti yerine kullanılır yeşil çizgileri göstermez

using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class Db : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ProductShop> ProductShops { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        //DbContext db = new Db(new DbContextOptions(...))
        public Db(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  // tanımladıklarımızı özelleştirebiliyoruz
        {
            modelBuilder.Entity<ProductShop>().HasKey(ps => new { ps.ProductId, ps.ShopId }); // bu ikisinin primary key olması gerektiğini söyledik
                                                                                              // Tek olsaydı ( ps => ps.ProductId ) şeklinde tanımlayabilirdik ama 1 den fazla olduğu için new'ledik
                                                                                              // BURASI ÇOK ÖNEMLİ 
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)    // 1 product'ın bir categorisi var
                .WithMany(d => d.Products)  // 1 kategorinin 1'den fazla product'ı var
                .HasForeignKey(p => p.CategoryId)   // opsiyonel    // product ile ilişkisi olduğu için belirttik belirtmesekte olurdu ama
                .OnDelete(DeleteBehavior.NoAction);     // product ve category birbirine bağlı category silindiğinde product'larında silinmemis için yazıldı. silersen sana hata fırlatacak

            // HasOne ve WithMany tanımlamadan OnDelete tanımlayamayız bunlar ÇOK ÖNEMLİ!!

            modelBuilder.Entity<UserDetails>()
                .HasOne(ud => ud.User)
                .WithOne(u => u.UserDetails)
                .HasForeignKey<UserDetails>(ud => ud.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<City>()
                .HasOne(city => city.Country)
                .WithMany(country => country.Cities)
                .OnDelete(DeleteBehavior.NoAction); // illede foreign key methodunu kullanmaya gerek yok

            modelBuilder.Entity<UserDetails>()
                .HasOne(ud => ud.Country)
                .WithMany(c => c.UserDetails)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetails>()
               .HasOne(ud => ud.City)
               .WithMany(c => c.UserDetails)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();   // IsUnique() kullanıcı tablosunun kullanıcı adının tekil girilmesini sağlar veri tabanıyla ilgili

            modelBuilder.Entity<Product>().HasIndex(u => u.Name);   // HasIndex() ürün adlarını sıralayıp sayfalandırıyor. En çok arama yapılan index'lenebilir
        }
    }
}
