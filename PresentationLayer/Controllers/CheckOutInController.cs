using DomainLayer.Common;
using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckOutInController : ControllerBase
    {
        private readonly ICheckOutInServices _outInServices;

        public CheckOutInController(ICheckOutInServices outInServices)
        {
            _outInServices = outInServices;
        }

        [HttpGet]
        [Route("Fetch")]
        public async Task<IEnumerable<CheckOutInBookViewModel>> Fetch()
        {
            return await _outInServices.Fetch();
        }

        [HttpGet]
        [Route("FetchStatus/BookStatus")]
        public async Task<IEnumerable<CheckOutInBookViewModel>> FetchStatus(bool BookStatus)
        {
            return await _outInServices.FetchStatus(BookStatus);
        }

        [HttpGet]
        [Route("CheckIn/NIN")]
        public async Task<IEnumerable<CheckOutInBookViewModel>> CheckIn(string NIN)
        {
            return await _outInServices.CheckIn(NIN);
        }

        [HttpPost]
        [Route("CheckInBook/NIN/BookTitle")]
        public async Task<StatusMessages> CheckInBook(string NIN, string BookTitle)
        {
            return await _outInServices.CheckInBook(NIN, BookTitle);
        }

        [HttpPost]
        [Route("CheckOut")]
        public async Task<StatusMessages> CheckOut(CheckOutInBookViewModel model)
        {
            return await _outInServices.CheckOut(model);
        }
    }
}
