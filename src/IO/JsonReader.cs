using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using SGson.Grammar;
using SGson.Grammar.VarName;
using SGson.Grammar.Number;

namespace SGson.IO
{
	public class JsonReader
	{
		private readonly TextReader reader;

		private int line = 1;
		private int column = 0;
		private int position = 0;
		private Queue<char> charQueue = new Queue<char>();
		private int charQueueMaxLength = 30;

		private static IgnoredCharMap ignoredCharMap = IgnoredCharMap.Instance;
		private static WhiteCharMap whiteCharMap = WhiteCharMap.Instance;
		private static VarNameCharMap varNameCharMap = VarNameCharMap.Instance;

		private int? peekBlockBuffer;

		private char[] shortBuffer = new char[4];

		private StringPool stringPool = new StringPool();
		private char[] stringBuffer = new char[1024];
		private const int stringBufferLength = 1024;
		private int stringBufferPos = 0; 
		private const int maxLengthToPool = 32;


		public JsonReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new Exception("reader == null");
			}
			this.reader = reader;
		}

		public JsonReader(string str)
		{
			this.reader = new StringReader(str);
		}

		public JsonElement ToJsonElement()
		{
			Init();
			JsonElement je = ReadJsonElement();
			Close();
			return je;
		}

		public JsonElement ReadJsonElement()
		{
			try
			{
				if (PeekSkipWhite() == -1)
				{
					return null;
				}
				else
				{
					return ReadValue();
				}
			}
			catch (JsonParseException je)
			{
				throw je;
			}
			catch (Exception e)
			{
				throw CreateJsonParseException("An exception occurred while reading the json text.", e);
			}
		}

		private void Init()
		{
			line = 1;
			column = 0;
			position = 0;
			charQueue.Clear();
		}

		private JsonElement ReadValue()
		{
			int c = PeekSkipWhite();
			switch (c)
			{
				case '"':
				case '\'':
					return ReadString((char)c);
				case '-':
				case '+':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case '.':
					return ReadNumber();
				case '[':
					return ReadArray();
				case '{':
					return ReadMap();
				case 'F':
				case 'f':
					ReadFalse();
					return new JsonBoolean(false);
				case 'T':
				case 't':
					ReadTrue();
					return new JsonBoolean(true);
				case 'N':
				case 'n':
					ReadNull();
					return JsonNull.Instance;
				default:
					Read();
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the beginning of a value.", (char)c));
			}
		}

		private JsonMap ReadMap()
		{
			int c = ReadSkipWhite();
			if (c != '{')
			{
				if (c == -1)
				{
					throw CreateJsonParseException("Unstarted map, '{' is expected.");
				}
				throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the beginning of the map.", c));
			}
			JsonMap jm = new JsonMap();
			c = PeekSkipWhite();
			if (c == '}')
			{
				Read();
				return jm;
			}
			while (true)
			{
				string key = ReadKey();
				c = ReadSkipWhite();
				if (c != ':')
				{
					if (c == -1)
					{
						throw CreateJsonParseException("Unterminated map, next ':' is expected.");
					}
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' after the key '{1}'.", (char)c, key));
				}
				JsonElement value = ReadValue();
				jm[key] = value;
				c = ReadSkipWhite();
				if (c == '}')
				{
					break;
				}
				if (c == -1)
				{
					throw CreateJsonParseException("Unterminated map, '}' or next ',' is expected.");
				}
				if (c != ',')
				{
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' after a value in the map scope.", (char)c));
				}
				else
				{
					if (PeekSkipWhite() == '}')
					{
						Read();
						break;
					}
				}
			}
			return jm;
		}

		private JsonArray ReadArray()
		{
			int c = ReadSkipWhite();
			if (c != '[')
			{
				if (c == -1)
				{
					throw CreateJsonParseException("Unstarted array, '[' is expected.");
				}
				throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the beginning of an array.", (char)c));
			}
			JsonArray ja = new JsonArray();
			c = PeekSkipWhite();
			if (c == ']')
			{
				Read();
				return ja;
			}
			if (c == -1)
			{
				throw CreateJsonParseException("Unterminated array, ']' or next value is expected.");
			}
			while (true)
			{
				JsonElement value = ReadValue();
				ja.Add(value);
				c = ReadSkipWhite();
				if (c == ']')
				{
					break;
				}
				if (c == -1)
				{
					throw CreateJsonParseException("Unterminated array, ']' or next ',' is expected.");
				}
				if (c != ',')
				{
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' after a value in the array scope.", (char)c));
				}
				else
				{
					if (PeekSkipWhite() == ']')
					{
						Read();
						break;
					}
				}
			}
			return ja;
		}

		private JsonNumber ReadNumber()
		{
			bool? isNegative = null;
			int radix = 0;
			List<char> chars = new List<char>();
			int c = ReadSkipWhite();
			if (c == '-')
			{
				isNegative = true;
				c = ReadSkipWhite();
			}
			else if (c == '+')
			{
				isNegative = false;
				c = ReadSkipWhite();
			}
			if (c == '0')
			{
				c = Peek();
				if (c == '.')
				{
					if (isNegative == true)
					{
						chars.Add('-');
					}
					if (isNegative == false)
					{
						chars.Add('+');
					}
					chars.Add('0');
					while (NumberCharType.IsDigit(c = Peek()) || c == '.' || c == 'e' || c == 'E' || c == '+' || c == '-')
					{
						chars.Add((char)c);
						Read();
					}
					try
					{
						return new JsonNumber(new String(chars.ToArray()));
					}
					catch (Exception e)
					{
						throw CreateJsonParseException(String.Format("Fail to parse number '{0}'", new String(chars.ToArray())), e);
					}
				}
				if (c == 'x' || c == 'X')
				{
					radix = 16;
					Read();
					while (NumberCharType.IsHexDigit(c = Peek()))
					{
						chars.Add((char)c);
						Read();
					}
				}
				else if (c == 'o' || c == 'O')
				{
					radix = 8;
					Read();
					while (NumberCharType.IsOctDigit(c = Peek()))
					{
						chars.Add((char)c);
						Read();
					}
				}
				else if (c == 'b' || c == 'B')
				{
					radix = 2;
					Read();
					while (NumberCharType.IsBinaryDigit(c = Peek()))
					{
						chars.Add((char)c);
						Read();
					}
				}
				else if (NumberCharType.IsDigit(c))
				{
					radix = 8;
					chars.Add('0');
					while (NumberCharType.IsDigit(c = Peek()))
					{
						chars.Add((char)c);
						if (!NumberCharType.IsOctDigit(c))
						{
							radix = 10;
						}
						Read();
					}
				}
				else
				{
					return new JsonNumber(0);
				}
				long value = 0;
				try
				{
					value = Convert.ToInt64(new String(chars.ToArray()), radix);
					if (isNegative == true)
					{
						checked
						{
							value = 0 - value;
						}
					}
				}
				catch (Exception e)
				{
					throw CreateJsonParseException(String.Format("Fail to parse number '{0}'", new String(chars.ToArray())), e);
				}
				return new JsonNumber(value);
			}
			else if (NumberCharType.IsDigit((char)c) || c == '.')
			{
				if (isNegative == true)
				{
					chars.Add('-');
				}
				if (isNegative == false)
				{
					chars.Add('+');
				}
				chars.Add((char)c);
				while (NumberCharType.IsDigit(c = Peek()) || c == '.' || c == 'e' || c == 'E' || c == '+' || c == '-')
				{
					chars.Add((char)c);
					Read();
				}
				try
				{
					return new JsonNumber(new String(chars.ToArray()));
				}
				catch (Exception e)
				{
					throw CreateJsonParseException(String.Format("Fail to parse number '{0}'.", new String(chars.ToArray())), e);
				}
			}
			else if (c == -1)
			{
				throw CreateJsonParseException("Unstarted number, unexpected end.");
			}
			else
			{
				throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the beginning of the number or after the sign.", (char)c));
			}
		}

		private JsonString ReadString(char quote)
		{
			int c = ReadSkipWhite();
			if (c != quote)
			{
				if (c == -1)
				{
					throw CreateJsonParseException(String.Format("Unstarted string, {0} is expected.", quote));
				}
				throw CreateJsonParseException(String.Format("Unexpected char {0} at the beginning of a string.", (char)c));
			}
			for (stringBufferPos = 0; stringBufferPos < maxLengthToPool; stringBufferPos++)
			{
				c = Read();
				if (c == quote)
				{
					return stringPool.Intern(stringBuffer, stringBufferPos);
				}
				else if (c == -1)
				{
					throw CreateJsonParseException("Unterminated string.");
				}
				else if (c == '\\')
				{
					stringBuffer[stringBufferPos] = ReadEscapeChar();
				}
				else
				{
					stringBuffer[stringBufferPos] = (char)c;
				}
			}
			StringBuilder sb = new StringBuilder().Append(stringBuffer, 0, stringBufferPos);
			while (true)
			{
				for (stringBufferPos = 0; stringBufferPos < stringBuffer.Length; stringBufferPos++)
				{
					c = Read();
					if (c == quote)
					{
						break;
					}
					else if (c == -1)
					{
						throw CreateJsonParseException("Unterminated string.");
					}
					else if (c == '\\')
					{
						stringBuffer[stringBufferPos] = ReadEscapeChar();
					}
					else
					{
						stringBuffer[stringBufferPos] = (char)c;
					}
				}
				sb.Append(stringBuffer, 0, stringBufferPos);
				if (stringBufferPos < stringBuffer.Length)
				{
					break;
				}
			}
			return new JsonString(sb.ToString());
		}

		private char ReadEscapeChar()
		{
			int c = Read();
			switch (c)
			{
				case -1:
					throw CreateJsonParseException("Unterminated escape char.");
				case 'u':
					if (ReadTo(shortBuffer, 0, 4) < 4)
					{
						throw CreateJsonParseException("Unterminated escape char.");
					}
					int number = 0;
					for (int i = 0; i < 4; i++)
					{
						char hexChar = shortBuffer[i];
						if (hexChar >= 0x30 && hexChar <= 0x39)
						{
							number = (number << 4) + (hexChar - 0x30);
						}
						else if (hexChar >= 0x41 && hexChar <= 0x46)
						{
							number = (number << 4) + (hexChar - 0x37);
						}
						else if (hexChar >= 0x61 && hexChar <= 0x66)
						{
							number = (number << 4) + (hexChar - 0x57);
						}
						else
						{
							throw CreateJsonParseException(String.Format("Unexpected char '{0}' in a unicode escaped char."), null, i - 3);
						}
					}
					return (char)number;
				case 't':
					return '\t';
				case 'b':
					return '\b';
				case 'n':
					return '\n';
				case 'r':
					return '\r';
				case 'f':
					return '\f';
				case '\'':
					return '\'';
				case '"':
					return '"';
				case '\\':
					return '\\';
				default:
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the beginning of a escaped char.", (char)c));
			}
		}

		private void ReadFalse()
		{
			int c = ReadSkipWhite();
			if (c != 'F' && c != 'f')
			{
				if (c == -1)
				{
					throw CreateJsonParseException("Unstarted false, 'F' or 'f' is expected.");
				}
				throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the beginning of false.", c));
			}
			int c1 = Peek();
			if (c1 == 'A' || c1 == 'a')
			{
				int count = ReadTo(shortBuffer, 0, 4);
				if (count < 4)
				{
					throw CreateJsonParseException("Unterminated 'false' token.");
				}
				if (shortBuffer[1] != 'L' && shortBuffer[1] != 'l')
				{
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the 3rd position of false.", shortBuffer[1]), null, -2);
				}
				if (shortBuffer[2] != 'S' && shortBuffer[2] != 's')
				{
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the 4th position of false.", shortBuffer[2]), null, -1);
				}
				if (shortBuffer[3] != 'E' && shortBuffer[3] != 'e')
				{
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the 5th position of false.", shortBuffer[3]));
				}
			}
		}

		private void ReadTrue()
		{
			int c = ReadSkipWhite();
			if (c != 'T' && c != 't')
			{
				if (c == -1)
				{
					throw CreateJsonParseException("Unstarted true, 'T or 't' is expected.");
				}
				throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the beginning of true.", (char)c));
			}
			int c1 = Peek();
			if (c1 == 'R' || c1 == 'r')
			{
				int count = ReadTo(shortBuffer, 0, 3);
				if (count < 3)
				{
					throw CreateJsonParseException("Unterminated 'true' token.");
				}
				if (shortBuffer[1] != 'U' && shortBuffer[1] != 'u')
				{
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the 3rd position of true.", shortBuffer[1]));
				}
				if (shortBuffer[2] != 'E' && shortBuffer[2] != 'e')
				{
					throw CreateJsonParseException(String.Format("Unexpected char '{0}' at the 4th position of true.", shortBuffer[2]));
				}
			}
		}

		private void ReadNull()
		{
			int c = ReadSkipWhite();
			if (c != 'N' && c != 'n')
			{
				if (c == -1)
				{
					throw CreateJsonParseException("Unstarted null token, 'N' or 'n' is expected.");
				}
				throw CreateJsonParseException(String.Format("Unexpected char code at the beginning of null: {0}.", c));
			}
			int count = ReadTo(shortBuffer, 0, 3);
			if (count < 3)
			{
				throw CreateJsonParseException("Unterminated 'null' token.");
			}
			if (shortBuffer[0] != 'U' && shortBuffer[0] != 'u')
			{
				throw CreateJsonParseException(String.Format("Unexpected char code at the 2nd position of null: {0}.", shortBuffer[1]), null, -2);
			}
			if (shortBuffer[1] != 'L' && shortBuffer[1] != 'l')
			{
				throw CreateJsonParseException(String.Format("Unexpected char code at the 3rd position of null: {0}.", shortBuffer[1]), null, -1);
			}
			if (shortBuffer[2] != 'L' && shortBuffer[2] != 'l')
			{
				throw CreateJsonParseException(String.Format("Unexpected char code at the 4th position of null: {0}.", shortBuffer[2]));
			}
		}

		private string ReadKey()
		{
			int c = PeekSkipWhite();
			switch (c)
			{
				case '"':
				case '\'':
					return (string)ReadString((char)c);
				case '-':
				case '+':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case '.':
					return (string)ReadNumber();
				case -1:
					throw CreateJsonParseException("Unstarted key.");
				default:
					return ReadVarName();
			}
		}

		private string ReadVarName()
		{
			int c = ReadSkipWhite();
			if (c == -1)
			{
				throw CreateJsonParseException("Unterminated variable name.");
			}
			if (varNameCharMap[c] != VarNameCharType.AsFirst)
			{
				throw CreateJsonParseException(String.Format("Unexpected char '{0}' as the first character of the variable name.", (char)c));
			}
			stringBuffer[0] = (char)c;
			for (stringBufferPos = 1; stringBufferPos < maxLengthToPool; stringBufferPos++)
			{
				c = PeekSkipWhite();
				if (c == -1 || varNameCharMap[c] == VarNameCharType.Invalid)
				{
					return stringPool.Intern(stringBuffer, stringBufferPos);
				}
				else
				{
					stringBuffer[stringBufferPos] = (char)c;
				}
				Read();
			}
			StringBuilder sb = new StringBuilder().Append(stringBuffer, 0, stringBufferPos);
			while (true)
			{
				for (stringBufferPos = 0; stringBufferPos < stringBuffer.Length; stringBufferPos++)
				{
					c = PeekSkipWhite();
					if (c == -1 || varNameCharMap[c] == VarNameCharType.Invalid)
					{
						break;
					}
					else
					{
						stringBuffer[stringBufferPos] = (char)c;
					}
					Read();
				}
				sb.Append(stringBuffer, 0, stringBufferPos);
				if (stringBufferPos < stringBuffer.Length)
				{
					break;
				}
			}
			return sb.ToString();
		}

		private int ReadSkipWhite()
		{
			int c = PeekSkipWhite();
			Read();
			return c;
		}

		private int PeekSkipWhite()
		{
			while (true)
			{
				int c = Peek();
				if (c == '/')
				{
					Read();
					onCommentBegin();
				}
				else if (c == -1 || whiteCharMap[c] == WhiteCharType.Etc)
				{
					return c;
				}
				else
				{
					Read();
				}
			}
		}

		private void onCommentBegin()
		{
			int c = Read();
			if (c == '/')
			{
				onDoubleSlash();
			}
			else if (c == '*')
			{
				onSlashAsterisk();
			}
			else
			{
				throw CreateJsonParseException(String.Format("Unexpected char '{0}' as the second character of the comment.", (char)c));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void onDoubleSlash()
		{
			while (true)
			{
				int c = Read();
				if (c == '\n' || c == '\r' || c == -1)
				{
					break;
				}
			}
		}

		private void onSlashAsterisk()
		{
			while (true)
			{
				int c = Read();
				if (c == '*')
				{
					if (Peek() == '/')
					{
						Read();
						break;
					}
				}
				else if (c == -1)
				{
					throw CreateJsonParseException("Comment unterminated.");
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int ReadTo(char[] buffer, int index, int count)
		{
			int number = 0;
			int end = index + count;
			for (int i = index; i < end; i++)
			{
				int c = Read();
				if (c == -1)
				{
					break;
				}
				buffer[i] = (char)c;
				number ++;
			}
			return number;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int Read()
		{
			if (peekBlockBuffer == null)
			{
				_readBuffer();
			}
			int c = peekBlockBuffer.GetValueOrDefault();
			_readBuffer();
			return c;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int Peek()
		{
			if (peekBlockBuffer == null)
			{
				_readBuffer();
			}
			return peekBlockBuffer.GetValueOrDefault();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void _readBuffer()
		{
			while (true)
			{
				int c = reader.Read();
				peekBlockBuffer = c;
				if (c != -1)
				{
					if (charQueue.Count > charQueueMaxLength)
					{
						charQueue.Dequeue();
					}
					position++;
					if (c == '\r')
					{
						line++;
						column = 0;
					}
					else if (c == '\n' && charQueue.LastOrDefault() != '\r')
					{
						line++;
						column = 0;
					}
					else
					{
						column++;
					}
					charQueue.Enqueue((char)c);
				}
				if (c == -1 || ignoredCharMap[c] == IgnoredCharType.Etc)
				{
					break;
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool HasNextJsonElement()
		{
			return PeekSkipWhite() != -1;
		}

		public void Close()
		{
			reader.Close();
		}

		private JsonParseException CreateJsonParseException(string message, Exception innerException = null, int positionOffset = 0)
		{
			return new JsonParseException(message, innerException, position + positionOffset, line, column + positionOffset, new String(charQueue.ToArray()));
		}

	}
}