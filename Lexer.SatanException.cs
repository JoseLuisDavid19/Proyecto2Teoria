using System;

namespace EjemploLexer
{
    public partial class Lexer
    {
        public class LexerException : Exception
        {
            public LexerException(string nasty):base(nasty)
            {
                
            }
        }
    }
}