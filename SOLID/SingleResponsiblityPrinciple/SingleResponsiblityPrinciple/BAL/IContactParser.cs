using SingleResponsiblityPrinciple.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsiblityPrinciple.BAL
{
    public interface IContactParser
    {
        IList<ContactDTO> Parse(string contactList);
    }
}
