using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.WebData;

namespace crisicheckinweb.Wrappers
{
    public interface IWebSecurityWrapper
    {
        int CurrentUserId { get; }
    }
}
