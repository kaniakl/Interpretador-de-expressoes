using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace Interpretador
{
    public static class InterpretadorExpresseos
    {
        public static List<Token> Interpreta(string nomeArquivo)
        {
            string extensao = nomeArquivo.Split('.').ToList()[1];

            if (!extensao.Equals("calc"))
            {
                Console.WriteLine("Por favor, insira arquivos .calc!");
                Thread.Sleep(2000);
                return null;
            }
            else
            {
                List<Token> Tokens = new List<Token>();
                string[] linhas = File.ReadAllLines(nomeArquivo).Where(x => !String.IsNullOrEmpty(x)).ToArray();

                Parser.ParserExpressao(linhas, Tokens);

                return Tokens.ToList();
            }
        }
    }
}
