using System;

namespace TCPserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Worker worker = new Worker();
            worker.Start();

        }
    }
}
