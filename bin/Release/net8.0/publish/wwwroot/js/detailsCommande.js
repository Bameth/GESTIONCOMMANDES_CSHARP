function openPaiementModal() {
    document.getElementById('paiementModal').classList.remove('hidden');
    document.body.style.overflow = 'hidden';
}

function closePaiementModal() {
    document.getElementById('paiementModal').classList.add('hidden');
    document.body.style.overflow = 'auto';
    resetPaymentForms();
}

function openLivraisonModal() {
    document.getElementById('livraisonModal').classList.remove('hidden');
    document.body.style.overflow = 'hidden';
}

function closeLivraisonModal() {
    document.getElementById('livraisonModal').classList.add('hidden');
    document.body.style.overflow = 'auto';
}

function selectPaymentMethod(method) {
    // Reset all forms
    resetPaymentForms();
    
    // Remove active state from all payment options
    document.querySelectorAll('.payment-option').forEach(option => {
        option.classList.remove('border-indigo-500');
    });

    // Set selected payment type
    document.getElementById('selectedPaymentType').value = method;

    // Show corresponding form
    switch(method) {
        case 'OM':
            document.getElementById('omForm').classList.remove('hidden');
            break;
        case 'WAVE':
            document.getElementById('waveForm').classList.remove('hidden');
            break;
        case 'ESPECES':
            document.getElementById('espForm').classList.remove('hidden');
            break;
    }

    // Add active state to selected payment option
    event.currentTarget.classList.add('border-indigo-500');
}

function resetPaymentForms() {
    document.getElementById('omForm').classList.add('hidden');
    document.getElementById('waveForm').classList.add('hidden');
    document.getElementById('espForm').classList.add('hidden');
    document.getElementById('selectedPaymentType').value = '';
    document.getElementById('reference').value = '';
    
    // Reset all input fields
    const forms = ['numOM', 'numWave', 'montantEsp'];
    forms.forEach(formId => {
        const input = document.getElementById(formId);
        if (input) input.value = '';
    });
}

function validatePaymentSelection() {
    const paymentType = document.getElementById('selectedPaymentType').value;
    if (!paymentType) {
        alert('Veuillez sélectionner un mode de paiement');
        return false;
    }

    // Generate a random reference number
    const reference = 'REF' + Date.now() + Math.floor(Math.random() * 1000);
    document.getElementById('reference').value = reference;

    // Validate specific payment method fields
    switch(paymentType) {
        case 'OM':
            if (!document.getElementById('numOM').value) {
                alert('Veuillez entrer votre numéro Orange Money');
                return false;
            }
            break;
        case 'WAVE':
            if (!document.getElementById('numWave').value) {
                alert('Veuillez entrer votre numéro Wave');
                return false;
            }
            break;
        case 'ESPECES':
            if (!document.getElementById('montantEsp').value) {
                alert('Veuillez entrer le montant');
                return false;
            }
            break;
    }

    return true;
}

// Close modals when clicking outside
window.onclick = function(event) {
    const paiementModal = document.getElementById('paiementModal');
    const livraisonModal = document.getElementById('livraisonModal');
    
    if (event.target === paiementModal) {
        closePaiementModal();
    }
    if (event.target === livraisonModal) {
        closeLivraisonModal();
    }
}