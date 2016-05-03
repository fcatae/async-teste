using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Init();

            var program = Teste();

            //System.Runtime.CompilerServices.AsyncTaskMethodBuilder.RunProgram(program);

            Console.ReadKey();

            
        }

        static async Task Teste()
        {
            Console.WriteLine("Teste-start");
            int i = 0;
            var abc = "abc";

            for (int a = 0; a < 10; a++)
            {
                //Console.WriteLine("Teste-for-start");
                await DelayTime(1000);
                i = 1;
                await DelayTime(1000);
                i = 2;
                abc = "def";
                Console.WriteLine(a);
                //Console.WriteLine("Teste-for-end");
            }
            Console.WriteLine("Teste-end");
        }

        static async Task DelayTime(int n)
        {
            Console.WriteLine("DelayTime-start");
            await Task.Delay(n);
            Console.WriteLine("DelayTime-end");
        }
    }
}
