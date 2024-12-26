function updateQuantity(ligneId, delta) {
    var input = document.querySelector(`input[value='${ligneId}']`);
    var currentQuantity = parseInt(input.value);
    var newQuantity = currentQuantity + delta;

    if (newQuantity < 1) {
        newQuantity = 1;  // Eviter d'aller en dessous de 1
    }

    // Envoie la nouvelle quantité via AJAX
    fetch(`/Panier/UpdateQuantity?ligneId=${ligneId}&quantity=${newQuantity}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                input.value = newQuantity;
                if (newQuantity === 1) {
                    // Remplacer l'icône moins par une poubelle
                    input.previousElementSibling.innerHTML = '<i class="fas fa-trash text-red-600 dark:text-red-500"></i>';
                }
            }
        });
}

function removeFromCart(ligneId) {
    fetch(`/Panier/Retirer?ligneId=${ligneId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Rafraîchir la page ou supprimer la ligne
                location.reload();
            }
        });
}
