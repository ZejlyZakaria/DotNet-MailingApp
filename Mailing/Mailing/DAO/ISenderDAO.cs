using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailing.Models;

namespace Mailing.DAO
{
    public interface ISenderDAO
    {
        int Add(Sender s);
        List<Sender> SelectAllSenders();
    }
}
