using System.Collections.Generic;
using Autentykacja.Models;

namespace Autentykacja.Services
{
    public interface IUserService
    {
        public IEnumerable<User> GetUsers ();
         public User GetUser(string username, string password);
         
    }
}