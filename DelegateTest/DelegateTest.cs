using System;
using System.Threading;

namespace DelegateTest
{
    public delegate void ShowMessage(int n);
    public class DelegateTest
    {
        static void Main(string[] arg)
        {
            const int N = 15;
            ShowMessage asyncCall = new ShowMessage(AsynchronousMethod);

            // asyncCall を非同期で呼び出す。
            IAsyncResult ar = asyncCall.BeginInvoke(N, null, null);

            // ↓この部分は asyncCall によって呼び出されるメソッドと同時に実行されます。
            for (int i = 0; i < N; ++i)
            {
                Thread.Sleep(2000);
                Console.Write("Main ({0})\n", i);
            }

            // asyncCall の処理が終わるのを待つ。
            asyncCall.EndInvoke(ar);

            Console.Write(" 処理完了\n");
        }
        static void AsynchronousMethod(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Thread.Sleep(1000);
                Console.Write("AsynchronousMethod ({0})\n", i);
            }
        }
//        static void B(IAsyncResult ar) { }
    }
}