using DomainLayer.Common;
using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PresentationLayer.Controllers
{
    //[Authorize(Roles = "Administrator")]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookServices _bookServices;

        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<StatusMessages> Create(BookViewModel model)
        {
            try
            {
                var res = await _bookServices.Create(model);
                return new StatusMessages { IsSuccess = res.IsSuccess, Messages = res.Messages };
            }
            catch (Exception err)
            {
                return new StatusMessages { IsSuccess = false, Messages = err.Message };
            }
        }

        [HttpGet]
        [Route("Fetch")]
        public async Task<IEnumerable<BookViewModel>> Fetch()
        {
            try
            {
                var res = await _bookServices.Fetch();
                return res;
            }
            catch (Exception err )
            {
                throw new Exception(err.Message);
            }
        }

        [HttpGet]
        [Route("FetchBookDetails/BookTitle")]
        public async Task<IEnumerable<BookViewModel>> FetchBookDetails(string BookTitle)
        {
            try
            {
                var res = await _bookServices.FetchBookDetails(BookTitle);
                return res;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
        
        [HttpGet]
        [Route("SearchBooks/Title/ISBN/Status")]
        public async Task<IEnumerable<BookViewModel>> SearchBooks(string Title, string ISBN, string Status)
        {
            try
            {
                var res = await _bookServices.SearchBooks(Title, ISBN, Status);
                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("Modify")]
        public async Task<StatusMessages> Modify(BookViewModel model)
        {
            try
            {
                var res = await _bookServices.Modify(model);
                return new StatusMessages { IsSuccess = res.IsSuccess, Messages = res.Messages };
            }
            catch (Exception err)
            {
                return new StatusMessages { IsSuccess = false, Messages = err.Message };
            }
        }

        [HttpPost]
        [Route("Suspend/Id")]
        public async Task<StatusMessages> Suspend(string Id)
        {
            try
            {
                var res = await _bookServices.Suspend(Id);
                return new StatusMessages { IsSuccess = res.IsSuccess, Messages = res.Messages };
            }
            catch (Exception err)
            {
                return new StatusMessages { IsSuccess = false, Messages = err.Message };
            }
        }

        [HttpPost]
        [Route("Remove/Id")]
        public async Task<StatusMessages> Remove(string Id)
        {
            try
            {
                var res = await _bookServices.Remove(Id);
                return new StatusMessages { IsSuccess = res.IsSuccess, Messages = res.Messages };
            }
            catch (Exception err)
            {
                return new StatusMessages { IsSuccess = false, Messages = err.Message };
            }
        }
    }
}
