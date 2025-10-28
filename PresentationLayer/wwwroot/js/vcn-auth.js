// VCH Delivery System - Multiple Categories Support

let map, pickupMarker, dropoffMarker;
let pickupData = null;
let dropoffData = null;
let selectedProductCategories = []; // Changed to array
let estimatedWeight = null;
let calculatedDistance = null;
let calculatedEta = null;

// Initialize on page load
document.addEventListener('DOMContentLoaded', function () {
    initMap();
    initDateInputs();
    initAddressAutocomplete();
    loadProductCategories();
    getCurrentLocation();
    initPopupEvents();
    
    // Check if we should calculate distance/time on page load
    setTimeout(() => {
        if (pickupData && dropoffData) {
            calculateDistanceAndTime();
        }
        
        // Debug buttons removed for production
    }, 1000);
});

// Utility function to force hide overlay
function forceHideOverlay() {
    const overlay = document.getElementById('popupOverlay');
    if (overlay) {
        overlay.classList.remove('show');
        overlay.style.display = 'none';
        overlay.style.visibility = 'hidden';
        overlay.style.opacity = '0';
        overlay.style.pointerEvents = 'none';

        // Force remove all classes
        overlay.className = 'popup-overlay';

        setTimeout(() => {
            overlay.style.display = '';
            overlay.style.visibility = '';
            overlay.style.opacity = '';
            overlay.style.pointerEvents = '';
        }, 100);
    }
}

// Function to completely reset overlay
function resetOverlay() {
    const overlay = document.getElementById('popupOverlay');
    if (overlay) {
        overlay.className = 'popup-overlay';
        overlay.style.cssText = '';
        overlay.classList.remove('show');
    }
}

// Function to hide all popups and overlay
function hideAllPopups() {
    // Hide all popups
    const popups = ['pickupInfoPopup', 'productCategoryPopup', 'serviceCategoryPopup'];
    popups.forEach(popupId => {
        const popup = document.getElementById(popupId);
        if (popup) {
            popup.classList.remove('show');
        }
    });

    // Reset overlay
    resetOverlay();
}

// Function to update customer info display
function updateCustomerInfoDisplay(name, phone, floor) {
    const placeholder = document.getElementById('customerInfoPlaceholder');
    const content = document.getElementById('customerInfoContent');
    const nameEl = document.getElementById('customerName');
    const phoneEl = document.getElementById('customerPhone');
    const floorEl = document.getElementById('customerFloor');

    if (placeholder && content && nameEl && phoneEl && floorEl) {
        // Ẩn placeholder, hiện content
        placeholder.style.display = 'none';
        content.style.display = 'block';

        // Cập nhật thông tin với khả năng edit
        nameEl.innerHTML = `
            <span class="customer-info-text">${name}</span>
            <button type="button" class="btn-edit-customer" onclick="editCustomerInfoViaPopup()" title="Sửa thông tin khách hàng">✏️</button>
        `;
        phoneEl.innerHTML = `
            <span class="customer-info-text">📞 ${phone}</span>
        `;
        floorEl.innerHTML = `
            <span class="customer-info-text">${floor ? `🏢 ${floor}` : ''}</span>
        `;
        
        // Add CSS for edit buttons
        addCustomerEditStyles();
    }
}

// Function to edit customer info via popup
function editCustomerInfoViaPopup() {
    // Get current values
    const nameEl = document.getElementById('customerName');
    const phoneEl = document.getElementById('customerPhone');
    const floorEl = document.getElementById('customerFloor');
    
    const getName = () => {
        const textSpan = nameEl?.querySelector('.customer-info-text');
        return textSpan ? textSpan.textContent : nameEl?.textContent || '';
    };
    
    const getPhone = () => {
        const textSpan = phoneEl?.querySelector('.customer-info-text');
        return textSpan ? textSpan.textContent.replace('📞 ', '') : phoneEl?.textContent?.replace('📞 ', '') || '';
    };
    
    const getFloor = () => {
        const textSpan = floorEl?.querySelector('.customer-info-text');
        return textSpan ? textSpan.textContent.replace('🏢 ', '') : floorEl?.textContent?.replace('🏢 ', '') || '';
    };
    
    // Pre-fill the popup with current values
    const nameInput = document.getElementById('pickupName');
    const phoneInput = document.getElementById('pickupPhone');
    const floorInput = document.getElementById('pickupFloor');
    
    if (nameInput) nameInput.value = getName();
    if (phoneInput) phoneInput.value = getPhone();
    if (floorInput) floorInput.value = getFloor();
    
    // Show the popup
    showPickupInfoPopup();
}

// Add CSS styles for customer edit buttons
function addCustomerEditStyles() {
    // Only add styles once
    if (document.getElementById('customerEditStyles')) return;
    
    const style = document.createElement('style');
    style.id = 'customerEditStyles';
    style.textContent = `
        .btn-edit-customer {
            background: #007bff;
            color: white;
            border: none;
            padding: 4px 8px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 12px;
            margin-left: 8px;
            opacity: 0.8;
            transition: all 0.2s;
        }
        
        .btn-edit-customer:hover {
            opacity: 1;
            background: #0056b3;
            transform: scale(1.05);
        }
        
        .customer-info-text {
            display: inline-block;
        }
    `;
    
    document.head.appendChild(style);
}

// Initialize popup events
function initPopupEvents() {
    // Close popup when clicking overlay
    const overlay = document.getElementById('popupOverlay');
    if (overlay) {
        overlay.addEventListener('click', function (e) {
            // Only close if clicking directly on overlay, not on popup content
            if (e.target === overlay) {
                hidePickupInfoPopup();
            }
        });
    }

    // Close popup with Escape key
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            // Check if pickup popup is open
            const popup = document.getElementById('pickupInfoPopup');
            if (popup && popup.classList.contains('show')) {
                hidePickupInfoPopup();
            }
        }
    });
}

// ============ MAP FUNCTIONS ============
function initMap() {
    if (L) {
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
            map.panTo(latlng);
        }).addTo(map);
    }
}

function getCurrentLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            function (position) {
                const lat = position.coords.latitude;
                const lng = position.coords.longitude;
                map.setView([lat, lng], 15);
                setPickupLocation(lat, lng);
            },
            function (error) {
            }
        );
    }
}

// ============ DATE FUNCTIONS ============
function initDateInputs() {
    const today = new Date().toISOString().split('T')[0];
    const deliveryDateInput = document.getElementById('deliveryDate');
    const pickupDateInput = document.getElementById('pickupDate');

    deliveryDateInput.min = today;
    pickupDateInput.min = today;
    deliveryDateInput.value = today;

    deliveryDateInput.addEventListener('change', function () {
        const deliveryDate = new Date(this.value);
        deliveryDate.setDate(deliveryDate.getDate() + 3);
        pickupDateInput.value = deliveryDate.toISOString().split('T')[0];
        pickupDateInput.min = this.value;
    });

    const defaultPickupDate = new Date();
    defaultPickupDate.setDate(defaultPickupDate.getDate() + 3);
    pickupDateInput.value = defaultPickupDate.toISOString().split('T')[0];
}

// ============ ADDRESS AUTOCOMPLETE ============
function initAddressAutocomplete() {
    const pickupInput = document.getElementById('pickupAddressInput');
    const pickupResults = document.getElementById('pickupAutocompleteResults');
    const dropoffInput = document.getElementById('dropoffAddressInput');
    const dropoffResults = document.getElementById('dropoffAutocompleteResults');

    let pickupTimeout;
    pickupInput.addEventListener('input', function () {
        clearTimeout(pickupTimeout);
        const query = this.value.trim();
        if (query.length < 2) {
            pickupResults.classList.remove('show');
            return;
        }
        pickupTimeout = setTimeout(() => {
            searchAddress(query, pickupResults, 'pickup');
        }, 300);
    });

    let dropoffTimeout;
    dropoffInput.addEventListener('input', function () {
        clearTimeout(dropoffTimeout);
        const query = this.value.trim();
        if (query.length < 2) {
            dropoffResults.classList.remove('show');
            return;
        }
        dropoffTimeout = setTimeout(() => {
            searchAddress(query, dropoffResults, 'dropoff');
        }, 300);
    });

    document.addEventListener('click', function (e) {
        if (!pickupInput.contains(e.target) && !pickupResults.contains(e.target)) {
            pickupResults.classList.remove('show');
        }
        if (!dropoffInput.contains(e.target) && !dropoffResults.contains(e.target)) {
            dropoffResults.classList.remove('show');
        }
    });
}

async function searchAddress(query, resultsContainer, type) {
    try {
        const response = await fetch(
            `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)},Hanoi,Vietnam&limit=5&accept-language=vi`
        );
        const results = await response.json();
        displaySearchResults(results, resultsContainer, type);
    } catch (error) {
        console.error('Error searching address:', error);
    }
}

function displaySearchResults(results, container, type) {
    container.innerHTML = '';
    if (results.length === 0) {
        container.innerHTML = '<div class="autocomplete-item">Không tìm thấy kết quả</div>';
        container.classList.add('show');
        return;
    }

    results.forEach(result => {
        const item = document.createElement('div');
        item.className = 'autocomplete-item';
        item.innerHTML = `
            <div class="autocomplete-item-main">${result.name || result.display_name.split(',')[0]}</div>
            <div class="autocomplete-item-sub">${result.display_name}</div>
        `;
        item.addEventListener('click', function () {
            selectAddress(result, type);
            container.classList.remove('show');
        });
        container.appendChild(item);
    });
    container.classList.add('show');
}

function selectAddress(result, type) {
    const lat = parseFloat(result.lat);
    const lng = parseFloat(result.lon);
    const addressLine = result.display_name;

    if (type === 'pickup') {
        document.getElementById('pickupAddressInput').value = addressLine;
        setPickupLocation(lat, lng, addressLine);
        // Không hiện popup nữa, chỉ set địa chỉ
    } else {
        document.getElementById('dropoffAddressInput').value = addressLine;
        setDropoffLocation(lat, lng, addressLine);
    }
}

// ============ LOCATION MARKERS ============
function setPickupLocation(lat, lng, addressLine = null) {
    if (!addressLine) {
        reverseGeocode(lat, lng).then(address => {
            pickupData = { street: address, lat: lat, lng: lng };
            document.getElementById('pickupAddressInput').value = address;
            document.getElementById('selectedPickupAddress').innerHTML = address;
            document.getElementById('selectedPickupAddress').classList.add('show');
            
            // Calculate distance and time if both locations are set
            setTimeout(() => checkAndUpdateDistanceTime(), 100);
        });
    } else {
        pickupData = { street: addressLine, lat: lat, lng: lng };
        document.getElementById('selectedPickupAddress').innerHTML = addressLine;
        document.getElementById('selectedPickupAddress').classList.add('show');
        
        // Calculate distance and time if both locations are set
        setTimeout(() => calculateDistanceAndTime(), 100);
    }

    if (pickupMarker) {
        pickupMarker.setLatLng([lat, lng]);
    } else {
        pickupMarker = L.marker([lat, lng], {
            icon: L.divIcon({
                className: 'custom-marker',
                html: '<div style="background:#f26722;width:28px;height:28px;border-radius:50%;border:4px solid white;box-shadow:0 2px 10px rgba(242,103,34,0.5)"></div>',
                iconSize: [28, 28],
                iconAnchor: [14, 14]
            })
        }).addTo(map).bindPopup('🔴 Địa chỉ lấy hàng');
    }
    map.setView([lat, lng], 15);
}

function setDropoffLocation(lat, lng, addressLine = null) {
    if (!addressLine) {
        reverseGeocode(lat, lng).then(address => {
            dropoffData = { street: address, lat: lat, lng: lng };
            document.getElementById('dropoffAddressInput').value = address;
            document.getElementById('selectedDropoffAddress').innerHTML = address;
            document.getElementById('selectedDropoffAddress').classList.add('show');
            
            // Calculate distance and time if both locations are set
            setTimeout(() => checkAndUpdateDistanceTime(), 100);
        });
    } else {
        dropoffData = { street: addressLine, lat: lat, lng: lng };
        document.getElementById('selectedDropoffAddress').innerHTML = addressLine;
        document.getElementById('selectedDropoffAddress').classList.add('show');
        
        // Calculate distance and time if both locations are set
        setTimeout(() => calculateDistanceAndTime(), 100);
    }

    if (dropoffMarker) {
        dropoffMarker.setLatLng([lat, lng]);
    } else {
        dropoffMarker = L.marker([lat, lng], {
            icon: L.divIcon({
                className: 'custom-marker',
                html: '<div style="background:#4CAF50;width:32px;height:32px;border-radius:50%;border:4px solid white;box-shadow:0 3px 12px rgba(76,175,80,0.5)"></div>',
                iconSize: [32, 32],
                iconAnchor: [16, 16]
            })
        }).addTo(map).bindPopup('🟢 Khu vực tìm kho');
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
        console.error('Error reverse geocoding:', error);
        return `Vị trí: ${lat.toFixed(6)}, ${lng.toFixed(6)}`;
    }
}

// ============ DISTANCE & TIME CALCULATION ============
function calculateDistanceAndTime() {
    if (!pickupData || !dropoffData) {
        // Hide display if we don't have both locations
        const displayElement = document.getElementById('distanceTimeDisplay');
        if (displayElement) {
            displayElement.style.display = 'none';
        }
        return;
    }

    // Calculate distance using Haversine formula
    const distance = calculateHaversineDistance(
        pickupData.lat, pickupData.lng,
        dropoffData.lat, dropoffData.lng
    );
    
    // Calculate estimated time based on distance and vehicle type
    const vehicleType = document.getElementById('selectedVehicle')?.value;
    const eta = calculateEstimatedTime(distance, vehicleType);
    
    calculatedDistance = distance;
    calculatedEta = eta;
    
    // Update UI to show distance and time
    updateDistanceTimeDisplay(distance, eta);
}

function calculateHaversineDistance(lat1, lon1, lat2, lon2) {
    const R = 6371; // Earth's radius in kilometers
    const dLat = (lat2 - lat1) * Math.PI / 180;
    const dLon = (lon2 - lon1) * Math.PI / 180;
    const a = Math.sin(dLat/2) * Math.sin(dLat/2) +
              Math.cos(lat1 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180) *
              Math.sin(dLon/2) * Math.sin(dLon/2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
    return R * c; // Distance in kilometers
}

function calculateEstimatedTime(distanceKm, vehicleType) {
    // Base speed in km/h for different vehicle types
    const speeds = {
        'van1000': 30, 'pickup': 25, 'truck1500': 20, 'truck3000': 18, 'container': 15,
        'xe_may': 30, 'xe_tai_nho': 25, 'xe_tai_lon': 20, 'xe_container': 15
    };
    
    const baseSpeed = speeds[vehicleType] || 25;
    const timeHours = distanceKm / baseSpeed;
    const timeMinutes = Math.round(timeHours * 60);
    
    // Buffer time for loading/unloading
    const bufferMinutes = {
        'van1000': 15, 'pickup': 30, 'truck1500': 45, 'truck3000': 50, 'container': 60,
        'xe_may': 15, 'xe_tai_nho': 30, 'xe_tai_lon': 45, 'xe_container': 60
    };
    
    const buffer = bufferMinutes[vehicleType] || 30;
    
    // Traffic factor based on distance
    let trafficFactor = 1.0;
    if (distanceKm > 10) trafficFactor = 1.3;
    else if (distanceKm > 5) trafficFactor = 1.2;
    
    return Math.round((timeMinutes * trafficFactor) + buffer);
}

function updateDistanceTimeDisplay(distance, eta) {
    // Create or update distance/time display
    let displayElement = document.getElementById('distanceTimeDisplay');
    
    if (!displayElement) {
        // Create display element if it doesn't exist
        displayElement = document.createElement('div');
        displayElement.id = 'distanceTimeDisplay';
        displayElement.className = 'distance-time-info';
        displayElement.style.cssText = `
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            border-radius: 12px;
            padding: 20px;
            margin: 15px 0;
            text-align: center;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
            color: white;
        `;
        
        // Try to find the vehicle selection section and insert after it
        const vehicleSection = document.querySelector('.vehicle-selection') || 
                              document.querySelector('[class*="vehicle"]') ||
                              document.querySelector('.section-title');
        
        if (vehicleSection && vehicleSection.parentNode) {
            // Insert after the vehicle section
            vehicleSection.parentNode.insertBefore(displayElement, vehicleSection.nextSibling);
        } else {
            // Fallback: insert at the beginning of the form
            const form = document.getElementById('booking-form') || document.querySelector('form');
            if (form) {
                form.insertBefore(displayElement, form.firstChild);
            }
        }
    }
    
    // Show the display element with animation
    displayElement.style.display = 'block';
    displayElement.style.opacity = '0';
    displayElement.style.transform = 'translateY(-10px)';
    
    // Animate in
    setTimeout(() => {
        displayElement.style.transition = 'all 0.3s ease';
        displayElement.style.opacity = '1';
        displayElement.style.transform = 'translateY(0)';
    }, 10);
    
    const hours = Math.floor(eta / 60);
    const minutes = eta % 60;
    const timeText = hours > 0 ? `${hours}h ${minutes}p` : `${minutes}p`;
    
    // Get vehicle type display name
    const vehicleNames = {
        'van1000': '🚐 Van 1000kg',
        'pickup': '🚛 Pickup Truck',
        'truck1500': '🚚 Truck 1500kg',
        'truck3000': '🚛 Truck 3000kg',
        'container': '📦 Container',
        // Legacy support
        'xe_may': '🏍️ Xe máy',
        'xe_tai_nho': '🚛 Xe tải nhỏ', 
        'xe_tai_lon': '🚚 Xe tải lớn',
        'xe_container': '📦 Container'
    };
    
    const selectedVehicle = document.getElementById('selectedVehicle')?.value;
    const vehicleName = vehicleNames[selectedVehicle] || '🚗 Phương tiện';
    
    displayElement.innerHTML = `
        <div style="display: flex; justify-content: space-around; align-items: center; color: white;">
            <div style="text-align: center;">
                <div style="font-size: 24px; font-weight: bold; margin-bottom: 5px;">📏 ${distance.toFixed(1)} km</div>
                <div style="font-size: 14px; opacity: 0.9;">Khoảng cách</div>
            </div>
            <div style="text-align: center;">
                <div style="font-size: 24px; font-weight: bold; margin-bottom: 5px;">⏱️ ${timeText}</div>
                <div style="font-size: 14px; opacity: 0.9;">Thời gian ước tính</div>
            </div>
        </div>
        <div style="text-align: center; margin-top: 10px; font-size: 12px; opacity: 0.8;">
            ${vehicleName} • 💡 Thông tin này sẽ được lưu vào đơn hàng
        </div>
    `;
    
}

// Function to check and update distance/time when needed
function checkAndUpdateDistanceTime() {
    // Only calculate if we have both locations and a vehicle selected
    if (pickupData && dropoffData && document.getElementById('selectedVehicle')?.value) {
        calculateDistanceAndTime();
    }
}



// Create pickup info popup dynamically
function createPickupInfoPopup() {
    // Create overlay
    const overlay = document.createElement('div');
    overlay.id = 'popupOverlay';
    overlay.style.cssText = `
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        z-index: 9999;
        display: none;
    `;
    
    // Create popup
    const popup = document.createElement('div');
    popup.id = 'pickupInfoPopup';
    popup.style.cssText = `
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
        z-index: 10000;
        min-width: 400px;
        display: none;
    `;
    
    popup.innerHTML = `
        <h3>Thông tin khách hàng</h3>
        <div style="margin-bottom: 15px;">
            <label>Tên khách hàng:</label>
            <input type="text" id="pickupName" placeholder="Nhập tên khách hàng" style="width: 100%; padding: 8px; margin-top: 5px;">
        </div>
        <div style="margin-bottom: 15px;">
            <label>Số điện thoại:</label>
            <input type="tel" id="pickupPhone" placeholder="Nhập số điện thoại" style="width: 100%; padding: 8px; margin-top: 5px;">
        </div>
        <div style="margin-bottom: 20px;">
            <label>Tầng (tùy chọn):</label>
            <input type="text" id="pickupFloor" placeholder="Nhập tầng" style="width: 100%; padding: 8px; margin-top: 5px;">
        </div>
        <div style="text-align: right;">
            <button type="button" onclick="hidePickupInfoPopup()" style="padding: 8px 16px; margin-right: 10px; background: #6c757d; color: white; border: none; border-radius: 4px; cursor: pointer;">Hủy</button>
            <button type="button" onclick="saveCustomerInfo()" style="padding: 8px 16px; background: #007bff; color: white; border: none; border-radius: 4px; cursor: pointer;">Lưu thông tin</button>
        </div>
    `;
    
    // Add to body
    document.body.appendChild(overlay);
    document.body.appendChild(popup);
    
    // Add click outside to close
    overlay.onclick = (e) => {
        if (e.target === overlay) {
            hidePickupInfoPopup();
        }
    };
    
    // Show popup
    showPickupInfoPopup();
}


// ============ PICKUP INFO POPUP ============
function showPickupInfoPopup() {
<<<<<<< HEAD
    try {
        console.log('Showing pickup info popup');
        const popup = document.getElementById('pickupInfoPopup');
        const overlay = document.getElementById('popupOverlay');

        if (!popup) {
            console.error('pickupInfoPopup element not found!');
            return;
        }

        if (!overlay) {
            console.error('popupOverlay element not found!');
            return;
        }

        popup.classList.add('show');
        overlay.classList.add('show');

        // Pre-fill values if available
        const nameField = document.getElementById('pickupName');
        const phoneField = document.getElementById('pickupPhone');
        const floorField = document.getElementById('pickupFloor');

        if (pickupData && pickupData.recipientName) {
            nameField.value = pickupData.recipientName;
            phoneField.value = pickupData.recipientPhone || '';
            floorField.value = pickupData.floor || '';
        } else {
            nameField.value = '';
            phoneField.value = '';
            floorField.value = '';
        }

        console.log('Pickup info popup shown successfully');
    } catch (error) {
        console.error('Error showing pickup info popup:', error);
        alert('Có lỗi xảy ra khi hiển thị popup. Vui lòng thử lại.');
=======
    const popup = document.getElementById('pickupInfoPopup');
    const overlay = document.getElementById('popupOverlay');
    
    if (!popup || !overlay) {
        createPickupInfoPopup();
        return;
    }
    
    // Hide any existing popups
    hideAllPopups();
    
    // Show popup
    overlay.style.display = 'block';
    overlay.style.visibility = 'visible';
    overlay.style.opacity = '1';
    overlay.style.zIndex = '9999';
    overlay.classList.add('show');
    
    popup.style.display = 'block';
    popup.style.visibility = 'visible';
    popup.style.opacity = '1';
    popup.style.zIndex = '10000';
    popup.classList.add('show');
        
    // Pre-fill values if available
    const nameField = document.getElementById('pickupName');
    const phoneField = document.getElementById('pickupPhone');
    const floorField = document.getElementById('pickupFloor');
    
    if (pickupData && pickupData.recipientName) {
        if (nameField) nameField.value = pickupData.recipientName;
        if (phoneField) phoneField.value = pickupData.recipientPhone || '';
        if (floorField) floorField.value = pickupData.floor || '';
    } else {
        if (nameField) nameField.value = '';
        if (phoneField) phoneField.value = '';
        if (floorField) floorField.value = '';
>>>>>>> origin/Tuanlmhe
    }
}

function hidePickupInfoPopup() {
    try {
        const popup = document.getElementById('pickupInfoPopup');
        const overlay = document.getElementById('popupOverlay');

        // Kiểm tra và ẩn popup
        if (popup) {
            popup.classList.remove('show');
            popup.style.display = 'none';
        }

        // Kiểm tra và ẩn overlay
        if (overlay) {
            overlay.classList.remove('show');
            overlay.style.display = 'none';
        } else {
            console.warn('popupOverlay element not found');
            // Tìm và ẩn tất cả overlay có thể
            document.querySelectorAll('.popup-overlay').forEach(el => {
                el.classList.remove('show');
                el.style.display = 'none';
            });
        }

    } catch (error) {
        console.error('Error hiding pickup info popup:', error);
    }
}

function savePickupInfo() {
    try {
        const name = document.getElementById('pickupName').value.trim();
        const phone = document.getElementById('pickupPhone').value.trim();
        const floor = document.getElementById('pickupFloor').value.trim();


        if (!name || !phone) {
            alert('Vui lòng nhập đầy đủ tên và số điện thoại!');
            return;
        }

        // Validate phone number format
        const phoneRegex = /^[0-9]{8,11}$/;
        if (!phoneRegex.test(phone)) {
            alert('Số điện thoại không hợp lệ! Vui lòng nhập số điện thoại từ 8-11 chữ số.');
            return;
        }

        if (pickupData) {
            pickupData.recipientName = name;
            pickupData.recipientPhone = phone;
            pickupData.floor = floor;
        } else {
            // Tạo pickupData mới nếu chưa có
            pickupData = {
                street: '',
                lat: 0,
                lng: 0,
                recipientName: name,
                recipientPhone: phone,
                floor: floor
            };
        }

        // Hiển thị thông tin khách hàng trong ô
        updateCustomerInfoDisplay(name, phone, floor);

        // Ẩn popup trước
        const popup = document.getElementById('pickupInfoPopup');
        if (popup) {
            popup.classList.remove('show');
            popup.style.display = 'none';
        }

        // Đợi một chút rồi ẩn overlay
        setTimeout(() => {
            const overlay = document.getElementById('popupOverlay');
            if (overlay) {
                overlay.classList.remove('show');
                overlay.style.display = 'none';
            }
            // Backup: ẩn tất cả popup-overlay
            document.querySelectorAll('.popup-overlay').forEach(el => {
                el.classList.remove('show');
                el.style.display = 'none';
            });
        }, 100);

    } catch (error) {
        console.error('Error in savePickupInfo:', error);
        alert('Có lỗi xảy ra khi lưu thông tin. Vui lòng thử lại.');
    }
}

// ============ PRODUCT CATEGORY (Multiple Selection) ============
function loadProductCategories() {
    const categories = [
        { id: 1, name: 'Thức ăn', icon: '🍔', description: 'Thực phẩm, đồ ăn nhanh, đồ uống' },
        { id: 2, name: 'Đồ gia dụng', icon: '🏠', description: 'Đồ dùng hàng ngày trong gia đình' },
        { id: 3, name: 'Đồ dễ vỡ', icon: '⚠️', description: 'Đồ dễ vỡ, cần cẩn thận khi vận chuyển' },
        { id: 4, name: 'Đồ điện tử', icon: '📱', description: 'Thiết bị điện tử, máy móc' },
        { id: 5, name: 'Quần áo', icon: '👕', description: 'Quần áo, giày dép, phụ kiện thời trang' },
        { id: 6, name: 'Sách vở', icon: '📚', description: 'Sách, tài liệu, văn phòng phẩm' },
        { id: 7, name: 'Nội thất', icon: '🛋️', description: 'Đồ nội thất, bàn ghế' },
        { id: 8, name: 'Mỹ phẩm', icon: '💄', description: 'Mỹ phẩm, chăm sóc cá nhân' },
        { id: 9, name: 'Dược phẩm', icon: '💊', description: 'Thuốc, dược phẩm, thiết bị y tế' },
        { id: 10, name: 'Khác', icon: '📦', description: 'Các loại hàng hóa khác' }
    ];

    const listContainer = document.getElementById('productCategoryList');
    listContainer.innerHTML = categories.map(cat => `
        <div class="autocomplete-item" style="cursor: pointer;">
            <label style="display: flex; align-items: center; gap: 12px; cursor: pointer; width: 100%; padding: 4px 0;">
                <input type="checkbox" 
                       class="product-checkbox" 
                       data-product-id="${cat.id}" 
                       data-product-name="${cat.name}"
                       data-product-icon="${cat.icon}"
                       onchange="toggleProductCategory(this)"
                       style="width: 18px; height: 18px; cursor: pointer;">
                <span style="font-size: 24px;">${cat.icon}</span>
                <div style="flex: 1;">
                    <div style="font-weight: 500; color: #333; margin: 0;">${cat.name}</div>
                    <div style="font-size: 12px; color: #666; margin-top: 2px;">${cat.description}</div>
                </div>
            </label>
        </div>
    `).join('');
}

function toggleProductCategory(checkbox) {
    const id = parseInt(checkbox.dataset.productId);
    const name = checkbox.dataset.productName;
    const icon = checkbox.dataset.productIcon;

    if (checkbox.checked) {
        if (!selectedProductCategories.find(c => c.id === id)) {
            selectedProductCategories.push({ id, name, icon });
        }
    } else {
        selectedProductCategories = selectedProductCategories.filter(c => c.id !== id);
    }

    updateProductCategoryDisplay();
}

function updateProductCategoryDisplay() {
    const btn = document.getElementById('productCategoryBtn');
    const displayText = document.getElementById('selectedProductText');

    if (selectedProductCategories.length === 0) {
        displayText.textContent = 'Chọn loại hàng hóa';
        btn.style.color = '#999';
    } else if (selectedProductCategories.length === 1) {
        displayText.innerHTML = `${selectedProductCategories[0].icon} ${selectedProductCategories[0].name}`;
        btn.style.color = '#333';
    } else {
        const icons = selectedProductCategories.map(c => c.icon).join(' ');
        displayText.innerHTML = `${icons} (${selectedProductCategories.length} loại)`;
        btn.style.color = '#333';
    }

    // Update hidden input
    const ids = selectedProductCategories.map(c => c.id).join(',');
    document.getElementById('productCategory').value = ids;
}

function showProductCategoryPopup() {
    // Restore checkbox states
    document.querySelectorAll('#productCategoryList .product-checkbox').forEach(checkbox => {
        const id = parseInt(checkbox.dataset.productId);
        checkbox.checked = selectedProductCategories.some(c => c.id === id);
    });

    const productPopup = document.getElementById('productCategoryPopup');
    const overlay = document.getElementById('popupOverlay');

    if (productPopup) {
        productPopup.classList.add('show');
    }

    if (overlay) {
        overlay.classList.add('show');
    }
}

function hideProductCategoryPopup() {
    const productPopup = document.getElementById('productCategoryPopup');
    const pickupPopup = document.getElementById('pickupInfoPopup');
    const servicePopup = document.getElementById('serviceCategoryPopup');
    const overlay = document.getElementById('popupOverlay');

    if (productPopup) {
        productPopup.classList.remove('show');
    }

    if (overlay &&
        (!pickupPopup || !pickupPopup.classList.contains('show')) &&
        (!servicePopup || !servicePopup.classList.contains('show'))) {
        overlay.classList.remove('show');
    }
}

// Keep this for backward compatibility
function selectProductCategory(id, name, icon) {
    selectedProductCategories = [{ id, name, icon }];
    updateProductCategoryDisplay();
    hideProductCategoryPopup();
}

// ============ ITEMS MANAGEMENT ============
function addItemRow() {
    const itemsList = document.getElementById('itemsList');
    const rowCount = itemsList.children.length;

    const newRow = document.createElement('div');
    newRow.className = 'item-row';
    newRow.innerHTML = `
        <input type="text" class="item-input item-input-name" placeholder="Tên đồ dùng" name="Items[${rowCount}].Name">
        <input type="number" class="item-input item-input-quantity" placeholder="Số lượng" name="Items[${rowCount}].Quantity" min="1">
        <button type="button" class="btn-icon btn-remove" onclick="removeItemRow(this)">−</button>
    `;
    itemsList.appendChild(newRow);
}

function removeItemRow(button) {
    const row = button.closest('.item-row');
    if (document.querySelectorAll('.item-row').length > 1) {
        row.remove();
    }
}

// ============ VEHICLE SELECTION ============
function handleWeightInput() {
    const weightInput = document.getElementById('estimatedWeight');
    const weight = parseFloat(weightInput.value);

    if (isNaN(weight) || weight <= 0) {
        document.getElementById('vehicleSuggestions').classList.remove('show');
        return;
    }

    estimatedWeight = weight;
    showVehicleSuggestions(weight);
}

function showVehicleSuggestions(weight) {
    const suggestions = document.getElementById('vehicleSuggestions');
    suggestions.classList.add('show');

    const options = suggestions.querySelectorAll('.vehicle-option');
    options.forEach(option => {
        const maxWeight = parseFloat(option.dataset.maxWeight);
        if (weight > maxWeight) {
            option.style.opacity = '0.4';
            option.style.pointerEvents = 'none';
        } else {
            option.style.opacity = '1';
            option.style.pointerEvents = 'auto';
        }
    });
}

function selectVehicle(element, vehicleType) {
    // Update UI
    document.querySelectorAll('.vehicle-option').forEach(opt => opt.classList.remove('active'));
    element.classList.add('active');
    document.getElementById('selectedVehicle').value = vehicleType;
    
    
    // Recalculate distance and time when vehicle type changes
    // Use setTimeout to ensure DOM is updated first
    setTimeout(() => {
        if (pickupData && dropoffData) {
            calculateDistanceAndTime();
        }
    }, 100);
}

// ============ SUBMIT ORDER ============
async function submitOrder() {
    // Validation - Kiểm tra thông tin khách hàng
    const customerName = document.getElementById('customerName');
    const customerPhone = document.getElementById('customerPhone');
    const customerFloor = document.getElementById('customerFloor');
<<<<<<< HEAD

    console.log('Customer validation:', {
        customerName: customerName?.textContent,
        customerPhone: customerPhone?.textContent,
        customerFloor: customerFloor?.textContent
    });

    if (!customerName || !customerName.textContent || !customerPhone || !customerPhone.textContent) {
=======
    
    // Get actual text content (handle both text and span elements)
    const getName = () => {
        const textSpan = customerName?.querySelector('.customer-info-text');
        return textSpan ? textSpan.textContent : customerName?.textContent || '';
    };
    
    const getPhone = () => {
        const textSpan = customerPhone?.querySelector('.customer-info-text');
        return textSpan ? textSpan.textContent.replace('📞 ', '') : customerPhone?.textContent?.replace('📞 ', '') || '';
    };
    
    const getFloor = () => {
        const textSpan = customerFloor?.querySelector('.customer-info-text');
        return textSpan ? textSpan.textContent.replace('🏢 ', '') : customerFloor?.textContent?.replace('🏢 ', '') || '';
    };
    
    const nameValue = getName();
    const phoneValue = getPhone();
    const floorValue = getFloor();
    
    
    if (!nameValue || !phoneValue) {
>>>>>>> origin/Tuanlmhe
        alert('⚠️ Vui lòng nhập đầy đủ thông tin khách hàng!');
        return;
    }

    // Validation - Kiểm tra địa chỉ lấy hàng
    const pickupAddress = document.getElementById('pickupAddressInput').value.trim();
    if (!pickupAddress) {
        alert('⚠️ Vui lòng chọn địa chỉ lấy hàng!');
        return;
    }

    if (!dropoffData) {
        alert('⚠️ Vui lòng chọn khu vực tìm kho!');
        return;
    }

    const deliveryDate = document.getElementById('deliveryDate').value;
    const pickupDate = document.getElementById('pickupDate').value;

    if (!deliveryDate || !pickupDate) {
        alert('⚠️ Vui lòng chọn ngày vận chuyển và ngày lấy đồ!');
        return;
    }

    if (new Date(pickupDate) < new Date(deliveryDate)) {
        alert('⚠️ Ngày lấy đồ phải sau hoặc bằng ngày vận chuyển!');
        return;
    }

    if (selectedProductCategories.length === 0) {
        alert('⚠️ Vui lòng chọn ít nhất một loại hàng hóa!');
        return;
    }

    if (!estimatedWeight || estimatedWeight <= 0) {
        alert('⚠️ Vui lòng nhập trọng lượng ước tính!');
        return;
    }

    const selectedVehicle = document.getElementById('selectedVehicle').value;
    if (!selectedVehicle) {
        alert('⚠️ Vui lòng chọn phương tiện vận chuyển!');
        return;
    }

    // Collect items
    const items = Array.from(document.querySelectorAll('.item-row')).map(row => {
        const name = row.querySelector('.item-input-name').value.trim();
        const quantity = parseInt(row.querySelector('.item-input-quantity').value) || 0;
        return { name, quantity };
    }).filter(item => item.name && item.quantity > 0);

    // Build order data
    const orderData = {
        pickupAddress: {
            stored: false,
            addressLine: pickupAddress,
            label: 'Điểm lấy hàng',
            ward: '',
            district: '',
            city: 'Hà Nội',
            latitude: pickupData ? pickupData.lat : 0,
            longitude: pickupData ? pickupData.lng : 0,
            recipientName: nameValue,
            recipientPhone: phoneValue,
            floor: floorValue
        },
        dropoffAddress: {
            stored: false,
            addressLine: dropoffData.street,
            label: 'Khu vực tìm kho',
            ward: '',
            district: '',
            city: 'Hà Nội',
            latitude: dropoffData.lat,
            longitude: dropoffData.lng
        },
        deliveryDate: deliveryDate,
        pickupDate: pickupDate,
        note: document.getElementById('orderNote').value || '',
        vehicleType: selectedVehicle,
        productCategories: selectedProductCategories.map(c => c.id),  // Array of IDs
        estimatedWeight: estimatedWeight,
        items: items,
        // Add calculated distance and time
        distanceKm: calculatedDistance,
        etaMinutes: calculatedEta
    };

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

        // Check content type before parsing
        const contentType = response.headers.get('content-type');
        let result;

        if (contentType && contentType.includes('application/json')) {
            result = await response.json();
        } else {
            // If not JSON, get as text
            const text = await response.text();
            console.error('Server response (not JSON):', text);
            result = { success: false, message: text };
        }

        if (response.ok && result.success) {
            const productNames = selectedProductCategories.map(c => c.name).join(', ');

            alert(`✅ ${result.message}\n\n📦 Mã đơn hàng: #${result.orderId}\n🏪 Số kho được thông báo: ${result.nearbyStoresCount}\n📅 Ngày vận chuyển: ${deliveryDate}\n📅 Ngày lấy đồ: ${pickupDate}\n 📦 Hàng hóa: ${productNames}`);
            window.location.href = `/Delivery/Order?orderId=${result.orderId}`;
        } else {
            // Log full error for debugging
            console.error('Order submission failed:', result);
            console.error('Sent data:', orderData);

            alert('❌ ' + (result.message || 'Có lỗi xảy ra. Vui lòng thử lại!'));
            bookBtn.textContent = originalText;
            bookBtn.disabled = false;
        }
    } catch (error) {
        console.error('Error submitting order:', error);
        console.error('Sent data:', orderData);
        alert('❌ Không thể kết nối đến máy chủ. Vui lòng kiểm tra kết nối và thử lại!');
        bookBtn.textContent = originalText;
        bookBtn.disabled = false;
    }
}