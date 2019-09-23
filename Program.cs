using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using EjemploLexer.Sintatico;

namespace EjemploLexer
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = System.IO.File.ReadAllText(@"D:\Downloads\input.g4");
            var lex = new Lexer(program);
            Parser parser = new Parser(lex);
            try
            {
                parser.Parse();
                Console.WriteLine("Es correcta la sintaxis");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();

        }
    }
}
