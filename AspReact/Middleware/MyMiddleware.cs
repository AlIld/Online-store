using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AspReact.Middleware
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _path;

        public MyMiddleware(RequestDelegate next, string path)
        {
            _next = next;
            _path = path;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await _next(context);

            try
            {
                using (StreamWriter sw = new StreamWriter(_path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine($"Request for {context.Request.Path} elapsed {stopwatch.ElapsedMilliseconds} ms");
                    Console.WriteLine($"Request for {context.Request.Path} elapsed {stopwatch.ElapsedMilliseconds} ms");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}