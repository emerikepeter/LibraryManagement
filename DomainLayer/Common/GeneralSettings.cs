using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Common
{
    public class StatusMessages
    {
        public bool IsSuccess { get; set; }
        public string Messages { get; set; }
    }

    public class CustomMessages
    {
        public string Submitted { get; set; } = "Submitted Successfully";
        public string Updated { get; set; } = "Updated Successfully";
        public string Deleted { get; set; } = "Deleted Successfully";
        public string Success { get; set; } = "Changes saved Successfully";
        public string Declined { get; set; } = "Your request was declined, please try again";
        public string Error { get; set; } = "Unable to perform your request due to ";
    }
}
