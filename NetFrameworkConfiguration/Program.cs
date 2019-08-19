using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrameworkConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            var segreto = "Stringa segreta che non voglio vedere su git ma voglio modificare a Runtime";

            Console.WriteLine("La stringa segreta è {0}", segreto);

            // La modifica della stringa richiede la ri-compilazione del programma
            // La stringa è presente nei sorgenti e quindi condivisa su git

        }
    }
}
