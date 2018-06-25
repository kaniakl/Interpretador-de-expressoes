using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Interpretador
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Token> Tokens = null;
            Parser syntaxParser = new Parser();

            while (true)
            {
                Console.WriteLine("Selecionar arquivo? (s/n)");
                ConsoleKeyInfo resposta = Console.ReadKey();
                if (resposta.KeyChar.Equals('s'))
                {
                    Console.WriteLine("\nInforme seu arquivo: ");
                    string nomeArquivo = Console.ReadLine();
                    if (!String.IsNullOrEmpty(nomeArquivo))
                    {
                        Tokens = InterpretadorExpresseos.Interpreta(nomeArquivo);
                        if (Tokens.Any(x => x == null) || Tokens == null)
                        {
                            return;
                        }
                        ImprimeTokens.Imprime(Tokens);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Por favor, insira um nome de arquivo corretamente!\n");
                    }
                }
                else if (resposta.KeyChar.Equals('n'))
                {
                    Console.Write("\nFechando programa...");
                    Thread.Sleep(1000);
                    return;
                }
                else
                {
                    Console.WriteLine("\nFavor inserir opcao correta (s/n)");
                }
            }

            List<Arvore> arvores = Parser.CriaArvores(Tokens);

            if (arvores.Any(x => x == null) || arvores == null || arvores.Any(x => x.NoPrincipal == null || String.IsNullOrEmpty(x.NoPrincipal.Valor)))
            {
                Console.WriteLine("Existe valores inadequados no programa");
            }
            else
            {
                Console.WriteLine("Seu programa passou pela analise sintatica");
            }

            int i = 0;
            foreach (Arvore linhaDeCodigo in arvores)
            {
                syntaxParser.analiseSemantica(linhaDeCodigo.NoPrincipal, Tokens[i]);
                i++;
            }
            foreach (KeyValuePair<string, int> entry in syntaxParser.variaveis)
            {
                Console.WriteLine("Variavel> {0} ", entry.Key);
                Console.WriteLine("Valor> {0} ", entry.Value);
            }

            Console.ReadKey();
        }
    }
}
