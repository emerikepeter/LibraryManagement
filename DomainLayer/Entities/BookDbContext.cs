using DomainLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class BookDbContext: IdentityDbContext<UserModel>
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }

        public virtual DbSet<BookModel> Tbl_Books { get; set; }
        public virtual DbSet<CheckOutInBookModel> Tbl_CheckOutInBook { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SeedDataModel();
            base.OnModelCreating(modelBuilder);
        }
    }
}
