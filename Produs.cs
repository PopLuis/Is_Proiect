using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTab
{
    public class Produs
    {
        public string Nume { get; set; }
        public decimal Pret { get; set; }
        public string Descriere { get; set; }
        public string Categorie { get; set; } // desktop, laptop, imprimanta, periferice, componente

        public int Cantitate { get; set; } = 1;
    }

}
