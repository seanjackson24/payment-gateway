using System;
using Xunit;
using PaymentGateway.Services;

namespace PaymentGateway.Tests
{
	public class CardMaskServiceTests
	{
		private readonly CardMasker _cardMasker = new CardMasker();
		private const string whitespace = " ";
		private const string empty = "";

		[Theory]
		[InlineData(null)]
		[InlineData(empty)]
		[InlineData(whitespace)]
		public void NullOrWhitespace_ThrowsException(string input)
		{
			Assert.Throws<ArgumentException>(() => _cardMasker.MaskCardNumber(input));
		}

		[Theory]
		[InlineData("12345678901234", "123456****1234")]
		[InlineData("123456789012345", "123456*****2345")]
		[InlineData("1234567890123456", "123456******3456")]
		[InlineData("12345678901234567", "123456*******4567")]
		[InlineData("123456789012345678", "123456********5678")]
		[InlineData("1234567890123456789", "123456*********6789")]
		public void DigitOnlyCardNumber_MasksAllButTen(string input, string expected)
		{
			var result = _cardMasker.MaskCardNumber(input);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("1234567890123456")]
		[InlineData("1234 5678 9012 3456")]
		[InlineData("1234-5678-9012-3456")]
		[InlineData("12345678--9012-3456")]
		[InlineData("12345678- -9012 -3456")]
		[InlineData(" 1234567890123456")]
		[InlineData("1234567890123456 ")]
		[InlineData("-1234567890123456")]
		public void ExtraCharacters_AreRemoved(string input)
		{
			const string expected = "123456******3456";
			var result = _cardMasker.MaskCardNumber(input);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("1234567890123")]
		[InlineData("12345678901234567890")]
		public void InvalidLengthData_ThrowsException(string input)
		{
			Assert.Throws<ArgumentException>(() => _cardMasker.MaskCardNumber(input));
		}
	}
}
