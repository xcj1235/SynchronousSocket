using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestCallback
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i=0;i<5;i++)
            {
                var task = DoSomethingAsync(i);
            }

           // task.Wait();
            Console.WriteLine("finished!!");
            Console.ReadKey();
        }
        static async Task DoSomethingAsync(int index)
        {
            if (index == 0)
            {
                await Task.Run(() => Thread.Sleep(6000));
            }
            else if (index == 1)
            {
                await Task.Run(() => Thread.Sleep(5000));
            }
            else if(index == 2)
            {
                await Task.Run(() => Thread.Sleep(4000));
            }
            else if (index == 3)
            {
                await Task.Run(() => Thread.Sleep(3000));
            }
            else if (index == 4)
            {
                await Task.Run(() => Thread.Sleep(2000));
            }
            else if (index == 5)
            {
                await Task.Run(() => Thread.Sleep(1000));
            }
            //Thread.Sleep(3000);

            Console.WriteLine("End of DoSomethingAsync{0}",index);
        }
    }
}
