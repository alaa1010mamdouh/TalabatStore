using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Identity
{
    public class AppUser:IdentityUser
    {
        public address Address { get; set; }
        public string DisplayName { get; set; }
    }
}
