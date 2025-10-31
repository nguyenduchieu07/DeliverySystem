// Booking Page JavaScript
let map, warehouseMarker;
let warehouseData = null;
let selectedWarehouse = null;
let nearbyWarehouses = [];

// Initialize on page load
document.addEventListener('DOMContentLoaded', function () {
    console.log('DOMContentLoaded - Initializing booking page...');
    
    initMap();
    initDateInputs();
    initAddressAutocomplete();
    initEstimationCard();
    
    // Đợi map khởi tạo xong rồi mới load warehouses
    // Tự động load kho gần khi trang load
    // Thử lấy vị trí hiện tại, nếu không được thì dùng vị trí mặc định (Hà Nội)
    function loadWarehousesAfterMapReady() {
        if (!map) {
            console.log('Waiting for map to be ready...');
            setTimeout(loadWarehousesAfterMapReady, 100);
            return;
        }
        
        console.log('Map is ready, loading warehouses...');
        
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const lat = position.coords.latitude;
                    const lng = position.coords.longitude;
                    console.log('Got current position:', lat, lng);
                    setWarehouseLocation(lat, lng);
                    loadNearbyWarehouses(lat, lng);
                },
                (error) => {
                    console.log('Geolocation error:', error.message);
                    // Nếu không lấy được vị trí, dùng vị trí mặc định (Hà Nội)
                    const defaultLat = 21.028511;
                    const defaultLng = 105.804817;
                    console.log('Using default location:', defaultLat, defaultLng);
                    loadNearbyWarehouses(defaultLat, defaultLng);
                },
                { timeout: 5000, enableHighAccuracy: false }
            );
        } else {
            // Trình duyệt không hỗ trợ geolocation, dùng vị trí mặc định
            const defaultLat = 21.028511;
            const defaultLng = 105.804817;
            console.log('Geolocation not supported, using default location:', defaultLat, defaultLng);
            loadNearbyWarehouses(defaultLat, defaultLng);
        }
    }
    
    // Đợi một chút để đảm bảo map đã khởi tạo
    setTimeout(loadWarehousesAfterMapReady, 500);
});

// ============ MAP FUNCTIONS ============
function initMap() {
    const mapElement = document.getElementById('map');
    if (!mapElement) {
        console.error('Map element #map not found!');
        return false;
    }

    // Kiểm tra Leaflet đã load chưa
    if (typeof L === 'undefined') {
        console.error('Leaflet library (L) not loaded! Make sure Leaflet script is loaded before booking.js');
        // Retry sau 100ms
        setTimeout(initMap, 100);
        return false;
    }

    try {
        console.log('Initializing map...');
        map = L.map('map').setView([21.028511, 105.804817], 13);
        
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; OpenStreetMap contributors',
            maxZoom: 19
        }).addTo(map);

        // Đợi map load xong
        map.whenReady(function() {
            console.log('✅ Map initialized and ready');
        });

        // Geocoder control - chỉ thêm nếu có
        if (typeof L.Control !== 'undefined' && L.Control.geocoder) {
            L.Control.geocoder({
                defaultMarkGeocode: false,
                placeholder: 'Tìm kiếm địa điểm...',
                errorMessage: 'Không tìm thấy'
            }).on('markgeocode', function (e) {
                const latlng = e.geocode.center;
                setWarehouseLocation(latlng.lat, latlng.lng);
            }).addTo(map);
        } else {
            console.warn('Geocoder not available');
        }
        
        return true;
    } catch (error) {
        console.error('Error initializing map:', error);
        return false;
    }
}

// ============ DATE FUNCTIONS ============
function initDateInputs() {
    const today = new Date().toISOString().split('T')[0];
    const startDateInput = document.getElementById('storageStartDate');
    const endDateInput = document.getElementById('storageEndDate');

    if (startDateInput) {
        startDateInput.min = today;
        if (!startDateInput.value) {
            startDateInput.value = today;
        }
    }
    if (endDateInput) {
        endDateInput.min = today;
        if (!endDateInput.value) {
            const defaultEndDate = new Date();
            defaultEndDate.setDate(defaultEndDate.getDate() + 30);
            endDateInput.value = defaultEndDate.toISOString().split('T')[0];
        }

        if (startDateInput) {
            startDateInput.addEventListener('change', function () {
                const startDate = new Date(this.value);
                const minEndDate = new Date(startDate);
                minEndDate.setDate(minEndDate.getDate() + 1);
                if (endDateInput) {
                    endDateInput.min = minEndDate.toISOString().split('T')[0];
                    if (new Date(endDateInput.value) < minEndDate) {
                        endDateInput.value = minEndDate.toISOString().split('T')[0];
                    }
                }
            });
        }
    }
}

// ============ ADDRESS AUTOCOMPLETE ============
function initAddressAutocomplete() {
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
                        const addressText = it.display_name || '';
                        
                        // Cập nhật địa chỉ vào input và warehouseData
                        input.value = addressText;
                        setWarehouseLocation(lat, lng, addressText);
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

// ============ CURRENT LOCATION ============
function getCurrentLocation() {
    if (!navigator.geolocation) {
        alert('Trình duyệt không hỗ trợ lấy vị trí hiện tại');
        return;
    }

        navigator.geolocation.getCurrentPosition(
            (position) => {
                const lat = position.coords.latitude;
                const lng = position.coords.longitude;
                
                // Lấy địa chỉ từ reverse geocode trước rồi mới set location
                reverseGeocode(lat, lng).then(address => {
                    const input = document.getElementById('warehouseAreaInput');
                    if (input) input.value = address;
                    setWarehouseLocation(lat, lng, address);
                });
            },
        (error) => {
            alert('Không thể lấy vị trí hiện tại: ' + error.message);
        }
    );
}

function searchLocation() {
    const input = document.getElementById('warehouseAreaInput');
    if (input && input.value.trim().length >= 2) {
        input.dispatchEvent(new Event('input'));
    }
}

// ============ WAREHOUSE LOCATION ============
function setWarehouseLocation(lat, lng, addressLine = null) {
    // Lưu địa chỉ nhận hàng (PickupAddress) - đây là địa chỉ từ input hoặc vị trí hiện tại
    warehouseData = { lat: lat, lng: lng, address: addressLine || '' };

    // Cập nhật input với địa chỉ
    const input = document.getElementById('warehouseAreaInput');
    if (input && addressLine) {
        input.value = addressLine;
    }

    if (map) {
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
            }).addTo(map).bindPopup('📍 Địa chỉ nhận hàng (Kéo để di chuyển)');

            warehouseMarker.on('dragend', async function (e) {
                const newPos = e.target.getLatLng();
                warehouseData.lat = newPos.lat;
                warehouseData.lng = newPos.lng;
                const newAddress = await reverseGeocode(newPos.lat, newPos.lng);
                warehouseData.address = newAddress;
                // Cập nhật input với địa chỉ mới
                const input = document.getElementById('warehouseAreaInput');
                if (input) input.value = newAddress;
                loadNearbyWarehouses(newPos.lat, newPos.lng);
            });
        }
        map.setView([lat, lng], 15);
    }

    loadNearbyWarehouses(lat, lng);
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

// ============ LOAD NEARBY WAREHOUSES ============
async function loadNearbyWarehouses(lat, lng) {
    if (!lat || !lng) {
        console.error('loadNearbyWarehouses: Invalid coordinates', lat, lng);
        return;
    }

    console.log('Loading nearby warehouses for:', lat, lng);

    try {
        const url = `/Quote/NearbyWarehouses?lat=${lat}&lng=${lng}&take=10`;
        console.log('Fetching:', url);
        
        const response = await fetch(url);
        
        if (!response.ok) {
            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }
        
        const warehouses = await response.json();
        console.log('Received warehouses:', warehouses);
        
        if (!warehouses || !Array.isArray(warehouses)) {
            console.error('Invalid warehouses data:', warehouses);
            return;
        }
        
        nearbyWarehouses = warehouses;
        renderWarehouseList(warehouses);
        displayWarehousesOnMap(warehouses);
    } catch (error) {
        console.error('Error loading warehouses:', error);
        alert('Không thể tải danh sách kho. Vui lòng thử lại sau.');
    }
}

function displayWarehousesOnMap(warehouses) {
    console.log('displayWarehousesOnMap called with:', warehouses);
    
    if (!map) {
        console.error('Map is not initialized!');
        return;
    }

    // Remove old warehouse markers (keep warehouseMarker if exists)
    const markersToRemove = [];
    map.eachLayer(layer => {
        if (layer instanceof L.Marker && layer !== warehouseMarker) {
            markersToRemove.push(layer);
        }
    });
    markersToRemove.forEach(marker => map.removeLayer(marker));

    if (!warehouses || warehouses.length === 0) {
        console.log('No warehouses to display');
        return;
    }

    console.log(`Displaying ${warehouses.length} warehouses on map`);

    // Tạo bounds để fit tất cả kho vào view
    const bounds = [];
    let markersAdded = 0;
    
    warehouses.forEach((warehouse, index) => {
        // ASP.NET Core mặc định serialize JSON thành camelCase
        // Nên property sẽ là "latitude" và "longitude", không phải "Latitude" và "Longitude"
        let lat = warehouse.latitude || warehouse.Latitude || warehouse.lat || warehouse.Lat;
        let lng = warehouse.longitude || warehouse.Longitude || warehouse.lng || warehouse.Lng;
        
        // Kiểm tra và convert
        if (lat == null || lng == null || lat === undefined || lng === undefined) {
            console.warn(`Warehouse ${index} missing coordinates`);
            console.warn('Full object:', warehouse);
            console.warn('Available fields:', Object.keys(warehouse));
            // Log từng field để debug
            Object.keys(warehouse).forEach(key => {
                console.warn(`  ${key}:`, warehouse[key], typeof warehouse[key]);
            });
            return;
        }
        
        const latNum = parseFloat(lat);
        const lngNum = parseFloat(lng);
        
        if (isNaN(latNum) || isNaN(lngNum)) {
            console.error(`Warehouse ${index} has invalid coordinates:`, lat, lng);
            return;
        }
        
        console.log(`Adding marker for warehouse ${index}:`, warehouse.name || warehouse.Name, 'at', latNum, lngNum);
        
        bounds.push([latNum, lngNum]);
        
        // ASP.NET Core serialize thành camelCase
        const warehouseName = warehouse.name || warehouse.Name || 'Kho';
        const warehouseId = warehouse.id || warehouse.Id;
        const distanceKm = warehouse.distanceKm || 0;
        const address = warehouse.full || warehouse.addressLine || warehouse.AddressLine || '';
        const storeName = warehouse.storeName || warehouse.StoreName || '';
        
        try {
            // Tạo marker với icon đẹp hơn
            const marker = L.marker([latNum, lngNum], {
                icon: L.divIcon({
                    className: 'warehouse-marker',
                    html: `<div style="background:linear-gradient(135deg, #667eea 0%, #764ba2 100%);color:white;padding:8px 12px;border-radius:20px;font-size:13px;font-weight:700;box-shadow:0 4px 12px rgba(102,126,234,0.5);white-space:nowrap;border:2px solid white;">
                            🏪 ${warehouseName}
                          </div>`,
                    iconSize: [150, 40],
                    iconAnchor: [75, 20]
                })
            }).addTo(map);
            
            markersAdded++;
            
            // Bind popup với thông tin chi tiết
            const popupContent = `
                <div style="min-width:200px;">
                    <h4 style="margin:0 0 8px 0;color:#667eea;">${warehouseName}</h4>
                    <p style="margin:4px 0;font-size:13px;color:#666;">${storeName ? '📦 ' + storeName : ''}</p>
                    <p style="margin:4px 0;font-size:12px;color:#888;">${address}</p>
                    <p style="margin:8px 0 0 0;font-size:12px;">
                        <strong style="color:#27ae60;">📍 ${distanceKm.toFixed(2)} km</strong>
                    </p>
                    <button onclick="selectWarehouseFromMap('${warehouseId}')" style="margin-top:8px;padding:6px 12px;background:#667eea;color:white;border:none;border-radius:6px;cursor:pointer;font-size:12px;width:100%;">
                        Chọn kho này
                    </button>
                </div>
            `;
            marker.bindPopup(popupContent);

            // Lưu warehouseId vào marker options để có thể highlight khi chọn
            marker.options.warehouseId = warehouseId?.toString();
            marker.options.warehouse = warehouse; // Lưu cả object warehouse
            
            marker.on('click', function() {
                selectWarehouse(warehouse);
                // Mở popup khi click
                marker.openPopup();
            });
        } catch (error) {
            console.error(`Error adding marker for warehouse ${index}:`, error);
        }
    });

    console.log(`Added ${markersAdded} markers to map`);

    // Fit map view để hiển thị tất cả kho
    if (bounds.length > 0) {
        // Nếu có warehouseMarker, thêm vào bounds
        if (warehouseMarker) {
            const markerLatLng = warehouseMarker.getLatLng();
            bounds.push([markerLatLng.lat, markerLatLng.lng]);
        }
        
        try {
            console.log('Fitting bounds for', bounds.length, 'locations');
            map.fitBounds(bounds, { 
                padding: [50, 50],
                maxZoom: 15 // Giới hạn zoom tối đa
            });
        } catch (e) {
            console.error('Error fitting bounds:', e);
        }
    } else {
        console.warn('No bounds to fit');
    }
}

// Helper function để chọn kho từ map popup
function selectWarehouseFromMap(warehouseId) {
    const warehouse = nearbyWarehouses.find(w => {
        const wId = w.id || w.Id;
        return wId && wId.toString() === warehouseId.toString();
    });
    if (warehouse) {
        selectWarehouse(warehouse);
    }
}

function renderWarehouseList(warehouses) {
    const listContainer = document.getElementById('warehouseList');
    if (!listContainer) return;

    listContainer.innerHTML = '';

    if (!warehouses || warehouses.length === 0) {
        listContainer.innerHTML = '<div style="padding: 15px; text-align: center; color: #666;">Không tìm thấy kho nào gần đây.</div>';
        return;
    }

    warehouses.forEach(warehouse => {
        // ASP.NET Core serialize thành camelCase
        const warehouseId = warehouse.id || warehouse.Id;
        const item = document.createElement('div');
        item.className = 'warehouse-item';
        item.dataset.warehouseId = warehouseId;
        
        // Check if this is the selected warehouse
        if (selectedWarehouse) {
            const selectedId = selectedWarehouse.id || selectedWarehouse.Id;
            if (selectedId === warehouseId) {
                item.classList.add('selected');
            }
        }

        item.innerHTML = `
            <div class="warehouse-name">${warehouse.name || warehouse.Name || 'Kho'}</div>
            <div class="warehouse-address">${warehouse.full || warehouse.addressLine || warehouse.AddressLine || ''}</div>
            <div class="warehouse-info">
                <div>📦 ${warehouse.storeName || warehouse.StoreName || ''}</div>
                <div class="warehouse-distance">${warehouse.distanceKm || 0} km</div>
            </div>
        `;

        item.addEventListener('click', function() {
            selectWarehouse(warehouse);
        });

        listContainer.appendChild(item);
    });
}

function selectWarehouse(warehouse) {
    console.log('Selecting warehouse:', warehouse);
    selectedWarehouse = warehouse;
    const warehouseIdInput = document.getElementById('warehouseIdInput');
    const warehouseNameDisplay = document.getElementById('warehouseNameDisplay');
    
    // ASP.NET Core serialize thành camelCase
    const warehouseId = warehouse.id || warehouse.Id;
    if (warehouseIdInput && warehouseId) {
        warehouseIdInput.value = warehouseId.toString();
    }
    if (warehouseNameDisplay) {
        warehouseNameDisplay.value = warehouse.name || warehouse.Name || '';
    }

    // Update UI - highlight selected warehouse
    document.querySelectorAll('.warehouse-item').forEach((item) => {
        item.classList.remove('selected');
        const itemId = item.dataset.warehouseId || item.getAttribute('data-warehouse-id');
        if (itemId && warehouseId && itemId.toString() === warehouseId.toString()) {
            item.classList.add('selected');
        }
    });

    // Highlight warehouse marker on map
    if (map) {
        map.eachLayer(layer => {
            if (layer instanceof L.Marker && layer !== warehouseMarker) {
                const layerId = layer.options?.warehouseId;
                const warehouseObj = layer.options?.warehouse;
                const whName = warehouse.Name || warehouse.name || warehouseObj?.Name || warehouseObj?.name || 'Kho';
                
                if (layerId === warehouseId?.toString()) {
                    // Highlight selected warehouse
                    layer.setIcon(L.divIcon({
                        className: 'warehouse-marker',
                        html: `<div style="background:linear-gradient(135deg, #27ae60 0%, #2ecc71 100%);color:white;padding:8px 12px;border-radius:20px;font-size:13px;font-weight:700;box-shadow:0 4px 12px rgba(39,174,96,0.6);white-space:nowrap;border:3px solid #ffd700;">
                                🏪 ${whName}
                              </div>`,
                        iconSize: [150, 40],
                        iconAnchor: [75, 20]
                    }));
                    // Mở popup và center vào marker đã chọn
                    layer.openPopup();
                    map.setView(layer.getLatLng(), Math.max(map.getZoom(), 14));
                } else {
                    // Reset other markers to normal state
                    if (warehouseObj) {
                        layer.setIcon(L.divIcon({
                            className: 'warehouse-marker',
                            html: `<div style="background:linear-gradient(135deg, #667eea 0%, #764ba2 100%);color:white;padding:8px 12px;border-radius:20px;font-size:13px;font-weight:700;box-shadow:0 4px 12px rgba(102,126,234,0.5);white-space:nowrap;border:2px solid white;">
                                    🏪 ${whName}
                                  </div>`,
                            iconSize: [150, 40],
                            iconAnchor: [75, 20]
                        }));
                    }
                }
            }
        });
    }

    // Không load slots tự động - chỉ load khi nhấn "Xem sơ đồ kho"
    // Slots sẽ được load khi user nhấn nút "Xem sơ đồ kho & chọn vị trí"
}

// ============ WAREHOUSE SLOTS ============
async function loadWarehouseSlots(warehouseId) {
    try {
        // Reset selected slots khi load kho mới
        selectedSlots = [];
        updateSelectedSlotsSummary();
        
        const response = await fetch(`/api/warehouses/${warehouseId}/slots`);
        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }
        const slots = await response.json();
        renderWarehouseGrid(slots);
    } catch (error) {
        console.error('Error loading slots:', error);
        const grid = document.getElementById('warehouseGrid');
        if (grid) {
            grid.innerHTML = '<div style="padding: 20px; text-align: center; color: #e74c3c;">❌ Không thể tải sơ đồ kho. Vui lòng thử lại sau.</div>';
        }
    }
}

function renderWarehouseGrid(slots) {
    const grid = document.getElementById('warehouseGrid');
    if (!grid) return;

    grid.innerHTML = '';

    if (!slots || slots.length === 0) {
        grid.innerHTML = '<div style="padding: 20px; text-align: center; color: #666;">Kho này chưa có slot nào.</div>';
        return;
    }

    // Find max row and col to create grid
    let maxRow = 0, maxCol = 0;
    slots.forEach(slot => {
        if (slot.row > maxRow) maxRow = slot.row;
        if (slot.col > maxCol) maxCol = slot.col;
    });

    // Create grid CSS
    if (maxRow > 0 && maxCol > 0) {
        grid.style.gridTemplateColumns = `repeat(${maxCol}, minmax(60px, 1fr))`;
    }

    // Create a map for quick lookup
    const slotMap = new Map();
    slots.forEach(slot => {
        const key = `${slot.row}-${slot.col}`;
        slotMap.set(key, slot);
    });

    // Render slots in grid order
    for (let r = 1; r <= maxRow; r++) {
        for (let c = 1; c <= maxCol; c++) {
            const key = `${r}-${c}`;
            const slot = slotMap.get(key);
            
            const slotEl = document.createElement('div');
            if (slot) {
                let statusClass = 'available';
                if (slot.status === 'blocked') statusClass = 'blocked';
                else if (slot.status === 'occupied') statusClass = 'occupied';
                else if (slot.status === 'reserved') statusClass = 'reserved';

                slotEl.className = `warehouse-slot ${statusClass}`;
                slotEl.dataset.slotId = slot.id;
                slotEl.textContent = slot.code || `${r}-${c}`;
                slotEl.title = `Slot: ${slot.code || 'N/A'}\nSize: ${slot.size || 'N/A'}\nPrice: ${slot.basePricePerHour || 'N/A'} đ/h`;

                if (statusClass !== 'blocked' && statusClass !== 'occupied') {
                    slotEl.addEventListener('click', function() {
                        toggleSlotSelection(slotEl, slot);
                    });
                }
            } else {
                // Empty cell
                slotEl.className = 'warehouse-slot blocked';
                slotEl.style.opacity = '0.2';
            }

            grid.appendChild(slotEl);
        }
    }
}

let selectedSlots = []; // Array to store selected slot IDs

function toggleSlotSelection(element, slot) {
    if (element.classList.contains('occupied') || element.classList.contains('blocked')) {
        return;
    }

    const slotId = slot.id || element.dataset.slotId;
    const isSelected = element.classList.contains('selected');
    
    if (isSelected) {
        // Deselect
        element.classList.remove('selected');
        selectedSlots = selectedSlots.filter(id => id !== slotId);
    } else {
        // Select
        element.classList.add('selected');
        if (slotId && !selectedSlots.includes(slotId)) {
            selectedSlots.push(slotId);
        }
    }
    
    updateSelectedSlotsSummary();
}

function updateSelectedSlotsSummary() {
    const selected = document.querySelectorAll('.warehouse-slot.selected');
    const summary = document.getElementById('selectedSlotsSummary');
    const slotsList = document.getElementById('selectedSlotsList');
    
    if (summary && slotsList) {
        if (selected.length > 0) {
            summary.style.display = 'block';
            const slotCodes = Array.from(selected).map(el => el.textContent.trim()).filter(t => t).join(', ');
            slotsList.textContent = `${selected.length} slot(s): ${slotCodes}`;
        } else {
            summary.style.display = 'none';
            slotsList.textContent = 'Chưa chọn slot nào';
        }
    }
}

function showWarehouseSlots() {
    // Lấy warehouseId từ selectedWarehouse hoặc từ input hidden
    let warehouseId = null;
    let warehouseName = '';
    
    if (selectedWarehouse) {
        warehouseId = selectedWarehouse.Id || selectedWarehouse.id;
        warehouseName = selectedWarehouse.Name || selectedWarehouse.name || '';
    }
    
    // Nếu không có, lấy từ input hidden
    if (!warehouseId) {
        const warehouseIdInput = document.getElementById('warehouseIdInput');
        const warehouseNameDisplay = document.getElementById('warehouseNameDisplay');
        if (warehouseIdInput && warehouseIdInput.value) {
            warehouseId = warehouseIdInput.value;
        }
        if (warehouseNameDisplay && warehouseNameDisplay.value) {
            warehouseName = warehouseNameDisplay.value;
        }
    }
    
    if (!warehouseId) {
        alert('⚠️ Vui lòng chọn kho từ danh sách trước!');
        return;
    }
    
    const section = document.getElementById('warehouseSlotSection');
    if (section) {
        // Hiển thị thông tin kho đã chọn
        const warehouseInfo = document.getElementById('selectedWarehouseInfo');
        const warehouseNameSpan = document.getElementById('selectedWarehouseName');
        if (warehouseInfo) warehouseInfo.style.display = 'block';
        if (warehouseNameSpan) warehouseNameSpan.textContent = warehouseName || 'Kho đã chọn';
        
        section.classList.add('show');
        
        // Hiển thị loading
        const grid = document.getElementById('warehouseGrid');
        if (grid) {
            grid.innerHTML = '<div style="padding: 20px; text-align: center; color: #667eea;">🔄 Đang tải sơ đồ kho...</div>';
        }
        
        // Load slots với đúng warehouseId
        loadWarehouseSlots(warehouseId);
        
        // Scroll to section
        setTimeout(() => {
            section.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }, 100);
    }
}

function toggleSlotSection() {
    const section = document.getElementById('warehouseSlotSection');
    if (section) {
        section.classList.toggle('show');
    }
}

// ============ ITEMS MANAGEMENT ============
function addItemRow(name = '', category = '', quantity = 1, weight = 0) {
    const tbody = document.getElementById('itemsTableBody');
    if (!tbody) return;

    // Xóa dòng "Chưa có món nào" nếu có
    const emptyRow = tbody.querySelector('tr td[colspan]');
    if (emptyRow) {
        emptyRow.closest('tr').remove();
    }

    // Tìm index tiếp theo (bỏ qua empty row)
    const existingRows = tbody.querySelectorAll('tr:not(:has(td[colspan]))');
    const idx = existingRows.length;
    
    const row = document.createElement('tr');
    row.style.cssText = 'border-bottom: 1px solid #e0e0e0;';
    row.innerHTML = `
        <td style="padding: 10px;">
            <input type="text" class="form-control" name="Items[${idx}].Name" value="${name}" placeholder="Ví dụ: Bàn học sinh">
        </td>
        <td style="padding: 10px;">
            <input type="text" class="form-control" name="Items[${idx}].Category" value="${category}" placeholder="Ví dụ: Nội thất">
        </td>
        <td style="padding: 10px;">
            <input type="number" class="form-control" name="Items[${idx}].Quantity" value="${quantity}" min="1" placeholder="1" style="text-align: center;">
        </td>
        <td style="padding: 10px;">
            <input type="number" class="form-control" name="Items[${idx}].EstimatedWeightKg" value="${weight}" min="0" step="0.1" placeholder="0" style="text-align: center;">
        </td>
        <td style="padding: 10px; text-align: center;">
            <button type="button" class="location-btn" onclick="removeItemRow(this)" style="padding: 6px 10px; background: #e74c3c;">✖</button>
        </td>
    `;
    tbody.appendChild(row);
}

// Function để load dữ liệu mẫu (có thể gọi từ console hoặc button)
function loadSampleData() {
    const sampleItems = [
        { name: 'Bàn học sinh', category: 'Nội thất phòng học', quantity: 15, weight: 12 },
        { name: 'Ghế học sinh', category: 'Nội thất phòng học', quantity: 15, weight: 5 },
        { name: 'Bàn giáo viên', category: 'Nội thất phòng học', quantity: 1, weight: 25 },
        { name: 'Ghế giáo viên', category: 'Nội thất phòng học', quantity: 1, weight: 8 }
    ];
    
    // Xóa tất cả rows hiện tại
    const tbody = document.getElementById('itemsTableBody');
    if (!tbody) return;
    
    tbody.innerHTML = '';
    
    // Thêm sample data
    sampleItems.forEach(item => {
        addItemRow(item.name, item.category, item.quantity, item.weight);
    });
    
    console.log('✅ Đã load dữ liệu mẫu:', sampleItems);
}

function removeItemRow(button) {
    const row = button.closest('tr');
    if (row) {
        row.remove();
        
        // Nếu không còn row nào, thêm dòng "Chưa có món nào"
        const tbody = document.getElementById('itemsTableBody');
        if (tbody && tbody.children.length === 0) {
            const emptyRow = document.createElement('tr');
            emptyRow.innerHTML = `
                <td colspan="5" style="padding: 15px; text-align: center; color: #999; font-style: italic;">
                    Chưa có món nào. Nhấn "+ Thêm món" để thêm đồ dùng.
                </td>
            `;
            tbody.appendChild(emptyRow);
        }
        
        // Cập nhật lại index của các input
        updateItemIndexes();
    }
}

function updateItemIndexes() {
    const tbody = document.getElementById('itemsTableBody');
    if (!tbody) return;
    
    const rows = tbody.querySelectorAll('tr');
    rows.forEach((row, index) => {
        if (row.querySelector('td[colspan]')) return; // Skip empty row
        
        const inputs = row.querySelectorAll('input');
        inputs.forEach(input => {
            const name = input.name;
            if (name) {
                const newName = name.replace(/Items\[\d+\]/, `Items[${index}]`);
                input.name = newName;
            }
        });
    });
}

// ============ IMAGE PREVIEW ============
function previewTotalImage(input) {
    const file = input.files[0];
    const preview = document.getElementById('productImagePreview');
    if (file && preview) {
        const reader = new FileReader();
        reader.onload = function(e) {
            preview.src = e.target.result;
            preview.style.display = 'block';
        };
        reader.readAsDataURL(file);
    }
}

// ============ SUBMIT ORDER ============
async function submitWarehouseOrder() {
    // Validation - Lấy địa chỉ từ input tìm kiếm hoặc warehouseData
    const warehouseAreaInput = document.getElementById('warehouseAreaInput');
    const pickupAddressText = warehouseAreaInput?.value?.trim() || warehouseData?.address || '';
    
    if (!pickupAddressText) {
        alert('⚠️ Vui lòng nhập địa chỉ nhận hàng hoặc chọn vị trí hiện tại!');
        warehouseAreaInput?.focus();
        return;
    }

    // Validation - Kiểm tra có tọa độ không
    if (!warehouseData || !warehouseData.lat || !warehouseData.lng) {
        alert('⚠️ Vui lòng chọn khu vực muốn tìm kho bằng cách nhập địa chỉ hoặc nhấn "Vị trí hiện tại"!');
        warehouseAreaInput?.focus();
        return;
    }

    if (!selectedWarehouse) {
        alert('⚠️ Vui lòng chọn kho từ danh sách!');
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

    // Collect items from table rows
    const items = [];
    const rows = document.querySelectorAll('#itemsTableBody > tr');
    rows.forEach((row, idx) => {
        // Skip empty row
        if (row.querySelector('td[colspan]')) return;
        
        const nameInput = row.querySelector('input[name*=".Name"]');
        const categoryInput = row.querySelector('input[name*=".Category"]');
        const quantityInput = row.querySelector('input[name*=".Quantity"]');
        const weightInput = row.querySelector('input[name*=".EstimatedWeightKg"]');
        
        const name = nameInput?.value?.trim();
        const category = categoryInput?.value?.trim();
        const quantity = parseInt(quantityInput?.value) || 0;
        const weight = parseFloat(weightInput?.value) || 0;

        if (name && quantity > 0) {
            items.push({ name, category, quantity, estimatedWeightKg: weight });
        }
    });

    if (items.length === 0) {
        alert('⚠️ Vui lòng nhập ít nhất một món đồ!');
        return;
    }

    // Collect special requirements
    const specialRequirements = [];
    document.querySelectorAll('input[name="SpecialRequirements"]:checked').forEach(cb => {
        specialRequirements.push(cb.value);
    });

    // Build form data
    const formData = new FormData();
    
    // PickupAddress - Lấy từ input tìm kiếm hoặc warehouseData
    // Đây là địa chỉ nhận hàng (nơi khách hàng muốn gửi hàng đi)
    const pickupAddressLine = pickupAddressText;
    const pickupLat = warehouseData.lat;
    const pickupLng = warehouseData.lng;
    
    formData.append('PickupAddress.AddressLine', pickupAddressLine);
    formData.append('PickupAddress.Latitude', pickupLat);
    formData.append('PickupAddress.Longitude', pickupLng);
    
    // WarehouseArea - Địa chỉ kho đã chọn (nơi lưu trữ)
    const warehouseAreaLine = selectedWarehouse.full || selectedWarehouse.addressLine || selectedWarehouse.AddressLine || selectedWarehouse.name || 'Kho đã chọn';
    const warehouseLat = selectedWarehouse.latitude || selectedWarehouse.Latitude || selectedWarehouse.lat || selectedWarehouse.Lat;
    const warehouseLng = selectedWarehouse.longitude || selectedWarehouse.Longitude || selectedWarehouse.lng || selectedWarehouse.Lng;
    
    // Gửi WarehouseId (ưu tiên) để tìm warehouse chính xác
    const warehouseId = selectedWarehouse.id || selectedWarehouse.Id;
    if (warehouseId) {
        formData.append('WarehouseId', warehouseId.toString());
    }
    
    formData.append('WarehouseArea.AddressLine', warehouseAreaLine);
    formData.append('WarehouseArea.Latitude', warehouseLat);
    formData.append('WarehouseArea.Longitude', warehouseLng);
    formData.append('StorageStartDate', startDate);
    formData.append('StorageEndDate', endDate);
    formData.append('Note', document.getElementById('orderNote')?.value || '');

    items.forEach((item, idx) => {
        formData.append(`Items[${idx}].Name`, item.name);
        formData.append(`Items[${idx}].Category`, item.category || '');
        formData.append(`Items[${idx}].Quantity`, item.quantity);
        formData.append(`Items[${idx}].EstimatedWeightKg`, item.estimatedWeightKg || 0);
    });

    specialRequirements.forEach((req, idx) => {
        formData.append(`SpecialRequirements[${idx}]`, req);
    });

    // Add product image if available
    const productImageInput = document.getElementById('productImageInput');
    if (productImageInput && productImageInput.files && productImageInput.files[0]) {
        formData.append('productImage', productImageInput.files[0]);
    }

    // Submit
    const bookBtn = document.getElementById('bookBtn');
    if (bookBtn) {
        const originalText = bookBtn.textContent;
        bookBtn.textContent = '⏳ Đang gửi yêu cầu...';
        bookBtn.disabled = true;

        try {
            console.log('Submitting order with data:');
            console.log('PickupAddress:', pickupAddressLine, pickupLat, pickupLng);
            console.log('WarehouseArea:', warehouseAreaLine, warehouseLat, warehouseLng);
            console.log('WarehouseId:', warehouseId);
            console.log('SelectedWarehouse:', selectedWarehouse);
            console.log('Items:', items);
            console.log('Dates:', startDate, endDate);
            
            const response = await fetch('/Quote/CreateWarehouseOrder', {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                },
                body: formData
            });

            console.log('Response status:', response.status, response.statusText);
            console.log('Response headers:', Object.fromEntries(response.headers.entries()));

            let result;
            const contentType = response.headers.get('content-type') || '';
            
            try {
                if (contentType.includes('application/json')) {
                    result = await response.json();
                    console.log('Response JSON:', result);
                } else {
                    const text = await response.text();
                    console.error('Server response (not JSON):', text);
                    console.error('Status:', response.status);
                    console.error('StatusText:', response.statusText);
                    
                    // Thử parse như JSON nếu có thể
                    try {
                        result = JSON.parse(text);
                    } catch {
                        // Nếu không parse được, dùng text như message
                        result = { 
                            success: false, 
                            message: text || `Lỗi ${response.status}: ${response.statusText}`,
                            status: response.status
                        };
                    }
                }
            } catch (parseError) {
                console.error('Error parsing response:', parseError);
                result = { 
                    success: false, 
                    message: `Lỗi khi xử lý phản hồi từ server (${response.status})`,
                    status: response.status
                };
            }

            if (response.ok && result && result.success) {
                // Hiển thị bảng báo giá
                if (result.quote) {
                    showQuoteBreakdown(result.quote, result.orderId);
                } else {
                    alert(`✅ ${result.message}\n\n📦 Mã đơn hàng: ${result.orderId}`);
                    
                    // Redirect to success page
                    if (result.orderId) {
                        window.location.href = '/Booking/Success?Id=' + encodeURIComponent(result.orderId);
                    }
                }
            } else {
                // Lỗi từ server
                const errorMessage = result?.message || result?.detail || `Lỗi ${response.status}: ${response.statusText}`;
                console.error('Order submission failed:', {
                    status: response.status,
                    result: result,
                    responseText: result
                });
                
                alert(`❌ ${errorMessage}\n\nVui lòng kiểm tra lại:\n- Địa chỉ nhận hàng đã nhập chưa?\n- Đã chọn kho chưa?\n- Đã nhập ít nhất một món đồ chưa?`);
                
                bookBtn.textContent = originalText;
                bookBtn.disabled = false;
            }
        } catch (error) {
            console.error('Error submitting order:', error);
            console.error('Error stack:', error.stack);
            alert(`❌ Không thể kết nối đến máy chủ!\n\nChi tiết: ${error.message}\n\nVui lòng kiểm tra kết nối và thử lại.`);
            bookBtn.textContent = originalText;
            bookBtn.disabled = false;
        }
    }
}

// Hiển thị bảng báo giá chi tiết
function showQuoteBreakdown(quote, orderId) {
    const formatCurrency = (amount) => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(amount);
    };
    
    const formatNumber = (num) => {
        return new Intl.NumberFormat('vi-VN').format(num);
    };
    
    let html = `
        <div style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.7); z-index: 10000; display: flex; align-items: center; justify-content: center; padding: 20px;">
            <div style="background: white; border-radius: 16px; max-width: 800px; width: 100%; max-height: 90vh; overflow-y: auto; box-shadow: 0 10px 40px rgba(0,0,0,0.3);">
                <div style="padding: 24px; border-bottom: 2px solid #667eea;">
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h2 style="margin: 0; color: #667eea; font-size: 24px;">📄 Báo Giá Chi Tiết</h2>
                        <button onclick="this.closest('[style*=position]').remove()" style="background: #e74c3c; color: white; border: none; border-radius: 8px; padding: 8px 16px; cursor: pointer; font-size: 18px;">✖</button>
                    </div>
                    <p style="margin: 8px 0 0; color: #666;">Mã đơn hàng: <strong>${orderId}</strong></p>
                </div>
                
                <div style="padding: 24px;">
                    <!-- Thông tin kho -->
                    <div style="margin-bottom: 24px; padding: 16px; background: #f8f9fa; border-radius: 8px;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">🏪 Thông tin kho</h3>
                        <p style="margin: 4px 0;"><strong>Tên kho:</strong> ${quote.warehouseName || 'N/A'}</p>
                        <p style="margin: 4px 0;"><strong>Địa chỉ:</strong> ${quote.warehouseAddress || 'N/A'}</p>
                    </div>
                    
                    <!-- Thông tin slot -->
                    <div style="margin-bottom: 24px; padding: 16px; background: #f8f9fa; border-radius: 8px;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">📦 Thông tin ô kho</h3>
                        <p style="margin: 4px 0;"><strong>Mã slot:</strong> ${quote.slotCode || 'N/A'}</p>
                        <p style="margin: 4px 0;"><strong>Kích thước:</strong> ${quote.slotDimensions || `${quote.slotLengthM || 0}m × ${quote.slotWidthM || 0}m × ${quote.slotHeightM || 0}m`}</p>
                        <p style="margin: 4px 0;"><strong>Thể tích:</strong> ${formatNumber(quote.slotVolumeM3 || 0)} m³</p>
                        <p style="margin: 4px 0;"><strong>Diện tích:</strong> ${formatNumber(quote.slotAreaM2 || 0)} m²</p>
                    </div>
                    
                    <!-- Yêu cầu tính toán -->
                    ${quote.requiredVolumeM3 || quote.requiredAreaM2 ? `
                    <div style="margin-bottom: 24px; padding: 16px; background: #fff3cd; border-radius: 8px; border-left: 4px solid #ffc107;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">📊 Yêu cầu tính toán</h3>
                        ${quote.requiredVolumeM3 ? `<p style="margin: 4px 0;"><strong>Thể tích cần:</strong> ${formatNumber(quote.requiredVolumeM3)} m³</p>` : ''}
                        ${quote.requiredAreaM2 ? `<p style="margin: 4px 0;"><strong>Diện tích cần:</strong> ${formatNumber(quote.requiredAreaM2)} m²</p>` : ''}
                        ${quote.analysisDetails ? `<div style="margin-top: 12px; padding: 12px; background: white; border-radius: 6px; font-size: 14px; color: #555;">${quote.analysisDetails.replace(/\n/g, '<br>')}</div>` : ''}
                    </div>
                    ` : ''}
                    
                    <!-- Thông tin thời gian -->
                    <div style="margin-bottom: 24px; padding: 16px; background: #f8f9fa; border-radius: 8px;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">📅 Thời gian lưu trữ</h3>
                        <p style="margin: 4px 0;"><strong>Từ ngày:</strong> ${quote.storageStartDate || 'N/A'}</p>
                        <p style="margin: 4px 0;"><strong>Đến ngày:</strong> ${quote.storageEndDate || 'N/A'}</p>
                        <p style="margin: 4px 0;"><strong>Thời gian:</strong> ${quote.storageDurationDays || quote.storageDurationHours ? `${Math.floor(quote.storageDurationHours / 24)} ngày (${quote.storageDurationHours || 0} giờ)` : 'N/A'}</p>
                    </div>
                    
                    <!-- Bảng giá -->
                    <div style="margin-bottom: 24px; padding: 16px; background: #e8f5e9; border-radius: 8px; border-left: 4px solid #4caf50;">
                        <h3 style="margin: 0 0 16px; color: #333; font-size: 18px;">💵 Chi tiết tính giá</h3>
                        <table style="width: 100%; border-collapse: collapse;">
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">Giá slot theo giờ:</td>
                                <td style="text-align: right; padding: 8px 0; font-weight: 600;">${formatCurrency(quote.pricePerHour || 0)}/giờ</td>
                            </tr>
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">Số giờ:</td>
                                <td style="text-align: right; padding: 8px 0;">${formatNumber(quote.storageDurationHours || 0)} giờ</td>
                            </tr>
                            <tr style="border-bottom: 2px solid #4caf50;">
                                <td style="padding: 8px 0; font-weight: 600;">Phí slot:</td>
                                <td style="text-align: right; padding: 8px 0; font-weight: 600;">${formatCurrency(quote.baseSlotPrice || quote.subtotal || 0)}</td>
                            </tr>
                            ${quote.addonDetails && quote.addonDetails.length > 0 ? `
                            ${quote.addonDetails.map(addon => `
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">
                                    ${addon.name || ''}
                                    ${addon.isDaily ? ` (${formatNumber(addon.quantity || 0)} ngày)` : ' (một lần)'}
                                </td>
                                <td style="text-align: right; padding: 8px 0;">
                                    ${formatCurrency(addon.unitPrice || 0)}${addon.isDaily ? '/ngày' : ''} 
                                    ${addon.isDaily ? `× ${formatNumber(addon.quantity || 0)}` : ''} 
                                    = ${formatCurrency(addon.total || 0)}
                                </td>
                            </tr>
                            `).join('')}
                            <tr style="border-bottom: 2px solid #4caf50;">
                                <td style="padding: 8px 0; font-weight: 600;">Tổng phí dịch vụ:</td>
                                <td style="text-align: right; padding: 8px 0; font-weight: 600;">${formatCurrency(quote.totalAddonPrice || 0)}</td>
                            </tr>
                            ` : ''}
                            <tr style="border-bottom: 2px solid #4caf50; background: white; padding: 12px 0;">
                                <td style="padding: 12px 0; font-weight: 600;">Tạm tính (chưa VAT):</td>
                                <td style="text-align: right; padding: 12px 0; font-weight: 600; font-size: 18px;">${formatCurrency(quote.subtotal || 0)}</td>
                            </tr>
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">VAT (${quote.vatRate || 10}%):</td>
                                <td style="text-align: right; padding: 8px 0;">${formatCurrency(quote.vatAmount || 0)}</td>
                            </tr>
                        </table>
                        <div style="margin-top: 16px; padding: 16px; background: #4caf50; color: white; border-radius: 8px; display: flex; justify-content: space-between; align-items: center;">
                            <span style="font-size: 20px; font-weight: 700;">THÀNH TIỀN:</span>
                            <span style="font-size: 24px; font-weight: 700;">${formatCurrency(quote.totalAmount || 0)}</span>
                        </div>
                    </div>
                    
                    <!-- Nút hành động -->
                    <div style="display: flex; gap: 12px; margin-top: 24px;">
                        <button onclick="window.location.href='/Booking/Success?Id=' + encodeURIComponent('${orderId}')" style="flex: 1; background: #667eea; color: white; border: none; border-radius: 8px; padding: 14px; font-size: 16px; font-weight: 600; cursor: pointer;">✅ Xác nhận đơn hàng</button>
                        <button onclick="this.closest('[style*=position]').remove()" style="flex: 1; background: #95a5a6; color: white; border: none; border-radius: 8px; padding: 14px; font-size: 16px; font-weight: 600; cursor: pointer;">Đóng</button>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    document.body.insertAdjacentHTML('beforeend', html);
}

// ============ ESTIMATION CARD (CHỈ TÍNH DỊCH VỤ THÊM) ============
function initEstimationCard() {
    // Lắng nghe thay đổi ngày và checkbox dịch vụ
    const startDateInput = document.getElementById('storageStartDate');
    const endDateInput = document.getElementById('storageEndDate');
    
    if (startDateInput) {
        startDateInput.addEventListener('change', updateEstimationCard);
    }
    if (endDateInput) {
        endDateInput.addEventListener('change', updateEstimationCard);
    }
    
    // Lắng nghe thay đổi checkbox dịch vụ đặc biệt
    document.querySelectorAll('input[name="SpecialRequirements"]').forEach(checkbox => {
        checkbox.addEventListener('change', updateEstimationCard);
    });
    
    // Tính lần đầu
    updateEstimationCard();
}

function updateEstimationCard() {
    // Giá dịch vụ (theo ngày hoặc một lần)
    const addonPrices = {
        '🧊 Kho mát': 50000, // VND/ngày
        '💧 Chống ẩm': 30000, // VND/ngày
        '🔒 An ninh cao': 40000, // VND/ngày
        '🛡️ Bảo hiểm hàng hóa': 100000, // VND (một lần)
        '🏢 Kho có thang máy': 20000, // VND/ngày
        '📹 Giám sát 24/7': 60000 // VND/ngày
    };
    
    // Dịch vụ tính theo ngày
    const dailyAddons = new Set(['🧊 Kho mát', '💧 Chống ẩm', '🔒 An ninh cao', '🏢 Kho có thang máy', '📹 Giám sát 24/7']);
    
    // Tính số ngày
    const startDate = document.getElementById('storageStartDate')?.value;
    const endDate = document.getElementById('storageEndDate')?.value;
    
    let totalDays = 0;
    if (startDate && endDate) {
        const start = new Date(startDate);
        const end = new Date(endDate);
        if (end > start) {
            totalDays = Math.ceil((end - start) / (1000 * 60 * 60 * 24));
        }
    }
    
    // Tính tổng giá dịch vụ
    let totalAddonPrice = 0;
    const addonBreakdown = [];
    
    document.querySelectorAll('input[name="SpecialRequirements"]:checked').forEach(checkbox => {
        const serviceName = checkbox.value;
        if (addonPrices[serviceName]) {
            const unitPrice = addonPrices[serviceName];
            const isDaily = dailyAddons.has(serviceName);
            const serviceTotal = isDaily ? unitPrice * totalDays : unitPrice;
            
            totalAddonPrice += serviceTotal;
            addonBreakdown.push({
                name: serviceName,
                unitPrice: unitPrice,
                isDaily: isDaily,
                quantity: isDaily ? totalDays : 1,
                total: serviceTotal
            });
        }
    });
    
    // Cập nhật UI
    const formatCurrency = (amount) => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(amount);
    };
    
    // Cập nhật thời gian
    const estDaysEl = document.getElementById('estDays');
    if (estDaysEl) {
        estDaysEl.textContent = totalDays > 0 ? `${totalDays} ngày` : '0 ngày';
    }
    
    // Cập nhật chi tiết dịch vụ đã chọn
    const estAddonDetailsEl = document.getElementById('estAddonDetails');
    if (estAddonDetailsEl) {
        if (addonBreakdown.length > 0) {
            estAddonDetailsEl.innerHTML = addonBreakdown.map(addon => {
                return `
                    <div style="padding: 6px 0; border-bottom: 1px solid #eee; font-size: 0.9rem;">
                        <div style="display: flex; justify-content: space-between;">
                            <span>${addon.name}</span>
                            <span style="font-weight: 600;">${formatCurrency(addon.total)}</span>
                        </div>
                        <div style="font-size: 0.85rem; color: #666; margin-top: 2px;">
                            ${formatCurrency(addon.unitPrice)}${addon.isDaily ? '/ngày' : ''} 
                            ${addon.isDaily ? `× ${addon.quantity} ngày` : '(một lần)'}
                        </div>
                    </div>
                `;
            }).join('');
        } else {
            estAddonDetailsEl.innerHTML = '<div style="color: #999; font-style: italic; text-align: center; padding: 10px;">Chưa chọn dịch vụ nào</div>';
        }
    }
    
    // Cập nhật tổng
    const estSubtotalEl = document.getElementById('estSubtotal');
    const estVATEl = document.getElementById('estVAT');
    const estTotalEl = document.getElementById('estTotal');
    
    if (estSubtotalEl) {
        estSubtotalEl.textContent = formatCurrency(totalAddonPrice);
    }
    
    const vatAmount = totalAddonPrice * 0.1;
    const grandTotal = totalAddonPrice + vatAmount;
    
    if (estVATEl) {
        estVATEl.textContent = formatCurrency(vatAmount);
    }
    
    if (estTotalEl) {
        estTotalEl.textContent = formatCurrency(grandTotal);
    }
    
    console.log('Estimation card updated - Total addons:', totalAddonPrice, 'VND');
}

// Expose functions globally for debugging (sau khi đã định nghĩa)
window.bookingDebug = {
    map: () => map,
    warehouseMarker: () => warehouseMarker,
    nearbyWarehouses: () => nearbyWarehouses,
    selectedWarehouse: () => selectedWarehouse,
    loadNearbyWarehouses: loadNearbyWarehouses,
    displayWarehousesOnMap: displayWarehousesOnMap,
    updateEstimationCard: updateEstimationCard
};
