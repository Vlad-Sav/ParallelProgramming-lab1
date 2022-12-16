using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgramming_lab1
{
    public class ParallelActions
    {
      
        public static void Producer(ref RingBuffer ringBuffer)
        {
            while(true)
            {
                var symbol = Console.ReadKey().KeyChar;
                Console.WriteLine("");
                if (symbol == 27)
                    Environment.Exit(0);
                ringBuffer.Empty.WaitOne();
                ringBuffer.Busy.WaitOne();

                ringBuffer.Buffer[ringBuffer.Head] = symbol;
                ringBuffer.Head = (ringBuffer.Head + 1) % ringBuffer.Buffer.Length;
                Console.WriteLine("Producer записал символ " + symbol + " в буфер");

                ringBuffer.Busy.Release();
                ringBuffer.Full.Release();
            }
        }

        public static void Consumer(ref RingBuffer ringBuffer, CharType charType)
        {
            while(true)
            {
                ringBuffer.Full.WaitOne();
                ringBuffer.Busy.WaitOne();

                char symbol = ringBuffer.Buffer[ringBuffer.Tail];

                switch (charType)
                {
                    case CharType.Letter:
                        if (char.IsLetter(symbol))
                        {
                            ringBuffer.Tail = (ringBuffer.Tail + 1) % ringBuffer.Buffer.Length;

                            Console.WriteLine("Consumer_1 считывает символ " + symbol + " из буфера");

                            ringBuffer.Busy.Release();
                            ringBuffer.Empty.Release();
                        }
                        else
                        {
                            ringBuffer.Full.Release();
                            ringBuffer.Busy.Release();
                        }
                        break;
                    case CharType.Digit:
                        if (char.IsDigit(symbol))
                        {
                            ringBuffer.Tail = (ringBuffer.Tail + 1) % ringBuffer.Buffer.Length;

                            Console.WriteLine("Consumer_2 считывает символ " + symbol + " из буфера");

                            ringBuffer.Busy.Release();
                            ringBuffer.Empty.Release();
                        }
                        else
                        {
                            ringBuffer.Full.Release();
                            ringBuffer.Busy.Release();
                        }
                        break;
                    case CharType.Other:
                        if (!char.IsLetterOrDigit(symbol))
                        {
                            ringBuffer.Tail = (ringBuffer.Tail + 1) % ringBuffer.Buffer.Length;

                            Console.WriteLine("Consumer_3 считывает символ " + symbol + " из буфера");

                            ringBuffer.Busy.Release();
                            ringBuffer.Empty.Release();
                        }
                        else
                        {
                            ringBuffer.Full.Release();
                            ringBuffer.Busy.Release();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
