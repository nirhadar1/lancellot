using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ok = JsonClientApp.JsonClient.Instance.execute(args);
        }
    }
}
