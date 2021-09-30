using System;
using System.Globalization;

namespace SalesforceSharp.Extensions
{
    /// <summary>
    /// Exception helper.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Throws an ArgumentNullException if argument is null.
        /// </summary>
        /// <param name="argumentName">The argument name.</param>
        /// <param name="argument">The argument.</param>
        public static void ThrowIfNull(string argumentName, object argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException if argument is null or an ArgumentException if string is empty.
        /// </summary>
        /// <param name="argumentName">The argument name.</param>
        /// <param name="argument">The argument.</param>
        public static void ThrowIfNullOrEmpty(string argumentName, string argument)
        {
            ThrowIfNull(argumentName, argument);

            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException("Argument '{0}' can't be empty.".With(argumentName), argumentName);
            }
        }

        /// <summary>
		/// Format the specified string. Is a String.Format(CultureInfo.InvariantCulture,..) shortcut.
		/// </summary>
		/// <param name="source">The source string.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The formatted string.</returns>
		public static string With(this string source, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, source, args);
        }
    }
}
