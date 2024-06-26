using System;
using System.Collections.Generic;
using System.Text;

namespace GCodeNet
{
    internal class GCodeTokenizer
    {
        private string gcode;

        public GCodeTokenizer(string gcode)
        {
            //gcode = RemoveWhitespace(gcode);
            //CheckForIllegalChars(gcode);
            //gcode = gcode.ToUpper();

            this.gcode = gcode;
        }

        private void CheckForIllegalChars(string gcode)
        {
            var list = new HashSet<char>();
            for (var c='a'; c<='z';c++)
            {
                list.Add(c);
            }
            for (var c = 'A'; c <= 'Z'; c++)
            {
                list.Add(c);
            }
            for (var c = '0'; c <= '9'; c++)
            {
                list.Add(c);
            }
            list.Add('.');
            list.Add('+');
            list.Add('-');
            list.Add(':');

            foreach (var c in gcode)
            {
                if (!list.Contains(c))
                {
                    throw new System.Exception($"Illegal character in gcode: '{c}'");
                }
            }
        }

        private string RemoveWhitespace(string str)
        {
            //this.gcode = Regex.Replace(gcode, @"\s+", ""); This is a bit slower
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if (!char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public IEnumerable<string[]> GetCommandTokens()
        {
            var isFirstToken = true;

            var tokens = new List<string>();
            foreach (var token in GetTokens())
            {
                if (isFirstToken)
                {
                    if (!IsValidCommandType(token))
                    {
                        throw new Exception("The first token must be a valid CommandType: G,M,N, etc");
                    }
                    isFirstToken = false;
                }

                if (tokens.Count == 2 && tokens[0] == "M" && tokens[1] == "117")
                {
                }
                else if (!IsValidParameter(token) && !IsValidCommandType(token) && !IsParameterValue(token))
                {
                    throw new Exception("Invalid token: " + token);
                }

                if (IsValidCommandType(token))
                {
                    if (tokens.Count == 1)
                    {
                        throw new Exception("A command subtype must always follow a command type.");
                    }
                    else if (tokens.Count > 1)
                    {
                        yield return tokens.ToArray();
                    }
                    tokens.Clear();
                }
                tokens.Add(token);
            }

            if (tokens.Count == 1)
            {
                throw new Exception("A command subtype must always follow a command type.");
            }
            else if (tokens.Count > 1)
            {
                yield return tokens.ToArray();
            }
        }

        private bool IsValidCommandType(string token)
        {
            return Enum.IsDefined(typeof(CommandType), token);
        }

        private bool IsValidParameter(string token)
        {
            if (token[0] == '"' && token[token.Length - 1] == '"')
                return true;
            return Enum.IsDefined(typeof(ParameterType), token);
        }

        private bool IsParameterValue(string token)
        {
            decimal tmp;
            return decimal.TryParse(token, out tmp);
        }

        public IEnumerable<string> GetTokens()
        {
            var buff = new StringBuilder();

            var lastToken = "";

            var i = 0;
            while(i < gcode.Length)
            {
                var token = GetNextToken(ref i);
                if (!string.IsNullOrEmpty(token))
                {
                    yield return token;
                }

                //This is a special formatting case for M117
                if (lastToken == "M" && token == "117")
                {
                    var displayStr = ReadUntilEndOfLine(ref i);
                    yield return ParameterType.D.ToString(); //Assign any param to this string because one is not provided.
                    yield return '"' + displayStr + '"';
                }
                lastToken = token;
            }
        }

        private string ReadUntilEndOfLine(ref int i)
        {
            ConsumeWhiteSpace(ref i);

            var token = new StringBuilder();
            while (i < gcode.Length)
            {
                var c = gcode[i];
                if (c == '\n')
                {
                    return token.ToString().TrimEnd('\n', '\r');
                }
                else
                {
                    token.Append(c);
                    i++;
                }
            }
            return token.ToString().TrimEnd('\n','\r');
        }

        private void ConsumeWhiteSpace(ref int i)
        {
            while (i < gcode.Length)
            {
                var c = gcode[i];
                if (!char.IsWhiteSpace(c))
                {
                    break;
                }
                i++;
            }
        }

        private string GetNextToken(ref int i)
        {
            ConsumeWhiteSpace(ref i);
            if (i >= gcode.Length) return null;

            if (char.IsLetter(gcode[i]))
            {
                return gcode[i++].ToString().ToUpper();
            }

            var token = new StringBuilder();
            while (i < gcode.Length)
            {
                var c = gcode[i];
                if ((c >= '0' && c <= '9') || c == '+' || c == '-' || c == '.' || c == ':')
                {
                    token.Append(c);
                    i++;
                }
                else 
                {
                    if (token.Length == 0) throw new Exception("invalid token character");
                    return token.ToString().ToUpper();
                }
            }

            if (token.Length > 0)
            {
                return token.ToString();
            }
            return null;
        }
    }

}