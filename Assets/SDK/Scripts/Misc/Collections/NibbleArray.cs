using System;
using UnityEngine;

namespace ThunderRoad
{
	[Serializable]
	public class NibbleArray
	{

        [SerializeField]
		private byte[] data;

		public NibbleArray(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
			}
			
			//cant have odd number of nibbles
			if (length % 2 != 0)
			{
				throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be an odd number");
			}
			
			// Calculate required byte array size based on desired nibble count
			data = new byte[(length + 1) / 2];
		}

		public int Length => data.Length * 2; // Adjusted length to reflect total nibbles

		public byte this[int index]
		{
			get
			{
				if (index < 0 || index >= Length)
				{
					throw new IndexOutOfRangeException(nameof(index));
				}

				// Map index to byte array index and nibble position within that byte
				int byteIndex = index / 2;
				NibblePosition position = (index % 2 == 0) ? NibblePosition.Lower : NibblePosition.Upper;

				switch (position)
				{
					case NibblePosition.Lower:
						return GetLowerNibble(byteIndex);
					case NibblePosition.Upper:
						return GetUpperNibble(byteIndex);
					default:
						throw new ArgumentOutOfRangeException(nameof(index), $"Invalid nibble position for index {index}.");
				}
			}
			set
			{
				if (index < 0 || index >= Length)
				{
					throw new IndexOutOfRangeException(nameof(index));
				}

				if (value > 0xF)
				{
					throw new ArgumentOutOfRangeException(nameof(value), "Nibble value cannot exceed 15.");
				}

				// Map index to byte array index and nibble position
				int byteIndex = index / 2;
				NibblePosition position = (index % 2 == 0) ? NibblePosition.Lower : NibblePosition.Upper;

				switch (position)
				{
					case NibblePosition.Lower:
						SetLowerNibble(byteIndex, value);
						break;
					case NibblePosition.Upper:
						SetUpperNibble(byteIndex, value);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(index), $"Invalid nibble position for index {index}.");
				}
			}
		}

		private byte GetLowerNibble(int index)
		{
			byte b = data[index];
			return (byte)(b & 0x0F);
		}

		private byte GetUpperNibble(int index)
		{
			byte b = data[index];
			return (byte)((b & 0xF0) >> 4);
		}

		private byte SetNibbles(byte lowerNibble, byte upperNibble)
		{
			return (byte)((lowerNibble & 0x0F) | (upperNibble << 4));
		}

		private void SetLowerNibble(int index, byte value)
		{
			if (value > 15)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Lower nibble value cannot exceed 15.");
			}

			data[index] = SetNibbles(value, GetUpperNibble(index));
		}

		private void SetUpperNibble(int index, byte value)
		{
			if (value > 15)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Upper nibble value cannot exceed 15.");
			}

			data[index] = SetNibbles(GetLowerNibble(index), value);
		}

		private enum NibblePosition
		{
			Lower,
			Upper
		}
	}
}
