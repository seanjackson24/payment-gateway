using System;
using System.Linq;

namespace PaymentGateway.Services
{
	public static class CardMasker
	{
		private const char maskCharacter = '.';
		/// <summary>
		/// Masks a standard 16 digit card number using 6.4 notation. If less than 16 digits are passed in, it will just return a mask. If more than 16 digits are passed in, it will mask 6.
		///</summary>
		public static string MaskCardNumber(string unmaskedCardNumber)
		{
			// TODO: check for spaces and dashes
			if (string.IsNullOrWhiteSpace(unmaskedCardNumber))
			{
				throw new ArgumentException("Cannot mask null or empty card number");
			}
			if (unmaskedCardNumber.Length < 16)
			{
				return new String(maskCharacter, unmaskedCardNumber.Length);
			}

			string six = unmaskedCardNumber.Substring(0, 6);
			string dot = new String(maskCharacter, 6);
			string four = unmaskedCardNumber.Substring(12);
			return ($"{six}{dot}{four}");
		}
	}
}