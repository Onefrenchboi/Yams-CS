using System;
using System.Threading;
using System.IO;
using System.Text;
using System.Linq;



public class yams{


    public struct Tracker{
        public string nom; //nom du joueur
        public int id; //id du joueur
        public int[] resultats; //dés lancés
        public int ScoreTour; //Score au tour donné
        public int ScoreTotal; //Score total du joueur
        public string Challenge; //Challenge choisi au nième round
        public int score_bonus; //Score a compter pour savoir si on a le bonus
        public int bonus; //le bonus (0 ou 35) 
        
    }

    public struct Challenge{
        public string nom_chall; //Nom du challenge
        public string code_chall; //code du challenge pour le json
        public bool used; //Si le challenge a déjà été utilisé, False de base.
    }

    public static void InitFichier(Tracker joueur1, Tracker joueur2){
        /*Procédure : Initfichier
        Idée/Principe : Un simple FileStream et StreamWriter pour ecrire le début du fichier json, et les joueurs
        Entrée : Les deux joueurs
        Sortie : Un fichier est crée.
        */

        using (FileStream fs = new FileStream("TP1-M-A-S.json", FileMode.Create)){
            DateTime date = DateTime.Today; //Donne la date du jour
            using (StreamWriter sw = new StreamWriter(fs)){
                sw.Write(
                "{{ \n" + // on echappe le { avec une deuxieme (jsp pourquoi, c# c'est drôle hein ?)
                "   \"parameters\": {{ \n " +
                "       \"code\": \"TP1-Mathéo-Aymeric-Sarah\", \n"+
                "       \"date\": \"{0}\" \n"+
                "   }}, \n",date.ToString("dd-MM-yyyy"));

                sw.Write(
                "   \"players\": [ \n"+ //Initialisation des deux joueurs
                "       {{\n"+ 
                "       \"id\": \"{0}\", \n"+
                "       \"pseudo\": \"{1}\" \n"+
                "       }},\n",joueur1.id, joueur1.nom);

                sw.Write(
                "       {{\n"+
                "       \"id\": \"{0}\", \n"+
                "       \"pseudo\": \"{1}\" \n"+
                "       }}\n"+
                "],\n",joueur2.id, joueur2.nom);

                sw.Write("\"rounds\": [\n");
            }

        }
    }

    public static void WriteRound(Tracker joueur, int id_round){
       /*Procédure : WriteRound
        Idée/Principe : une procedure qui va ecrire le round numéro "id_round" pour le joueur dit
        On va verifier l'id du joueur, et selon lequel c'est, écrire les resultats du round avec un streawriter, sur le fichier crée auparavant
        Entrée :  Joueur et l'id du round
        Sortie : Le fichier json est update.
        */


        string arraydice = "[" + string.Join(",", joueur.resultats) + "]"; //pour avoir les dés sous la forme qu'on veut.
        using (StreamWriter sw = new StreamWriter("TP1-M-A-S.json",true)){ //"true" pour append au fichier.
            if(joueur.id==1){//Si joueur 1, on ecrit l'id du tour, et "results", sinon on ecrit seulement les données.
                sw.Write(
                "{{\n"+
                "   \"id\": {0},\n"+
                "   \"results\": [\n"+
                "    {{\n"+
                "       \"id_player\": {1},\n"+
                "       \"dice\": {2},\n"+
                "       \"challenge\": \"{3}\",\n"+
                "       \"score\": {4}\n"+
                "   }}",id_round,joueur.id,arraydice,joueur.Challenge,joueur.ScoreTour);  
            }
            else{
                sw.Write(",\n"); //On rajoute une virgule si c'est le deuxieme joueur, pour séparer.
                sw.Write(
                "    {{\n"+
                "       \"id_player\": {1},\n"+
                "       \"dice\": {2},\n"+
                "       \"challenge\": \"{3}\",\n"+
                "       \"score\": {4}\n"+
                "   }}\n"+
                "  ]\n"+
                "}}",id_round,joueur.id,arraydice,joueur.Challenge,joueur.ScoreTour);
                if (id_round<13){
                    sw.Write(",");
                }
                sw.Write("\n");
            } 
        
        };
    }

    public static void FinishFile(Tracker joueur1, Tracker joueur2){
        /*Procédure : FinishFile
        Idée/Principe : La derniere partie du fichier, idem que les deux fonctions precedentes
        Entrée : les deux joueurs
        Sortie : Un fichier est crée.
        */
        using (StreamWriter sw = new StreamWriter("TP1-M-A-S.json",true)){
            sw.Write(
                "],\n"+
                "\"final_result\": [\n"+
                "{{\n"+
                "   \"id_player\": {0},\n"+
                "   \"bonus\": {1},\n"+
                "   \"score\": {2}\n"+
                "}},\n"+
                "{{\n"+
                "   \"id_player\": {3},\n"+
                "   \"bonus\": {4},\n"+
                "   \"score\": {5}\n"+
                "}}\n"+
                "]\n"+
                "}}",joueur1.id,joueur1.bonus,joueur1.ScoreTotal,joueur2.id,joueur2.bonus,joueur2.ScoreTotal);
        }
    }

    public static int[] LancerDes(int[] des, ref Tracker joueur){
        /*Fonction : Lancer dés
        Idée/Principe : Une simple boucle qui relance 5 int pour simuler des dés. Si le dé est "0", on le relance
        Entrée : Une liste de dés et un joueur
        Sortie : liste des
        */
        Random random = new Random();
        for (int i = 0; i < des.Length; i++){
            if (des[i] == 0){ // Relancer uniquement les dés marqués comme "non gardés"
                des[i] = random.Next(1, 7); // Générer une valeur entre 1 et 6
            }
            Console.WriteLine(des[i]);

        }
        joueur.resultats=des;
        return des;
    }

    public static Tracker InitialiserJoueur(int id){
        /*Fonction : Initialisation des joueurs
        Principe : simple création d'un joueur
        Entrée : L'ID du joueur
        Sortie : Le joueur (struct Tracker).*/
        Console.Write("Entrez le pseudo du joueur {0} : ",id);
        string nom = Console.ReadLine();
        return new Tracker{
            nom = nom,
            id = id,
            resultats = new int[5],
            ScoreTour = 0,
            ScoreTotal = 0,
            Challenge="",
            bonus=0
        };
    }


/*
Fonctions : test.
Idée/Principe : Les fonctions qui vont tester les challenges
Entrée : la liste des dés pour toutes sauf pour le test de "nombre de" (pour compter le bonus mineur)
Sortie : int resultat
*/
    public static int test_brelan(int [] des){
        int i = 0;
        while (i<3){
            if ((des[i] == des[i+1]) && (des[i+1] == des[i+2])){
                return des[i]*3;
            }
            i++;
        }
        return 0;
    }

    public static int test_carre(int [] des){
        int i = 0;
        while (i<2){
            if ((des[i] == des[i+1]) && (des[i+1] == des[i+2]) && (des[i+2] == des[i+3])){
                return des[i]*4;
            }
            i++;
        }
        return 0;
    }

    public static int test_full(int [] des){
        if (((des[0] == des[1]) && (des[2] == des[3]) && (des[3] == des[4])) || ((des[0] == des[1]) && (des[1] == des[2]) && (des[3] == des[4]))){
                return 25;
        }
        return 0;
    }

    public static int test_pt_suite(int [] des){
        bool reussi = false;
        int [] uniques = new int [5];
        int nbs_utilisés = 0;
        for (int k = 0; k<5; k++){
          if (Array.IndexOf(uniques, des[k]) == -1){
            uniques[nbs_utilisés] = des[k];
            nbs_utilisés+=1;
          }
        }
        if (nbs_utilisés <4){
          return 0;
        }
        else if (nbs_utilisés == 5){
          for (int i = 0; i<2; i++){
            if ((uniques[i]+1 == uniques[i+1]) && (uniques[i+1]+1 == uniques[i+2]) && (uniques[i+2]+1 == uniques[i+3])){
                reussi = true;
            }
          }
        }
        else{
          if ((uniques[0]+1 == uniques[1]) && (uniques[1]+1 == uniques[2]) && (uniques[2]+1 == uniques[3])){
                reussi = true;
            }
        }

        if (reussi == false) {
            return 0;
        }
        return 30;
      }

    public static int test_gd_suite(int [] des){
        if ((des[0]+1 == des[1]) && (des[1]+1 == des[2]) && (des[2]+1 == des[3]) && (des[3]+1 == des[4])){
            return 40;
        }
        return 0;
    }

    public static int test_yam(int [] des){
        bool reussi = true;
        for (int i = 1; i<5; i++){
            if (des[i] != des[0]){
                reussi = false;
            }
        }
        if (reussi == true){
            return 50;
        }
        return 0;
    }

    public static int test_chance(int [] des){
        int total = 0;
        for (int i = 0; i<5; i++){
            total+= des[i];
        }
        return total;
    }

    public static int test_nombrede(int[] des, int choix, ref Tracker joueur){
        int total=0;
        for(int i=0; i<5;i++){
            if (des[i]==choix){
                total+=choix;
            }
        }
        joueur.score_bonus+=total;
        return total;
    }

    public static int CalculerPoints(int choix, int[] des, ref Tracker joueur){
        /*Fonction : Calcul
        Principe : On va lancer la fonction correspondante pour les challenges
        Entrée : Le choix (int), et la liste des dés
        Sortie : le score final*/
        Array.Sort(des);
        int points=0;
        if (choix<=6){points=test_nombrede(des,choix,ref joueur);}
        if (choix==7){points=test_brelan(des);}
        if (choix==8){points=test_carre(des);}
        if (choix==9){points=test_full(des);}
        if (choix==10){points=test_pt_suite(des);}
        if (choix==11){points=test_gd_suite(des);}
        if (choix==12){points=test_yam(des);}
        if (choix==13){points=test_chance(des);}

        return points;
    }

    public static void ChoisirChallenge(ref Tracker joueur,Challenge[] challenges, int[] des){
        /*Fonction : ChoisirChallenge
        Principe : on demande quel challenge choisir a l'aide d'indices donnés.
        Suite a cela, on va lancer la fonction pour tester la validité du challenge et calculer les points
        Entrée : Le joueur, les challenges, les dés
        Sortie : Void.
        */
        Console.Write("Choisissez un défi (via son nombre):  ");
        int choix = int.Parse(Console.ReadLine());
        if (challenges[choix].used==false && choix!=0){  
            joueur.Challenge=challenges[choix].code_chall   ;
            //Calculer les points pour le défi choisi (à implémenter)
            int points = CalculerPoints(choix, des, ref joueur);
            joueur.ScoreTour=points;
            joueur.ScoreTotal+=points; 
            challenges[choix].used=true;
            Console.WriteLine("{0} gagne {1} points pour {2}.",joueur.nom,points,challenges[choix].nom_chall);
        }
        else{
            Console.WriteLine("\nDéfi invalide.\n");
            ChoisirChallenge(ref joueur,challenges, des);
        }
    }


    public static void JouerTour(ref Tracker joueur, Challenge[] challenges){
        /*Fonction : Jouer Tour
        Principe : La fonction maitre du jeu, qui va executer chaque tour.
        Elle va lancer les dés, verifier si le joueur veut les relancer, et va ensuite demander de choisir le challenge
        Entrée : Le joueur, et la liste des challenges
        Précondition : lors du choix des dés, le joueur doit etre capable de lire et faire comme on lui dit
        Sortie : Void*/
        Console.WriteLine("-------------------------------");
        Console.WriteLine("Tour de " + joueur.nom);
        int[] des = new int[5];

        //On donne tout les challenges
        Console.WriteLine("Défis disponibles : ");
        for(int i=1; i<14;i++){
            Console.Write("{0} : {1} ",i,challenges[i].nom_chall);
            if (challenges[i].used==true){Console.Write("(X)");}
            Console.WriteLine();
        }
        Console.WriteLine("Bonus {0}/63",joueur.score_bonus);
        Console.WriteLine("----------Lancé de dés--------------");

        for (int i = 0; i < 3; i++){
            LancerDes(des, ref joueur);
            Console.WriteLine("-------------------------------");
            if (i < 2){ 
                try { //on essaie ceci
                    Console.Write("Entrez les indices des dés à relancer (séparés par des espaces, ou vide pour garder tous) : ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input)) break; //Si rien, on sort de la boucle directement
                    string[] indices= input.Split(); //On split, pour avoir les indices des dés a enlever
                    foreach (var index in indices){ //!ON ASSUME QUE LE JOUEUR RENTRE DES NOMBRES ENTRE 1 ET 5
                        int id = int.Parse(index); // Convertir l'indice directement sans vérification
                        if (id < 1 || id > 5){ //Si l'index est hors de range on lance une erreur
                            throw new ArgumentOutOfRangeException("index", "L'indice doit être entre 1 et 5.");
                        }
                        des[id - 1] = 0; // Marquer les dés à garder
                    }
                    
                }
                catch (ArgumentOutOfRangeException ex){ //On attrape l'erreur, et on quitte avec un message et le code d'erreur 1
                    Console.WriteLine("Erreur d'indice : " + ex.Message);
                    Environment.Exit(1); //quitte avec code d'erreur 1.
                }
            }
        }
        
        ChoisirChallenge(ref joueur,challenges, des);

    }

    static void checkbonus(ref Tracker joueur){
        if (joueur.score_bonus>=63){
            Console.WriteLine("-------------------------------");
            joueur.ScoreTotal+=35;
            joueur.bonus=35;
            Console.WriteLine("{0} gagne 35 points (Bonus)",joueur.nom);
        }
    }
    static void Main(){
        //Création des deux joueurs
        Tracker joueur1 = InitialiserJoueur(1);
        Tracker joueur2 = InitialiserJoueur(2);
       
        //Création du fichier
        InitFichier(joueur1,joueur2);
        
        //tableau des challenges des joueurs
        string[] NomsChallenges = { "Un", "Deux", "Trois", "Quatre", "Cinq", "Six", "Brelan", "Carré", "Full", "Petite Suite", "Grande Suite", "Yams", "Chance" };
        string[] CodesChallenges = { "nombre1", "nombre2", "nombre3", "nombre4", "nombre5", "nombre6", "brelan", "carre", "full", "petite", "grande", "yams", "chance" };
        Challenge[] challenges1 = new Challenge[14];
        Challenge[] challenges2 = new Challenge[14];
        challenges1[0] = new Challenge { nom_chall = "PLACEHOLDER"};; 
        challenges2[0] = new Challenge { nom_chall = "PLACEHOLDER"};;
        //?On déclare challenge[0] comme un placeholder, afin d'avoir les challenges de 1 à 13 et non de 0 à 12 => purement esthétique
        //?(On évite ainsi de devoir jouer avec les indices pour l'affichage.)
       
        for (int i=1;i<=1;i++){
            challenges1[i] = new Challenge{nom_chall = NomsChallenges[i-1], used=false, code_chall = CodesChallenges[i-1]};
            challenges2[i] = new Challenge{nom_chall = NomsChallenges[i-1], used=false, code_chall = CodesChallenges[i-1]};
        }

        const int nombreDeTours = 13;

        // Gestion de la partie
        for (int tour = 1; tour <= nombreDeTours; tour++){
            Console.WriteLine("\n--- Tour " + tour + "/" + nombreDeTours + " ---");
            JouerTour(ref joueur1,challenges1);
            WriteRound(joueur1, tour);
            JouerTour(ref joueur2,challenges2);
            WriteRound(joueur2, tour);

        }
        checkbonus(ref joueur1);
        checkbonus(ref joueur2);
        if (joueur1.ScoreTour>joueur2.ScoreTour){
            Console.WriteLine("Le gagnant est {0}, avec {1} points",joueur1.nom,joueur1.ScoreTotal);
        }
        else{
            Console.WriteLine("Le gagnant est {0}, avec {1} points",joueur2.nom,joueur2.ScoreTotal);
        }    

        FinishFile(joueur1,joueur2);
    }
}