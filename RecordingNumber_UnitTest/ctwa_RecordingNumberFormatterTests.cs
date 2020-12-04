using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ctwa_Helper.Tests
{
    [TestClass()]
    public class ctwa_RecordingNumberFormatterTests
    {

        [DataRow("20090618000167", "167", "2009-06-18", ctwa_RecordingNumberFormatter.Operation.King)]
        [DataRow("19990614001671", "1671", "1999-06-14", ctwa_RecordingNumberFormatter.Operation.King)]
        [DataRow("7401040052", "52", "1974-01-04", ctwa_RecordingNumberFormatter.Operation.King)]
        [DataRow("9906132006", "2006", "1999-06-13", ctwa_RecordingNumberFormatter.Operation.King)]
        [DataRow("6655729", "6655729", "1969-07-04", ctwa_RecordingNumberFormatter.Operation.King)]
        [DataRow("200001010167", "167", "2000-01-01", ctwa_RecordingNumberFormatter.Operation.Pierce)]
        [DataRow("8008080167", "167", "1980-08-08", ctwa_RecordingNumberFormatter.Operation.Pierce)]
        [DataRow("167351", "167351", "1962-02-17", ctwa_RecordingNumberFormatter.Operation.Pierce)]
        [DataTestMethod()]
        public void FormatRecordingNumberTest(string result, string instrumentnumber, string recorddate, ctwa_RecordingNumberFormatter.Operation operation)
        {
            var rnf = new ctwa_RecordingNumberFormatter();
            var testdate = DateTime.Parse(recorddate);
            string actual = rnf.FormatRecordingNumber(instrumentnumber,  testdate, operation);
            Assert.AreEqual(result, actual);
        }
        
        [DataRow("123", "000123")]
        [DataRow("", "000123456")]
        [DataRow("7891", "001234567891")]
        [DataRow("", "000123A")]
        [DataTestMethod()]
        public void GetInstrumentNumberTest(string result, string data)
        {
            var rnf = new ctwa_RecordingNumberFormatter();
            string actual = rnf.GetInstrumentNumber(data);

            Assert.AreEqual(result, actual);
        }
    }
}