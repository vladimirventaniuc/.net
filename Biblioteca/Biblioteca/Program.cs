using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.API;

namespace Biblioteca.API
{
    class Program
    {
        static void Main(string[] args)
        {
            BibliotecaManager task = new BibliotecaManager();
            task.Start();
        }
    }
}
