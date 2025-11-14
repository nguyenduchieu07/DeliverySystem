// Toast Notification Function - Đảm bảo function trong global scope
window.showToast = function(message, type = 'info', duration = 5000) {
    console.log('showToast called:', { message, type, duration });
    
    // Đảm bảo document.body đã sẵn sàng
    if (!document.body) {
        console.error('Document body not ready');
        return;
    }
    
    // Tạo container nếu chưa có
    let container = document.getElementById('toast-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toast-container';
        container.className = 'toast-container';
        // Đảm bảo container hiển thị với inline style
        container.style.cssText = 'position: fixed; top: 20px; right: 20px; z-index: 99999; display: flex; flex-direction: column; gap: 10px; max-width: 400px; pointer-events: none;';
        document.body.appendChild(container);
        console.log('Toast container created');
    }

    // Tạo toast element
    const toast = document.createElement('div');
    toast.className = `toast ${type}`;
    // Đảm bảo toast hiển thị với inline style
    toast.style.cssText = 'background: white; border-radius: 8px; padding: 16px 20px; box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15); display: flex; align-items: flex-start; gap: 12px; min-width: 300px; max-width: 400px; position: relative; overflow: hidden; pointer-events: auto; animation: slideInRight 0.3s ease;';
    if (type === 'error') {
        toast.style.borderLeft = '4px solid #dc3545';
    } else if (type === 'success') {
        toast.style.borderLeft = '4px solid #28a745';
    } else if (type === 'warning') {
        toast.style.borderLeft = '4px solid #ffc107';
    } else {
        toast.style.borderLeft = '4px solid #17a2b8';
    }

    // Icon theo type
    const icons = {
        success: '✅',
        error: '❌',
        warning: '⚠️',
        info: 'ℹ️'
    };

    // Title theo type
    const titles = {
        success: 'Thành công',
        error: 'Lỗi',
        warning: 'Cảnh báo',
        info: 'Thông báo'
    };

    // Escape HTML để tránh XSS
    function escapeHtml(text) {
        if (!text) return "";
        const div = document.createElement("div");
        div.textContent = text;
        return div.innerHTML;
    }

    toast.innerHTML = `
        <span class="toast-icon">${icons[type] || icons.info}</span>
        <div class="toast-content">
            <div class="toast-title">${titles[type] || titles.info}</div>
            <div class="toast-message">${escapeHtml(message)}</div>
        </div>
        <button class="toast-close" onclick="this.closest('.toast').remove()">×</button>
    `;

    container.appendChild(toast);
    console.log('Toast element added to container');
    console.log('Container position:', container.getBoundingClientRect());
    console.log('Toast position:', toast.getBoundingClientRect());
    console.log('Toast computed styles:', window.getComputedStyle(toast));

    // Auto remove sau duration
    const timeout = setTimeout(() => {
        toast.classList.add('hiding');
        setTimeout(() => {
            if (toast.parentNode) {
                toast.remove();
            }
        }, 300);
    }, duration);

    // Click để đóng sớm
    toast.addEventListener('click', (e) => {
        if (e.target.classList.contains('toast-close') || e.target.closest('.toast-close')) {
            clearTimeout(timeout);
            toast.classList.add('hiding');
            setTimeout(() => {
                if (toast.parentNode) {
                    toast.remove();
                }
            }, 300);
        }
    });
};

// Cũng export function để tương thích
if (typeof module !== 'undefined' && module.exports) {
    module.exports = { showToast: window.showToast };
}

