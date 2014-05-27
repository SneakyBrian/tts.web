using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using NAudio.Lame;
using NAudio.Wave;

namespace tts.web.Handlers
{
    public class TTSRequestHandler
    {
        public Task HandleRequest(IOwinContext context)
        {
            return Task.Run(async () =>
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

                var bitRate = 128;
                int.TryParse(context.Request.Query["bitrate"], out bitRate);

                context.Response.ContentType = "audio/mpeg";

                using (var synth = new SpeechSynthesizer())
                {
                    synth.SelectVoiceByHints(gender, age);

                    using (var wavStream = new MemoryStream())
                    {
                        synth.SetOutputToWaveStream(wavStream);

                        synth.Speak(text);

                        await wavStream.FlushAsync();

                        wavStream.Seek(0, SeekOrigin.Begin);

                        using (var wavReader = new WaveFileReader(wavStream))
                        {
                            using (var mp3Stream = new MemoryStream())
                            {
                                using (var mp3Writer = new LameMP3FileWriter(mp3Stream, wavReader.WaveFormat, bitRate))
                                {
                                    await wavReader.CopyToAsync(mp3Writer);
                                }

                                await mp3Stream.FlushAsync();

                                await context.Response.WriteAsync(mp3Stream.GetBuffer());
                            }
                        }
                    }
                }
            });
        }
    }
}