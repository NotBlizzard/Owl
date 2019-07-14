using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owl.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string EmailAddressName { get; set; }
        public string EmailAddress { get; set; }
    }
}
