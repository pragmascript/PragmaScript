using System;
using System.Collections.Generic;
using System.Text;

namespace PragmaScript
{
    public static class Preprocessor
    {
        static void skipWhitespace(string text, ref int idx)
        {
            while (char.IsWhiteSpace(text.at(idx)))
            {
                idx++;
            }
        }
        class PrepIf
        {
            public bool Condition;
            public bool InElse;
        }
        public static string Preprocess(string text, Platform platform)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            HashSet<string> defines = new HashSet<string>();
            defines.Add("TRUE");
            if (CompilerOptionsBuild._i.debug)
            {
                defines.Add("DEBUG");
            }
            else
            {
                defines.Add("RELEASE");
            }


            if (platform == Platform.WindowsX64)
            {
                defines.Add("PLATFORM_WINDOWS");
                CompilerOptionsBuild._i.libs.Add("kernel32.lib");
                CompilerOptionsBuild._i.lib_path.Add(Program.RelDir("lib"));
            }
            else if (platform == Platform.LinuxX64)
            {
                defines.Add("PLATFORM_LINUX");
            }

            StringBuilder result = new StringBuilder(text.Length);
            int idx = 0;


            Stack<PrepIf> ifs = new Stack<PrepIf>();

            bool inside_line_comment = false;
            bool inside_string = false;

            Func<string, bool> nextString = (s) =>
            {
                for (int i = 0; i < s.Length; ++i)
                {
                    if (char.ToUpper(text.at(idx + 1 + i)) != s[i])
                    {
                        return false;
                    }
                }
                return true;
            };
            while (true)
            {
                if (text.at(idx - 1) == '/' && text.at(idx) == '/')
                {
                    inside_line_comment = true;
                }
                if (text.at(idx) == '"')
                {
                    if (inside_string)
                    {
                        if (text.at(idx - 1) != '\\')
                        {
                            inside_string = false;
                        }
                    }
                    else
                    {
                        inside_string = true;
                    }
                }

                if (!inside_string && !inside_line_comment && text.at(idx) == '#')
                {
                    if (nextString("IF"))
                    {
                        idx += 3;
                        if (idx >= text.Length)
                        {
                            return result.ToString();
                        }
                        skipWhitespace(text, ref idx);
                        if (idx >= text.Length)
                        {
                            return result.ToString();
                        }
                        int ident_start = idx;
                        idx--;
                        while (Token.isIdentifierChar(text.at(++idx))) { }
                        int ident_end = idx;
                        var ident = text.Substring(ident_start, ident_end - ident_start);
                        var con = defines.Contains(ident);
                        ifs.Push(new PrepIf { Condition = con, InElse = false });
                    }
                    else if (nextString("DEFINE"))
                    {
                        idx += 7;
                        if (idx >= text.Length)
                        {
                            return result.ToString();
                        }
                        skipWhitespace(text, ref idx);
                        if (idx >= text.Length)
                        {
                            return result.ToString();
                        }
                        int ident_start = idx;
                        idx--;
                        while (Token.isIdentifierChar(text.at(++idx))) { }
                        int ident_end = idx;
                        var ident = text.Substring(ident_start, ident_end - ident_start);
                        defines.Add(ident);
                        Console.WriteLine($"#DEFINE {ident}");
                    }
                    else if (nextString("UNDEF"))
                    {
                        idx += 7;
                        if (idx >= text.Length)
                        {
                            return result.ToString();
                        }
                        skipWhitespace(text, ref idx);
                        if (idx >= text.Length)
                        {
                            return result.ToString();
                        }
                        int ident_start = idx;
                        idx--;
                        while (Token.isIdentifierChar(text.at(++idx))) { }
                        int ident_end = idx;
                        var ident = text.Substring(ident_start, ident_end - ident_start);
                        defines.Remove(ident);
                        Console.WriteLine($"#UNDEF {ident}");
                    }
                    else
                    {
                        int ident_start = idx + 1;
                        while (Token.isIdentifierChar(text.at(++idx))) { }
                        int ident_end = idx;
                        var ident = text.Substring(ident_start, ident_end - ident_start);

                        if (ident.ToUpper() == "ELSE")
                        {
                            ifs.Peek().InElse = true;
                        }
                        else if (ident.ToUpper() == "ENDIF")
                        {
                            ifs.Pop();
                        }
                    }
                }
                var skip = false;
                if (ifs.Count > 0)
                {
                    var _if = ifs.Peek();
                    skip = !(_if.Condition ^ _if.InElse);
                }

                // HACK: keep newlines for error reporting
                if (!skip || text[idx] == '\r' || text[idx] == '\n')
                {
                    if (text[idx] == '\n')
                    {
                        inside_line_comment = false;
                    }
                    result.Append(text[idx]);
                }
                idx++;
                if (idx >= text.Length)
                {
                    var result_string = result.ToString();
                    return result_string;
                }
            }
        }

    }
}