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

        //users
        public DbSet<UsersDTO> Users { get; set; }

        public DbSet<RolesDTO> Roles { get; set; }

        public DbSet<UsersRoleDTO> UsersRole { get; set;  }

        public DbSet<OrderDTO> Orders { get; set; }

        public DbSet<OrderDetailsDTO> OrdersDetails { get; set; }
    }
} 