using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplicationTest
{
    class Program
    {
        /// <summary>
        /// http://rxwiki.wikidot.com/101samples
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Buffer_Simple.start();
            Timeout_Simple.start();

            /*
             * 
             *  This is good stuff. use instead of ITimer
             * 
            var oneNumberPerSecond = Observable.Interval(TimeSpan.FromSeconds(1));

            var lowNums = from n in oneNumberPerSecond
                          where n < 5
                          select n;

            Console.WriteLine("Numbers < 5:");

            lowNums.Subscribe(lowNum =>
            {
                Console.WriteLine(lowNum);
            });

            Console.ReadKey();
            */

            IEnumerable<int> someInts = new List<int> { 1, 2, 3, 4, 5 };

            // To convert a generic IEnumerable into an IObservable, use the ToObservable extension method.
            IObservable<int> observable = someInts.ToObservable();
            observable.Subscribe(onNext);

            StartBackgroundWork();
            //CombineLatestParallelExecution();
        }


        private void window()
        {
            IObservable<long> mainSequence = Observable.Interval(TimeSpan.FromSeconds(1));
            IObservable<IObservable<long>> seqWindowed = mainSequence.Window(() =>
            {
                IObservable<long> seqWindowControl = Observable.Interval(TimeSpan.FromSeconds(6));
                return seqWindowControl;
            });

            seqWindowed.Subscribe(seqWindow =>
            {
                Console.WriteLine("\nA new window into the main sequence has opened: {0}\n",
                                    DateTime.Now.ToString());
                seqWindow.Subscribe(x => { Console.WriteLine("Integer : {0}", x); });
            });

            Console.ReadLine();
        }


        public static void onNext(int i)
        {
            Console.WriteLine(i);
        }

        public static async void StartBackgroundWork()
        {
            Console.WriteLine("Shows use of Start to start on a background thread:");
            Console.WriteLine("Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
            var o = Observable.Start(() =>
            {
                //This starts on a background thread.
                Console.WriteLine("Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("From background thread. Does not block main thread.");
                Console.WriteLine("Calculating...");
                Thread.Sleep(5000);
                Console.WriteLine("Background work completed.");
            });
            await o.FirstAsync();   // subscribe and wait for completion of background operation.  If you remove await, the main thread will complete first.
            Console.WriteLine("Main thread completed.");
        }

        private static void CombineLatestParallelExecution()
        {
            var o = Observable.CombineLatest(
                Observable.Start(() =>
                {
                    Console.WriteLine("Executing 1st on Thread: {0}", Thread.CurrentThread.ManagedThreadId);
                    return "Result A";
                }),
                Observable.Start(() =>
                {
                    Console.WriteLine("Executing 2nd on Thread: {0}", Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(5000);
                    return "Result B";
                }),
                Observable.Start(() =>
                {
                    Console.WriteLine("Executing 3rd on Thread: {0}", Thread.CurrentThread.ManagedThreadId);
                    return "Result C";
                })
                ).Finally(() => Console.WriteLine("Done!"));

            foreach (string r in o.First())
                Console.WriteLine(r);
        }
    }

    
}
