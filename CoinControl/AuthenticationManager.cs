using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinControl
{
    public static class AuthenticationManager
    {
        public static int LoggedInUserId { get; set; }
        //Change the datasource hereeee!!!!
        // public static string ConnectionString { get; set; } = "Data Source=EPIOW\\SQLEXPRESS01;Initial Catalog=CoinControl;Integrated Security=True;Trust Server Certificate=True";
    }
    public class User
    {
        public long User_ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

}
