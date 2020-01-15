using System.Data.Entity;

namespace Cms.Models.Data
{
    //"Db" - ten sam jak w webconfig! - Db ->  connectionStrings
    public class Db : DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
        public DbSet<SideBarDTO> SideBar { get; set; }
        public DbSet<CategoriesDTO> Categories { get; set; }
        public DbSet<ProductsDTO> Products { get; set; }
        
    }
} 