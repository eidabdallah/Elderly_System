using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.BLL.Service.Interface
{
    public interface IAutoFinishStaysService
    {
        Task<int> RunAsync(DateTime today);
    }
}
