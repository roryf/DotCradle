using System;
using System.IO;

namespace DotCradle.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadToEnd(this Stream stream)
        {
            var result = new byte[0];
            var buffer = new byte[4096];
            int numberOfBytesRead;
            while ((numberOfBytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var oldLength = result.Length;
                result = IncreaseCapacity(result, numberOfBytesRead);
                Array.Copy(buffer, 0, result, oldLength, numberOfBytesRead);
            }
            stream.Close();
            return result;
        }

        private static byte[] IncreaseCapacity(byte[] buffer, int numberOfBytesToIncrease)
        {
            var result = new byte[buffer.Length + numberOfBytesToIncrease];
            Array.Copy(buffer, result, buffer.Length);
            return result;
        }
    }
}