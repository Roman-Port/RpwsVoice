using HttpMultipartParser;
using Newtonsoft.Json;
using RpwsVoice.VoiceService.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RpwsVoice.VoiceService
{
    public class HttpDecoder
    {
        public static Task<Tuple<RequestData, List<Stream>>> DecodeHttpData(Microsoft.AspNetCore.Http.HttpContext e)
        {
            //Parse the form data
            var parser = new MultipartFormDataParser(e.Request.Body, Encoding.UTF8);

            //The first two parts are special. Open them
            RequestData request = JsonConvert.DeserializeObject<RequestData>(parser.Parameters[0].Data);
            //Todo: Decode 2nd one

            //Now, we have all of the parts. Grab their streams and return them.
            List<Stream> outputData = new List<Stream>();
            for (int i = 2; i < parser.Parameters.Count; i++)
                outputData.Add(parser.Parameters[i].ByteData);

            //Create the tuple and return the data.
            Tuple<RequestData, List<Stream>> output = new Tuple<RequestData, List<Stream>>(request, outputData);
            return Task.FromResult<Tuple<RequestData, List<Stream>>>(output);
        }

        private static byte[] ReadContent(Stream s)
        {
            byte[] buf = new byte[262144];
            int lastByte = s.ReadByte();
            int index = 0;
            while(lastByte != -1)
            {
                buf[index] = (byte)lastByte;
                index++;
                lastByte = s.ReadByte();
            }
            byte[] output = new byte[index];
            Buffer.BlockCopy(buf, 0, output, 0, output.Length);
            return output;
        }
    }
}
