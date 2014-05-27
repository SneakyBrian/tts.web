using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace tts.web.Handlers
{
    public class TTSRequestHandler
    {
        public Task HandleRequest(IOwinContext context)
        {
            return Task.Run(() =>
            {
                var text = context.Request.Query["text"];

                if (text.Length > 140)
                {
                    text = text.Substring(0, 140);
                }
                
                var gender = VoiceGender.Female;                
                Enum.TryParse<VoiceGender>(context.Request.Query["gender"], true, out gender);

                var age = VoiceAge.Adult;
                Enum.TryParse<VoiceAge>(context.Request.Query["age"], true, out age);

                context.Response.ContentType = "audio/wav";

                using(var synth = new SpeechSynthesizer())
                {
                    synth.SelectVoiceByHints(gender, age);

                    using(var stream = new MemoryStream())
                    {
                        synth.SetOutputToWaveStream(stream);

                        synth.Speak(text);

                        context.Response.Write(stream.GetBuffer());                    
                    }
                }
            });            
        }
    }
}