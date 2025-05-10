using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Infrastructure.Interface
{
    public interface IPolicyService
    {
        Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action);
    }
}
