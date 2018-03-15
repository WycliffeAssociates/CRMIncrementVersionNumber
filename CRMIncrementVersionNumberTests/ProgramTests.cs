using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRMIncrementVersionNumber;
using System.Collections.Generic;

namespace CRMIncrementVersionNumberTests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void VersionParse()
        {
            List<int> expected = new List<int>() { 1, 2, 3, 4 };
            List<int> parsed = Program.ParseVersion(string.Join(".", expected));
            Assert.AreEqual(expected.Count, parsed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], parsed[i]);
            }
        }

        [ExpectedException(typeof(Exception))]
        [TestMethod]
        public void VersionParseWithError()
        {
            Program.ParseVersion("1.2.r");
        }

        [TestMethod]
        public void IncrementVersion()
        {
            Assert.AreEqual("1.2.4", Program.IncrementVersionNumber("1.2.3", "0.0.1"));
        }

        [TestMethod]
        public void IncrementVersionWithAdd()
        {
            Assert.AreEqual("1.2.3", Program.IncrementVersionNumber("1.2", "0.0.3"));
        }

        [TestMethod]
        public void TestWithLargeNegativeIncrement()
        {
            Assert.AreEqual("1.3.0", Program.IncrementVersionNumber("1.2.9", "0.1.-1000"));
        }
    }
}
