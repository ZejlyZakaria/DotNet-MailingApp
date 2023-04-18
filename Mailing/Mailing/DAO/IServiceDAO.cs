using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailing.Models;

namespace Mailing.DAO
{
    public interface IServiceDAO
    {
        int Add(Service s);
        int Delete(int id);
        List<Service> SelectAllServices();
    }
}
