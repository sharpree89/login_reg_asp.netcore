using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Quotes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello from Program.cs!");
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
