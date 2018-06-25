using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Interpretador
{

    public class Parser
    {
        public Dictionary<string, int> variaveis = new Dictionary<string, int>();

        public static void ParserExpressao(string[] fileLines, List<Token> Tokens)
        {
            foreach (string linha in fileLines)
            {
                Token token = RotulaPalavras(linha);
                if (token == null)
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
                else if (Regex.IsMatch(palavras[i], Dicionario.ATRIBUIDOR))
                {
                    token.Atribuidor.Add(palavras[i]);
                }
                else if (Regex.IsMatch(palavras[i], Dicionario.REGEX_NUMEROS))
                {
                    token.Numeros.Add(palavras[i]);
                }
                else if (Regex.IsMatch(palavras[i], Dicionario.REGEX_ACESSO_VARIAVEL))
                {
                    token.AcessoValorVariaveis.Add(palavras[i]);
                }
                else if (Regex.IsMatch(palavras[i], Dicionario.REGEX_OPERADORES))
                {
                    token.Operadores.Add(palavras[i]);
                }
                else if (Regex.IsMatch(palavras[i], Dicionario.DELIMITADORES))
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

            foreach (Token token in Tokens)
            {
                Arvore arvore = MontaArvore(token);
                if (arvore == null)
                {
                    return null;
                }
                arvores.Add(arvore);
            }

            return arvores.ToList();
        }

        public static Arvore MontaArvore(Token token)
        {
            Arvore arvore = null;
            No auxDebug = null;

            if (!ValidaPrograma(token))
            {
                return null;
            }

            if (Regex.IsMatch(token.Texto, Dicionario.ATRIBUIDOR))
            {
                //Não é apenas uma expressão;
                if (Regex.IsMatch(token.Texto, Dicionario.REGEX_OPERADORES))
                {
                    List<string> split = token.Texto.Split('=').ToList();
                    string variavel = split[0].ToString();
                    List<string> expressao = split[1].Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();
                    List<string> ordemOp = precedencia(expressao);
                    arvore = new Arvore()
                    {
                        NoPrincipal = new No()
                    };
                    AdicionaAtribuidor(arvore.NoPrincipal, variavel);
                    auxDebug = MontaArvoreDaExpressao(arvore.NoPrincipal.NoDireito, expressao, ordemOp);
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
            else if (Regex.IsMatch(token.Texto, Dicionario.REGEX_OPERADORES))
            {
                List<string> expressao = token.Texto.Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();

                arvore = new Arvore()
                {
                    NoPrincipal = new No()
                };
                List<string> ordemOp = precedencia(expressao);
                auxDebug = MontaArvoreDaExpressao(arvore.NoPrincipal, expressao, ordemOp);
            }
            Console.WriteLine("Debug: ");
            printArvore(arvore.NoPrincipal, 0);
            return arvore;
        }


        public static void AdicionaAtribuidor(No no, string variavel)
        {
            if (String.IsNullOrEmpty(no.Valor) || no == null)
            {
                no.Valor = "=";
                no.NoDireito = new No();
                no.NoEsquerda = new No()
                {
                    Valor = variavel
                };
                return;
            }
            else
            {
                AdicionaAtribuidor(no.NoEsquerda, variavel);
            }
        }

        public static List<string> precedencia(List<string> expr)
        {
            List<string> precedenciaOp = new List<string>();
            for (int i = 0; i < expr.Count - 1; i++)
            {
                if (Regex.IsMatch(expr[i], Dicionario.REGEX_OPERADORES))
                {
                    if (expr[i] == "^")
                    {
                        precedenciaOp.Insert(0, expr[i]);
                    }
                    else if (expr[i] == "+" || expr[i] == "-")
                    {
                        precedenciaOp.Add(expr[i]);
                    }
                    else
                    {
                        if (precedenciaOp.Count() == 0)
                        {
                            precedenciaOp.Add(expr[i]);
                            break;
                        }
                        for (int itLista = 0; itLista < precedenciaOp.Count(); itLista++)
                        {
                            if (precedenciaOp[itLista] == "^")
                            {
                                continue;
                            }
                            else
                            {
                                precedenciaOp.Insert(itLista, expr[i]);
                                break;
                            }
                        }
                    }
                }
            }
            precedenciaOp.Reverse();
            return precedenciaOp;
        }

        public static No MontaArvoreDaExpressao(No no, List<string> expressao, List<string> precedenciaOp)
        {
            string valor = String.Empty;

            //Checar numeros 
            for (int i = expressao.Count - 1; i >= 0; i--)
            {
                if (Regex.IsMatch(expressao[i], Dicionario.REGEX_NUMEROS))
                {
                    valor = expressao[i].ToString();
                    string expressaoSplit = String.Join<string>(" ", expressao);
                    List<string> expressaoTratada = expressaoSplit.Split(valor[0]).ToList();
                    List<string> expressaoDireita = expressaoTratada[0].ToString().Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();
                    List<string> expressaoEsquerda = expressaoTratada[1].ToString().Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();
                    no.Valor = valor;
                }
            }

            valor = String.Empty;

            //Checar delimitadores;
            for (int i = expressao.Count - 1; i >= 0; i--)
            {
                if (Regex.IsMatch(expressao[i], Dicionario.DELIMITADORES))
                {
                    valor = expressao[i].ToString();
                    string expressaoSplit = String.Join<string>(" ", expressao);
                    List<string> expressaoTratada = expressaoSplit.Split(valor[0]).ToList();
                    List<string> expressaoDireita = expressaoTratada[0].ToString().Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();
                    List<string> expressaoEsquerda = expressaoTratada[1].ToString().Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();
                    no.Valor = valor;
                    no.NoDireito = new No();
                    no.NoEsquerda = new No();
                    MontaArvoreDaExpressao(no.NoDireito, expressaoDireita, precedenciaOp);
                    MontaArvoreDaExpressao(no.NoEsquerda, expressaoEsquerda, precedenciaOp);
                }
            }

            valor = String.Empty;

            //Checar operadores
            for (int i = expressao.Count - 1; i >= 0; i--)
            {
                if (Regex.IsMatch(expressao[i], Dicionario.REGEX_OPERADORES))
                {
                    if (precedenciaOp.Count() > 0) 
                    {
                        if (expressao[i] != precedenciaOp[0])
                        {
                            continue;
                        }
                        else
                        {
                            precedenciaOp.RemoveAt(0);
                            valor = expressao[i].ToString();
                            string expressaoSplit = String.Join<string>(" ", expressao);
                            List<string> expressaoTratada = expressaoSplit.Split(valor.ToCharArray(), 2).ToList();
                            List<string> expressaoDireita = expressaoTratada[0].ToString().Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();
                            List<string> expressaoEsquerda = expressaoTratada[1].ToString().Split(' ').ToList().Where(x => !String.IsNullOrEmpty(x)).ToList();
                            no.Valor = valor;
                            no.NoDireito = new No();
                            no.NoEsquerda = new No();
                            MontaArvoreDaExpressao(no.NoDireito, expressaoDireita, precedenciaOp);
                            MontaArvoreDaExpressao(no.NoEsquerda, expressaoEsquerda, precedenciaOp);
                        }
                    }
                }
            }

            return no;
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

            foreach (string acesso in token.AcessoValorVariaveis)
            {
                if (!Regex.IsMatch(acesso, Dicionario.REGEX_ACESSO_VARIAVEL))
                {
                    Console.WriteLine("O valor {0} foi declarado de forma errada", acesso);
                    return false;
                }
            }

            return true;
        }

        public int analiseSemantica(No expressao, Token tokens)
        {
            int resultado = avaliacao_expr(expressao, tokens);
            variaveis.Add(procurar_variavel(expressao, tokens), resultado);
            return 0;
        }

        private static string procurar_variavel(No expressao, Token tokens)
        {
            string str = "VALOR_EXPRESSAO";
            if (expressao.NoEsquerda != null && str.Equals("VALOR_EXPRESSAO"))
            {
                str = procurar_variavel(expressao.NoEsquerda, tokens);
            }
            if (tokens.Atribuidor.Contains(expressao.Valor))
            {
                str = expressao.NoEsquerda.Valor.Replace("var ", "").Replace(" ", "");
                return str;
            }
            if (expressao.NoDireito != null && str.Equals("VALOR_EXPRESSAO"))
            {
                str = procurar_variavel(expressao.NoDireito, tokens);
            }

            return str;

        }

        public int avaliacao_expr(No expressao, Token tokens)
        {
            string dec_var = "";
            if (expressao.Valor == "")
            {
                return 0;
            }
            if (tokens.Atribuidor.Contains(expressao.Valor))
            {
                try
                {
                    if (variaveis.ContainsKey((expressao.NoEsquerda.Valor.Replace("var ", ""))))
                    {
                        throw new CustomException();
                    }
                    else
                    {
                        return avaliacao_expr(expressao.NoDireito, tokens);
                    }
                }
                catch (CustomException e)
                {
                    e.VariavelDuplicada(expressao.NoEsquerda.Valor.Replace("var ", ""));
                    Console.ReadKey();
                    Environment.Exit(3);
                    return 0;
                }
            }
            try
            {
                if (tokens.Numeros.Contains(expressao.Valor))
                {
                    string replace_delim = "";
                    Match m = Regex.Match(expressao.Valor, Dicionario.DELIMITADORES);
                    if (m.Success)
                    {
                        replace_delim = m.Value;
                    }
                    return Int32.Parse(expressao.Valor
                        .Replace(replace_delim, ""));
                }
                else if (Regex.IsMatch(expressao.Valor, Dicionario.REGEX_VARIAVEIS))
                {
                    if (!tokens.Variaveis.Contains(expressao.Valor))
                    {
                        throw new CustomException();
                    }
                    else
                    {
                        return variaveis[expressao.Valor];
                    }
                }
                else if (tokens.Operadores.Contains(expressao.Valor))
                {
                    if (expressao.Valor == "+")
                    {
                        return avaliacao_expr(expressao.NoDireito, tokens) +
                            avaliacao_expr(expressao.NoEsquerda, tokens);
                    }
                    else if (expressao.Valor == "-")
                    {
                        return avaliacao_expr(expressao.NoDireito, tokens) -
                            avaliacao_expr(expressao.NoEsquerda, tokens);
                    }
                    else if (expressao.Valor == "*")
                    {
                        return avaliacao_expr(expressao.NoDireito, tokens) *
                            avaliacao_expr(expressao.NoEsquerda, tokens);
                    }
                    else if (expressao.Valor == "^")
                    {
                        return (int)Math.Pow(avaliacao_expr(expressao.NoDireito, tokens),
                            avaliacao_expr(expressao.NoEsquerda, tokens));
                    }
                    else
                    {
                        int divisor = avaliacao_expr(expressao.NoDireito, tokens);
                        int dividendo = avaliacao_expr(expressao.NoEsquerda, tokens);
                        try
                        {
                            if (dividendo == 0)
                            {
                                throw new CustomException();
                            }
                            else
                            {
                                return divisor / dividendo;
                            }
                        }
                        catch (CustomException e)
                        {
                            e.DivisaoPorZero();
                            Console.ReadKey();
                            Environment.Exit(1);
                            return 0;
                        }
                    }
                }
                else
                {
                    return -avaliacao_expr(expressao, tokens);
                }
            }
            catch (CustomException e)
            {
                e.VariavelNaoDeclarada(dec_var);
                Console.ReadKey();
                Environment.Exit(2);
                return 0;
            }
        }


        private static void printArvore(No no, int altura)
        {
            if (no != null)
            {
                printArvore(no.NoEsquerda, altura += 1);
                Console.WriteLine("[{0}] ", no.Valor);
                printArvore(no.NoDireito, altura += 1);
            }
        }
    }

    class CustomException : ApplicationException
    {
        public void VariavelDuplicada(string variavel)
        {
            Console.WriteLine("a variavel {0} foi declarada duas ou mais vezes." + 
                " Abortando o programa. . .", variavel);
        }
        public void DivisaoPorZero()
        {
            Console.WriteLine("Divisão por zero encontrada no programa." + 
                " Abortando o programa. . .");
        }
        public void VariavelNaoDeclarada(string variavel)
        {
            Console.WriteLine("Uma tentativa de uso de variavel não" +
                " declarada foi encontrada. Abortando o programa...", variavel);
        }
    }
}
