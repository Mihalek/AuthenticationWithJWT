using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autentykacja.Models;

namespace Autentykacja.Services
{
    public class iUserService : IUserService
    {
        private readonly List<User> Users;
        public iUserService()
        {
           Users = new List<User>(){
               new User{
                   Id=1,
                   Username="name1",
                   Password="password1",
                   Role="Admin"
               },
                new User{
                   Id=2,
                   Username="name2",
                   Password="password2",
                   Role="User"
               }
           };
        }
        
        public IEnumerable<User> GetUsers (){
        return (Users);
        }
        public User GetUser (string username, string password){

        var user = Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
        return user;
        
        }

    }
}