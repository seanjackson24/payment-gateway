using System;

namespace PaymentGateway.Services
{
	public class CardMasker
	{
		private const char maskCharacter = '*';
		/// <summary>
		/// Masks a standard 16 digit card number using 6.4 notation. If less than 16 digits are passed in, it will just return a mask. If more than 16 digits are passed in, it will mask 6.
		///</summary>
		public string MaskCardNumber(string unmaskedCardNumber)
		{
			unmaskedCardNumber = unmaskedCardNumber?.Replace(" ", "")?.Replace("-", "");
			// TODO: check for spaces and dashes
			if (string.IsNullOrWhiteSpace(unmaskedCardNumber))
			{
				throw new ArgumentException("Cannot mask null or empty card number");
			}

			if (unmaskedCardNumber.Length < 14 || unmaskedCardNumber.Length > 19)
			{
				throw new ArgumentException("Card number must be between 14 and 19 characters long");
			}


			string six = unmaskedCardNumber.Substring(0, 6);
			string dot = new String(maskCharacter, unmaskedCardNumber.Length - 10);
			string four = unmaskedCardNumber.Substring(unmaskedCardNumber.Length - 4);
			return ($"{six}{dot}{four}");
		}
	}
}