using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ipfs
{
    [TestClass]
    public class MultiBaseTest
    {
        [TestMethod]
        public void Codec()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            var bytes1 = MultiBase.Decode(MultiBase.Encode(bytes));
            var bytes2 = MultiBase.Decode(MultiBase.Encode(bytes, "base16"));
            CollectionAssert.AreEqual(bytes, bytes1);
            CollectionAssert.AreEqual(bytes, bytes2);
        }

        [TestMethod]
        public void Encode_Unknown_Algorithm()
        {
            var bytes = new byte[] { 1, 2, 3, 4, 5 };
            ExceptionAssert.Throws<KeyNotFoundException>(() => MultiBase.Encode(bytes, "unknown"));
        }

        [TestMethod]
        public void Decode_Bad_Formats()
        {
            ExceptionAssert.Throws<FormatException>(() => MultiBase.Decode("?"));
            ExceptionAssert.Throws<FormatException>(() => MultiBase.Decode("??"));
            ExceptionAssert.Throws<FormatException>(() => MultiBase.Decode("???"));
            ExceptionAssert.Throws<FormatException>(() => MultiBase.Decode("fXX"));
        }

        class TestVector
        {
            public string Algorithm { get; set; }
            public string Input { get; set; }
            public string Output { get; set; }
        }

        TestVector[] TestVectors = new TestVector[]
        {
            new TestVector {
                Algorithm = "base16",
                Input = "yes mani !",
                Output = "f796573206d616e692021"
            },
            new TestVector {
                Algorithm = "base32",
                Input = "yes mani !",
                Output = "bpfsxgidnmfxgsibb"
            },
            new TestVector {
                Algorithm = "base32pad",
                Input = "yes mani !",
                Output = "cpfsxgidnmfxgsibb"
            },
            new TestVector {
                Algorithm = "base32",
                Input = "f",
                Output = "bmy"
            },
            new TestVector {
                Algorithm = "base32pad",
                Input = "f",
                Output = "cmy======"
            },
            new TestVector {
                Algorithm = "base32hex",
                Input = "f",
                Output = "vco"
            },
            new TestVector {
                Algorithm = "base32hexpad",
                Input = "f",
                Output = "tco======"
            },
            new TestVector {
                Algorithm = "base64pad",
                Input = "f",
                Output = "MZg=="
            },
            new TestVector {
                Algorithm = "base64",
                Input = "f",
                Output = "mZg"
            },
            new TestVector {
                Algorithm = "base64",
                Input = "���",
                Output = "mw7fDr8O/"
            },
            new TestVector {
                Algorithm = "base64url",
                Input = "���",
                Output = "uw7fDr8O_"
            },

        };

        /// <summary>
        ///   Test vectors from various sources.
        /// </summary>
        [TestMethod]
        public void CheckMultiBase()
        {
            foreach (var v in TestVectors)
            {
                var bytes = Encoding.UTF8.GetBytes(v.Input);
                var s = MultiBase.Encode(bytes, v.Algorithm);
                Assert.AreEqual(v.Output, s);
                CollectionAssert.AreEqual(bytes, MultiBase.Decode(s));
            }
        }

    }
}
