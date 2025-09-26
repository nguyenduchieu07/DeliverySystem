document.addEventListener('click', (e) => {
    const label = e.target.closest('label[for="navToggle"]');
    if (label) {
        const cb = document.getElementById('navToggle');
        const sidebar = document.querySelector('.sidebar');
        if (cb && sidebar) { sidebar.classList.toggle('open'); }
    }
});