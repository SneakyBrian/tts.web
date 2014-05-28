using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using Microsoft.WindowsAzure.ServiceRuntime;
using Owin;
using tts.web.Handlers;

namespace tts.web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CheckAddBinPath();

            var reqHandler = new TTSRequestHandler() { Cache = new FileCache(RoleEnvironment.GetLocalResource("fileCache").RootPath) };

            app.Run(reqHandler.HandleRequest);
        }

        public static void CheckAddBinPath()
        {
            // find path to 'bin' folder
            var binPath = Path.Combine(Environment.GetEnvironmentVariable("RoleRoot") + @"\", @"approot\bin");
            // get current search path from environment
            var path = Environment.GetEnvironmentVariable("PATH") ?? "";

            // add 'bin' folder to search path if not already present
            if (!path.Split(Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase))
            {
                path = string.Join(Path.PathSeparator.ToString(), new string[] { path, binPath });
                Environment.SetEnvironmentVariable("PATH", path);
            }
        }
    }
}