using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Lacellot
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ok =  ManagerLoader.Instance.execute(args);

        }
    }
}
