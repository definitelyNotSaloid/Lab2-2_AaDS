using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Compatibility;
using Lab22_AaDS.Encoder;

namespace Lab22_AaDS.Tests
{
    [TestFixture]
    public class Tests
    {
        private static List<string> testStrings = new List<string>
        {
            "Hello world!",
            "A",
            "Forget me not",
            "Face the fear, build the future",
            "Glory to mankind!",
            "Ruin has come to our family.\n" +
            "You remember our venerable house, opulent and imperial, gazing proudly from its stoic perch above the moor. \n" +
            "I lived all my years in that ancient rumor-shadowed manor, fattened by decadence and luxury. And yet, I began to tire of... conventional extravagance. Singular unsettling tales suggested the mansion itself was a gateway to some fabulous and unnameable power.\n" +
            "With relic and ritual, I bent every effort towards the excavation and recovery of those long buried secrets, exhausting what remained of our family fortune on... swarthy workmen and... sturdy shovels. At last, in the salt-soaked crags beneath the lowest foundations, we unearthed that damnable portal of antediluvian evil. Our every step unsettled the ancient earth, but we were in a realm of death and madness! In the end, I alone fled, laughing and wailing, through those blackened arcades of antiquity, until consciousness failed me.\n" +
            "You remember our venerable house, opulent and imperial. It is a festering abomination! I beg you, return home, claim your birthright, and deliver our family from the ravenous clutching shadows",

            "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
        };

        private HuffmanEncoder testedEncoder = new HuffmanEncoder();

        [SetUp]
        public void SetUp()
        {
            testedEncoder = new HuffmanEncoder();
        }

        [Test]
        public void Test_Basics()
        {
            foreach (var str in testStrings)
            {
                var encoded = testedEncoder.Encode(str, out Encoder.Encoding encoding);

                Assert.AreEqual(str, encoding.Decode(encoded));
            }
        }
    }
}
