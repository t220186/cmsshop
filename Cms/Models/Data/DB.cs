﻿using System.Data.Entity;

namespace Cms.Models.Data
{
    //"Db" - ten sam jak w webconfig! - Db ->  connectionStrings
    public class Db : DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }


    }
}