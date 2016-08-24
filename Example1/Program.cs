using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();

            a.RaiseEvent();

            a.Event += SayHi;
            a.RaiseEvent();
            a.Event -= SayHi;
            a.RaiseEvent();

            
        }

        static string SayHi()
        {
            Console.WriteLine("Hi");
            return "Hi";
        }
    }

    class A
    {

        public event Func<string> Event;

        public void RaiseEvent()
        {
            if (Event != null)
                Event();
        }
    }
}
