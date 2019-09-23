using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace EjemploLexer.Sintatico
{
    public class Parser
    {
        private Lexer _lexer;
        private Token _currenToken;
        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _currenToken = lexer.GetNextToken();
        }


        public void Parse()
        {
            Codigo();
            if(_currenToken.Type!=TokenTypes.EOF)
                throw new Exception("Se esperaba Eof");

        }

        private void Codigo()
        {
            if (_currenToken.Type == TokenTypes.STATIC_TYPE)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
            }
                
            ListaSentencias();
;           
        }

        private void ListaSentencias()
        {
            //Lista_Sentencias->funcName Lista_Sentencias
            if (_currenToken.Type==TokenTypes.VOID_TYPE||  _currenToken.Type ==TokenTypes.STRING_TYPE || _currenToken.Type == TokenTypes.BOOLEAN_TYPE
                || _currenToken.Type == TokenTypes.INT_TYPE || _currenToken.Type == TokenTypes.REAL_TYPE)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                funcName();
                body();
            }
            //Lista_Sentencia->Epsilon
            else
            {
                
            }
            
        }

        private void paramList()
        {
            if (_currenToken.Type == TokenTypes.STRING_TYPE || _currenToken.Type == TokenTypes.BOOLEAN_TYPE 
            || _currenToken.Type == TokenTypes.INT_TYPE || _currenToken.Type == TokenTypes.REAL_TYPE)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                if (_currenToken.Type != TokenTypes.ID)
                {
                    Console.WriteLine(_currenToken);
                    _currenToken = _lexer.GetNextToken();
                    if (_currenToken.Lexeme == ",")
                    {
                        Console.WriteLine(_currenToken);
                        _currenToken = _lexer.GetNextToken();
                        paramList();
                    }
                   
                }
                else
                {
                    throw new Exception("Se espera un tipo de valor para el parametro");
                }
            }
            
           
        }

        private void funcName()
        {
            //id = expresion ;
            if (_currenToken.Type == TokenTypes.ID)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                if(_currenToken.Type!=TokenTypes.PARENTESIS_OPEN)
                    throw new Exception("Se esperaba ( ");
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                paramList();
                if(_currenToken.Type!=TokenTypes.PARENTESIS_CLOSE)
                    throw  new Exception("Se esperaba un )");
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();

            }
            else
            {
                throw new Exception("se espara un nombre para la funcion");
            }
        }

        private void body()
        {
            if (_currenToken.Type == TokenTypes.LLAVE_OPEN)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
               stmts();
           
            }
        }

        private void stmts()
        {
            if (_currenToken.Type == TokenTypes.WHILE)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                Expresion();
                bodyBlock();
                stmts();
            }
            else if (_currenToken.Type == TokenTypes.IF)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                Expresion();
                bodyBlock();
                stmts();
            }
            else if (_currenToken.Type == TokenTypes.LLAVE_CLOSE)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
            }
            else
            {
                stmt();
                stmts();
            }
        }

        private void bodyBlock()
        {
            if(_currenToken.Type != TokenTypes.LLAVE_OPEN )
                throw new Exception("se espera una {");
            Console.WriteLine(_currenToken);
            _currenToken = _lexer.GetNextToken();
            stmt();
            if(_currenToken.Type!=TokenTypes.LLAVE_CLOSE)
                throw new Exception("se epera un }");
            Console.WriteLine(_currenToken);
            _currenToken = _lexer.GetNextToken();
        }

        private void stmt()
        {
            if (_currenToken.Type == TokenTypes.STRING_TYPE || _currenToken.Type == TokenTypes.BOOLEAN_TYPE
                || _currenToken.Type == TokenTypes.INT_TYPE || _currenToken.Type == TokenTypes.REAL_TYPE)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                if (_currenToken.Type != TokenTypes.ID)
                    throw new Exception("Se espera un nombre para variable");
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                if (_currenToken.Type == TokenTypes.ASIG)
                {
                    Console.WriteLine(_currenToken);
                    _currenToken = _lexer.GetNextToken();
                    if (_currenToken.Type == TokenTypes.ID || _currenToken.Type == TokenTypes.Digit)
                    {
                        Console.WriteLine(_currenToken);
                        _currenToken = _lexer.GetNextToken();
                        operation();
                    }
                    else
                    {
                        throw new Exception("se espera un valor para asignar");
                    }
                }

                if (_currenToken.Type == TokenTypes.FN_STM)
                {
                    Console.WriteLine(_currenToken);
                    _currenToken = _lexer.GetNextToken();
                }
                else
                {
                    throw new Exception("se espera un ;");
                }

                stmt();
            }
           
        }

        private void operation()
        {
                
                if (_currenToken.Type == TokenTypes.ADD_OP || _currenToken.Type == TokenTypes.SUB_OP
                  || _currenToken.Type == TokenTypes.MULT_OP || _currenToken.Type == TokenTypes.DIV_OP)
                {
                    Console.WriteLine(_currenToken);
                    _currenToken = _lexer.GetNextToken();
                    if (_currenToken.Type == TokenTypes.ID || _currenToken.Type == TokenTypes.Digit)
                    {
                        Console.WriteLine(_currenToken);
                        _currenToken = _lexer.GetNextToken();
                        operation();
                    }

                }
            
            
        }
        private void Expresion()
        {
            if(_currenToken.Type!=TokenTypes.PARENTESIS_OPEN)
                throw new Exception("se espera un (");
            Console.WriteLine(_currenToken);
            _currenToken = _lexer.GetNextToken();
            Term();
            if (_currenToken.Type != TokenTypes.PARENTESIS_CLOSE) 
                throw new Exception("se espera un )");
            Console.WriteLine(_currenToken);
            _currenToken = _lexer.GetNextToken();

        }

        private void ExpresionP()
        {
            //+term ExpresionP
            if (_currenToken.Type == TokenTypes.ADD_OP)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                Term();
                ExpresionP();
            }
            //-term ExpresionP
            else if (_currenToken.Type == TokenTypes.SUB_OP)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                Term();
                ExpresionP();
            }
            else if(_currenToken.Type == TokenTypes.ASIG)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                if (_currenToken.Type == TokenTypes.ASIG)
                {
                    Console.WriteLine(_currenToken);
                    _currenToken = _lexer.GetNextToken();
                    Term();
                    ExpresionP();
                }
                else
                {
                    throw  new Exception("se espera un = para comparar");
                }
            }
            // Epsilon
            else
            {
               
            }
        }

        private void Term()
        {
            Factor();
            TermP();
        }
        private void TermP()
        {
            //*Factor TermP
            if (_currenToken.Type == TokenTypes.MULT_OP)
            {
                _currenToken = _lexer.GetNextToken();
                Factor();
                TermP();
            }
            // / Factor TermP
            else if (_currenToken.Type == TokenTypes.DIV_OP)
            {
                _currenToken = _lexer.GetNextToken();
                Factor();
                TermP();
            }
            // Epsilon
            else
            {

            }
        }

        private void Factor()
        {
            if (_currenToken.Type == TokenTypes.ID)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                ExpresionP();
            }
            else if (_currenToken.Type == TokenTypes.Digit)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
                ExpresionP();
            }
            else if (_currenToken.Type == TokenTypes.TRUE_LITERAL)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
            }
            else if (_currenToken.Type == TokenTypes.FALSE_LITERAL)
            {
                Console.WriteLine(_currenToken);
                _currenToken = _lexer.GetNextToken();
            }

            else
            {
                throw new Exception("Se esperaba una factor");
            }
        }

    }
}
