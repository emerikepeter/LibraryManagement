using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class CheckOutInBookModel : BaseModel
    {
        public string BookId { get; set; }

        public DateTime CheckOutDate { get; set; }

        public DateTime ExpectedReturnDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public bool IsReturned { get; set; }

        public string FullName { get; set; }
        
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public int PenaltyDays { get; set; }
        public int PenaltyAmount { get; set; }

        [ForeignKey("BookId")]
        public BookModel Books { get; set; }
    }
}
