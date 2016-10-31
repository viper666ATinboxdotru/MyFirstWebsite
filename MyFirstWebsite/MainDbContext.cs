using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MyFirstWebsite.Models;


namespace MyFirstWebsite
{
    public class MainDbContext : DbContext
    {
        public MainDbContext() :
            base("name=DefaultConnection")
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    //modelBuilder.Entity<Item>().ToTable("Item");
        //    modelBuilder.Entity<Lists>().ToTable("List");

        //}
        public DbSet<Users> Users { get; set; }

        public DbSet<Lists> Lists { get; set; }
    }
}