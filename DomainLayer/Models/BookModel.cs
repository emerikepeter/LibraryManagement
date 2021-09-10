using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class BookModel: BaseModel
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int PublishYear { get; set; }
        public int CoverPrice { get; set; }
        public string AvailabilityStatus { get; set; }//check-out/check-in
    }
}
