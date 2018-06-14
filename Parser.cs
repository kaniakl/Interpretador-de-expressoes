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
            Arvore arvore = null;

            if(!ValidaPrograma(token))
            {
                return null;
            }

            if(Regex.IsMatch(token.Texto, Dicionario.ATRIBUIDOR))
            {
                //Não é apenas uma expressão;
                if(Regex.IsMatch(token.Texto, Dicionario.REGEX_OPERADORES))
                {
                    List<string> split = token.Texto.Split('=').ToList();
                    string variavel = split[0].ToString();
                    List<string> expressao = split[1].Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();

                    arvore = new Arvore()
                    {
                        NoPrincipal = new No()
                    };
                    MontaArvoreDaExpressao(arvore.NoPrincipal, expressao);
                }
                else
                {
                    List<string> declaracaoVariavelSimples = token.Texto.Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x) && !x.Equals("var")).ToList();
                    arvore = new Arvore()
                    {
                        NoPrincipal = new No()
                        {
                            Valor = declaracaoVariavelSimples[1],
                            NoDireito = new No()
                            {
                                Valor = declaracaoVariavelSimples[2]
                            },
                            NoEsquerda = new No()
                            {
                                Valor = declaracaoVariavelSimples[0]
                            }
                        }
                    };
                }
            }

            return null;
        }

        public static void MontaArvoreDaExpressao(No no, List<string> expressao)
        {
            //string valor = String.Empty;
            //for(int i = expressao.Count - 1; i > 0; i--)
            //{
            //    if(Regex.IsMatch(expressao[i], Dicionario.DELIMITADORES))
            //    {
            //        valor = expressao[i].ToString();
            //        string expressaoSplit = String.Join<string>(" ", expressao);
            //        List<string> expressaoTratada = expressaoSplit.Split(valor[0]).ToList();
            //        List<string> expressaoDireita = expressaoTratada[0].ToString().Split(' ').ToList();
            //        List<string> expressaoEsquerda = expressaoTratada[1].ToString().Split(' ').ToList();
            //        no.Valor = valor;
            //        no.NoDireito = new No();
            //        no.NoEsquerda = new No();
            //        MontaArvoreDaExpressao(no.NoDireito, expressaoDireita);
            //        MontaArvoreDaExpressao(no.NoEsquerda, expressaoEsquerda);
            //    }
            //}
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
