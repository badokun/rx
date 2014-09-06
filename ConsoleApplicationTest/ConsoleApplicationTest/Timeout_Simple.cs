using System;
using System.Reactive.Linq;

namespace ConsoleApplicationTest
{
    internal class Timeout_Simple
    {
        public static void start()
        {
            Console.WriteLine(DateTime.Now);

            // create a single event in 10 seconds time
            var observable = Observable.Timer(TimeSpan.FromSeconds(8)).Timestamp();

            // raise exception if no event received within 9 seconds
            var observableWithTimeout = Observable.Timeout(observable, TimeSpan.FromSeconds(9));

            using (observableWithTimeout.Subscribe(
                x => Console.WriteLine("{0}: {1}", x.Value, x.Timestamp),
                ex => Console.WriteLine("{0} {1}", ex.Message, DateTime.Now)))
            {
                Console.WriteLine("Press any key to unsubscribe");
                Console.ReadKey();
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}