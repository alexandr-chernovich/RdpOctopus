using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RdpOctopus.Services
{
    public class FileCompressor
    {
        private const string Alphabet = "1qaz2wsx3edc4rfv5tgb6yhn7ujm8ik";

        public string CompressFileToString(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            byte[] fileBytes = File.ReadAllBytes(filePath);
            byte[] compressedBytes = CompressBytes(fileBytes);
            return EncodeBytesToString(compressedBytes);
        }

        public void DecompressStringToFile(string encodedString, string outputPath)
        {
            byte[] compressedBytes = DecodeStringToBytes(encodedString);
            byte[] decompressedBytes = DecompressBytes(compressedBytes);
            File.WriteAllBytes(outputPath, decompressedBytes);
        }

        public byte[] CompressBytes(byte[] data)
        {
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionLevel.Optimal))
                {
                    gzipStream.Write(data, 0, data.Length);
                }
                return outputStream.ToArray();
            }
        }

        public byte[] DecompressBytes(byte[] compressedData)
        {
            using (var inputStream = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                gzipStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }

        public string EncodeBytesToString(byte[] bytes)
        {
            var result = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
            {
                // Разбиваем байт на две части по 4 бита (значения 0-15)
                int firstPart = b >> 4;
                int secondPart = b & 0x0F;

                result.Append(Alphabet[firstPart]);
                result.Append(Alphabet[secondPart]);
            }

            return result.ToString();
        }

        public byte[] DecodeStringToBytes(string encoded)
        {
            if (string.IsNullOrEmpty(encoded) || encoded.Length % 2 != 0)
                throw new ArgumentException("Некорректная строка для декодирования");

            var bytes = new List<byte>(encoded.Length / 2);

            for (int i = 0; i < encoded.Length; i += 2)
            {
                char firstChar = encoded[i];
                char secondChar = encoded[i + 1];

                int firstValue = Alphabet.IndexOf(firstChar);
                int secondValue = Alphabet.IndexOf(secondChar);

                if (firstValue == -1 || secondValue == -1)
                    throw new ArgumentException($"Недопустимый символ в строке: {firstChar} или {secondChar}");

                byte b = (byte)((firstValue << 4) | (secondValue & 0x0F));
                bytes.Add(b);
            }

            return bytes.ToArray();
        }
    }
}
