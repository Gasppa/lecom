using System;

namespace Lecom.Model
{
    public class Exame
    {
        public string codigoCliente { get; set; }
        public string codigoExame { get; set; }
        public double precoAcordadoExame { get; set; }
        public double precoFaturadoExame { get; set; }
        public double comissaoTotal { get; set; }
        public double comissaoFaturada { get; set; }
    }
}
