using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal struct Carte
    {
        private Couleur CouleurCarte { get; set; }
        private Valeur ValeurCarte { get; set; }

        public Carte(Couleur pCouleur, Valeur pValeur) { 
            this.CouleurCarte = pCouleur;
            this.ValeurCarte = pValeur;

        }

        public Couleur GetCouleur()
        {
            return CouleurCarte;
        }

        public Valeur GetValeur()
        {
            return ValeurCarte;
        }

        public override string ToString()
        {
            string message = this.ValeurCarte.ToString() + " de " + this.CouleurCarte.ToString();
            return message;
        }
    }

    enum Couleur
    {
        Coeur,
        Carreau,
        Trefle,
        Pique
    }

    enum Valeur
    {
        As,
        Deux,
        Trois,
        Quatre,
        Cinq,
        Six,
        Sept,
        Huit,
        Neuf,
        Dix,
        Valet,
        Reine,
        Roi
    }
}
