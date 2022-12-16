using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgramming_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Вводите символы (символ Escape для остановки): ");

            var buffer = new RingBuffer(10);

            var producer = new Task(() => ParallelActions.Producer(ref buffer));

            var consumer1 = new Task(() => ParallelActions.Consumer(ref buffer, CharType.Digit));

            var consumer2 = new Task(() => ParallelActions.Consumer(ref buffer, CharType.Letter));

            var consumer3 = new Task(() => ParallelActions.Consumer(ref buffer, CharType.Other));

            producer.Start();
            consumer1.Start();
            consumer2.Start();
            consumer3.Start();

            Task.WaitAll(producer, consumer1, consumer2, consumer3);
        }
    }
}
