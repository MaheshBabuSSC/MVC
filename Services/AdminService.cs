using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcWebApiSwaggerApp.Models;
using System.Text.Json;



namespace MvcWebApiSwaggerApp.Services
{
    public class AdminService
    {

        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }


        public List<UsersList> GetUsers()
        {
            return _context.Set<UsersList>()
                .FromSqlRaw("EXEC sp_GetUsers")
                .ToList();
        }
    };




    

}


