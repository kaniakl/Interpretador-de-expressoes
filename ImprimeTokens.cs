using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpretador
{
    public static class ImprimeTokens
    {
        public static void Imprime(List<Token> Tokens)
        {
            int count = 1;
            foreach(Token token in Tokens)
            {
                Console.Write("Linha {0}:\n", count);
                Console.Write("Variaveis: ");
                foreach (string varivais in token.Variaveis)
                {
                    Console.Write("{0} ", varivais);
                }
                Console.Write("\nNumeros: ");
                foreach(string numeros in token.Numeros)
                {
                    Console.Write("{0} ", numeros);
                }
                Console.Write("\nOperadores: ");
                foreach(string operadores in token.Operadores)
                {
                    Console.Write("{0}", operadores);
                }
                Console.Write("\n");
            }
        }
    }
}
