using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_core_belarus.Middleware
{
    public class ImageCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration _config;
        private readonly string _filepath;

        public ImageCacheMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
            _filepath = Directory.GetCurrentDirectory() + "\\wwwroot\\" + _config.GetSection("ImageCache")["Path"];

        }
        public async Task Invoke(HttpContext httpContext)
        {
            string filename = $"{httpContext.Request.Path.ToUriComponent().Replace('/', '_')}.jpg";

            processCacheExpiration();

            if (File.Exists(_filepath + filename))
            {
                FileStream resultStream = new FileStream(_filepath + filename, FileMode.Open);
                int size = (int)resultStream.Length;
                byte[] buff = new byte[size];
                resultStream.Read(buff, 0, size);
                httpContext.Response.Body.Position = 0;
                httpContext.Response.Body.Write(buff, 0, size);
                httpContext.Response.ContentType = "image/jpeg";
            }
            else
            {
                await _next(httpContext);

                if (httpContext.Response.ContentType != null && httpContext.Response.ContentType.Contains("image/jpeg"))
                {
                    if (!Directory.Exists(_filepath))
                    {
                        Directory.CreateDirectory(_filepath);
                    }
                    if (Directory.GetFiles(_filepath).Count() < Convert.ToInt32(_config.GetSection("ImageCache")["MaxFiles"]))
                    {
                        int size = (int)httpContext.Response.ContentLength;
                        FileStream file = new FileStream(_filepath + filename, FileMode.Create);
                        byte[] buff = new byte[size];
                        httpContext.Response.Body.Position = 0;
                        httpContext.Response.Body.Read(buff, 0, size);
                        file.Write(buff, 0, size);
                        file.Close();
                    }
                }
            }
        }

        private void processCacheExpiration()
        {
            DateTime deadline = DateTime.Now - new TimeSpan(0, Convert.ToInt32(_config.GetSection("ImageCache")["ExparationMinutes"]), 0);

            if (Directory.Exists(_filepath))
            {
                var files = Directory.GetFiles(_filepath);
                foreach (var file in files)
                {
                    if (Directory.GetLastAccessTime(file) < deadline)
                        File.Delete(file);
                }
            }
        }
    }
}
