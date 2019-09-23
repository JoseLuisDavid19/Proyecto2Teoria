using System;
using System.Collections.Generic;

namespace EjemploLexer
{
     public partial class Lexer
     {
         private string _sourceCode;
         private int _currentPointer;
         private Dictionary<string, TokenTypes> _reserveWords;
         private Dictionary<string, TokenTypes> _symbols;
        public Lexer(string sourceCode)
        {
            _sourceCode = sourceCode;
            _currentPointer = 0;
            _reserveWords = new Dictionary<string, TokenTypes>();
            _reserveWords.Add("while", TokenTypes.WHILE);
            _reserveWords.Add("static", TokenTypes.STATIC_TYPE);
            _reserveWords.Add("if", TokenTypes.IF);
            _reserveWords.Add("then", TokenTypes.IF_THEN);
            _reserveWords.Add("int", TokenTypes.INT_TYPE);
            _reserveWords.Add("boolean", TokenTypes.BOOLEAN_TYPE);
            _reserveWords.Add("void", TokenTypes.VOID_TYPE);
            _reserveWords.Add("real", TokenTypes.REAL_TYPE);
            _reserveWords.Add("string", TokenTypes.STRING_TYPE);
            _reserveWords.Add("true", TokenTypes.TRUE_LITERAL);
            _reserveWords.Add("false", TokenTypes.FALSE_LITERAL);
            _reserveWords.Add("return", TokenTypes.RETURN);
            _reserveWords.Add("BEGIN", TokenTypes.INICIO);
            _reserveWords.Add("END", TokenTypes.END);


            _symbols = new Dictionary<string, TokenTypes>();
            _symbols.Add("+", TokenTypes.ADD_OP);
            _symbols.Add("-", TokenTypes.SUB_OP);
            _symbols.Add("*", TokenTypes.MULT_OP);
            _symbols.Add("/", TokenTypes.DIV_OP);
            _symbols.Add("=", TokenTypes.ASIG);
            _symbols.Add(";", TokenTypes.FN_STM);
            _symbols.Add("(", TokenTypes.PARENTESIS_OPEN);
            _symbols.Add(")", TokenTypes.PARENTESIS_CLOSE);
            _symbols.Add("{", TokenTypes.LLAVE_OPEN);
            _symbols.Add("}", TokenTypes.LLAVE_CLOSE);
            
            

        }

        public Token GetNextToken()
        {
            var tmp = GetCurrentSymbol();
            var lexeme = "";

            while (Char.IsWhiteSpace(tmp))
            {
                _currentPointer++;
                tmp = GetCurrentSymbol();
            }
            if (tmp == '\0')
            {
                return new Token {Type = TokenTypes.EOF};
            }
            
            if (Char.IsLetter(tmp))
            {
                lexeme += tmp;
                _currentPointer++;
                return GetId(lexeme);
           
            }

            if (Char.IsDigit(tmp))
            {
                lexeme += tmp;
                _currentPointer++;
                return GetDigit(lexeme);
            }

            if (_symbols.ContainsKey(tmp.ToString()))
            {
                _currentPointer++;
                return new Token {Type = _symbols[tmp.ToString()],Lexeme = tmp.ToString()};
            }

          
            throw new LexerException("Nasty");
        }

         private Token GetDigit(string lexeme)
         {
            var tmp1 = GetCurrentSymbol();
            while (Char.IsDigit(tmp1))
            {
                lexeme += tmp1;
                _currentPointer++;
                tmp1 = GetCurrentSymbol();
            }
            return new Token { Type = TokenTypes.Digit, Lexeme = lexeme };
        }

         private Token GetId(string lexeme)
         {
             var tmp1 = GetCurrentSymbol();
             while (Char.IsLetterOrDigit(tmp1))
             {
                 lexeme += tmp1;
                 _currentPointer++;
                 tmp1 = GetCurrentSymbol();
             }

             return new Token {Type = _reserveWords.ContainsKey(lexeme)?_reserveWords[lexeme]:TokenTypes.ID
                 , Lexeme = lexeme};
         }

         private char GetCurrentSymbol()
         {
             if (_currentPointer < _sourceCode.Length)
             {
                 return _sourceCode[_currentPointer];
             }
             return '\0';
         }
    }
}