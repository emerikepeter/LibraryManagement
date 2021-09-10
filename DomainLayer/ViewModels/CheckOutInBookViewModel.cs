using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.ViewModels
{
    public class CheckOutInBookViewModel : BaseModel
    {
        public string BookId { get; set; }

        public DateTime CheckOutDate { get; set; }

        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public DateTime ExpectedReturnDate { get; set; }


        //book details
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublishYear { get; set; }
        public int CoverPrice { get; set; }
        public string AvailabilityStatus { get; set; }

        public List<BookModel> BookList { get; set; } = new List<BookModel>();
    }
}