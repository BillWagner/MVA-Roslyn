using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary
{
    class IfSample
    {
        public void testCode()
        {
            bool b1 = true;
            bool b2 = true;
            bool b3 = true;
            bool b4 = true;
            bool b5 = true;
            bool b6 = true;

            if (b1)
                Console.WriteLine("b1");

            if (b2)
            {
                Console.WriteLine("b1");
            }

            if (b3)
                Console.WriteLine("b3");
            else
                Console.WriteLine("not b3");

            if (b4)
            {
                Console.WriteLine("b4");
            }
            else
                Console.WriteLine("not b4");

            if (b5)
                Console.WriteLine("b5");
            else
            {
                Console.WriteLine("not b5");
            }

            if (b6)
            {
                Console.WriteLine("b6");
            }
            else
            {
                Console.WriteLine("not b6");
            }

        }
    }
}
