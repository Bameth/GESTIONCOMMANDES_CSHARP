// Fonction pour afficher le popup d'ajout au panier
function afficherPopup() {
    const popup = document.getElementById('popup');
    popup.classList.remove('hidden');
    popup.classList.add('opacity-100');
    setTimeout(() => {
        popup.classList.add('opacity-0');
        popup.classList.add('hidden');
    }, 5000);
}

// Fonction pour changer l'image principale
function changerImagePrincipale(element) {
    const imagePrincipale = document.getElementById("imagePrincipale");
    imagePrincipale.src = element.src; // Change la source de l'image principale
}
