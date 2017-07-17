using System;
using System.Globalization;

namespace CustomerService
{
    public class CustomerFormatProvider : IFormatProvider, ICustomFormatter
    {
        #region Constants

        private const string NamePhoneFormatStr = "NP";
        private const string NameRevenueFormatStr = "NR";
        private const string TwoDigitsAfterCommaFormatStr = "C2";

        #endregion

        private readonly IFormatProvider parent;

        #region Cunstructors

        /// <summary>
        /// Initializes the current culture format provider
        /// </summary>
        public CustomerFormatProvider() : this(CultureInfo.CurrentCulture) { }

        /// <summary>
        /// Initializes the format provider with the specified culture provider
        /// </summary>
        /// <param name="parent">Culture provider</param>
        public CustomerFormatProvider(IFormatProvider parent)
        {
            this.parent = parent;
        }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Returns an object that provides formatting services for the specified type
        /// </summary>
        /// <param name="formatType">Format type</param>
        /// <returns>Object providing formatting services</returns>
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(Customer) ? this : parent.GetFormat(formatType);
        }

        /// <summary>
        /// Converts the value of a specified object to an equivalent string representation
        /// using specified format and culture-specific formatting information
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="arg">Value to convert</param>
        /// <param name="provider">Culture provider</param>
        /// <returns>Object's string representation</returns>
        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (!(arg is Customer))
                return ProcessOtherFormats(format, arg);

            return GetFormattedString(format ?? string.Empty, (Customer) arg, provider ?? parent);
        }

        #endregion

        #region Private Methods

        private string GetFormattedString(string format, Customer customer, IFormatProvider provider)
        {
            switch (format.ToUpperInvariant())
            {
                case NamePhoneFormatStr:
                    return $"Name: {customer.Name}, Phone: {customer.ContactPhone}";
                case NameRevenueFormatStr:
                    return $"Name: {customer.Name}, Revenue: {customer.Revenue.ToString(TwoDigitsAfterCommaFormatStr, provider)}";
                default:
                    return customer.GetFormattedString(format, provider);
            }
        }

        private string ProcessOtherFormats(string format, object arg)
        {
            IFormattable formattable = arg as IFormattable;

            if (formattable != null)
                return formattable.ToString(format, CultureInfo.CurrentCulture);

            return ReferenceEquals(arg, null) ? String.Empty : arg.ToString();
        }

        #endregion
    }
}
