using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_net_core_belarus.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }

        public AuthMessageSenderOptions(string user, string key)
        {
            SendGridUser = user;
            SendGridKey = key;
        }
    }
}
