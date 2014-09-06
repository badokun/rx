using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CombineLatestParallelExecution();
        }

        static void CombineLatestParallelExecution()
        {
            var o = Observable.CombineLatest(
    Observable.Start(() => { Console.WriteLine("Executing 1st on Thread: {0}", Thread.CurrentThread.ManagedThreadId); return "Result A"; }),
    Observable.Start(() => { Console.WriteLine("Executing 2nd on Thread: {0}", Thread.CurrentThread.ManagedThreadId); return "Result B"; }),
    Observable.Start(() => { Console.WriteLine("Executing 3rd on Thread: {0}", Thread.CurrentThread.ManagedThreadId); return "Result C"; })
).Finally(() => Console.WriteLine("Done!"));

            foreach (string r in o.First())
                Console.WriteLine(r);
        }
    }

    
}
