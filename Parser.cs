using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Interpretador
{
    public static class Parser
    {
        public static void ParserExpressao(string[] fileLines, List<Token> Tokens)
        {
            foreach (string linha in fileLines)
            {
                Token token = RotulaPalavras(linha);
                Tokens.Add(token);
            }

        }

        private static Token RotulaPalavras(string linha)
        {
            Token token = new Token();
            List<string> palavras = linha.Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();

            for (int i = 0; i < palavras.Count; i++)
            {
                if (Regex.IsMatch(palavras[i], Dicionario.REGEX_VARIAVEIS) && !palavras[i].Equals("var"))
                {
                    token.Variaveis.Add(palavras[i]);
                }

                if (Regex.IsMatch(palavras[i], Dicionario.REGEX_NUMEROS))
                {
                    token.Numeros.Add(palavras[i]);
                }

                if (Regex.IsMatch(palavras[i], Dicionario.REGEX_OPERADORES))
                {
                    token.Operadores.Add(palavras[i]);
                }
            }

            return token;
        }
    }
}
