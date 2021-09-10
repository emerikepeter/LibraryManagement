using DomainLayer.Common;
using DomainLayer.Entities;
using DomainLayer.Models;
using DomainLayer.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Implementations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class CheckOutInServices : ICheckOutInServices
    {
        private readonly BookDbContext _context;

        public CustomMessages CustomMessages { get; set; } = new CustomMessages();

        public CheckOutInServices(BookDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CheckOutInBookViewModel>> CheckIn(string NIN)
        {
            var res = await (from _cbk in _context.Tbl_CheckOutInBook
                             join _bks in _context.Tbl_Books on _cbk.BookId equals _bks.Id
                             where _cbk.NationalIdentificationNumber == NIN
                             select new CheckOutInBookViewModel
                             {
                                 BookId = _cbk.BookId,
                                 FullName = _cbk.FullName,
                                 Email = _cbk.Email,
                                 PhoneNumber = _cbk.PhoneNumber,
                                 NationalIdentificationNumber = _cbk.NationalIdentificationNumber,
                                 CheckOutDate = DateTime.Now,
                                 ExpectedReturnDate = _cbk.ExpectedReturnDate,
                                 Title = _bks.Title,
                                 ISBN = _bks.ISBN,
                                 PublishYear = _bks.PublishYear,
                             }).ToListAsync();
            return res;
        }


        public async Task UpdatePenalty(string NIN, string BookTitle)
        {
            var getBook = await _context.Tbl_Books.FirstOrDefaultAsync(p => p.Title.Replace(" ", "").ToLower() == BookTitle.Replace(" ", "").ToLower());
            var res = await _context.Tbl_CheckOutInBook.FirstOrDefaultAsync(p => p.NationalIdentificationNumber == NIN && p.BookId == getBook.Id);

            if (res != null && DateTime.Now > res.ExpectedReturnDate)
            {
                var getDays = DateTime.Now - res.ExpectedReturnDate;
                int dd = getDays.Days;

                res.PenaltyDays = dd;
                res.PenaltyAmount = dd * 200;
                _context.Tbl_CheckOutInBook.Update(res);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<StatusMessages> CheckInBook(string NIN, string BookTitle)
        {
            try
            {
                NumberFormatInfo myNumberFormatInfo = new CultureInfo("ig-NG", false).NumberFormat;

                await UpdatePenalty(NIN, BookTitle);

                var getBook = await _context.Tbl_Books.FirstOrDefaultAsync(p => p.Title.Replace(" ", "").ToLower() == BookTitle.Replace(" ", "").ToLower());
                var res = await _context.Tbl_CheckOutInBook.FirstOrDefaultAsync(p => p.NationalIdentificationNumber == NIN && p.BookId == getBook.Id);

                if (res.PenaltyAmount < 1 && res.PenaltyDays < 1)
                    return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Updated };

                if (getBook != null)
                {
                    if (res != null && res.ExpectedReturnDate > DateTime.Now)
                    {
                        res.IsReturned = true;
                        res.ReturnDate = DateTime.Now;
                        _context.Tbl_CheckOutInBook.Update(res);
                        var result  = await _context.SaveChangesAsync();

                        if(result > 0)
                            return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Updated };

                        else
                            return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Declined };
                    }
                    else
                        return new StatusMessages { IsSuccess = true, Messages = $"Expected Return Date has been exceeded with {res.PenaltyDays} day(s), it has attracted the payment of {res.PenaltyAmount.ToString("C", myNumberFormatInfo)} as panalty" };
                }
                return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Declined };
            }
            catch (SqlException err)
            {
                return new StatusMessages { IsSuccess = true, Messages = err.Message };
            }
        }

        public async Task<StatusMessages> CheckOut(CheckOutInBookViewModel model)
        {
            try
            {
                CheckOutInBookModel formData = new CheckOutInBookModel
                {
                    Id = Guid.NewGuid().ToString().Replace(" ", ""),
                    BookId = model.BookId,
                    FullName = model.FullName,
                    Email = model.Email,
                    NationalIdentificationNumber = model.NationalIdentificationNumber,
                    PhoneNumber = model.PhoneNumber,
                    CheckOutDate = DateTime.Now,
                    ExpectedReturnDate = DateTime.Now.AddDays(12), // 10days plus Saturday and Sunday.....
                };

                _context.Tbl_CheckOutInBook.Add(formData);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Submitted };

                else
                    return new StatusMessages { IsSuccess = true, Messages = CustomMessages.Submitted };
            }
            catch (SqlException err)
            {
                return new StatusMessages { IsSuccess = true, Messages = err.Message };
            }
        }

        public async Task<IEnumerable<CheckOutInBookViewModel>> Fetch()
        {
            var res = await (from _cbk in _context.Tbl_CheckOutInBook
                             join _bks in _context.Tbl_Books on _cbk.BookId equals _bks.Id
                             where _cbk.IsDeleted == false
                             select new CheckOutInBookViewModel
                             {
                                 FullName = _cbk.FullName,
                                 Email = _cbk.Email,
                                 PhoneNumber = _cbk.PhoneNumber,
                                 NationalIdentificationNumber = _cbk.NationalIdentificationNumber,
                                 CheckOutDate = DateTime.Now,
                                 IsReturned = _cbk.IsReturned,
                                 ExpectedReturnDate = _cbk.ExpectedReturnDate,
                                 Title = _bks.Title,
                                 ISBN = _bks.ISBN,
                                 PublishYear = _bks.PublishYear,
                             }).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<CheckOutInBookViewModel>> FetchStatus(bool BookStatus)
        {
            var res = await (from _cbk in _context.Tbl_CheckOutInBook
                             join _bks in _context.Tbl_Books on _cbk.BookId equals _bks.Id
                             where _cbk.IsDeleted == false && _cbk.IsReturned == BookStatus
                             select new CheckOutInBookViewModel
                             {
                                 FullName = _cbk.FullName,
                                 Email = _cbk.Email,
                                 PhoneNumber = _cbk.PhoneNumber,
                                 NationalIdentificationNumber = _cbk.NationalIdentificationNumber,
                                 CheckOutDate = DateTime.Now,
                                 IsReturned = _cbk.IsReturned,
                                 ExpectedReturnDate = _cbk.ExpectedReturnDate,
                                 Title = _bks.Title,
                                 ISBN = _bks.ISBN,
                                 PublishYear = _bks.PublishYear,
                             }).ToListAsync();
            return res;
        }
    }
}
