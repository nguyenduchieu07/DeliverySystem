// VCN Authentication - Common JavaScript

// Toggle password visibility
function togglePasswordVisibility(inputId, button) {
    const passwordInput = document.getElementById(inputId);

    if (passwordInput.type === 'password') {
        passwordInput.type = 'text';
        button.textContent = '🙈';
    } else {
        passwordInput.type = 'password';
        button.textContent = '👁';
    }
}

// Email validation
function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Show loading state on button
function showButtonLoading(buttonId, loadingText = 'Đang xử lý...') {
    const button = document.getElementById(buttonId);
    if (button) {
        button.dataset.originalText = button.textContent;
        button.textContent = loadingText;
        button.disabled = true;
    }
}

// Reset button state
function resetButtonState(buttonId) {
    const button = document.getElementById(buttonId);
    if (button && button.dataset.originalText) {
        button.textContent = button.dataset.originalText;
        button.disabled = false;
    }
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', function () {
    // Animate vehicles
    const vehicles = document.querySelectorAll('.vehicle');
    vehicles.forEach((vehicle, index) => {
        setTimeout(() => {
            vehicle.style.animationDelay = (index * 0.5) + 's';
        }, index * 200);
    });
});