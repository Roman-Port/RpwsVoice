using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RpwsVoice.VoiceService
{
    public static class SpeexWithHeaderByteConverter
    {
        public static string CreateBase64Payload(List<Stream> input)
        {
            //Now, convert this to the special format Google wants. This format requires the length of each chunk between each of the chunks. Create this request.
            MemoryStream voiceRequest = new MemoryStream();
            for (int i = 0; i<input.Count; i++)
            {
                Stream part = input[i];

                //Write the length of this as a byte.
                if (part.Length > 255 || part.Length < 0)
                    throw new Exception($"Failed to form voice request packet to send to Google: The length of a part is {part.Length}, which is either larger than 255 or less than 0.");
                voiceRequest.WriteByte((byte)part.Length);

                //Now, copy the actual data
                part.CopyTo(voiceRequest);
            }

            //Rewind the stream and convert it to base64
            voiceRequest.Position = 0;
            byte[] buf = new byte[voiceRequest.Length];
            voiceRequest.Read(buf, 0, buf.Length);
            return Convert.ToBase64String(buf);
        }
    }
}
