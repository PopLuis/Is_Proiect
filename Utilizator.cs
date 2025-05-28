using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTab
{
    public class Utilizator
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } = "client"; // implicit client
    }

}
