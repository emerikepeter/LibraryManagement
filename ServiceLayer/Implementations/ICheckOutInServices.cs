using DomainLayer.Common;
using DomainLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Implementations
{
    public interface ICheckOutInServices
    {
        Task<IEnumerable<CheckOutInBookViewModel>> CheckIn(string NIN);
        Task<StatusMessages> CheckInBook(string NIN, string BookTitle);
        Task<StatusMessages> CheckOut(CheckOutInBookViewModel model);
        Task<IEnumerable<CheckOutInBookViewModel>> Fetch();
        Task<IEnumerable<CheckOutInBookViewModel>> FetchStatus(bool BookStatus);
    }
}