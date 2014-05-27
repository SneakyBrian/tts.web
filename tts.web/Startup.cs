using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using tts.web.Handlers;

namespace tts.web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var reqHandler = new TTSRequestHandler();

            app.Run(reqHandler.HandleRequest);
        }
    }
}