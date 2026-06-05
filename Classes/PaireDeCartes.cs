using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal class PaireDeCartes
    {
        private List<Carte> CarteList = new List<Carte>();

        public List<Carte> GetCarteList()
        {
            return CarteList;
        }

        //Générer toutes les cartes
        public void GenererCarte()
        {
            foreach (Couleur CouleurCarte in Enum.GetValues(typeof(Couleur)))
            {
                foreach (Valeur ValeurCarte in Enum.GetValues(typeof(Valeur)))
                {
                    Carte carte = new Carte(CouleurCarte, ValeurCarte);
                    CarteList.Add(carte);
                }
            }
        }

        //Mélange les cartes
        public void MelangerCartes()
        {
            Random rng = new Random();
            int n = CarteList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Carte temp = CarteList[k];
                CarteList[k] = CarteList[n];
                CarteList[n] = temp;
            }
        }
    }
}
