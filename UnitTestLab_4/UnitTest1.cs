using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _4_lab_db;

namespace UnitTestLab_4
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Form1 Form1 = new Form1();

            int previousCounter = Form1.GetCount();
            Form1.AddRecord();
            int currentCounter = Form1.GetCount();
            Assert.AreEqual(previousCounter+1, currentCounter);
        }
    }
}
