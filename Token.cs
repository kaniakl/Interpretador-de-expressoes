using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpretador
{
    public class Token
    {
        public List<string> Variaveis { get; set; }
        public List<string> Numeros { get; set; }
        public List<string> Operadores { get; set; }
        public List<string> Delimitadores { get; set; }
        public List<string> Atribuidor { get; set; }
        public List<string> AcessoValorVariaveis { get; set; }
        public string Texto { get; set; }

        public Token()
        {
            this.Variaveis = new List<string>();
            this.Numeros = new List<string>();
            this.Operadores = new List<string>();
            this.Delimitadores = new List<string>();
            this.Atribuidor = new List<string>();
            this.AcessoValorVariaveis = new List<string>();
            this.Texto = String.Empty;
        }
    }
}
