using Microsoft.EntityFrameworkCore;

namespace MvcWebApiSwaggerApp.Models
{
    public class Admin
    {
    }


    [Keyless]

    public class UsersList
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


    }
}
