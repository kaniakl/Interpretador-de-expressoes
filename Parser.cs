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
                if(token == null)
                {
                    return;
                }
                Tokens.Add(token);
            }

        }

        private static Token RotulaPalavras(string linha)
        {
            Token token = new Token()
            {
                Texto = linha
            };
            List<string> palavras = linha.Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();
            for (int i = 0; i < palavras.Count; i++)
            {
                if (Regex.IsMatch(palavras[i], Dicionario.REGEX_VARIAVEIS) && !palavras[i].Equals("var"))
                {
                    token.Variaveis.Add(palavras[i]);
                }
                else if(Regex.IsMatch(palavras[i], Dicionario.ATRIBUIDOR))
                {
                    token.Atribuidor.Add(palavras[i]);
                }
                else if (Regex.IsMatch(palavras[i], Dicionario.REGEX_NUMEROS))
                {
                    token.Numeros.Add(palavras[i]);
                }
                else if(Regex.IsMatch(palavras[i], Dicionario.REGEX_ACESSO_VARIAVEL))
                {
                    token.AcessoValorVariaveis.Add(palavras[i]);
                }
                else if (Regex.IsMatch(palavras[i], Dicionario.REGEX_OPERADORES))
                {
                    token.Operadores.Add(palavras[i]);
                }
                else if(Regex.IsMatch(palavras[i], Dicionario.DELIMITADORES))
                {
                    token.Delimitadores.Add(palavras[i]);
                }
                else
                {
                    if (!palavras[i].Equals("var"))
                    {
                        Console.WriteLine("A palavra {0}, nao pertence a linguagem", palavras[i]);
                        return null;
                    }
                }
            }

            return token;
        }

        public static List<Arvore> CriaArvores(List<Token> Tokens)
        {
            List<Arvore> arvores = new List<Arvore>();

            foreach(Token token in Tokens)
            {
                Arvore arvore = MontaArvore(token);
                if(arvore == null)
                {
                    return null;
                }
                arvores.Add(arvore);
            }

            return null;
        }

        public static Arvore MontaArvore(Token token)
        {
            Stack<string> pilha = new Stack<string>();

            if(!ValidaPrograma(token))
            {
                return null;
            }



            return null;
        }

        public static bool ValidaPrograma(Token token)
        {
            foreach (string numero in token.Numeros)
            {
                if (!Regex.IsMatch(numero, Dicionario.REGEX_NUMEROS))
                {
                    Console.WriteLine("Este {0} esta declarado de forma errada", numero);
                    return false;
                }
            }

            foreach (string atribuidor in token.Atribuidor)
            {
                if (!Regex.IsMatch(atribuidor, Dicionario.ATRIBUIDOR))
                {
                    Console.WriteLine("Este {0} parametro esta declarado de forma errada", atribuidor);
                    return false;
                }
            }

            foreach (string variaveis in token.Variaveis)
            {
                if (!Regex.IsMatch(variaveis, Dicionario.REGEX_VARIAVEIS))
                {
                    Console.WriteLine("A variavel {0} foi declarada de forma errada", variaveis);
                    return false;
                }
            }

            foreach (string delimitador in token.Delimitadores)
            {
                if (!Regex.IsMatch(delimitador, Dicionario.DELIMITADORES))
                {
                    Console.WriteLine("O delimitador {0} foi declarado de forma errada", delimitador);
                    return false;
                }
            }

            foreach (string operador in token.Operadores)
            {
                if (!Regex.IsMatch(operador, Dicionario.REGEX_OPERADORES))
                {
                    Console.WriteLine("O operador {0} foi declarado de forma errada", operador);
                    return false;
                }
            }

            foreach(string acesso in token.AcessoValorVariaveis)
            {
                if(!Regex.IsMatch(acesso, Dicionario.REGEX_ACESSO_VARIAVEL))
                {
                    Console.WriteLine("O valor {0} foi declarado de forma errada", acesso);
                    return false;
                }
            }

            return true;
        }
    }
}
