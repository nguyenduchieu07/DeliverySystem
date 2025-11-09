// Booking Page JavaScript
let map, warehouseMarker;
let warehouseData = null;
let selectedWarehouse = null;
let nearbyWarehouses = [];
let currentQuotationId = null;
let savedPickupAddress = null; // Lưu địa chỉ pickup để dùng khi confirm quotation
let savedItems = []; // Lưu items để dùng khi confirm quotation
function getCsrf() {
    return (
        document.querySelector('input[name="__RequestVerificationToken"]')?.value ||
        ""
    );
}

// Initialize on page load
document.addEventListener("DOMContentLoaded", function () {
    console.log("DOMContentLoaded - Initializing booking page...");

    initMap();
    initDateInputs();
    initAddressAutocomplete();
    initEstimationCard();

    // Đợi map khởi tạo xong rồi mới load warehouses
    // Tự động load kho gần khi trang load
    // Thử lấy vị trí hiện tại, nếu không được thì dùng vị trí mặc định (Hà Nội)
    function loadWarehousesAfterMapReady() {
        if (!map) {
            console.log("Waiting for map to be ready...");
            setTimeout(loadWarehousesAfterMapReady, 100);
            return;
        }

        console.log("Map is ready, loading warehouses...");

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const lat = position.coords.latitude;
                    const lng = position.coords.longitude;
                    console.log("Got current position:", lat, lng);
                    setWarehouseLocation(lat, lng);
                    loadNearbyWarehouses(lat, lng);
                },
                (error) => {
                    console.log("Geolocation error:", error.message);
                    // Nếu không lấy được vị trí, dùng vị trí mặc định (Hà Nội)
                    const defaultLat = 21.028511;
                    const defaultLng = 105.804817;
                    console.log("Using default location:", defaultLat, defaultLng);
                    loadNearbyWarehouses(defaultLat, defaultLng);
                },
                { timeout: 5000, enableHighAccuracy: false }
            );
        } else {
            // Trình duyệt không hỗ trợ geolocation, dùng vị trí mặc định
            const defaultLat = 21.028511;
            const defaultLng = 105.804817;
            console.log(
                "Geolocation not supported, using default location:",
                defaultLat,
                defaultLng
            );
            loadNearbyWarehouses(defaultLat, defaultLng);
        }
    }

    // Đợi một chút để đảm bảo map đã khởi tạo
    setTimeout(loadWarehousesAfterMapReady, 500);
});

// ============ MAP FUNCTIONS ============
function initMap() {
    const mapElement = document.getElementById("map");
    if (!mapElement) {
        console.error("Map element #map not found!");
        return false;
    }

    // Kiểm tra Leaflet đã load chưa
    if (typeof L === "undefined") {
        console.error(
            "Leaflet library (L) not loaded! Make sure Leaflet script is loaded before booking.js"
        );
        // Retry sau 100ms
        setTimeout(initMap, 100);
        return false;
    }

    try {
        console.log("Initializing map...");
        map = L.map("map").setView([21.028511, 105.804817], 13);

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            attribution: "&copy; OpenStreetMap contributors",
            maxZoom: 19,
        }).addTo(map);

        // Đợi map load xong
        map.whenReady(function () {
            console.log("✅ Map initialized and ready");
        });

        // Geocoder control - chỉ thêm nếu có
        if (typeof L.Control !== "undefined" && L.Control.geocoder) {
            L.Control.geocoder({
                defaultMarkGeocode: false,
                placeholder: "Tìm kiếm địa điểm...",
                errorMessage: "Không tìm thấy",
            })
                .on("markgeocode", function (e) {
                    const latlng = e.geocode.center;
                    setWarehouseLocation(latlng.lat, latlng.lng);
                })
                .addTo(map);
        } else {
            console.warn("Geocoder not available");
        }

        return true;
    } catch (error) {
        console.error("Error initializing map:", error);
        return false;
    }
}

// ============ DATE FUNCTIONS ============
function initDateInputs() {
    const today = new Date().toISOString().split("T")[0];
    const startDateInput = document.getElementById("storageStartDate");
    const endDateInput = document.getElementById("storageEndDate");

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
            endDateInput.value = defaultEndDate.toISOString().split("T")[0];
        }

        if (startDateInput) {
            startDateInput.addEventListener("change", function () {
                const startDate = new Date(this.value);
                const minEndDate = new Date(startDate);
                minEndDate.setDate(minEndDate.getDate() + 1);
                if (endDateInput) {
                    endDateInput.min = minEndDate.toISOString().split("T")[0];
                    if (new Date(endDateInput.value) < minEndDate) {
                        endDateInput.value = minEndDate.toISOString().split("T")[0];
                    }
                }
            });
        }
    }
}

// ============ ADDRESS AUTOCOMPLETE ============
function initAddressAutocomplete() {
    const input = document.getElementById("warehouseAreaInput");
    const results = document.getElementById("warehouseAreaResults");

    if (!input || !results) return;

    let timeoutId;
    input.addEventListener("input", function () {
        const query = this.value.trim();
        clearTimeout(timeoutId);

        if (query.length < 2) {
            results.classList.remove("show");
            results.innerHTML = "";
            return;
        }

        timeoutId = setTimeout(async () => {
            try {
                const res = await fetch(
                    `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(
                        query
                    )},Hanoi,Vietnam&limit=8&accept-language=vi`
                );
                const items = await res.json();

                results.innerHTML = "";
                if (!items.length) {
                    results.classList.add("show");
                    results.innerHTML =
                        '<div class="autocomplete-item">Không tìm thấy kết quả</div>';
                    return;
                }

                items.forEach((it) => {
                    const el = document.createElement("div");
                    el.className = "autocomplete-item";
                    el.innerHTML = `
                        <div class="autocomplete-item-main">${it.name || (it.display_name || "").split(",")[0]
                        }</div>
                        <div class="autocomplete-item-sub">${it.display_name || ""
                        }</div>
                    `;
                    el.addEventListener("click", () => {
                        const lat = parseFloat(it.lat);
                        const lng = parseFloat(it.lon);
                        const addressText = it.display_name || "";

                        // Cập nhật địa chỉ vào input và warehouseData
                        input.value = addressText;
                        setWarehouseLocation(lat, lng, addressText);
                        results.classList.remove("show");
                        results.innerHTML = "";
                    });
                    results.appendChild(el);
                });
                results.classList.add("show");
            } catch (e) {
                console.error("Search error:", e);
            }
        }, 300);
    });

    // Close on outside click
    document.addEventListener("click", (e) => {
        if (!results.contains(e.target) && e.target !== input) {
            results.classList.remove("show");
        }
    });
}

// ============ CURRENT LOCATION ============
function getCurrentLocation() {
    if (!navigator.geolocation) {
        alert("Trình duyệt không hỗ trợ lấy vị trí hiện tại");
        return;
    }

    navigator.geolocation.getCurrentPosition(
        (position) => {
            const lat = position.coords.latitude;
            const lng = position.coords.longitude;

            // Lấy địa chỉ từ reverse geocode trước rồi mới set location
            reverseGeocode(lat, lng).then((address) => {
                const input = document.getElementById("warehouseAreaInput");
                if (input) input.value = address;
                setWarehouseLocation(lat, lng, address);
            });
        },
        (error) => {
            alert("Không thể lấy vị trí hiện tại: " + error.message);
        }
    );
}

function searchLocation() {
    const input = document.getElementById("warehouseAreaInput");
    if (input && input.value.trim().length >= 2) {
        input.dispatchEvent(new Event("input"));
    }
}

// ============ WAREHOUSE LOCATION ============
function setWarehouseLocation(lat, lng, addressLine = null) {
    // Lưu địa chỉ nhận hàng (PickupAddress) - đây là địa chỉ từ input hoặc vị trí hiện tại
    warehouseData = { lat: lat, lng: lng, address: addressLine || "" };

    // Cập nhật input với địa chỉ
    const input = document.getElementById("warehouseAreaInput");
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
                    className: "custom-marker",
                    html: '<div style="background:#f26722;width:32px;height:32px;border-radius:50%;border:4px solid white;box-shadow:0 3px 12px rgba(242,103,34,0.6);display:flex;align-items:center;justify-content:center;font-size:16px;">📍</div>',
                    iconSize: [32, 32],
                    iconAnchor: [16, 16],
                }),
            })
                .addTo(map)
                .bindPopup("📍 Địa chỉ nhận hàng (Kéo để di chuyển)");

            warehouseMarker.on("dragend", async function (e) {
                const newPos = e.target.getLatLng();
                warehouseData.lat = newPos.lat;
                warehouseData.lng = newPos.lng;
                const newAddress = await reverseGeocode(newPos.lat, newPos.lng);
                warehouseData.address = newAddress;
                // Cập nhật input với địa chỉ mới
                const input = document.getElementById("warehouseAreaInput");
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
        console.error("Reverse geocode error:", error);
        return `Vị trí: ${lat.toFixed(6)}, ${lng.toFixed(6)}`;
    }
}

// ============ LOAD NEARBY WAREHOUSES ============
async function loadNearbyWarehouses(lat, lng) {
    if (!lat || !lng) {
        console.error("loadNearbyWarehouses: Invalid coordinates", lat, lng);
        return;
    }

    console.log("Loading nearby warehouses for:", lat, lng);

    try {
        const url = `/Quote/NearbyWarehouses?lat=${lat}&lng=${lng}&take=10`;
        console.log("Fetching:", url);

        const response = await fetch(url);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }

        const warehouses = await response.json();
        console.log("Received warehouses:", warehouses);

        if (!warehouses || !Array.isArray(warehouses)) {
            console.error("Invalid warehouses data:", warehouses);
            return;
        }

        nearbyWarehouses = warehouses;
        renderWarehouseList(warehouses);
        displayWarehousesOnMap(warehouses);
    } catch (error) {
        console.error("Error loading warehouses:", error);
        alert("Không thể tải danh sách kho. Vui lòng thử lại sau.");
    }
}

function displayWarehousesOnMap(warehouses) {
    console.log("displayWarehousesOnMap called with:", warehouses);

    if (!map) {
        console.error("Map is not initialized!");
        return;
    }

    // Remove old warehouse markers (keep warehouseMarker if exists)
    const markersToRemove = [];
    map.eachLayer((layer) => {
        if (layer instanceof L.Marker && layer !== warehouseMarker) {
            markersToRemove.push(layer);
        }
    });
    markersToRemove.forEach((marker) => map.removeLayer(marker));

    if (!warehouses || warehouses.length === 0) {
        console.log("No warehouses to display");
        return;
    }

    console.log(`Displaying ${warehouses.length} warehouses on map`);

    // Tạo bounds để fit tất cả kho vào view
    const bounds = [];
    let markersAdded = 0;

    warehouses.forEach((warehouse, index) => {
        // ASP.NET Core mặc định serialize JSON thành camelCase
        // Nên property sẽ là "latitude" và "longitude", không phải "Latitude" và "Longitude"
        let lat =
            warehouse.latitude ||
            warehouse.Latitude ||
            warehouse.lat ||
            warehouse.Lat;
        let lng =
            warehouse.longitude ||
            warehouse.Longitude ||
            warehouse.lng ||
            warehouse.Lng;

        // Kiểm tra và convert
        if (lat == null || lng == null || lat === undefined || lng === undefined) {
            console.warn(`Warehouse ${index} missing coordinates`);
            console.warn("Full object:", warehouse);
            console.warn("Available fields:", Object.keys(warehouse));
            // Log từng field để debug
            Object.keys(warehouse).forEach((key) => {
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

        console.log(
            `Adding marker for warehouse ${index}:`,
            warehouse.name || warehouse.Name,
            "at",
            latNum,
            lngNum
        );

        bounds.push([latNum, lngNum]);

        // ASP.NET Core serialize thành camelCase
        const warehouseName = warehouse.name || warehouse.Name || "Kho";
        const warehouseId = warehouse.id || warehouse.Id;
        const distanceKm = warehouse.distanceKm || 0;
        const address =
            warehouse.full || warehouse.addressLine || warehouse.AddressLine || "";
        const storeName = warehouse.storeName || warehouse.StoreName || "";

        try {
            // Tạo marker với icon đẹp hơn
            const marker = L.marker([latNum, lngNum], {
                icon: L.divIcon({
                    className: "warehouse-marker",
                    html: `<div style="background:linear-gradient(135deg, #667eea 0%, #764ba2 100%);color:white;padding:8px 12px;border-radius:20px;font-size:13px;font-weight:700;box-shadow:0 4px 12px rgba(102,126,234,0.5);white-space:nowrap;border:2px solid white;">
                            🏪 ${warehouseName}
                          </div>`,
                    iconSize: [150, 40],
                    iconAnchor: [75, 20],
                }),
            }).addTo(map);

            markersAdded++;

            // Bind popup với thông tin chi tiết
            const popupContent = `
                <div style="min-width:200px;">
                    <h4 style="margin:0 0 8px 0;color:#667eea;">${warehouseName}</h4>
                    <p style="margin:4px 0;font-size:13px;color:#666;">${storeName ? "📦 " + storeName : ""
                }</p>
                    <p style="margin:4px 0;font-size:12px;color:#888;">${address}</p>
                    <p style="margin:8px 0 0 0;font-size:12px;">
                        <strong style="color:#27ae60;">📍 ${distanceKm.toFixed(
                    2
                )} km</strong>
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

            marker.on("click", function () {
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
            console.log("Fitting bounds for", bounds.length, "locations");
            map.fitBounds(bounds, {
                padding: [50, 50],
                maxZoom: 15, // Giới hạn zoom tối đa
            });
        } catch (e) {
            console.error("Error fitting bounds:", e);
        }
    } else {
        console.warn("No bounds to fit");
    }
}

// Helper function để chọn kho từ map popup
function selectWarehouseFromMap(warehouseId) {
    const warehouse = nearbyWarehouses.find((w) => {
        const wId = w.id || w.Id;
        return wId && wId.toString() === warehouseId.toString();
    });
    if (warehouse) {
        selectWarehouse(warehouse);
    }
}

function renderWarehouseList(warehouses) {
    const listContainer = document.getElementById("warehouseList");
    if (!listContainer) return;

    listContainer.innerHTML = "";

    if (!warehouses || warehouses.length === 0) {
        listContainer.innerHTML =
            '<div style="padding: 15px; text-align: center; color: #666;">Không tìm thấy kho nào gần đây.</div>';
        return;
    }

    warehouses.forEach((warehouse) => {
        // ASP.NET Core serialize thành camelCase
        const warehouseId = warehouse.id || warehouse.Id;
        const item = document.createElement("div");
        item.className = "warehouse-item";
        item.dataset.warehouseId = warehouseId;

        // Check if this is the selected warehouse
        if (selectedWarehouse) {
            const selectedId = selectedWarehouse.id || selectedWarehouse.Id;
            if (selectedId === warehouseId) {
                item.classList.add("selected");
            }
        }

        item.innerHTML = `
            <div class="warehouse-name">${warehouse.name || warehouse.Name || "Kho"
            }</div>
            <div class="warehouse-address">${warehouse.full ||
            warehouse.addressLine ||
            warehouse.AddressLine ||
            ""
            }</div>
            <div class="warehouse-info">
                <div>📦 ${warehouse.storeName || warehouse.StoreName || ""
            }</div>
                <div class="warehouse-distance">${warehouse.distanceKm || 0
            } km</div>
            </div>
        `;

        item.addEventListener("click", function () {
            selectWarehouse(warehouse);
        });

        listContainer.appendChild(item);
    });
}

function selectWarehouse(warehouse) {
    console.log("Selecting warehouse:", warehouse);
    selectedWarehouse = warehouse;
    const warehouseIdInput = document.getElementById("warehouseIdInput");
    const warehouseNameDisplay = document.getElementById("warehouseNameDisplay");

    // ASP.NET Core serialize thành camelCase
    const warehouseId = warehouse.id || warehouse.Id;
    if (warehouseIdInput && warehouseId) {
        warehouseIdInput.value = warehouseId.toString();
    }
    if (warehouseNameDisplay) {
        warehouseNameDisplay.value = warehouse.name || warehouse.Name || "";
    }

    // Update UI - highlight selected warehouse
    document.querySelectorAll(".warehouse-item").forEach((item) => {
        item.classList.remove("selected");
        const itemId =
            item.dataset.warehouseId || item.getAttribute("data-warehouse-id");
        if (itemId && warehouseId && itemId.toString() === warehouseId.toString()) {
            item.classList.add("selected");
        }
    });

    // Highlight warehouse marker on map
    if (map) {
        map.eachLayer((layer) => {
            if (layer instanceof L.Marker && layer !== warehouseMarker) {
                const layerId = layer.options?.warehouseId;
                const warehouseObj = layer.options?.warehouse;
                const whName =
                    warehouse.Name ||
                    warehouse.name ||
                    warehouseObj?.Name ||
                    warehouseObj?.name ||
                    "Kho";

                if (layerId === warehouseId?.toString()) {
                    // Highlight selected warehouse
                    layer.setIcon(
                        L.divIcon({
                            className: "warehouse-marker",
                            html: `<div style="background:linear-gradient(135deg, #27ae60 0%, #2ecc71 100%);color:white;padding:8px 12px;border-radius:20px;font-size:13px;font-weight:700;box-shadow:0 4px 12px rgba(39,174,96,0.6);white-space:nowrap;border:3px solid #ffd700;">
                                🏪 ${whName}
                              </div>`,
                            iconSize: [150, 40],
                            iconAnchor: [75, 20],
                        })
                    );
                    // Mở popup và center vào marker đã chọn
                    layer.openPopup();
                    map.setView(layer.getLatLng(), Math.max(map.getZoom(), 14));
                } else {
                    // Reset other markers to normal state
                    if (warehouseObj) {
                        layer.setIcon(
                            L.divIcon({
                                className: "warehouse-marker",
                                html: `<div style="background:linear-gradient(135deg, #667eea 0%, #764ba2 100%);color:white;padding:8px 12px;border-radius:20px;font-size:13px;font-weight:700;box-shadow:0 4px 12px rgba(102,126,234,0.5);white-space:nowrap;border:2px solid white;">
                                    🏪 ${whName}
                                  </div>`,
                                iconSize: [150, 40],
                                iconAnchor: [75, 20],
                            })
                        );
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
        console.error("Error loading slots:", error);
        const grid = document.getElementById("warehouseGrid");
        if (grid) {
            grid.innerHTML =
                '<div style="padding: 20px; text-align: center; color: #e74c3c;">❌ Không thể tải sơ đồ kho. Vui lòng thử lại sau.</div>';
        }
    }
}

function renderWarehouseGrid(slots) {
    const grid = document.getElementById("warehouseGrid");
    if (!grid) return;

    grid.innerHTML = "";

    if (!slots || slots.length === 0) {
        grid.innerHTML =
            '<div style="padding: 20px; text-align: center; color: #666;">Kho này chưa có slot nào.</div>';
        return;
    }

    // Find max row and col to create grid
    let maxRow = 0,
        maxCol = 0;
    slots.forEach((slot) => {
        if (slot.row > maxRow) maxRow = slot.row;
        if (slot.col > maxCol) maxCol = slot.col;
    });

    // Create grid CSS
    if (maxRow > 0 && maxCol > 0) {
        grid.style.gridTemplateColumns = `repeat(${maxCol}, minmax(60px, 1fr))`;
    }

    // Create a map for quick lookup
    const slotMap = new Map();
    slots.forEach((slot) => {
        const key = `${slot.row}-${slot.col}`;
        slotMap.set(key, slot);
    });

    // Render slots in grid order
    for (let r = 1; r <= maxRow; r++) {
        for (let c = 1; c <= maxCol; c++) {
            const key = `${r}-${c}`;
            const slot = slotMap.get(key);

            const slotEl = document.createElement("div");
            if (slot) {
                let statusClass = "available";
                if (slot.status === "blocked") statusClass = "blocked";
                else if (slot.status === "occupied") statusClass = "occupied";
                else if (slot.status === "reserved") statusClass = "reserved";

                slotEl.className = `warehouse-slot ${statusClass}`;
                slotEl.dataset.slotId = slot.id;
                slotEl.textContent = slot.code || `${r}-${c}`;
                slotEl.title = `Slot: ${slot.code || "N/A"}\nSize: ${slot.size || "N/A"
                    }\nPrice: ${slot.basePricePerHour || "N/A"} đ/h`;

                // Chỉ cho phép click nếu slot available (không blocked, occupied, hoặc reserved)
                if (statusClass === "available") {
                    slotEl.addEventListener("click", function () {
                        toggleSlotSelection(slotEl, slot);
                    });
                } else if (statusClass === "reserved") {
                    // Slot reserved hiển thị tooltip thông tin
                    slotEl.title = `Slot: ${slot.code || "N/A"}\nSize: ${slot.size || "N/A"
                    }\nPrice: ${slot.basePricePerHour || "N/A"} đ/h\n⚠️ Đang được giữ chỗ`;
                    slotEl.style.cursor = "not-allowed";
                }
            } else {
                // Empty cell
                slotEl.className = "warehouse-slot blocked";
                slotEl.style.opacity = "0.2";
            }

            grid.appendChild(slotEl);
        }
    }
}

let selectedSlots = []; // Array to store selected slot IDs

function toggleSlotSelection(element, slot) {
    if (
        element.classList.contains("occupied") ||
        element.classList.contains("blocked") ||
        element.classList.contains("reserved")
    ) {
        return;
    }

    const slotId = slot.id || element.dataset.slotId;
    const isSelected = element.classList.contains("selected");

    if (isSelected) {
        // Deselect
        element.classList.remove("selected");
        selectedSlots = selectedSlots.filter((id) => id !== slotId);
    } else {
        // Select
        element.classList.add("selected");
        if (slotId && !selectedSlots.includes(slotId)) {
            selectedSlots.push(slotId);
        }
    }

    updateSelectedSlotsSummary();
}

function updateSelectedSlotsSummary() {
    const selected = document.querySelectorAll(".warehouse-slot.selected");
    const summary = document.getElementById("selectedSlotsSummary");
    const slotsList = document.getElementById("selectedSlotsList");

    if (summary && slotsList) {
        if (selected.length > 0) {
            summary.style.display = "block";
            const slotCodes = Array.from(selected)
                .map((el) => el.textContent.trim())
                .filter((t) => t)
                .join(", ");
            slotsList.textContent = `${selected.length} slot(s): ${slotCodes}`;
        } else {
            summary.style.display = "none";
            slotsList.textContent = "Chưa chọn slot nào";
        }
    }
}

function showWarehouseSlots() {
    // Lấy warehouseId từ selectedWarehouse hoặc từ input hidden
    let warehouseId = null;
    let warehouseName = "";

    if (selectedWarehouse) {
        warehouseId = selectedWarehouse.Id || selectedWarehouse.id;
        warehouseName = selectedWarehouse.Name || selectedWarehouse.name || "";
    }

    // Nếu không có, lấy từ input hidden
    if (!warehouseId) {
        const warehouseIdInput = document.getElementById("warehouseIdInput");
        const warehouseNameDisplay = document.getElementById(
            "warehouseNameDisplay"
        );
        if (warehouseIdInput && warehouseIdInput.value) {
            warehouseId = warehouseIdInput.value;
        }
        if (warehouseNameDisplay && warehouseNameDisplay.value) {
            warehouseName = warehouseNameDisplay.value;
        }
    }

    if (!warehouseId) {
        alert("⚠️ Vui lòng chọn kho từ danh sách trước!");
        return;
    }

    const section = document.getElementById("warehouseSlotSection");
    if (section) {
        // Hiển thị thông tin kho đã chọn
        const warehouseInfo = document.getElementById("selectedWarehouseInfo");
        const warehouseNameSpan = document.getElementById("selectedWarehouseName");
        if (warehouseInfo) warehouseInfo.style.display = "block";
        if (warehouseNameSpan)
            warehouseNameSpan.textContent = warehouseName || "Kho đã chọn";

        section.classList.add("show");

        // Hiển thị loading
        const grid = document.getElementById("warehouseGrid");
        if (grid) {
            grid.innerHTML =
                '<div style="padding: 20px; text-align: center; color: #667eea;">🔄 Đang tải sơ đồ kho...</div>';
        }

        // Load slots với đúng warehouseId
        loadWarehouseSlots(warehouseId);

        // Scroll to section
        setTimeout(() => {
            section.scrollIntoView({ behavior: "smooth", block: "start" });
        }, 100);
    }
}

function toggleSlotSection() {
    const section = document.getElementById("warehouseSlotSection");
    if (section) {
        section.classList.toggle("show");
    }
}

// ============ ITEMS MANAGEMENT ============
function addItemRow(name = "", category = "", quantity = 1) {
    const tbody = document.getElementById("itemsTableBody");
    if (!tbody) return;

    // Xóa dòng "Chưa có món nào" nếu có
    const emptyRow = tbody.querySelector("tr td[colspan]");
    if (emptyRow) {
        emptyRow.closest("tr").remove();
    }

    // Tìm index tiếp theo (bỏ qua empty row)
    const existingRows = tbody.querySelectorAll("tr:not(:has(td[colspan]))");
    const idx = existingRows.length;

    const row = document.createElement("tr");
    row.style.cssText = "border-bottom: 1px solid #e0e0e0;";
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
        <td style="padding: 10px; text-align: center;">
            <button type="button" class="location-btn" onclick="removeItemRow(this)" style="padding: 6px 10px; background: #e74c3c;">✖</button>
        </td>
    `;
    tbody.appendChild(row);
}

function removeItemRow(button) {
    const row = button.closest("tr");
    if (row) {
        row.remove();

        // Nếu không còn row nào, thêm dòng "Chưa có món nào"
        const tbody = document.getElementById("itemsTableBody");
        if (tbody && tbody.children.length === 0) {
            const emptyRow = document.createElement("tr");
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
    const tbody = document.getElementById("itemsTableBody");
    if (!tbody) return;

    const rows = tbody.querySelectorAll("tr");
    rows.forEach((row, index) => {
        if (row.querySelector("td[colspan]")) return; // Skip empty row

        const inputs = row.querySelectorAll("input");
        inputs.forEach((input) => {
            const name = input.name;
            if (name) {
                const newName = name.replace(/Items\[\d+\]/, `Items[${index}]`);
                input.name = newName;
            }
        });
    });
}

// ============ IMAGE PREVIEW ============
async function previewTotalImage(input) {
    const files = input.files;
    const previewContainer = document.getElementById("productImagePreviewContainer");
    const oldPreview = document.getElementById("productImagePreview");

    // Xóa preview cũ nếu có
    if (previewContainer) {
        previewContainer.innerHTML = "";
        previewContainer.style.display = files.length > 0 ? "flex" : "none";
    }
    if (oldPreview) {
        oldPreview.style.display = "none";
    }

    if (files && files.length > 0) {
        console.log(`📷 ${files.length} image(s) selected:`);
        
        // Hiển thị preview cho tất cả các ảnh
        Array.from(files).forEach((file, index) => {
            console.log(`  - Image ${index + 1}:`, file.name, `(${(file.size / 1024).toFixed(2)} KB)`);
            
            if (previewContainer) {
                const previewDiv = document.createElement("div");
                previewDiv.style.cssText = "position: relative; display: inline-block;";
                
                const previewImg = document.createElement("img");
                previewImg.className = "image-preview";
                previewImg.style.cssText = "width: 100px; height: 100px; object-fit: cover; border-radius: 8px; border: 2px solid #ddd; display: block;";
                
                const reader = new FileReader();
                reader.onload = function (e) {
                    previewImg.src = e.target.result;
                    console.log(`  ✅ Preview ${index + 1} loaded successfully`);
                };
                reader.readAsDataURL(file);
                
                previewDiv.appendChild(previewImg);
                previewContainer.appendChild(previewDiv);
            }
        });

        // Gọi AI để phân tích tất cả ảnh và tự động điền vào bảng
        await analyzeImageAndFillItems(Array.from(files));
    } else {
        console.log("📷 No images selected (file input cleared)");
    }
}

// Hàm gọi AI để phân tích ảnh và tự động điền vào bảng items
async function analyzeImageAndFillItems(imageFiles) {
    const tbody = document.getElementById("itemsTableBody");
    if (!tbody) {
        console.error("Items table body not found");
        return;
    }

    // Chuyển đổi FileList hoặc Array thành Array
    const filesArray = Array.isArray(imageFiles) ? imageFiles : Array.from(imageFiles || []);

    if (filesArray.length === 0) {
        console.warn("No image files provided");
        return;
    }

    // Hiển thị loading indicator
    const loadingMsg = document.createElement("div");
    loadingMsg.id = "aiLoadingMsg";
    loadingMsg.style.cssText = "padding: 15px; background: #e8f0fe; border-radius: 8px; margin: 10px 0; text-align: center; color: #667eea;";
    loadingMsg.innerHTML = `🤖 AI đang phân tích ${filesArray.length} ảnh... Vui lòng đợi...`;
    tbody.parentElement.insertBefore(loadingMsg, tbody);

    try {
        const formData = new FormData();
        
        // Nếu chỉ có 1 ảnh, dùng productImage (backward compatible)
        // Nếu có nhiều ảnh, dùng productImages
        if (filesArray.length === 1) {
            formData.append("productImage", filesArray[0]);
        } else {
            filesArray.forEach((file, index) => {
                formData.append("productImages", file);
            });
        }

        const response = await fetch("/Quote/AnalyzeProductImage", {
            method: "POST",
            body: formData,
            headers: {
                "RequestVerificationToken": getCsrf()
            }
        });

        const result = await response.json();

        // Xóa loading indicator
        const loadingElement = document.getElementById("aiLoadingMsg");
        if (loadingElement) {
            loadingElement.remove();
        }

        if (!result.success) {
            alert("⚠️ " + (result.message || "Không thể phân tích ảnh. Vui lòng thử lại."));
            return;
        }

        if (!result.items || result.items.length === 0) {
            alert("ℹ️ " + (result.message || "Không phát hiện được sản phẩm trong ảnh."));
            return;
        }

        // Không xóa toàn bộ tbody, chỉ thêm items mới vào (để user có thể thêm thủ công)
        // Nếu muốn xóa và thay thế, uncomment dòng dưới:
        // tbody.innerHTML = "";

        // Điền dữ liệu từ AI vào bảng (thêm vào cuối danh sách hiện có)
        const existingRows = tbody.querySelectorAll("tr:not(:has(td[colspan]))");
        let startIndex = existingRows.length;
        
        result.items.forEach((item, index) => {
            addItemRowFromAI(item, startIndex + index);
        });

        // Cập nhật lại index của tất cả các rows để đảm bảo đúng format
        updateItemIndexes();

        // Hiển thị thông báo thành công
        const successMsg = document.createElement("div");
        successMsg.style.cssText = "padding: 10px; background: #d4edda; border-radius: 8px; margin: 10px 0; color: #155724;";
        successMsg.innerHTML = `✅ ${result.message || `Đã phát hiện ${result.items.length} loại sản phẩm và tự động thêm vào danh sách.`}`;
        tbody.parentElement.insertBefore(successMsg, tbody);

        // Tự động xóa thông báo sau 5 giây
        setTimeout(() => {
            if (successMsg.parentElement) {
                successMsg.remove();
            }
        }, 5000);

        console.log("✅ AI analysis completed:", result.items);
    } catch (error) {
        console.error("Error analyzing image(s):", error);
        
        // Xóa loading indicator
        const loadingElement = document.getElementById("aiLoadingMsg");
        if (loadingElement) {
            loadingElement.remove();
        }

        alert("⚠️ Có lỗi xảy ra khi phân tích ảnh. Vui lòng thử lại sau.");
    }
}

// Hàm thêm một dòng item từ kết quả AI
function addItemRowFromAI(item, index) {
    const tbody = document.getElementById("itemsTableBody");
    if (!tbody) return;

    const tr = document.createElement("tr");
    tr.style.borderBottom = "1px solid #e0e0e0";
    tr.innerHTML = `
        <td style="padding: 10px;">
            <input type="text" class="form-control" name="Items[${index}].Name" value="${escapeHtml(item.name || "")}" placeholder="Ví dụ: Bàn học sinh" required>
        </td>
        <td style="padding: 10px;">
            <input type="text" class="form-control" name="Items[${index}].Category" value="${escapeHtml(item.category || "")}" placeholder="Ví dụ: Nội thất">
        </td>
        <td style="padding: 10px;">
            <input type="number" class="form-control" name="Items[${index}].Quantity" value="${item.quantity || 1}" min="1" placeholder="1" style="text-align: center;" required>
        </td>
        <td style="padding: 10px; text-align: center;">
            <button type="button" class="location-btn" onclick="removeItemRow(this)" style="padding: 6px 10px; background: #e74c3c;">✖</button>
        </td>
    `;
    tbody.appendChild(tr);
}

// Hàm escape HTML để tránh XSS
function escapeHtml(text) {
    if (!text) return "";
    const div = document.createElement("div");
    div.textContent = text;
    return div.innerHTML;
}

// ============ SUBMIT ORDER ============
async function submitWarehouseOrder() {
    // Validation - Lấy địa chỉ từ input tìm kiếm hoặc warehouseData
    const warehouseAreaInput = document.getElementById("warehouseAreaInput");
    const pickupAddressText =
        warehouseAreaInput?.value?.trim() || warehouseData?.address || "";

    if (!pickupAddressText) {
        alert("⚠️ Vui lòng nhập địa chỉ nhận hàng hoặc chọn vị trí hiện tại!");
        warehouseAreaInput?.focus();
        return;
    }

    // Validation - Kiểm tra có tọa độ không
    if (!warehouseData || !warehouseData.lat || !warehouseData.lng) {
        alert(
            '⚠️ Vui lòng chọn khu vực muốn tìm kho bằng cách nhập địa chỉ hoặc nhấn "Vị trí hiện tại"!'
        );
        warehouseAreaInput?.focus();
        return;
    }

    if (!selectedWarehouse) {
        alert("⚠️ Vui lòng chọn kho từ danh sách!");
        return;
    }

    const startDate = document.getElementById("storageStartDate").value;
    const endDate = document.getElementById("storageEndDate").value;

    if (!startDate || !endDate) {
        alert("⚠️ Vui lòng chọn ngày nhập và xuất kho!");
        return;
    }

    if (new Date(endDate) <= new Date(startDate)) {
        alert("⚠️ Ngày xuất kho phải sau ngày nhập kho!");
        return;
    }

    // Collect items from table rows
    const items = [];
    const rows = document.querySelectorAll("#itemsTableBody > tr");
    rows.forEach((row, idx) => {
        // Skip empty row
        if (row.querySelector("td[colspan]")) return;

        const nameInput = row.querySelector('input[name*=".Name"]');
        const categoryInput = row.querySelector('input[name*=".Category"]');
        const quantityInput = row.querySelector('input[name*=".Quantity"]');

        const name = nameInput?.value?.trim();
        const category = categoryInput?.value?.trim();
        const quantity = parseInt(quantityInput?.value) || 0;

        if (name && quantity > 0) {
            items.push({ name, category, quantity });
        }
    });

    if (items.length === 0) {
        alert("⚠️ Vui lòng nhập ít nhất một món đồ!");
        return;
    }

    // Collect special requirements
    const specialRequirements = [];
    document
        .querySelectorAll('input[name="SpecialRequirements"]:checked')
        .forEach((cb) => {
            specialRequirements.push(cb.value);
        });

    // Build form data
    const formData = new FormData();

    // PickupAddress - Lấy từ input tìm kiếm hoặc warehouseData
    // Đây là địa chỉ nhận hàng (nơi khách hàng muốn gửi hàng đi)
    const pickupAddressLine = pickupAddressText;
    const pickupLat = warehouseData.lat;
    const pickupLng = warehouseData.lng;

    formData.append("PickupAddress.AddressLine", pickupAddressLine);
    formData.append("PickupAddress.Latitude", pickupLat);
    formData.append("PickupAddress.Longitude", pickupLng);

    // WarehouseArea - Địa chỉ kho đã chọn (nơi lưu trữ)
    const warehouseAreaLine =
        selectedWarehouse.full ||
        selectedWarehouse.addressLine ||
        selectedWarehouse.AddressLine ||
        selectedWarehouse.name ||
        "Kho đã chọn";
    const warehouseLat =
        selectedWarehouse.latitude ||
        selectedWarehouse.Latitude ||
        selectedWarehouse.lat ||
        selectedWarehouse.Lat;
    const warehouseLng =
        selectedWarehouse.longitude ||
        selectedWarehouse.Longitude ||
        selectedWarehouse.lng ||
        selectedWarehouse.Lng;

    // Gửi WarehouseId (ưu tiên) để tìm warehouse chính xác
    const warehouseId = selectedWarehouse.id || selectedWarehouse.Id;
    if (warehouseId) {
        formData.append("WarehouseId", warehouseId.toString());
    }

    formData.append("WarehouseArea.AddressLine", warehouseAreaLine);
    formData.append("WarehouseArea.Latitude", warehouseLat);
    formData.append("WarehouseArea.Longitude", warehouseLng);
    formData.append("StorageStartDate", startDate);
    formData.append("StorageEndDate", endDate);
    formData.append("Note", document.getElementById("orderNote")?.value || "");

    items.forEach((item, idx) => {
        formData.append(`Items[${idx}].Name`, item.name);
        formData.append(`Items[${idx}].Category`, item.category || "");
        formData.append(`Items[${idx}].Quantity`, item.quantity);
    });

    specialRequirements.forEach((req, idx) => {
        formData.append(`SpecialRequirements[${idx}]`, req);
    });

    // Add product image if available
    const productImageInput = document.getElementById("productImageInput");
    let hasImage = false;
    if (
        productImageInput &&
        productImageInput.files &&
        productImageInput.files[0]
    ) {
        formData.append("productImage", productImageInput.files[0]);
        hasImage = true;
        console.log(
            "📷 Product image will be uploaded:",
            productImageInput.files[0].name,
            `(${(productImageInput.files[0].size / 1024).toFixed(2)} KB)`
        );
    } else {
        console.log(
            "⚠️ No product image uploaded - Gemini analysis will be skipped"
        );
    }

    // Submit
    const bookBtn = document.getElementById("bookBtn");
    if (bookBtn) {
        const originalText = bookBtn.textContent;
        bookBtn.textContent = "⏳ Đang gửi yêu cầu...";
        bookBtn.disabled = true;

        try {
            console.log("=== Submitting order with data ===");
            console.log("PickupAddress:", pickupAddressLine, pickupLat, pickupLng);
            console.log(
                "WarehouseArea:",
                warehouseAreaLine,
                warehouseLat,
                warehouseLng
            );
            console.log("WarehouseId:", warehouseId);
            console.log("SelectedWarehouse:", selectedWarehouse);
            console.log("Items:", items);
            console.log("Dates:", startDate, endDate);
            console.log("Has product image:", hasImage);

            const response = await fetch("/Quote/CreateWarehouseOrder", {
                method: "POST",
                headers: {
                    RequestVerificationToken:
                        document.querySelector('input[name="__RequestVerificationToken"]')
                            ?.value || "",
                },
                body: formData,
            });

            console.log("Response status:", response.status, response.statusText);
            console.log(
                "Response headers:",
                Object.fromEntries(response.headers.entries())
            );

            let result;
            const contentType = response.headers.get("content-type") || "";

            try {
                if (contentType.includes("application/json")) {
                    result = await response.json();
                    console.log("Response JSON:", result);
                } else {
                    const text = await response.text();
                    console.error("Server response (not JSON):", text);
                    console.error("Status:", response.status);
                    console.error("StatusText:", response.statusText);

                    // Thử parse như JSON nếu có thể
                    try {
                        result = JSON.parse(text);
                    } catch {
                        // Nếu không parse được, dùng text như message
                        result = {
                            success: false,
                            message: text || `Lỗi ${response.status}: ${response.statusText}`,
                            status: response.status,
                        };
                    }
                }
            } catch (parseError) {
                console.error("Error parsing response:", parseError);
                result = {
                    success: false,
                    message: `Lỗi khi xử lý phản hồi từ server (${response.status})`,
                    status: response.status,
                };
            }

            if (response.ok && result && result.success) {
                // Lưu pickup address và items để dùng khi confirm quotation
                savedPickupAddress = {
                    addressLine: pickupAddressLine,
                    latitude: pickupLat,
                    longitude: pickupLng
                };
                savedItems = items; // Lưu items đã gửi
                currentQuotationId = result.quotationId; // Lưu quotation ID

                // Log kết quả Gemini analysis từ response
                if (result.quote) {
                    console.log("=== Quote Response ===");
                    console.log("OrderId:", result.orderId);
                    console.log(
                        "Has product image (server received):",
                        result.quote.hasProductImage ?? false
                    );
                    console.log(
                        "Gemini analysis available:",
                        result.quote.geminiAnalysisAvailable ?? false
                    );
                    console.log(
                        "Has Gemini analysis data:",
                        !!(
                            result.quote.analysisDetails ||
                            result.quote.requiredVolumeM3 ||
                            result.quote.requiredAreaM2
                        )
                    );

                    if (result.quote.hasProductImage === false) {
                        console.warn(
                            "⚠️ No product image was uploaded or received by server"
                        );
                    } else if (result.quote.geminiAnalysisAvailable === false) {
                        console.warn(
                            "⚠️ Product image was uploaded but Gemini analysis failed or returned no result"
                        );
                        if (result.quote.geminiError) {
                            console.error("❌ Gemini Error:", result.quote.geminiError);
                        } else {
                            console.warn(
                                "   Check server logs for Gemini API errors (no error message received)"
                            );
                        }
                    } else if (result.quote.geminiAnalysisAvailable === true) {
                        console.log("✅ Gemini analysis successful!");
                        if (result.quote.requiredVolumeM3) {
                            console.log(
                                "📊 Required Volume (from Gemini):",
                                result.quote.requiredVolumeM3,
                                "m³"
                            );
                        }
                        if (result.quote.requiredAreaM2) {
                            console.log(
                                "📊 Required Area (from Gemini):",
                                result.quote.requiredAreaM2,
                                "m²"
                            );
                        }
                        if (result.quote.analysisDetails) {
                            console.log(
                                "📝 Analysis Details (first 200 chars):",
                                result.quote.analysisDetails.substring(0, 200)
                            );
                        }
                        if (
                            result.quote.itemEstimates &&
                            result.quote.itemEstimates.length > 0
                        ) {
                            console.log(
                                "📦 Items found in image:",
                                result.quote.itemEstimates.length
                            );
                            result.quote.itemEstimates.forEach((item, idx) => {
                                console.log(
                                    `  ${idx + 1}. ${item.name}: ${item.quantity} cái, ${item.estimatedVolumeM3
                                    } m³`
                                );
                            });
                        } else {
                            console.warn("⚠️ Gemini analysis returned no items");
                        }
                    }
                }

                // Hiển thị bảng báo giá
                if (result.quote) {
                    // Reset button về trạng thái ban đầu trước khi hiển thị popup
                    bookBtn.textContent = originalText;
                    bookBtn.disabled = false;
                    showQuoteBreakdown(result.quote, result.quotationId);
                } else {
                    // Reset button về trạng thái ban đầu
                    bookBtn.textContent = originalText;
                    bookBtn.disabled = false;

                    alert(`✅ ${result.message}\n\n📦 Mã đơn hàng: ${result.orderId}`);

                    // Redirect to success page
                    if (result.orderId) {
                        window.location.href =
                            "/Booking/Success?Id=" + encodeURIComponent(result.orderId);
                    }
                }
            } else {
                // Lỗi từ server
                const errorMessage =
                    result?.message ||
                    result?.detail ||
                    `Lỗi ${response.status}: ${response.statusText}`;
                console.error("Order submission failed:", {
                    status: response.status,
                    result: result,
                    responseText: result,
                });

                alert(
                    `❌ ${errorMessage}\n\nVui lòng kiểm tra lại:\n- Địa chỉ nhận hàng đã nhập chưa?\n- Đã chọn kho chưa?\n- Đã nhập ít nhất một món đồ chưa?`
                );

                bookBtn.textContent = originalText;
                bookBtn.disabled = false;
            }
        } catch (error) {
            console.error("Error submitting order:", error);
            console.error("Error stack:", error.stack);
            alert(
                `❌ Không thể kết nối đến máy chủ!\n\nChi tiết: ${error.message}\n\nVui lòng kiểm tra kết nối và thử lại.`
            );
            bookBtn.textContent = originalText;
            bookBtn.disabled = false;
        }
    }
}

// Hiển thị bảng báo giá chi tiết
function showQuoteBreakdown(quote, quotationId) {
    const formatCurrency = (amount) => {
        return new Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
        }).format(amount);
    };

    const formatNumber = (num) => {
        return new Intl.NumberFormat("vi-VN").format(num);
    };

    let html = `
        <div style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.7); z-index: 10000; display: flex; align-items: center; justify-content: center; padding: 20px;">
            <div style="background: white; border-radius: 16px; max-width: 800px; width: 100%; max-height: 90vh; overflow-y: auto; box-shadow: 0 10px 40px rgba(0,0,0,0.3);">
                <div style="padding: 24px; border-bottom: 2px solid #667eea;">
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h2 style="margin: 0; color: #667eea; font-size: 24px;">📄 Báo Giá Chi Tiết</h2>
                        <button onclick="closeQuotePopup()" style="background: #e74c3c; color: white; border: none; border-radius: 8px; padding: 8px 16px; cursor: pointer; font-size: 18px;">✖</button>
                    </div>
                    <p style="margin: 8px 0 0; color: #666;">Mã báo giá: <strong>${quotationId}</strong></p>
                    <p style="margin: 4px 0 0; color: #ff9800; font-size: 14px;">⏰ Slot đã được giữ chỗ trong 24 giờ</p>
                </div>
                
                <div style="padding: 24px;">
                    <!-- Thông tin kho -->
                    <div style="margin-bottom: 24px; padding: 16px; background: #f8f9fa; border-radius: 8px;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">🏪 Thông tin kho</h3>
                        <p style="margin: 4px 0;"><strong>Tên kho:</strong> ${quote.warehouseName || "N/A"
        }</p>
                        <p style="margin: 4px 0;"><strong>Địa chỉ:</strong> ${quote.warehouseAddress || "N/A"
        }</p>
                    </div>
                    
                    <!-- Thông tin slot -->
                    <div style="margin-bottom: 24px; padding: 16px; background: #fff3cd; border-radius: 8px; border-left: 4px solid #ffc107;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">📦 Thông tin ô kho (Chưa gán)</h3>
                        <p style="margin: 4px 0; padding: 8px; background: #fff; border-radius: 4px; color: #856404; font-weight: 600;">⚠️ Ô kho chưa được gán vào đơn hàng. Vui lòng ấn "Xác nhận gán vào ô kho" để hoàn tất.</p>
                        <p style="margin: 8px 0 4px 0;"><strong>Mã slot:</strong> ${quote.slotCode || "N/A"
        }</p>
                        <p style="margin: 4px 0;"><strong>Kích thước:</strong> ${quote.slotDimensions ||
        `${quote.slotLengthM || 0}m × ${quote.slotWidthM || 0
        }m × ${quote.slotHeightM || 0}m`
        }</p>
                        <p style="margin: 4px 0;"><strong>Thể tích:</strong> ${formatNumber(
            quote.slotVolumeM3 || 0
        )} m³</p>
                        <p style="margin: 4px 0;"><strong>Diện tích:</strong> ${formatNumber(
            quote.slotAreaM2 || 0
        )} m²</p>
                    </div>
                    
                    <!-- Yêu cầu tính toán - chỉ hiển thị khi có kết quả thực từ Gemini (có analysisDetails) -->
                    ${quote.analysisDetails &&
            (quote.requiredVolumeM3 > 0 || quote.requiredAreaM2 > 0)
            ? `
                    <div style="margin-bottom: 24px; padding: 16px; background: #fff3cd; border-radius: 8px; border-left: 4px solid #ffc107;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">📊 Yêu cầu tính toán (từ phân tích ảnh)</h3>
                        ${quote.requiredVolumeM3 && quote.requiredVolumeM3 > 0
                ? `<p style="margin: 4px 0;"><strong>Thể tích cần:</strong> ${formatNumber(
                    quote.requiredVolumeM3
                )} m³</p>`
                : ""
            }
                        ${quote.requiredAreaM2 && quote.requiredAreaM2 > 0
                ? `<p style="margin: 4px 0;"><strong>Diện tích cần:</strong> ${formatNumber(
                    quote.requiredAreaM2
                )} m²</p>`
                : ""
            }
                        ${quote.analysisDetails
                ? `<div style="margin-top: 12px; padding: 12px; background: white; border-radius: 6px; font-size: 14px; color: #555;">${quote.analysisDetails.replace(
                    /\n/g,
                    "<br>"
                )}</div>`
                : ""
            }
                    </div>
                    `
            : ""
        }
                    
                    <!-- Thông tin thời gian -->
                    <div style="margin-bottom: 24px; padding: 16px; background: #f8f9fa; border-radius: 8px;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">📅 Thời gian lưu trữ</h3>
                        <p style="margin: 4px 0;"><strong>Từ ngày:</strong> ${quote.storageStartDate || "N/A"
        }</p>
                        <p style="margin: 4px 0;"><strong>Đến ngày:</strong> ${quote.storageEndDate || "N/A"
        }</p>
                        <p style="margin: 4px 0;"><strong>Thời gian:</strong> ${quote.storageDurationDays ||
            quote.storageDurationHours
            ? `${Math.floor(
                quote.storageDurationHours / 24
            )} ngày (${quote.storageDurationHours || 0} giờ)`
            : "N/A"
        }</p>
                    </div>
                    
                    <!-- Bảng giá -->
                    <div style="margin-bottom: 24px; padding: 16px; background: #e8f5e9; border-radius: 8px; border-left: 4px solid #4caf50;">
                        <h3 style="margin: 0 0 16px; color: #333; font-size: 18px;">💵 Chi tiết tính giá</h3>
                        <table style="width: 100%; border-collapse: collapse;">
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">Giá slot theo giờ:</td>
                                <td style="text-align: right; padding: 8px 0; font-weight: 600;">${formatCurrency(
            quote.pricePerHour || 0
        )}/giờ</td>
                            </tr>
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">Số giờ:</td>
                                <td style="text-align: right; padding: 8px 0;">${formatNumber(
            quote.storageDurationHours || 0
        )} giờ</td>
                            </tr>
                            <tr style="border-bottom: 2px solid #4caf50;">
                                <td style="padding: 8px 0; font-weight: 600;">Phí slot:</td>
                                <td style="text-align: right; padding: 8px 0; font-weight: 600;">${formatCurrency(
            quote.baseSlotPrice || quote.subtotal || 0
        )}</td>
                            </tr>
                            ${quote.addonDetails &&
            quote.addonDetails.length > 0
            ? `
                            ${quote.addonDetails
                .map(
                    (addon) => `
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">
                                    ${addon.name || ""}
                                    ${addon.isDaily
                            ? ` (${formatNumber(
                                addon.quantity || 0
                            )} ngày)`
                            : " (một lần)"
                        }
                                </td>
                                <td style="text-align: right; padding: 8px 0;">
                                    ${formatCurrency(addon.unitPrice || 0)}${addon.isDaily ? "/ngày" : ""
                        } 
                                    ${addon.isDaily
                            ? `× ${formatNumber(
                                addon.quantity || 0
                            )}`
                            : ""
                        } 
                                    = ${formatCurrency(addon.total || 0)}
                                </td>
                            </tr>
                            `
                )
                .join("")}
                            <tr style="border-bottom: 2px solid #4caf50;">
                                <td style="padding: 8px 0; font-weight: 600;">Tổng phí dịch vụ:</td>
                                <td style="text-align: right; padding: 8px 0; font-weight: 600;">${formatCurrency(
                    quote.totalAddonPrice || 0
                )}</td>
                            </tr>
                            `
            : ""
        }
                            <tr style="border-bottom: 2px solid #4caf50; background: white; padding: 12px 0;">
                                <td style="padding: 12px 0; font-weight: 600;">Tạm tính (chưa VAT):</td>
                                <td style="text-align: right; padding: 12px 0; font-weight: 600; font-size: 18px;">${formatCurrency(
            quote.subtotal || 0
        )}</td>
                            </tr>
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">VAT (${quote.vatRate || 10
        }%):</td>
                                <td style="text-align: right; padding: 8px 0;">${formatCurrency(
            quote.vatAmount || 0
        )}</td>
                            </tr>
                        </table>
                        <div style="margin-top: 16px; padding: 16px; background: #4caf50; color: white; border-radius: 8px; display: flex; justify-content: space-between; align-items: center;">
                            <span style="font-size: 20px; font-weight: 700;">THÀNH TIỀN:</span>
                            <span style="font-size: 24px; font-weight: 700;">${formatCurrency(
            quote.totalAmount || 0
        )}</span>
                        </div>
                    </div>
                    
                    <!-- Nút hành động -->
                    <div style="display: flex; flex-direction: column; gap: 12px; margin-top: 24px;">
                        <div style="display: flex; gap: 12px;">
                            <button onclick="confirmQuotation('${quotationId}', '${quote.slotId || ""}', this)" style="flex: 1; background: #667eea; color: white; border: none; border-radius: 8px; padding: 14px; font-size: 16px; font-weight: 600; cursor: pointer;">✅ Xác nhận đơn hàng</button>
                            <button onclick="requestPriceRevision('${quotationId}', '${quote.slotId || ""}', this)" style="flex: 1; background: #ff9800; color: white; border: none; border-radius: 8px; padding: 14px; font-size: 16px; font-weight: 600; cursor: pointer;">💰 Yêu cầu chỉnh giá</button>
                        </div>
                        <button onclick="closeQuotePopup()" style="background: #95a5a6; color: white; border: none; border-radius: 8px; padding: 14px; font-size: 16px; font-weight: 600; cursor: pointer;">Hủy / Đóng</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.body.insertAdjacentHTML("beforeend", html);
}

// Hàm để xác nhận quotation và tạo Order
async function confirmQuotation(quotationId, slotId, buttonElement) {
    if (!quotationId || !slotId) {
        alert("⚠️ Không có thông tin báo giá hoặc ô kho. Vui lòng thử lại.");
        return;
    }

    // Disable button để tránh click nhiều lần
    const originalText = buttonElement.textContent;
    buttonElement.disabled = true;
    buttonElement.textContent = "⏳ Đang xác nhận...";
    buttonElement.style.opacity = "0.6";
    buttonElement.style.cursor = "not-allowed";

    // Lấy pickup address và items từ biến đã lưu hoặc từ DOM
    let pickupAddress = savedPickupAddress;
    let items = savedItems;

    // Nếu không có trong biến đã lưu, lấy từ DOM
    if (!pickupAddress) {
        const warehouseAreaInput = document.getElementById("warehouseAreaInput");
        const pickupAddressText = warehouseAreaInput?.value?.trim() || warehouseData?.address || "";
        if (pickupAddressText && warehouseData) {
            pickupAddress = {
                addressLine: pickupAddressText,
                latitude: warehouseData.lat,
                longitude: warehouseData.lng
            };
        }
    }

    // Nếu không có items trong biến đã lưu, lấy từ bảng items
    if (!items || items.length === 0) {
        items = [];
        const rows = document.querySelectorAll("#itemsTableBody > tr");
        rows.forEach((row) => {
            if (row.querySelector("td[colspan]")) return; // Skip empty row

            const nameInput = row.querySelector('input[name*=".Name"]');
            const categoryInput = row.querySelector('input[name*=".Category"]');
            const quantityInput = row.querySelector('input[name*=".Quantity"]');

            const name = nameInput?.value?.trim();
            const category = categoryInput?.value?.trim();
            const quantity = parseInt(quantityInput?.value) || 0;

            if (name && quantity > 0) {
                items.push({ name, category, quantity });
            }
        });
    }

    try {
        const requestBody = {
            quotationId: quotationId,
            slotId: slotId,
        };

        // Thêm pickup address nếu có
        if (pickupAddress) {
            requestBody.pickupAddress = {
                addressLine: pickupAddress.addressLine,
                latitude: pickupAddress.latitude,
                longitude: pickupAddress.longitude
            };
        }

        // Thêm items nếu có
        if (items && items.length > 0) {
            requestBody.items = items.map(item => ({
                name: item.name,
                category: item.category || "",
                quantity: item.quantity
            }));
        }

        console.log("Confirming quotation with data:", requestBody);

        const response = await fetch("/Quote/ConfirmQuotation", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                RequestVerificationToken:
                    document.querySelector('input[name="__RequestVerificationToken"]')
                        ?.value || "",
            },
            body: JSON.stringify(requestBody),
        });

        const result = await response.json();

        if (response.ok && result.success) {
            // Xác nhận thành công - hiển thị thông báo và redirect
            alert(
                `✅ ${result.message || "Đã xác nhận báo giá thành công!"}\n\n📦 Ô kho: ${result.slotCode || "N/A"
                }\n📋 Mã đơn hàng: ${result.orderId}`
            );

            // Đóng popup
            const popup = buttonElement.closest('[style*="position: fixed"]');
            if (popup) {
                popup.remove();
            }

            // Redirect đến trang hợp đồng
            if (result.orderId) {
                window.location.href = "/Contract/GetContract?orderId=" + encodeURIComponent(result.orderId);
            }
        } else {
            // Lỗi khi xác nhận
            const errorMessage =
                result.message || "Có lỗi xảy ra khi xác nhận báo giá. Vui lòng thử lại.";
            alert(`❌ ${errorMessage}`);

            // Reset button
            buttonElement.disabled = false;
            buttonElement.textContent = originalText;
            buttonElement.style.opacity = "1";
            buttonElement.style.cursor = "pointer";
        }
    } catch (error) {
        console.error("Error confirming quotation:", error);
        alert(
            `❌ Không thể kết nối đến máy chủ!\n\nChi tiết: ${error.message}\n\nVui lòng kiểm tra kết nối và thử lại.`
        );

        // Reset button
        buttonElement.disabled = false;
        buttonElement.textContent = originalText;
        buttonElement.style.opacity = "1";
        buttonElement.style.cursor = "pointer";
    }
}

// Hàm để yêu cầu chỉnh giá
async function requestPriceRevision(quotationId, slotId, buttonElement) {
    if (!quotationId) {
        alert("⚠️ Không có thông tin báo giá. Vui lòng thử lại.");
        return;
    }

    // Yêu cầu nhập note
    const note = prompt(
        "💰 Yêu cầu chỉnh giá\n\nVui lòng nhập lý do và yêu cầu chỉnh giá của bạn:"
    );

    if (!note || note.trim() === "") {
        return; // Người dùng hủy hoặc không nhập gì
    }

    // Disable button để tránh click nhiều lần
    const originalText = buttonElement.textContent;
    buttonElement.disabled = true;
    buttonElement.textContent = "⏳ Đang gửi yêu cầu...";
    buttonElement.style.opacity = "0.6";
    buttonElement.style.cursor = "not-allowed";

    try {
        // Lấy thông tin từ quote để gửi kèm
        const startDate = document.getElementById("storageStartDate")?.value;
        const endDate = document.getElementById("storageEndDate")?.value;

        const response = await fetch("/Quote/RequestRevision", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                RequestVerificationToken:
                    document.querySelector('input[name="__RequestVerificationToken"]')
                        ?.value || "",
            },
            body: JSON.stringify({
                quotationId: quotationId,
                slotIds: slotId ? [slotId] : [],
                from: startDate ? new Date(startDate) : new Date(),
                to: endDate ? new Date(endDate) : new Date(),
                note: note.trim(),
            }),
        });

        if (response.ok) {
            alert(
                "✅ Yêu cầu chỉnh giá đã được gửi thành công!\n\nCửa hàng sẽ xem xét và phản hồi trong thời gian sớm nhất."
            );

            // Đóng popup
            const popup = buttonElement.closest('[style*="position: fixed"]');
            if (popup) {
                popup.remove();
            }
        } else {
            const result = await response.json().catch(() => ({}));
            const errorMessage =
                result.message || "Có lỗi xảy ra khi gửi yêu cầu chỉnh giá. Vui lòng thử lại.";
            alert(`❌ ${errorMessage}`);

            // Reset button
            buttonElement.disabled = false;
            buttonElement.textContent = originalText;
            buttonElement.style.opacity = "1";
            buttonElement.style.cursor = "pointer";
        }
    } catch (error) {
        console.error("Error requesting price revision:", error);
        alert(
            `❌ Không thể kết nối đến máy chủ!\n\nChi tiết: ${error.message}\n\nVui lòng kiểm tra kết nối và thử lại.`
        );

        // Reset button
        buttonElement.disabled = false;
        buttonElement.textContent = originalText;
        buttonElement.style.opacity = "1";
        buttonElement.style.cursor = "pointer";
    }
}

// Hàm để xác nhận gán slot vào order (deprecated - giữ lại để tương thích)
async function confirmAssignSlotToOrder(orderId, slotId, buttonElement) {
    if (!orderId || !slotId) {
        alert("⚠️ Không có thông tin đơn hàng hoặc ô kho. Vui lòng thử lại.");
        return;
    }

    // Disable button để tránh click nhiều lần
    const originalText = buttonElement.textContent;
    buttonElement.disabled = true;
    buttonElement.textContent = "⏳ Đang gán ô kho...";
    buttonElement.style.opacity = "0.6";
    buttonElement.style.cursor = "not-allowed";

    try {
        const response = await fetch("/Quote/AssignSlotToOrder", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                RequestVerificationToken:
                    document.querySelector('input[name="__RequestVerificationToken"]')
                        ?.value || "",
            },
            body: JSON.stringify({
                orderId: orderId,
                slotId: slotId,
            }),
        });

        const result = await response.json();

        if (response.ok && result.success) {
            // Gán slot thành công - hiển thị thông báo và redirect
            alert(
                `✅ ${result.message || "Đã gán ô kho thành công!"}\n\n📦 Ô kho: ${result.slotCode || "N/A"
                }\n📋 Mã đơn hàng: ${orderId}`
            );

            // Đóng popup
            const popup = buttonElement.closest('[style*="position: fixed"]');
            if (popup) {
                popup.remove();
            }

            // Redirect đến success page
            window.location.href =
                "/Booking/Success?Id=" + encodeURIComponent(orderId);
        } else {
            // Lỗi khi gán slot
            const errorMessage =
                result.message || "Có lỗi xảy ra khi gán ô kho. Vui lòng thử lại.";
            alert(`❌ ${errorMessage}`);

            // Reset button
            buttonElement.disabled = false;
            buttonElement.textContent = originalText;
            buttonElement.style.opacity = "1";
            buttonElement.style.cursor = "pointer";
        }
    } catch (error) {
        console.error("Error assigning slot to order:", error);
        alert(
            `❌ Không thể kết nối đến máy chủ!\n\nChi tiết: ${error.message}\n\nVui lòng kiểm tra kết nối và thử lại.`
        );

        // Reset button
        buttonElement.disabled = false;
        buttonElement.textContent = originalText;
        buttonElement.style.opacity = "1";
        buttonElement.style.cursor = "pointer";
    }
}

// Hàm để đóng popup (không gán slot)
function closeQuotePopup() {
    // Tìm popup báo giá (có chứa "Báo Giá Chi Tiết")
    const popups = document.querySelectorAll(
        '[style*="position: fixed"][style*="z-index: 10000"]'
    );
    let quotePopup = null;

    for (const popup of popups) {
        if (popup.textContent.includes("Báo Giá Chi Tiết")) {
            quotePopup = popup;
            break;
        }
    }

    if (quotePopup) {
        // Xác nhận với người dùng nếu họ muốn hủy
        if (
            confirm("Bạn có chắc muốn hủy? Ô kho sẽ không được gán vào đơn hàng này.")
        ) {
            quotePopup.remove();
        }
    }
}

// ============ ESTIMATION CARD (CHỈ TÍNH DỊCH VỤ THÊM) ============
function initEstimationCard() {
    // Lắng nghe thay đổi ngày và checkbox dịch vụ
    const startDateInput = document.getElementById("storageStartDate");
    const endDateInput = document.getElementById("storageEndDate");

    if (startDateInput) {
        startDateInput.addEventListener("change", updateEstimationCard);
    }
    if (endDateInput) {
        endDateInput.addEventListener("change", updateEstimationCard);
    }

    // Lắng nghe thay đổi checkbox dịch vụ đặc biệt
    document
        .querySelectorAll('input[name="SpecialRequirements"]')
        .forEach((checkbox) => {
            checkbox.addEventListener("change", updateEstimationCard);
        });

    // Tính lần đầu
    updateEstimationCard();
}

function updateEstimationCard() {
    // Giá dịch vụ (theo ngày hoặc một lần)
    const addonPrices = {
        "🧊 Kho mát": 50000, // VND/ngày
        "💧 Chống ẩm": 30000, // VND/ngày
        "🔒 An ninh cao": 40000, // VND/ngày
        "🛡️ Bảo hiểm hàng hóa": 100000, // VND (một lần)
        "🏢 Kho có thang máy": 20000, // VND/ngày
        "📹 Giám sát 24/7": 60000, // VND/ngày
    };

    // Dịch vụ tính theo ngày
    const dailyAddons = new Set([
        "🧊 Kho mát",
        "💧 Chống ẩm",
        "🔒 An ninh cao",
        "🏢 Kho có thang máy",
        "📹 Giám sát 24/7",
    ]);

    // Tính số ngày
    const startDate = document.getElementById("storageStartDate")?.value;
    const endDate = document.getElementById("storageEndDate")?.value;

    let totalDays = 0;
    if (startDate && endDate) {
        const start = new Date(startDate);
        const end = new Date(endDate);
        if (end > start) {
            totalDays = Math.ceil((end - start) / (1000 * 60 * 60 * 24));
        } else {
            totalDays = 0;
        }
    }

    // Tính tổng giá dịch vụ
    let totalAddonPrice = 0;
    const addonBreakdown = [];

    document
        .querySelectorAll('input[name="SpecialRequirements"]:checked')
        .forEach((checkbox) => {
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
                    total: serviceTotal,
                });
            }
        });

    // Cập nhật UI
    const formatCurrency = (amount) => {
        return new Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
        }).format(amount);
    };

    // Cập nhật thời gian
    const estDaysEl = document.getElementById("estDays");
    if (estDaysEl) {
        estDaysEl.textContent = totalDays > 0 ? `${totalDays} ngày` : "0 ngày";
    }

    // Cập nhật chi tiết dịch vụ đã chọn
    const estAddonDetailsEl = document.getElementById("estAddonDetails");
    if (estAddonDetailsEl) {
        if (addonBreakdown.length > 0) {
            estAddonDetailsEl.innerHTML = addonBreakdown
                .map((addon) => {
                    return `
                    <div style="padding: 6px 0; border-bottom: 1px solid #eee; font-size: 0.9rem;">
                        <div style="display: flex; justify-content: space-between;">
                            <span>${addon.name}</span>
                            <span style="font-weight: 600;">${formatCurrency(
                        addon.total
                    )}</span>
                        </div>
                        <div style="font-size: 0.85rem; color: #666; margin-top: 2px;">
                            ${formatCurrency(addon.unitPrice)}${addon.isDaily ? "/ngày" : ""
                        } 
                            ${addon.isDaily
                            ? `× ${addon.quantity} ngày`
                            : "(một lần)"
                        }
                        </div>
                    </div>
                `;
                })
                .join("");
        } else {
            estAddonDetailsEl.innerHTML =
                '<div style="color: #999; font-style: italic; text-align: center; padding: 10px;">Chưa chọn dịch vụ nào</div>';
        }
    }

    // Cập nhật tổng
    const estSubtotalEl = document.getElementById("estSubtotal");
    const estVATEl = document.getElementById("estVAT");
    const estTotalEl = document.getElementById("estTotal");

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

    console.log(
        "Estimation card updated - Total addons:",
        totalAddonPrice,
        "VND"
    );
}

// Expose functions globally for debugging (sau khi đã định nghĩa)
window.bookingDebug = {
    map: () => map,
    warehouseMarker: () => warehouseMarker,
    nearbyWarehouses: () => nearbyWarehouses,
    selectedWarehouse: () => selectedWarehouse,
    loadNearbyWarehouses: loadNearbyWarehouses,
    displayWarehousesOnMap: displayWarehousesOnMap,
    updateEstimationCard: updateEstimationCard,
};

async function acceptQuotationFromBreakdown(orderId, qidFromQuote) {
    const token =
        document.querySelector('input[name="__RequestVerificationToken"]')?.value ||
        "";
    const quotationId =
        qidFromQuote ||
        currentQuotationId ||
        document.getElementById("quotationIdInput")?.value;
    if (!quotationId) {
        alert("Không tìm thấy mã báo giá để xác nhận.");
        return;
    }

    try {
        const res = await fetch("/Quote/Accept", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                RequestVerificationToken: token,
            },
            body: JSON.stringify({
                quotationId: quotationId,
                slotIds: selectedSlots || [],
                from: document.getElementById("storageStartDate")?.value,
                to: document.getElementById("storageEndDate")?.value,
                selectedWarehouseId: selectedWarehouse?.id || selectedWarehouse?.Id,
            }),
        });

        const data = await res.json().catch(() => null);
        if (!res.ok || !data?.success) {
            alert(data?.message || "❌ Không chấp nhận được báo giá.");
            return;
        }

        if (data.redirectUrl) {
            window.location.href = data.redirectUrl; // ví dụ: /Payment?orderId=...
        } else if (data.orderId) {
            window.location.href =
                "/Payment?orderId=" + encodeURIComponent(data.orderId);
        } else {
            alert("✅ Đã chấp nhận báo giá.");
        }
    } catch (e) {
        console.error(e);
        alert("Có lỗi xảy ra. Vui lòng thử lại.");
    }
}
