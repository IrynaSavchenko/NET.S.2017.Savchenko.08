using System;
using System.Globalization;

namespace CustomerService
{
    /// <summary>
    /// Class working with customer info
    /// </summary>
    public class Customer : IFormattable
    {
        #region Constants

        private const string NameFormatStr = "N";
        private const string PhoneFormatStr = "P";
        private const string RevenueFormatStr = "R";
        private const string AllFormatStr = "NPR";
        private const string GeneralFormatStr = "G";
        private const string TwoDigitsAfterCommaFormatStr = "C2";

        #endregion

        #region Properties

        public string Name { get; }
        public string ContactPhone { get; }
        public decimal Revenue { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor initializing the customer's info
        /// </summary>
        /// <param name="name">Customer's name</param>
        /// <param name="contactPhone">Customer's contact phone</param>
        /// <param name="revenue">Customer's revenue</param>
        public Customer(string name, string contactPhone, decimal revenue)
        {
            CheckArguments(name, contactPhone, revenue);

            Name = name;
            ContactPhone = contactPhone;
            Revenue = revenue;
        }

        #endregion

        #region ToString Methods

        /// <summary>
        /// Represents a customer in a general format and a current culture
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString() => ToString(GeneralFormatStr);

        /// <summary>
        /// Represents a customer in the specified format and a current culture
        /// </summary>
        /// <param name="format">Format string</param>
        /// <returns>Formatted string</returns>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>
        /// Represents a customer in a general format and culture determined by the specified provider
        /// </summary>
        /// <param name="provider">Format provider</param>
        /// <returns>Formatted string</returns>
        public string ToString(IFormatProvider provider) => ToString(GeneralFormatStr, provider);

        /// <summary>
        /// Represents a customer in the specified format and culture
        /// </summary>
        /// <param name="format">Format string</param>
        /// <param name="provider">Format provider</param>
        /// <returns>Formatted string</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            string currentFormat = string.IsNullOrEmpty(format) ? GeneralFormatStr : format;

            ICustomFormatter formatter = provider?.GetFormat(GetType()) as ICustomFormatter;

            if(formatter != null)
                return formatter.Format(currentFormat, this, provider);

            return GetFormattedString(currentFormat, provider);
        }

        #endregion

        #region Private Methods

        internal string GetFormattedString(string format, IFormatProvider provider)
        {
            switch (format.ToUpperInvariant())
            {
                case NameFormatStr:
                    return $"Name: {Name}";
                case PhoneFormatStr:
                    return $"Phone: {ContactPhone}";
                case RevenueFormatStr:
                    return $"Revenue: {Revenue.ToString(TwoDigitsAfterCommaFormatStr, provider)}";
                case AllFormatStr:
                case GeneralFormatStr:
                    return
                        $"Name: {Name}, Phone: {ContactPhone}, Revenue: {Revenue.ToString(TwoDigitsAfterCommaFormatStr, provider)}";
                default:
                    throw new FormatException($"The format string {format} is not supported");
            }
        }

        private void CheckArguments(string name, string contactPhone, decimal revenue)
        {
            CheckArgumentForNull(name, nameof(name));
            CheckArgumentForNull(contactPhone, nameof(contactPhone));

            if (revenue < 0)
                throw new ArgumentException($"Argument {revenue} cannot be negative");
        }

        private void CheckArgumentForNull(object arg, string nameOfArg)
        {
            if (arg == null)
                throw new ArgumentNullException($"Argument {nameOfArg} cannot be null");
        }

        #endregion
    }
}
