using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP1_1035.Classes
{
    internal class JeuxDePeche
    {
        private bool OrientationJeu;
        private PileDepot PileDepot; 
        private PilePioche PilePioche; 
        private List<Joueur> Joueurs;
        private Observer Observateur;
        private int CurrentPlayerIndex;
        private Regles ReglesDuJeu;
        private Couleur CouleurActuelle;

        // Initialiser les piles et l'observateur dans le constructeur
        public JeuxDePeche(List<Joueur> joueurs, Observer observateur)
        {
            Joueurs = joueurs;
            Observateur = observateur; 
            PileDepot = new PileDepot(observateur, this); 
            PilePioche = new PilePioche(observateur);
            //JoueurActuel = joueurs.First();
            CurrentPlayerIndex = 0;
            ReglesDuJeu = new Regles();
            CouleurActuelle = Couleur.Coeur;
            OrientationJeu = true;
            Console.WriteLine("Le jeu va bientôt débuter");

            InitialiserJeu();

            CommencerJeu();
        }

        //Initialiser les éléments du jeu
        public void InitialiserJeu()
        {
            //Générer les cartes
            PaireDeCartes Paire = new PaireDeCartes();
            Paire.GenererCarte();
            Paire.MelangerCartes();

            //Mettre les cartes dans la pile de dépot et de pioche
            PileDepot.InitialiserDepot(Paire.GetCarteList());
            MelangerDepotPioche();
            CouleurActuelle = PileDepot.GetCarteDuDessus().GetCouleur();

            //Distribuer un nombre de carte aléatoire aux joueurs
            Console.WriteLine("Distribution des cartes...");
            Random ran = new Random();
            int nombreInitialCartes = ran.Next(5, 9);
            Thread.Sleep(3000);
            
            foreach (Joueur joueur in Joueurs)
            {
                DistribuerCartes(joueur, nombreInitialCartes);

                //Attacher les joueurs aux autres joueurs
                foreach (Joueur joueursNotifies in Joueurs)
                {
                    if(joueur.GetId() != joueursNotifies.GetId())
                    {
                        joueur.AttachJoueur(joueursNotifies);

                    }
                }
            }

            //Choisir le premier joueur
            CurrentPlayerIndex = ReglesDuJeu.ChoisirPremierJoueur(Joueurs.Count);

            Console.WriteLine();
            Console.WriteLine("La première carte de la pile de dépot est: " + PileDepot.GetCarteDuDessus().ToString());
            Thread.Sleep(1000);
        }

        // Démarrage du jeu
        public void CommencerJeu()
        {
            while (!ReglesDuJeu.EstPartieTerminee(Joueurs))
            {
                Console.WriteLine();
                Console.WriteLine("Tour du joueur " + Joueurs[CurrentPlayerIndex].GetNom());
        
                if (Joueurs[CurrentPlayerIndex] is JoueurStrategique joueurStrategique) // Tour du joueur stratégique, joue selon la stratégie établie
                {
                    joueurStrategique.JouerStrategiquement(this);
                }
                else // Joue normalement
                {
                    Joueurs[CurrentPlayerIndex].JouerCarte(ReglesDuJeu, this);
                }

                //Avoir l'index du prochain joueur
                CurrentPlayerIndex = GetNextPlayerIndex();

                Thread.Sleep(1500); // À changer selon la vitesse qu'on veut que les tours se déroulent 
            }

            PointsFinaux();
        }

        // Distribuer les cartes
        public void DistribuerCartes(Joueur pJoueur, int pQteCartes)
        {
            List<Carte> cartesDistribuees = PilePioche.PigerCartes(pQteCartes, PileDepot);

            foreach (var carte in cartesDistribuees)
            {
                pJoueur.AjouterCarte(carte);
            }
            
            NotifierObservateur($"{pQteCartes} cartes ont été distribuées à {pJoueur.GetNom()}.");
        }

        // Transférer les cartes de la pile de dépôt et les ajouter à la pile de pioche
        public void MelangerDepotPioche()
        {
            PilePioche.TransfertDepotVersPioche(PileDepot);

            NotifierObservateur("Les cartes de la pile de dépôt ont été mélangées et réintégrées à la pioche.");
        }

        // Notifier tous les observateurs
        private void NotifierObservateur(string message)
        {
            Observateur.NotifierObservateurs(message);
        }

        // Calculer les points
        public void PointsFinaux()
        {
            Console.WriteLine();
            
            foreach (var joueur in Joueurs)
            {
                int points = joueur.CalculerPoints();
                Console.WriteLine($"Le joueur {joueur.GetNom()} a {points} point(s).");
            }
        }

        //Saute le tour d'un joueur
        public void PasserAuJoueurSuivant()
        {
            CurrentPlayerIndex = GetNextPlayerIndex();
        }

        //Changer la direction du jeu
        public void ChangerDirection()
        {
            OrientationJeu = !OrientationJeu;
        }

        //Retourne le joueur suivant sans sauter de tours
        public Joueur ObtenirJoueurSuivant()
        {
            int PlayerIndex = GetNextPlayerIndex();

            return Joueurs[PlayerIndex];
        }

        //Redéfinire la couleur actuelle
        public void DefinirCouleurCourante(Couleur nouvelleCouleur)
        {
            CouleurActuelle = nouvelleCouleur;
        }

        //Get la couleur courante demandée
        public Couleur GetCouleurCourante()
        {
            return CouleurActuelle;
        }

        // Obtenir la carte du dessus
        public Carte ObtenirCarteDuDessus()
        {
            return PileDepot.GetCarteDuDessus();
        }

        //Retourne le joueur actuel
        public Joueur GetJoueurActuel()
        {
            return Joueurs[CurrentPlayerIndex];
        }

        //Jouer une carte sur la pile de dépot
        public bool JouerCarte(Joueur joueur, Carte carte)
        {
            return PileDepot.DeposerCarte(carte, CouleurActuelle);
        }

        //Pige un nombre de carte spécifié
        public List<Carte> PiocherCarte(int qte)
        {
            List<Carte> cartesPigees = PilePioche.PigerCartes(qte, PileDepot);
            return cartesPigees;

        }

        //Retourne l'index du prochain joueur joueur selon l'orientation du jeu
        public int GetNextPlayerIndex()
        {
            int Index = CurrentPlayerIndex;
            int NbreJoueurs = Joueurs.Count;

            if (OrientationJeu)
            {
                Index++;
                if (Index == NbreJoueurs)
                {
                    Index = 0;
                }
            }
            else
            {
                Index--;
                if (Index == -1)
                {
                    Index = NbreJoueurs - 1;
                }
            }
            return Index;
        }
    }
}