using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CustomerService.Tests
{
    [TestFixture]
    public class CustomerTests
    {
        #region Constants

        private const string EnUsCultureStr = "en-Us";
        private const string EnGbCultureStr = "en-GB";

        private const string NameFormatStr = "N";
        private const string PhoneFormatStr = "P";
        private const string RevenueFormatStr = "R";
        private const string AllFormatStr = "NPR";
        private const string GeneralFormatStr = "G";

        private const string NamePhoneFormatStr = "NP";
        private const string NameRevenueFormatStr = "NR";

        private const string DefaultStringResult =
            "Name: Jeffrey Richter, Phone: +1 (425) 555-0100, Revenue: $1,000,000.00";

        #endregion

        private static readonly Customer TestCustomer = new Customer("Jeffrey Richter", "+1 (425) 555-0100", 1000000);

        [TestCase("Jeffrey Richter", "+1 (425) 555-0100", 1000000, ExpectedResult = 
            "Name: Jeffrey Richter, Phone: +1 (425) 555-0100, Revenue: $1,000,000.00")]
        public string ToString_FormattedStr(string name, string phone, decimal revenue)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(EnUsCultureStr);
            return new Customer(name, phone, revenue).ToString();
        }

        [TestCase("Jeffrey Richter", "+1 (425) 555-0100", 1000000, GeneralFormatStr, ExpectedResult = DefaultStringResult)]
        [TestCase("Jeffrey Richter", "+1 (425) 555-0100", 1000000, AllFormatStr, ExpectedResult = DefaultStringResult)]
        [TestCase("Jeffrey Richter", "+1 (425) 555-0100", 1000000, null, ExpectedResult = DefaultStringResult)]
        [TestCase("Jeffrey Richter", "+1 (425) 555-0100", 1000000, "", ExpectedResult = DefaultStringResult)]
        [TestCase("Jeffrey Richter", "+1 (425) 555-0100", 1000000, NameFormatStr, ExpectedResult =
            "Name: Jeffrey Richter")]
        [TestCase("Jeffrey Richter", "+1 (425) 555-0100", 1000000, PhoneFormatStr, ExpectedResult =
            "Phone: +1 (425) 555-0100")]
        [TestCase("Jeffrey Richter", "+1 (425) 555-0100", 1000000, RevenueFormatStr, ExpectedResult =
            "Revenue: $1,000,000.00")]
        public string ToString_Format_FormattedStr(string name, string phone, decimal revenue, string format)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(EnUsCultureStr);
            return new Customer(name, phone, revenue).ToString(format);
        }

        private static IEnumerable<TestCaseData> TestProviderData
        {
            get
            {
                yield return new TestCaseData(TestCustomer, CultureInfo.GetCultureInfo(EnUsCultureStr))
                    .Returns(DefaultStringResult);
                yield return new TestCaseData(TestCustomer, CultureInfo.GetCultureInfo(EnGbCultureStr))
                    .Returns("Name: Jeffrey Richter, Phone: +1 (425) 555-0100, Revenue: £1,000,000.00");
            }
        }

        [Test, TestCaseSource(nameof(TestProviderData))]
        public string ToString_Provider_FormattedStr(Customer customer, IFormatProvider provider)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(EnUsCultureStr);
            return customer.ToString(provider);
        }

        private static IEnumerable<TestCaseData> TestFormatProviderData
        {
            get
            {
                yield return new TestCaseData(TestCustomer, RevenueFormatStr, CultureInfo.GetCultureInfo(EnUsCultureStr))
                    .Returns("Revenue: $1,000,000.00");
                yield return new TestCaseData(TestCustomer, RevenueFormatStr, CultureInfo.GetCultureInfo(EnGbCultureStr))
                    .Returns("Revenue: £1,000,000.00");
                yield return new TestCaseData(TestCustomer, NamePhoneFormatStr, new CustomerFormatProvider(CultureInfo.GetCultureInfo(EnUsCultureStr)))
                    .Returns("Name: Jeffrey Richter, Phone: +1 (425) 555-0100");
                yield return new TestCaseData(TestCustomer, NameRevenueFormatStr, new CustomerFormatProvider(CultureInfo.GetCultureInfo(EnUsCultureStr)))
                    .Returns("Name: Jeffrey Richter, Revenue: $1,000,000.00");
                yield return new TestCaseData(TestCustomer, NameRevenueFormatStr, new CustomerFormatProvider(CultureInfo.GetCultureInfo(EnGbCultureStr)))
                    .Returns("Name: Jeffrey Richter, Revenue: £1,000,000.00");
                yield return new TestCaseData(TestCustomer, NameFormatStr, new CustomerFormatProvider(CultureInfo.GetCultureInfo(EnGbCultureStr)))
                    .Returns("Name: Jeffrey Richter");
            }
        }

        [Test, TestCaseSource(nameof(TestFormatProviderData))]
        public string ToString_FormatAndProvider_FormattedStr(Customer customer, string format, IFormatProvider provider)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(EnUsCultureStr);
            return customer.ToString(format, provider);
        }

        [Test]
        public void ToString_InvalidFormat_ThrowsException()
        {
            Assert.Throws<FormatException>(() => TestCustomer.ToString(NamePhoneFormatStr));
            Assert.Throws<FormatException>(() => TestCustomer.ToString("A"));
        }
    }
}
