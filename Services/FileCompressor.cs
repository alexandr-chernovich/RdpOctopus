using System;
using System.Collections.Generic;
using System.IO;
using SevenZip.Compression.LZMA;
using System.Text;
using Encoder = SevenZip.Compression.LZMA.Encoder;
using Decoder = SevenZip.Compression.LZMA.Decoder;

namespace RdpOctopus.Services
{
    public class FileCompressor
    {
        private const string Alphabet = "qwertyuiopasdfgh";

        public string CompressFileToString(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            byte[] fileBytes = File.ReadAllBytes(filePath);
            byte[] compressedBytes = CompressBytes(fileBytes);
            string encoded = EncodeBytesToString(compressedBytes);
            return RleCompress(encoded);
        }

        public void DecompressStringToFile(string encodedString, string outputPath)
        {
            string rleDecompressed = RleDecompress(encodedString);
            byte[] compressedBytes = DecodeStringToBytes(rleDecompressed);
            byte[] decompressedBytes = DecompressBytes(compressedBytes);
            File.WriteAllBytes(outputPath, decompressedBytes);
        }

        public byte[] CompressBytes(byte[] data)
        {
            using var inStream = new MemoryStream(data);
            using var outStream = new MemoryStream();

            var encoder = new Encoder();

            encoder.WriteCoderProperties(outStream);

            long fileSize = inStream.Length;
            for (int i = 0; i < 8; i++)
                outStream.WriteByte((byte)(fileSize >> (8 * i)));

            encoder.Code(inStream, outStream, data.Length, -1, null);

            return outStream.ToArray();
        }

        public byte[] DecompressBytes(byte[] compressedData)
        {
            using var inStream = new MemoryStream(compressedData);
            using var outStream = new MemoryStream();

            var decoder = new Decoder();

            byte[] properties = new byte[5];
            inStream.Read(properties, 0, 5);
            decoder.SetDecoderProperties(properties);

            long fileSize = 0;
            byte[] sizeBuf = new byte[8];
            inStream.Read(sizeBuf, 0, 8);
            for (int i = 0; i < 8; i++)
                fileSize |= (long)sizeBuf[i] << (8 * i);

            decoder.Code(inStream, outStream, inStream.Length, fileSize, null);

            return outStream.ToArray();
        }

        public string EncodeBytesToString(byte[] bytes)
        {
            var result = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes)
            {
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

        private string RleCompress(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new StringBuilder();
            int i = 0;
            while (i < input.Length)
            {
                char current = input[i];
                int count = 1;
                while (i + count < input.Length && input[i + count] == current)
                    count++;

                result.Append(current);
                if (count > 1)
                    result.Append(count.ToString());

                i += count;
            }
            return result.ToString();
        }

        private string RleDecompress(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var result = new StringBuilder();
            int i = 0;
            while (i < input.Length)
            {
                char c = input[i];
                i++;
                int count = 1;

                if (i < input.Length && char.IsDigit(input[i]))
                {
                    int start = i;
                    while (i < input.Length && char.IsDigit(input[i]))
                        i++;
                    string numberStr = input.Substring(start, i - start);
                    count = int.Parse(numberStr);
                }

                result.Append(c, count);
            }
            return result.ToString();
        }
    }
}