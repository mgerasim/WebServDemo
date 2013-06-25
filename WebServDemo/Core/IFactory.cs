using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServDemo.Core
{
    public interface IFactory<T>
    {
        T createRepository();
    }
}
