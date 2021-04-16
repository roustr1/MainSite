using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Birthday
{
    public interface IBirthdayService
    {
         IEnumerable<UserModel> GetUsers(string urlPath);
    }
}
