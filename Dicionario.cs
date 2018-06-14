using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpretador
{
    public static class Dicionario
    {
        public static readonly string DELIMITADORES = "[\\{\\[\\(\\)\\]\\}]";
        public static readonly string REGEX_OPERADORES = "[\\+\\-\\/\\*\\^]{1}";
        public static readonly string ATRIBUIDOR = "=";
        public static readonly string REGEX_ACESSO_VARIAVEL = "\\@";
        public static readonly string OPERADOR_BINARIO = "-";
        public static readonly string REGEX_VARIAVEIS = "[a-z]";
        public static readonly string REGEX_NUMEROS = "[0-9]+([.][0-9]+)?";
        public static readonly string REGEX_EXPRESSAO = "[\\+\\-\\/\\*\\^]+";
        public static readonly string REGEX_DECLARACAO_VARIAVEL = "var\\s{1}[a-z]+\\s*=\\s*[0-9]+([.][0-9]+)?";
    }
}
