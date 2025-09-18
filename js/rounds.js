let num_round; //on declare les deux pour y avoir accès partout dans le js
let fileName = " ";

function chargerFichier() {
    // Récupérer la valeur du champ de saisie
    let nomFichier = document.getElementById("fileName").value;
    // On retourne le nom du fichier
    return nomFichier;
}



async function afficher_round(){ //affichage du round, idem que dans globale.js, seulement 1 fois au lieu de 13
    const ContainerRound = document.getElementById('main');
    ContainerRound.innerHTML = '';
    try {
        const response = await fetch("http://yams.iutrs.unistra.fr:3000/api/games/" + fileName + "/rounds/" + num_round);
        const data = await response.json();
        
        const roundHTML = `
        <div class="round">
        <p class="id"><strong>Round N°${data.id}</strong></p><hr>
        <ul><br>
        <li><p class="id"><strong>Joueur N°</strong>${data.results[0].id_player} :</p></li>
        <ul>    
        <li><p class="titles">Dé : </p>${data.results[0].dice}</li>
        <li><p class="titles">Challenge : </p>${data.results[0].challenge}</li>
        <li><p class="titles">Score : </p>${data.results[0].score}</li>
        </ul>
        <br>
        <li> <p class="id"><strong>Joueur N°</strong>${data.results[1].id_player} :</p></li>
        <ul>
        <li><p class="titles">Dé : </p>${data.results[1].dice}</li>
        <li><p class="titles">Challenge : </p>${data.results[1].challenge}</li>
        <li><p class="titles">Score : </p>${data.results[1].score}</li>
        </ul>
        </ul>
        </div>
        `;
        // Ajouter le HTML du round au conteneur principal
        ContainerRound.innerHTML += roundHTML;
    } catch (error) {
        console.error('Erreur de chargement du round', error);
        alert('Erreur de chargement du round');
    }

 
      
}

function previous(){ //Fonctions qui va afficher le round d'avant en appuyant sur la fleche de gauche
    if (num_round==1){
        num_round=13
    }
    else{
        num_round-=1;
    }
    afficher_round()
}

function next(){ //Fonctions qui va afficher le round d'apres en appuyant sur la fleche de droite
    if (num_round==13){
        num_round=1
    }
    else{
        num_round+=1;
    }
    afficher_round()
}


async function Main() {
    num_round=1; // On commence au premier round
    fileName=chargerFichier();

    // Code pour afficher le resultat final
    // Rien ne sert de le repeter a chaque fois, on le fait donc ici une seule fois
    const ContainerFinalResults = document.getElementById('ContainerFinalResults');
    ContainerFinalResults.innerHTML = '';
    try {
        const response = await fetch("http://yams.iutrs.unistra.fr:3000/api/games/" + fileName + "/final-result");
        const data = await response.json();


        const codeHTML = `
        
        <h2 class="titre">Le Résultat final</h2>
        <div class="final">
        <ul>
        <li>Joueur N°${data[0].id_player} :</li>
        <ul>
        <li>Bonus : ${data[0].bonus}</li>
        <li>Score : ${data[0].score}</li>
        </ul>
        <br>
        <li>Joueur N°${data[1].id_player} :</li>
        <ul>
        <li>Bonus : ${data[1].bonus}</li>
        <li>Score : ${data[1].score}</li>
        </ul>
        </ul>
        </div>`;
        ContainerFinalResults.innerHTML = codeHTML;
    } catch (error) {
        console.error('Erreur de chargement du résultat final', error);
        alert('Erreur de chargement du résultat final');
    }



    afficher_round()//affichage round
}
