using DomainLayer.Common;
using DomainLayer.Entities;
using DomainLayer.Models;
using DomainLayer.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class BookServices : IBookServices
    {
        private readonly BookDbContext _context;

        public CustomMessages CustomMessages { get; set; } = new CustomMessages();
        
        public BookServices(BookDbContext context)
        {
            _context = context;
        }

        public async Task<StatusMessages> Create(BookViewModel model)
        {
            try
            {
                BookModel formData = new BookModel
                {
                    Id = Guid.NewGuid().ToString().Replace(" ", ""),
                    Title = model.Title,
                    ISBN = model.ISBN,
                    PublishYear = model.PublishYear,
                    CoverPrice = model.CoverPrice,
                    AvailabilityStatus = "check-in",
                    IsActive = true,
                    DateCreated = DateTime.Now
                };
                _context.Tbl_Books.Add(formData);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Submitted };
                else
                    return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Declined };
            }
            catch (SqlException err)
            {
                return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Error + err.InnerException };
            }
        }

        public async Task<IEnumerable<BookViewModel>> Fetch()
        {
            var res = await (from _bks in _context.Tbl_Books
                             where _bks.IsDeleted == false
                             select new BookViewModel
                             {
                                 Id = _bks.Id,
                                 Title = _bks.Title,
                                 ISBN = _bks.ISBN,
                                 PublishYear = _bks.PublishYear,
                                 CoverPrice = _bks.CoverPrice,
                                 AvailabilityStatus = _bks.AvailabilityStatus,
                                 IsActive = _bks.IsActive,
                             }).OrderBy(p => p.PublishYear).ToListAsync();

            return res;
        }

        public async Task<IEnumerable<BookViewModel>> FetchBookDetails(string BookTitle)
        {
            var res = await (from _bks in _context.Tbl_Books
                             where _bks.IsDeleted == false && _bks.Title.Contains(BookTitle)
                             select new BookViewModel
                             {
                                 Id = _bks.Id,
                                 Title = _bks.Title,
                                 ISBN = _bks.ISBN,
                                 PublishYear = _bks.PublishYear,
                                 CoverPrice = _bks.CoverPrice,
                                 AvailabilityStatus = _bks.AvailabilityStatus,
                                 IsActive = _bks.IsActive,
                             }).OrderBy(p => p.Title).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<BookViewModel>> SearchBooks(string Title, string ISBN, string Status)
        {
            var res = await(from _bks in _context.Tbl_Books
                            where _bks.IsDeleted == false && 
                            _bks.Title.ToLower().Replace(" ", "") == Title.ToLower().Replace(" ", "") &&
                            _bks.ISBN.ToLower().Replace(" ", "") == ISBN.ToLower().Replace(" ", "") &&
                            _bks.AvailabilityStatus.ToLower().Replace(" ", "") == Status.ToLower().Replace(" ", "")

                            select new BookViewModel
                            {
                                Id = _bks.Id,
                                Title = _bks.Title,
                                ISBN = _bks.ISBN,
                                PublishYear = _bks.PublishYear,
                                CoverPrice = _bks.CoverPrice,
                                AvailabilityStatus = _bks.AvailabilityStatus,
                                IsActive = _bks.IsActive,
                            }).OrderBy(p => p.PublishYear).ToListAsync();
            return res;
        }

        public async Task<StatusMessages> Modify(BookViewModel model)
        {
            try
            {
                var res = await _context.Tbl_Books.FirstOrDefaultAsync(p => p.Id == model.Id);
                if (res != null)
                {
                    res.Title = model.Title;
                    res.ISBN = model.ISBN;
                    res.PublishYear = model.PublishYear;
                    res.CoverPrice = model.CoverPrice;
                    res.AvailabilityStatus = "check-in";

                    _context.Tbl_Books.Update(res);

                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                        return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Updated };
                    else
                        return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Declined };

                }else return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Declined };
            }
            catch (SqlException err)
            {
                return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Error + err.InnerException };
            }
        }

        public async Task<StatusMessages> Suspend(string Id)
        {
            try
            {
                var res = await _context.Tbl_Books.FirstOrDefaultAsync(p => p.Id == Id);
                if (res != null)
                {
                    if (res.IsActive)
                        res.IsActive = false;
                    else
                        res.IsActive = true;

                    _context.Tbl_Books.Update(res);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                        return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Success };
                }
                return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Declined };
            }
            catch (SqlException err)
            {
                return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Error + err.InnerException };
            }
        }

        public async Task<StatusMessages> Remove(string Id)
        {
            try
            {
                var res = await _context.Tbl_Books.FirstOrDefaultAsync(p => p.Id == Id);
                if (res != null)
                {
                    res.IsDeleted = true;

                    _context.Tbl_Books.Update(res);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                        return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Deleted };
                }
                return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Deleted };
            }
            catch (DbUpdateException err)
            {
                return new StatusMessages { IsSuccess = false, Messages = CustomMessages.Error + err.InnerException };
            }
        }
    }
}
