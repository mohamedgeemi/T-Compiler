using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler_Theory
{
    public class CompilerScanner
    {
        /// <summary>
        /// Reserved Words
        /// </summary>
        enum ReservedWords
        {
            If, Then, Else, End, Repeat, Until, Read, Write
        }

        /// <summary>
        /// Token Type
        /// </summary>
        enum TokenType
        {
            EndFile, Error,
            Identifier, Number,
            Comment,ReservedWords,Char
        } 

        /// <summary>
        /// SpecialSymbol(Symbol,Name)
        /// </summary>
        struct SpecialSymbols
        {
            public string Symbol,Name;
            public SpecialSymbols(string symbol, string name)
            {
                Symbol = symbol;
                Name = name;
            }
        };
        
        /// <summary>
        /// Array of each Special Symbol and it's Name 
        /// </summary>
        SpecialSymbols[] _SpecialSymbols = { 
                                               new SpecialSymbols("+","Plus"), new SpecialSymbols("-","Minus"), new SpecialSymbols("*","Times"), new SpecialSymbols("=","Equal"),
                                               new SpecialSymbols("/","Division"), new SpecialSymbols(">","GreaterThan"), new SpecialSymbols("<","LessThan"), new SpecialSymbols("(","LeftParentheses"),
                                               new SpecialSymbols(")","RightParentheses") ,new SpecialSymbols(";","SemiColon"), new SpecialSymbols(":=","Assign"), 
                                               new SpecialSymbols("<=","LessThanOrEqual") , new SpecialSymbols(">=","GreaterThanOrEqual"), new SpecialSymbols("!=","NotEqual")};
        /// <summary>
        /// Start Scanner
        /// </summary>
        /// <param name="FileData"></param>
        /// <param name="ScannerData"></param>
        public void StartScanner(string[] FileData, ref List<KeyValuePair<string, string>> ScannerData)
        {
            string output="", result;
            bool Error,IsComment=false;
            foreach (string line in FileData)
            {
                if (line.Length > 0)
                {
                    for (int index = 0; index < line.Length; index++)
                    {
                        ////Comment
                        if (IsComment)
                        {
                            if (index + 1 < line.Length && line[index] == '*' && line[index + 1] == '/')//End of Comment
                            {
                                index++;
                                IsComment = false;
                                output += "*/";
                                result = TokenType.Comment.ToString();
                                AddScannerList(ref ScannerData, output, result);//Display current result
                            }
                            else//Continue in Comment
                            {
                                output += line[index];
                            }
                            continue;
                        }
                        else
                        {
                            if (index + 1 < line.Length && line[index] == '/' && line[index + 1] == '*')//Start of Comment
                            {
                                index++;
                                IsComment = true;
                                output = "/*";
                                continue;
                            }
                        }
                        ////
                        ////char
                        if (index+2 < line.Length &&line[index] == '\'' && line[index+2] == '\'')
                        {
                            output = "";
                            output += line[index];
                            output += line[index + 1];
                            output += line[index + 2];
                            result = TokenType.Char.ToString();
                            AddScannerList(ref ScannerData, output, result);//Display current result
                            index += 2;
                            continue;
                        }
                        ////
                        ////Identifier Or Reserved Word
                        output = Identifier(line, ref index);//Get Identifier or empty
                        result = TokenType.Identifier.ToString();
                        if (output != "")
                        {
                            if (Enum.IsDefined(typeof(ReservedWords), output))//If Identifier is Reserved Word 
                            { result = TokenType.ReservedWords.ToString(); }
                            AddScannerList(ref ScannerData, output, result);//Display current result
                            index--;
                            continue;
                        }
                        ////
                        ////Number Or Error
                        Error = false;
                        output = Number(line, ref index, ref Error);//Get Number(with Error or not) or empty
                        result = TokenType.Number.ToString();
                        if (Error)//If error number like "3a"
                            result = TokenType.Error.ToString();
                        if (output != "")
                        {
                            AddScannerList(ref ScannerData, output, result);//Display current result
                            index--;
                            continue;
                        }
                        ////
                        ////SpecialSymbols
                        if (index + 1 < line.Length)//If this is Special Symbol with 2 char
                        {
                            string Symbol = line[index].ToString() + line[index + 1].ToString();
                            string SymbolName = GetSpecialSymbol(Symbol);//Display current result
                            if (SymbolName != "")
                            {
                                index++;
                                AddScannerList(ref ScannerData, Symbol, SymbolName);//Display current result
                                continue;
                            }
                        }
                        {//If this is Special Symbol with 1 char
                            string Symbol = line[index].ToString();
                            string SymbolName = GetSpecialSymbol(Symbol);
                            if (SymbolName != "")
                            {
                                AddScannerList(ref ScannerData, Symbol, SymbolName);//Display current result
                                continue;
                            }
                        }
                        ////
                        ////Other is Error
                        if (line[index] != ' ' && line[index] != '\t')
                        {
                            AddScannerList(ref ScannerData, line[index].ToString(), TokenType.Error.ToString());//Display current result
                        }
                    }
                }
            }
            if (IsComment)//Error in comment with out it's close "*/"
            {
                AddScannerList(ref ScannerData, output, TokenType.Error.ToString());//Display current result
            }
            AddScannerList(ref ScannerData, "", TokenType.EndFile.ToString());//Display current result
        }

        /// <summary>
        /// Check char or not
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>bool</returns>
        private bool IsChar(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
                return true;
            return false;
        }

        /// <summary>
        /// Check Digit or not
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>bool</returns>
        private bool IsDigit(char ch)
        {
            if (ch >= '0' && ch <= '9') return true;
            return false;
        }

        /// <summary>
        /// Check Dot or not
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>bool</returns>
        private bool IsDot(char ch)
        {
            if (ch == '.') return true;
            return false;
        }

        /// <summary>
        /// Check E or not
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>bool</returns>
        private bool IsE(char ch)
        {
            if (ch == 'E') return true;
            return false;
        }

        /// <summary>
        /// Check + or - or not
        /// </summary>
        /// <param name="ch"></param>
        /// <returns>bool</returns>
        private bool IsSign(char ch)
        {
            if (ch == '+' || ch == '-') return true;
            return false;
        }

        /// <summary>
        /// Get Identifier and stop index
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns>string</returns>
        private string Identifier(string input, ref int index)
        {
            string output = "";
            if (IsChar(input[index]))
            {
                output += input[index++];
                while (index < input.Length && (IsChar(input[index]) || IsDigit(input[index])))
                {
                    output += input[index++];
                }
            }
            return output;
        }

        /// <summary>
        /// Get Number(with error or not) and stop index
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <param name="Error"></param>
        /// <returns>string</returns>
        private string Number(string input, ref int index, ref bool Error, string output = "",bool E =false,bool Dot=false)
        {
            if (IsDigit(input[index]))
            {
                output += input[index++];
                while (index < input.Length && (IsDigit(input[index]) || (IsChar(input[index]) && !IsE(input[index])) || IsDot(input[index])))
                {
                    if (IsChar(input[index]) || (IsDot(input[index]) && Dot==true))
                    {
                        Error = true;
                    }
                    if (IsDot(input[index]))
                    { 
                        Dot = true;
                        if (index + 1 < input.Length && IsDigit(input[index + 1]))
                            ;
                        else
                        {
                            Error = true;
                        }
                    }
                    output += input[index++];
                }
                if(index < input.Length && IsE(input[index]))
                {
                    if (E) Error = true;
                    E = true;
                    output += input[index++];
                    if (index  < input.Length && IsDigit(input[index]) )
                    {
                        return Number(input, ref index, ref Error, output,E);
                    }
                    else if (index < input.Length && IsSign(input[index]) )
                    {
                        output += input[index++];
                        if (index < input.Length && IsDigit(input[index]))
                        {
                            return Number(input, ref index, ref Error, output,E);
                        }
                        else
                        {
                            Error = true;
                        }
                    }
                    else
                    {
                        Error = true;
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// Get SpecialSymbol or empty
        /// </summary>
        /// <param name="current"></param>
        /// <returns>string</returns>
        private string GetSpecialSymbol(string current)
        {
            foreach (var SpecialSymbol in _SpecialSymbols)
            {
                string Symbol = SpecialSymbol.Symbol;
                string Name = SpecialSymbol.Name;
                if (Symbol == current)
                {
                    return Name;
                }
            }
            return "";
        }

        /// <summary>
        /// Add Token to ScannerData List
        /// </summary>
        /// <param name="ScannerData"></param>
        /// <param name="output"></param>
        /// <param name="result"></param>
        private void AddScannerList(ref List<KeyValuePair<string, string>> ScannerData, string output, string result)
        {
            ScannerData.Add(new KeyValuePair<string, string>(output, result));
        }
    }
}
