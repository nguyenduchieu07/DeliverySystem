// Warehouse Booking System with Draggable Marker
let map, pickupMarker, warehouseMarker;
let pickupData = null;
let warehouseData = null;

// Initialize on page load
document.addEventListener('DOMContentLoaded', function () {
    initMap();
    initDateInputs();
    initWarehouseAreaSearch();
    initPickupAddressSelect();
});

// ============ MAP FUNCTIONS ============
function initMap() {
    map = L.map('map').setView([21.028511, 105.804817], 13);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    L.Control.geocoder({
        defaultMarkGeocode: false,
        placeholder: 'Tìm kiếm địa điểm...',
        errorMessage: 'Không tìm thấy'
    }).on('markgeocode', function (e) {
        const latlng = e.geocode.center;
        setWarehouseLocation(latlng.lat, latlng.lng);
    }).addTo(map);
}

// ============ DATE FUNCTIONS ============
function initDateInputs() {
    const today = new Date().toISOString().split('T')[0];
    const startDateInput = document.getElementById('storageStartDate');
    const endDateInput = document.getElementById('storageEndDate');

    startDateInput.min = today;
    endDateInput.min = today;
    startDateInput.value = today;

    // Mặc định xuất kho sau 30 ngày
    const defaultEndDate = new Date();
    defaultEndDate.setDate(defaultEndDate.getDate() + 30);
    endDateInput.value = defaultEndDate.toISOString().split('T')[0];

    startDateInput.addEventListener('change', function () {
        const startDate = new Date(this.value);
        const minEndDate = new Date(startDate);
        minEndDate.setDate(minEndDate.getDate() + 1);
        endDateInput.min = minEndDate.toISOString().split('T')[0];

        // Nếu ngày xuất kho < ngày nhập kho, tự động điều chỉnh
        if (new Date(endDateInput.value) < minEndDate) {
            endDateInput.value = minEndDate.toISOString().split('T')[0];
        }
    });
}

// ============ PICKUP ADDRESS SELECT ============
function initPickupAddressSelect() {
    const pickupSelect = document.getElementById('pickupAddressSelect');
    if (!pickupSelect) return;

    pickupSelect.addEventListener('change', function () {
        const opt = pickupSelect.options[pickupSelect.selectedIndex];
        const lat = parseFloat(opt.getAttribute('data-lat') || '');
        const lng = parseFloat(opt.getAttribute('data-lng') || '');
        const full = opt.getAttribute('data-full') || '';

        if (!isNaN(lat) && !isNaN(lng)) {
            setPickupLocation(lat, lng, full);
            const disp = document.getElementById('selectedPickupAddress');
            if (disp) {
                disp.textContent = full;
                disp.classList.add('show');
            }
        }
    });

    // Trigger once to initialize
    pickupSelect.dispatchEvent(new Event('change'));
}

// ============ WAREHOUSE AREA SEARCH ============
function initWarehouseAreaSearch() {
    const input = document.getElementById('warehouseAreaInput');
    const results = document.getElementById('warehouseAreaResults');

    if (!input || !results) return;

    let timeoutId;
    input.addEventListener('input', function () {
        const query = this.value.trim();
        clearTimeout(timeoutId);

        if (query.length < 2) {
            results.classList.remove('show');
            results.innerHTML = '';
            return;
        }

        timeoutId = setTimeout(async () => {
            try {
                const res = await fetch(
                    `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)},Hanoi,Vietnam&limit=8&accept-language=vi`
                );
                const items = await res.json();

                results.innerHTML = '';
                if (!items.length) {
                    results.classList.add('show');
                    results.innerHTML = '<div class="autocomplete-item">Không tìm thấy kết quả</div>';
                    return;
                }

                items.forEach(it => {
                    const el = document.createElement('div');
                    el.className = 'autocomplete-item';
                    el.innerHTML = `
                        <div class="autocomplete-item-main">${it.name || (it.display_name || '').split(',')[0]}</div>
                        <div class="autocomplete-item-sub">${it.display_name || ''}</div>
                    `;
                    el.addEventListener('click', () => {
                        const lat = parseFloat(it.lat);
                        const lng = parseFloat(it.lon);
                        setWarehouseLocation(lat, lng, it.display_name);
                        input.value = it.display_name || '';
                        results.classList.remove('show');
                        results.innerHTML = '';
                    });
                    results.appendChild(el);
                });
                results.classList.add('show');
            } catch (e) {
                console.error('Search error:', e);
            }
        }, 300);
    });

    // Close on outside click
    document.addEventListener('click', (e) => {
        if (!results.contains(e.target) && e.target !== input) {
            results.classList.remove('show');
        }
    });
}

// ============ LOCATION MARKERS ============
function setPickupLocation(lat, lng, addressLine = null) {
    pickupData = { street: addressLine || '', lat: lat, lng: lng };

    if (pickupMarker) {
        pickupMarker.setLatLng([lat, lng]);
    } else {
        pickupMarker = L.marker([lat, lng], {
            icon: L.divIcon({
                className: 'custom-marker',
                html: '<div style="background:#28a745;width:32px;height:32px;border-radius:50%;border:4px solid white;box-shadow:0 3px 12px rgba(40,167,69,0.6);display:flex;align-items:center;justify-content:center;font-size:16px;">🏠</div>',
                iconSize: [32, 32],
                iconAnchor: [16, 16]
            })
        }).addTo(map).bindPopup('🏠 Địa chỉ nhận hàng');
    }

    map.setView([lat, lng], 15);
}

function setWarehouseLocation(lat, lng, addressLine = null) {
    if (!addressLine) {
        reverseGeocode(lat, lng).then(address => {
            updateWarehouseData(lat, lng, address);
        });
    } else {
        updateWarehouseData(lat, lng, addressLine);
    }
}

function updateWarehouseData(lat, lng, address) {
    warehouseData = { street: address, lat: lat, lng: lng };

    // Update display
    const display = document.getElementById('selectedWarehouseArea');
    if (display) {
        display.textContent = address;
        display.classList.add('show');
    }

    const input = document.getElementById('warehouseAreaInput');
    if (input) {
        input.value = address;
    }

    // Create or update draggable marker
    if (warehouseMarker) {
        warehouseMarker.setLatLng([lat, lng]);
    } else {
        warehouseMarker = L.marker([lat, lng], {
            draggable: true,
            icon: L.divIcon({
                className: 'custom-marker',
                html: '<div style="background:#f26722;width:32px;height:32px;border-radius:50%;border:4px solid white;box-shadow:0 3px 12px rgba(242,103,34,0.6);display:flex;align-items:center;justify-content:center;font-size:16px;">📍</div>',
                iconSize: [32, 32],
                iconAnchor: [16, 16]
            })
        }).addTo(map).bindPopup('📍 Khu vực tìm kho (Kéo để di chuyển)');

        // Handle drag event
        warehouseMarker.on('dragend', async function (e) {
            const newPos = e.target.getLatLng();
            const newAddress = await reverseGeocode(newPos.lat, newPos.lng);
            updateWarehouseData(newPos.lat, newPos.lng, newAddress);

            // Show notification
            showNotification('📍 Đã cập nhật vị trí kho mới');
        });
    }

    map.setView([lat, lng], 15);
}

async function reverseGeocode(lat, lng) {
    try {
        const response = await fetch(
            `https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lng}&accept-language=vi`
        );
        const data = await response.json();
        return data.display_name || `Vị trí: ${lat.toFixed(6)}, ${lng.toFixed(6)}`;
    } catch (error) {
        console.error('Reverse geocode error:', error);
        return `Vị trí: ${lat.toFixed(6)}, ${lng.toFixed(6)}`;
    }
}

// ============ NOTIFICATION ============
function showNotification(message) {
    const notification = document.createElement('div');
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: #28a745;
        color: white;
        padding: 15px 20px;
        border-radius: 8px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        z-index: 10000;
        font-weight: 500;
        animation: slideIn 0.3s ease;
    `;
    notification.textContent = message;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

// ============ SUBMIT ORDER ============
async function submitWarehouseOrder() {
    // Validation
    if (!pickupData || !pickupData.street) {
        alert('⚠️ Vui lòng chọn địa chỉ nhận hàng!');
        return;
    }

    if (!warehouseData || !warehouseData.street) {
        alert('⚠️ Vui lòng chọn khu vực muốn tìm kho!');
        return;
    }

    const startDate = document.getElementById('storageStartDate').value;
    const endDate = document.getElementById('storageEndDate').value;

    if (!startDate || !endDate) {
        alert('⚠️ Vui lòng chọn ngày nhập và xuất kho!');
        return;
    }

    if (new Date(endDate) <= new Date(startDate)) {
        alert('⚠️ Ngày xuất kho phải sau ngày nhập kho!');
        return;
    }

    // Collect items
    const items = [];
    document.querySelectorAll('#itemsTableBody tr').forEach(row => {
        const name = row.querySelector('input[name*=".Name"]').value;
        const category = row.querySelector('input[name*=".Category"]').value;
        const quantity = parseInt(row.querySelector('input[name*=".Quantity"]').value) || 0;
        const weight = parseFloat(row.querySelector('input[name*=".EstimatedWeightKg"]').value) || 0;

        if (quantity > 0) {
            items.push({ name, category, quantity, estimatedWeightKg: weight });
        }
    });

    if (items.length === 0) {
        alert('⚠️ Vui lòng nhập số lượng cho ít nhất một món đồ!');
        return;
    }

    // Collect special requirements
    const specialRequirements = [];
    document.querySelectorAll('input[name="SpecialRequirements"]:checked').forEach(cb => {
        specialRequirements.push(cb.value);
    });

    // Build order data
    const orderData = {
        pickupAddress: {
            addressLine: pickupData.street,
            latitude: pickupData.lat,
            longitude: pickupData.lng
        },
        warehouseArea: {
            addressLine: warehouseData.street,
            latitude: warehouseData.lat,
            longitude: warehouseData.lng
        },
        storageStartDate: startDate,
        storageEndDate: endDate,
        items: items,
        specialRequirements: specialRequirements,
        note: document.getElementById('orderNote').value || ''
    };

    // Submit
    const bookBtn = document.getElementById('bookBtn');
    const originalText = bookBtn.textContent;
    bookBtn.textContent = '⏳ Đang gửi yêu cầu...';
    bookBtn.disabled = true;

    try {
        const response = await fetch('/Delivery/CreateWarehouseOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(orderData)
        });

        const contentType = response.headers.get('content-type');
        let result;

        if (contentType && contentType.includes('application/json')) {
            result = await response.json();
        } else {
            const text = await response.text();
            console.error('Server response (not JSON):', text);
            result = { success: false, message: 'Lỗi server' };
        }

        if (response.ok && result.success) {
            alert(`✅ ${result.message}\n\n📦 Mã đơn hàng: #${result.orderId}\n🏪 Số kho được thông báo: ${result.nearbyStoresCount}\n📅 Nhập kho: ${startDate}\n📅 Xuất kho: ${endDate}`);
            window.location.href = '/Delivery/Orders';
        } else {
            console.error('Order submission failed:', result);
            alert('❌ ' + (result.message || 'Có lỗi xảy ra. Vui lòng thử lại!'));
            bookBtn.textContent = originalText;
            bookBtn.disabled = false;
        }
    } catch (error) {
        console.error('Error submitting order:', error);
        alert('❌ Không thể kết nối đến máy chủ. Vui lòng kiểm tra kết nối và thử lại!');
        bookBtn.textContent = originalText;
        bookBtn.disabled = false;
    }
}

// Add CSS animations
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(400px);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(400px);
            opacity: 0;
        }
    }
    
    .leaflet-marker-draggable {
        cursor: move !important;
    }
`;
document.head.appendChild(style);