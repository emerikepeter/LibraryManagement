using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public static class ModelBuilderExtensions
    {
        public static void SeedDataModel(this ModelBuilder modelBuilder)
        {
            //BookModel-----
            modelBuilder.Entity<BookModel>().HasData(
                new BookModel { Id = Guid.NewGuid().ToString().Replace("-", ""), Title = "Data Management", ISBN = "978-3-16-148410", CoverPrice = 1800, PublishYear = 2019, AvailabilityStatus = "check-in", IsDeleted = false, IsActive = true, DateCreated = DateTime.Now },

                new BookModel { Id = Guid.NewGuid().ToString().Replace("-", ""), Title = "Data Science", ISBN = "788-5-01-008765", CoverPrice=2000, PublishYear = 2009, AvailabilityStatus= "check-in", IsDeleted = false, IsActive = true, DateCreated = DateTime.Now }
            );
        }
    }
}
