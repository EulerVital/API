using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API.Models
{
    public class eVeiculosInfo
    {
        public string cd_cnpj { get; set; }
        public string nm_razao { get; set; }
        public string dt_emplacamento { get; set; }
        public string cd_chassi { get; set; }
        public string cd_placa { get; set; }
        public string nm_fabricante { get; set; }
        public string nm_grupo_modelo_veiculo { get; set; }
        public string nm_modelo { get; set; }
        public string nm_segmento { get; set; }
        public string nm_sub_segmento { get; set; }
        public string nm_municipio { get; set; }
        public string nm_estado { get; set; }
        public string nm_ano_fabricacao { get; set; }
        public string nm_cilindrada { get; set; }

    }
}
