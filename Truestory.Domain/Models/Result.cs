using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truestory.Domain.Models
{
    public class Result<T> : ResultViewModel
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error) : base(isSuccess, error)
        {
            _value = value;
        }
    }
}
