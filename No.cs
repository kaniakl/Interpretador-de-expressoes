using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpretador
{
    public class No
    {
        public string Valor { get; set; }
        public No NoDireito { get; set; }
        public No NoEsquerda { get; set; }

        public No()
        {
            this.Valor = String.Empty;
            this.NoDireito = null;
            this.NoEsquerda = null;
        }
    }
}
