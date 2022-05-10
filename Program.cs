using System;
using Lab22_AaDS.Encoder;
using Lab22_AaDS.Collections;
using System.Collections.Generic;

namespace Lab22_AaDS
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>
            {
                "Hello world!",
                "a looooooooooooooooooooong string",
                "do a barrel roll",
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
            };


            foreach (var str in list)
            {
                var encoder = new HuffmanEncoder();
                Encoding encoding;
                var encoded = encoder.Encode(str, out encoding);
                Console.WriteLine($"Encoding: {str} ; Memory used: {str.Length}({str.Length * 2}) bytes");
                Console.WriteLine("Encoded string: " + encoded);
                Console.WriteLine($"Memory used: ~{encoded.Count/8} bytes");
                var charSet = new HashSet<char>(str);
                foreach (var ch in charSet)
                {
                    Console.WriteLine(ch + " = " + encoding.CodeOf(ch));
                }
                Console.WriteLine("Decoded: " + encoding.Decode(encoded));
                Console.WriteLine("_________");
            }
        }
    }
}
