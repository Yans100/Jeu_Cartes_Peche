
# Jeu de Pêche — INF1035

Simulation console d'un jeu de cartes de type "Pêche" en C#, avec joueur stratégique, cartes spéciales et patron de conception Observer.

## Fonctionnalités

- Partie automatique de 2 à 4 joueurs avec saisie des noms
- Un joueur stratégique désigné aléatoirement (joue la carte de plus haute valeur possible)
- Cartes spéciales : As (sauter un tour), Dix (inverser direction), Sept (piocher 2 cartes), Valet (changer couleur)
- Mécanique d'attaque : un joueur notifie les autres quand il n'a plus qu'une carte
- Transfert automatique de la pile de dépôt vers la pile de pioche quand elle est vide
- Calcul des points finaux à la fin de la partie
- Patron Observer pour les notifications de jeu et entre joueurs

## Patrons de conception

- **Observer** — notifications entre les composants du jeu et entre joueurs
- **Strategy** — `JoueurStrategique` surcharge le comportement de jeu
- **Héritage** — `Personne` → `Joueur` → `JoueurStrategique`
- **Interfaces** — `IObservateur`, `IObservateurJoueur`, `ISubjectJoueur`

## Technologies

- C# / .NET 6+

## Prérequis

- .NET 6+

## Lancer le projet

```bash
dotnet run
```

## Structure

```
Program.cs            — point d'entrée
TableDeJeux.cs        — saisie des joueurs et lancement
JeuxDePeche.cs        — logique principale de la partie
Joueur.cs             — joueur standard
JoueurStrategique.cs  — joueur IA stratégique
Regles.cs             — règles et effets des cartes spéciales
Carte.cs              — struct Carte, enums Couleur et Valeur
PaireDeCartes.cs      — génération et mélange du jeu complet
PileDepot.cs          — pile de dépôt
PilePioche.cs         — pile de pioche
Observer.cs           — gestionnaire de notifications
Personne.cs           — classe de base
IObservateur.cs       — interface Observer
IObservateurJoueur.cs — interface Observer joueur
ISubjectJoueur.cs     — interface Subject joueur
```

---

Projet universitaire en équipe — cours INF1035, UQTR.
