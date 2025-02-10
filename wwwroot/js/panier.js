document.addEventListener("DOMContentLoaded", function () {
    const cartStatus = document.getElementById("cartStatus");
    const notification = document.getElementById("notification");
    let updateTimeout;

    function showNotification(message, type = 'success') {
        const notificationMessage = document.getElementById("notificationMessage");
        notification.className = `fixed bottom-4 right-4 transform transition-all duration-300 ease-out ${type === 'success' ? 'bg-green-500' : 'bg-red-500'
            } text-white px-6 py-3 rounded-lg shadow-lg`;

        notificationMessage.textContent = message;
        notification.style.transform = 'translateY(0)';
        notification.style.opacity = '1';

        setTimeout(() => {
            notification.style.transform = 'translateY(100%)';
            notification.style.opacity = '0';
        }, 3000);
    }

    function updateItemTotal(itemElement, newQuantity) {
        const itemId = itemElement.dataset.itemId;
        const price = parseFloat(itemElement.dataset.itemPrice);
        const discountPercentage = parseFloat(itemElement.dataset.itemDiscount) || 0;  // Remise
        const totalElement = itemElement.querySelector('.item-total');
    
        // Calculer le prix sans remise pour afficher le prix normal
        const priceWithoutDiscount = price * newQuantity;
        
        // Calculer le prix avec la remise
        const priceAfterDiscount = price * (1 - discountPercentage / 100);
        const newTotalWithDiscount = (priceAfterDiscount * newQuantity).toFixed(2);
    
        // Mettre à jour le total affiché (prix avec remise)
        totalElement.textContent = new Intl.NumberFormat('fr-FR', {
            style: 'currency',
            currency: 'XOF'
        }).format(newTotalWithDiscount);
    
        // Mettre à jour également le total du panier
        const totalElementGlobal = document.getElementById("totalPanier");
        if (totalElementGlobal) {
            const currentTotal = parseFloat(totalElementGlobal.textContent.replace('XOF', '').replace(',', '').trim());
            const newTotal = currentTotal + (priceAfterDiscount * newQuantity);
            
            totalElementGlobal.textContent = new Intl.NumberFormat('fr-FR', {
                style: 'currency',
                currency: 'XOF'
            }).format(newTotal);
        }
    }
    


    function showLoadingState() {
        cartStatus.classList.remove('hidden');
        cartStatus.classList.add('flex');
    }

    function hideLoadingState() {
        cartStatus.classList.add('hidden');
        cartStatus.classList.remove('flex');
    }

    function updateCartUI(itemElement, data) {
        const quantityInput = itemElement.querySelector(".quantity-input");
        const decreaseButton = itemElement.querySelector("form button:first-of-type");
        const decreaseIcon = decreaseButton.querySelector("i");

        // Ajouter une classe pour l'animation de mise à jour
        itemElement.classList.add('scale-105', 'bg-gray-50', 'dark:bg-gray-700');
        setTimeout(() => {
            itemElement.classList.remove('scale-105', 'bg-gray-50', 'dark:bg-gray-700');
        }, 200);

        if (data.newQuantity <= 0) {
            itemElement.classList.add('scale-0', 'opacity-0');
            setTimeout(() => {
                itemElement.remove();
                checkEmptyCart();
            }, 300);
        } else {
            quantityInput.value = data.newQuantity;
            updateItemTotal(itemElement, data.newQuantity);

            if (data.newQuantity === 1) {
                decreaseIcon.className = "fas fa-trash text-red-600 h-4 w-4 transition-all duration-200";
            } else {
                decreaseIcon.className = "fas fa-minus text-gray-600 h-4 w-4 transition-all duration-200";
            }
        }

        // Mettre à jour le total
        // Mise à jour du total avec un formatage en CFA
        const totalElement = document.getElementById("totalPanier");
        if (totalElement) {
            totalElement.textContent = new Intl.NumberFormat('fr-FR', {
                style: 'currency',
                currency: 'XOF'  // CFA utilisé pour la devise
            }).format(data.total); // Formattage du total
        }

    }


    function checkEmptyCart() {
        const cartItems = document.getElementById("cartItems");
        const orderSummary = document.getElementById("orderSummary");
        const emptyCart = document.getElementById("emptyCart");

        if (!cartItems || cartItems.children.length === 0) {
            if (orderSummary) {
                orderSummary.style.display = "none";
            }
            if (emptyCart) {
                emptyCart.style.display = "block";
            }
            if (cartItems) {
                cartItems.closest('.lg\\:grid').style.display = "none";
            }
        }
    }


    // Gestionnaire d'événements pour les formulaires de quantité
    document.querySelectorAll(".quantity-form").forEach(form => {
        form.addEventListener("submit", function (e) {
            e.preventDefault();
            const itemElement = this.closest('[data-item-id]');

            showLoadingState();
            clearTimeout(updateTimeout);

            fetch(this.action, {
                method: "POST",
                body: new FormData(this)
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        updateCartUI(itemElement, data);
                        showNotification("Panier mis à jour avec succès");
                    } else {
                        showNotification(data.message || "Une erreur est survenue", 'error');
                    }
                })
                .catch(error => {
                    console.error("Erreur AJAX :", error);
                    showNotification("Impossible de modifier la quantité", 'error');
                })
                .finally(() => {
                    updateTimeout = setTimeout(hideLoadingState, 300);
                });
        });
    });

    // Ajouter des transitions CSS pour les animations
    const style = document.createElement('style');
    style.textContent = `
        .quantity-form button {
            transition: all 0.2s ease-in-out;
        }
        .quantity-form button:hover {
            transform: scale(1.05);
        }
        [data-item-id] {
            transition: all 0.3s ease-in-out;
        }
        #cartStatus {
            transition: all 0.3s ease-in-out;
        }
    `;
    document.head.appendChild(style);
});