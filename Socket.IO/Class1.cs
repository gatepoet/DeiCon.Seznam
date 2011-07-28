using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Socket.IO
{
    public class RNG
    {
        private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        public const string AlphaNumericCharacters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string NumericCharacters = "0123456789";
        public string GenerateNumeric(int length)
        {
            return GenerateCode(length, AlphaNumericCharacters.Substring(0,10));
        }
        public string GenerateAlphaNumeric(int length)
        {
            return GenerateCode(length, AlphaNumericCharacters);
        }

        internal string GenerateCode(int length, string chars, int bytes = 4)
        {
            var code = string.Empty;
            for (int i = 0; i < length; i++)
            {
                char ch = GenerateChar(bytes, chars);
                code += ch;
            }
            return code;
        }

        public char GenerateChar(int bytes, string chars)
        {
            var array = new byte[4];
            _rng.GetBytes(array);
            //var a = new byte[4];
            //for (int i = 0; i < array.Length; i++)
            //{
            //    a[i] = array[i];
            //}
            //uint n = BitConverter.ToUInt32(a, 0);
            uint n = BitConverter.ToUInt32(array, bytes-1);
            return chars[(int)(n % chars.Length)];
        }
    }

    public class RNGTests
    {
        public void t()
        {
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            Console.WriteLine(BitConverter.ToUInt32(bytes, 0));
            uint u = (uint) (bytes[0] << 24);
        }
        public void GenerateAlphaNumericSrings()
        {
            GenerateString(RNG.AlphaNumericCharacters);
        }
        public void GenerateNumericSrings()
        {
            GenerateString(RNG.AlphaNumericCharacters.Substring(0, 10));
        }
        public void GenerateString(string characters)
        {
            TestResult[] results = GetResults(characters);

            var firstColWidth = 8;
            var secondColWidth = 12;
            var colWidth = firstColWidth + secondColWidth;

            WriteHeader(results, colWidth);

            WriteResults(firstColWidth, secondColWidth, results);
        }

        private void WriteResults(int firstColWidth, int secondColWidth, TestResult[] results)
        {
            for (int i = 0; i < results[0].Values.Length; i++)
            {
                for (int j = 0; j < results.Length; j++)
                {
                    var item = results[j].Values[i];
                    Console.Write(item.Key.ToString().PadLeft(firstColWidth / 2).PadRight(firstColWidth));
                    Console.Write(item.Value.ToString().PadLeft(secondColWidth / 2).PadRight(secondColWidth));
                }
                Console.WriteLine();
            }
        }

        private void WriteHeader(TestResult[] results, int colWidth)
        {
            foreach (var result in results)
            {
                Console.Write(result.Name.PadLeft(colWidth / 2).PadRight(colWidth));
            }
            Console.WriteLine();

            for (int i = 0; i < (results.Length); i++)
            {
                Console.Write(' ');
                for (int j = 0; j < colWidth - 2; j++)
                {
                    Console.Write('-');
                }
                Console.Write(' ');
            }
            Console.WriteLine();
        }

        private TestResult[] GetResults(string chars)
        {
            var rng = new RNG();

            Func<KeyValuePair<char, int>, int> order = kvp => kvp.Key;
            return new[]
                       {
                           new TestResult("One byte", GetDict(chars, rng, 1).OrderByDescending(order)),
                           new TestResult("Two bytes", GetDict(chars, rng, 2).OrderByDescending(order)),
                           new TestResult("Three bytes", GetDict(chars, rng, 3).OrderByDescending(order)),
                           new TestResult("Four bytes", GetDict(chars, rng, 4).OrderByDescending(order)),
                       };
        }


        class TestResult
        {
            public string Name { get; set; }
            public KeyValuePair<char, int>[] Values { get; set; }

            public TestResult(string name, IEnumerable<KeyValuePair<char, int>> values)
            {
                Name = name;
                Values = values.ToArray();
            }
        }
        private Dictionary<char, int> GetDict(string chars, RNG rng, int bytes)
        {
            var dict = new Dictionary<char, int>(chars.Length);
            foreach (var character in chars)
            {
                dict[character] = 0;
            }
            for (int i = 0; i < 1000000; i++)
            {
                var ch = rng.GenerateChar(bytes, chars);
                dict[ch]++;
            }
            return dict;
        }
    }
}
