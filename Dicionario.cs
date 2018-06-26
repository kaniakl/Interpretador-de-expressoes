using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpretador
{
    public static class Dicionario
    {
        public static readonly string DELIMITADORES_ABERTURA = "[\\{\\[\\(]";
        public static readonly string DELIMITADORES_FECHAMENTO= "[)\\]\\}]";
        public static readonly string REGEX_OPERADORES = "[\\+\\-\\/\\*\\^]{1}";
        public static readonly string ATRIBUIDOR = "=";
        public static readonly string REGEX_ACESSO_VARIAVEL = "\\@[a-z]+";
        public static readonly string OPERADOR_BINARIO = "-";
        public static readonly string REGEX_VARIAVEIS = "[a-z]";
        public static readonly string REGEX_NUMEROS = "[\\-]{0,1}[0-9]+([.][0-9]+)?";
        public static readonly string REGEX_EXPRESSAO = "[\\+\\-\\/\\*\\^]+";
        public static readonly string REGEX_DECLARACAO_VARIAVEL = "var";
    }
}
