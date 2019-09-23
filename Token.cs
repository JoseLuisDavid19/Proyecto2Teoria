namespace EjemploLexer
{
    public class Token
    {
        public TokenTypes Type { get; set; }
        public string Lexeme { get; set; }

        public override string ToString()
        {
            return "Lexeme: "+ Lexeme+" Type: "+Type;
        }
    }
}