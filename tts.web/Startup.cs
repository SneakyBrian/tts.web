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
            var reqHandler = new TTSRequestHandler() { Cache = new FileCache(RoleEnvironment.GetLocalResource("fileCache").RootPath) };

            app.Run(reqHandler.HandleRequest);
        }
    }
}