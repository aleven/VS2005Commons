using System;
using System.Collections.Generic;
using System.Text;

namespace VS2005Commons
{
    public class PrezziUtils
    {
        public enum arrotondamento
        {
            nessuno,
            mezzo_euro_successivo,
            euro_successivo
        }

        public static double arrotondaPrezzo(double prezzo, arrotondamento arr)
        {
            double prezzoArrotondato = 0;

             // Gestione arrotondamento
            double decimali;
            decimali = prezzo % 1;
            int parteIntera;
            parteIntera = Convert.ToInt32( Math.Floor(prezzo) );

            if (arr == arrotondamento.mezzo_euro_successivo) {
                if (decimali > 0 && decimali <= 0.5) {
                    prezzoArrotondato = parteIntera + 0.5;
                }else{
                    prezzoArrotondato = parteIntera + 1;
                }
            }
            else if (arr == arrotondamento.euro_successivo)
            {
                prezzoArrotondato = Math.Ceiling(prezzo);
            }
            else if (arr == arrotondamento.nessuno)
            {
                prezzoArrotondato = prezzo;
            }
        
            return prezzoArrotondato;
        }
    }
}
