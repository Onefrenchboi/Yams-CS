// On charge le fichier JSON avec son identifiant

function chargerFichier() {
    // Récupérer la valeur du champ de saisie
    let nomFichier = document.getElementById("fileName").value;
    // On retourne le nom du fichier
    return nomFichier;
}

async function afficher_partie(fileName) {
    // On affiche les différents joueurs
    const ContainerPlayers = document.getElementById('ContainerPlayers');

    for (let i = 0; i <= 1; i++) {
        try {
            const response = await fetch("http://yams.iutrs.unistra.fr:3000/api/games/" + fileName + "/players");
            const data = await response.json();
            const codeHTML = 
            `<div class="players">
            <ul>
            <h3>Joueur N°${data[i].id}</h3>
            <li>Pseudo : ${data[i].pseudo}</li>
            </ul>
            </div>`;
            ContainerPlayers.innerHTML += codeHTML;
        } catch (error) {
            console.error('Erreur de chargement des joueurs', error);
            alert('Erreur de chargement des joueurs');
        }
    }
    document.getElementById('separation1').innerHTML+="<hr>";
    // On affiche les résultats des rounds
    const ContainerRound = document.getElementById('ContainerRound');
    ContainerRound.innerHTML = '';
    const temp='<a href="rounds.html"><h2 class="titre">Les Rounds</h2></a><br>';
    ContainerRound.innerHTML+=temp;
    for (let i = 1; i <= 13; i++) {
        try {
            const response = await fetch("http://yams.iutrs.unistra.fr:3000/api/games/" + fileName + "/rounds/" + i);
            const data = await response.json();

            // Création d'un conteneur individuel pour chaque round
            const roundHTML = `
            <a href="rounds.html"><div class="round">
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
            </div></a>
            `;

            // Ajouter le HTML du round au conteneur principal
            ContainerRound.innerHTML += roundHTML;
        } catch (error) {
            console.error('Erreur de chargement du round', error);
            alert('Erreur de chargement du round');
            break;
        }
    }
    
    document.getElementById('separation2').innerHTML+="<hr>  <a href=\"rounds.html\"> (cliquez ici ou sur n'importe quel round pour afficher individuellement)</a>";



    // On affiche le résultat final de la partie
    
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

}

function Main() {
    let fileName = chargerFichier();
    
    afficher_partie(fileName);
}
