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

// Flag để tránh load warehouses nhiều lần
let warehousesLoading = false;
let warehousesLoaded = false;

// Initialize on page load
document.addEventListener("DOMContentLoaded", function () {
    console.log("DOMContentLoaded - Initializing booking page...");

    initMap();
    initDateInputs();
    initAddressAutocomplete();
    initEstimationCard();
    initEmailValidation();

    // Đợi map khởi tạo xong rồi mới load warehouses
    // Tự động load kho gần khi trang load
    // Thử lấy vị trí hiện tại, nếu không được thì dùng vị trí mặc định (Hà Nội)
    function loadWarehousesAfterMapReady() {
        if (!map) {
            console.log("⏳ Waiting for map to be ready...");
            setTimeout(loadWarehousesAfterMapReady, 200);
            return;
        }

        // Tránh load nhiều lần
        if (warehousesLoading || warehousesLoaded) {
            console.log("⏭️ Warehouses already loading or loaded, skipping...");
            return;
        }

        console.log("✅ Map object exists, waiting for tiles to load...");

        // Đợi map hoàn toàn sẵn sàng (tiles đã load)
        // Leaflet không có property 'loaded', nên luôn dùng whenReady
        map.whenReady(function() {
            console.log("✅ Map is fully ready (tiles loaded), loading warehouses...");
            // Đợi thêm một chút để đảm bảo tiles đã render
            setTimeout(function() {
                loadWarehousesWithLocation();
            }, 300);
        });
    }

    function loadWarehousesWithLocation() {
        // Tránh load nhiều lần (chỉ check, không set flag ở đây)
        if (warehousesLoading) {
            console.log("⏭️ Warehouses already loading, skipping...");
            return;
        }

        console.log("🔄 Starting to load warehouses...");

        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const lat = position.coords.latitude;
                    const lng = position.coords.longitude;
                    console.log("📍 Got current position:", lat, lng);
                    setWarehouseLocation(lat, lng, null, false); // false = không gọi loadNearbyWarehouses ở đây
                    // Reset flags để đảm bảo load được
                    warehousesLoaded = false;
                    warehousesLoading = false;
                    loadNearbyWarehouses(lat, lng);
                },
                (error) => {
                    console.log("⚠️ Geolocation error:", error.message);
                    // Nếu không lấy được vị trí, dùng vị trí mặc định (Hà Nội)
                    const defaultLat = 21.028511;
                    const defaultLng = 105.804817;
                    console.log("📍 Using default location:", defaultLat, defaultLng);
                    setWarehouseLocation(defaultLat, defaultLng, null, false); // false = không gọi loadNearbyWarehouses ở đây
                    // Reset flags để đảm bảo load được
                    warehousesLoaded = false;
                    warehousesLoading = false;
                    loadNearbyWarehouses(defaultLat, defaultLng);
                },
                { timeout: 5000, enableHighAccuracy: false }
            );
        } else {
            // Trình duyệt không hỗ trợ geolocation, dùng vị trí mặc định
            const defaultLat = 21.028511;
            const defaultLng = 105.804817;
            console.log(
                "📍 Geolocation not supported, using default location:",
                defaultLat,
                defaultLng
            );
            setWarehouseLocation(defaultLat, defaultLng, null, false); // false = không gọi loadNearbyWarehouses ở đây
            // Reset flags để đảm bảo load được
            warehousesLoaded = false;
            warehousesLoading = false;
            loadNearbyWarehouses(defaultLat, defaultLng);
        }
    }

    // Đợi một chút để đảm bảo map đã khởi tạo
    setTimeout(loadWarehousesAfterMapReady, 800);
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
        console.log("🗺️ Initializing map...");
        map = L.map("map").setView([21.028511, 105.804817], 13);

        L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
            attribution: "&copy; OpenStreetMap contributors",
            maxZoom: 19,
        }).addTo(map);

        // Đợi map sẵn sàng rồi mới thêm labels cho quần đảo Hoàng Sa và Trường Sa
        map.whenReady(function() {
            console.log("✅ Map is ready, adding island labels...");
            // Đợi thêm một chút để đảm bảo tiles đã load
            setTimeout(function() {
                addVietnameseIslandLabels(map);
            }, 500);
        });
        
        // Đảm bảo labels được thêm lại nếu map được khởi tạo lại
        map.on('load', function() {
            setTimeout(function() {
                if (map && typeof addVietnameseIslandLabels === 'function') {
                    addVietnameseIslandLabels(map);
                }
            }, 300);
        });

        // Geocoder control - chỉ thêm nếu có
        if (typeof L.Control !== "undefined" && L.Control.geocoder) {
            L.Control.geocoder({
                defaultMarkGeocode: false,
                placeholder: "Tìm kiếm địa điểm...",
                errorMessage: "Không tìm thấy",
                geocoder: L.Control.Geocoder.nominatim({
                    geocodingQueryParams: {
                        addressdetails: 1,
                        'accept-language': 'vi', // Ngôn ngữ tiếng Việt
                        countrycodes: 'vn' // Chỉ tìm trong Việt Nam
                    },
                    reverseQueryParams: {
                        addressdetails: 1,
                        'accept-language': 'vi' // Ngôn ngữ tiếng Việt
                    }
                })
            })
                .on("markgeocode", function (e) {
                    const latlng = e.geocode.center;
                    // Reset flags để đảm bảo load được
                    warehousesLoaded = false;
                    warehousesLoading = false;
                    setWarehouseLocation(latlng.lat, latlng.lng, null, true);
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

// Thêm labels tiếng Việt cho quần đảo Hoàng Sa và Trường Sa (che phủ chữ Trung Quốc)
function addVietnameseIslandLabels(mapInstance) {
    if (!mapInstance || typeof L === "undefined") {
        console.warn("Cannot add island labels: mapInstance or L is undefined");
        return;
    }
    
    console.log("🏝️ Adding Vietnamese island labels to map...");
    
    // Thêm CSS để đảm bảo labels hiển thị trên cùng và che phủ tốt
    if (!document.getElementById('vietnamese-island-styles')) {
        const style = document.createElement('style');
        style.id = 'vietnamese-island-styles';
        style.textContent = `
            .vietnamese-island-label {
                z-index: 10000 !important;
            }
            .vietnamese-island-label div {
                pointer-events: none !important;
                background: rgba(255,255,255,0.98) !important;
                cursor: default !important;
            }
            .leaflet-marker-icon.vietnamese-island-label {
                z-index: 10000 !important;
            }
            .hoang-sa-label, .truong-sa-label {
                z-index: 10000 !important;
            }
        `;
        document.head.appendChild(style);
    }
    
    // Quần đảo Hoàng Sa (Paracel Islands) - khoảng 16.5°N, 112.0°E
    const hoangSaOverlay = L.marker([16.5, 112.0], {
        icon: L.divIcon({
            className: 'vietnamese-island-label hoang-sa-label',
            html: '<div style="background: rgba(255,255,255,0.98); padding: 10px 16px; border-radius: 8px; border: 3px solid #d32f2f; font-weight: bold; color: #d32f2f; font-size: 15px; white-space: nowrap; box-shadow: 0 4px 8px rgba(0,0,0,0.4); text-align: center; min-width: 300px; z-index: 10000;">🏝️ Quần đảo Hoàng Sa, Việt Nam</div>',
            iconSize: [220, 50],
            iconAnchor: [110, 25]
        }),
        zIndexOffset: 10000,
        interactive: false,
        keyboard: false
    });
    
    // Quần đảo Trường Sa (Spratly Islands) - khoảng 10.0°N, 114.0°E
    const truongSaOverlay = L.marker([10.0, 114.0], {
        icon: L.divIcon({
            className: 'vietnamese-island-label truong-sa-label',
            html: '<div style="background: rgba(255,255,255,0.98); padding: 10px 16px; border-radius: 8px; border: 3px solid #d32f2f; font-weight: bold; color: #d32f2f; font-size: 15px; white-space: nowrap; box-shadow: 0 4px 8px rgba(0,0,0,0.4); text-align: center; min-width: 300px; z-index: 10000;">🏝️ Quần đảo Trường Sa, Việt Nam</div>',
            iconSize: [220, 50],
            iconAnchor: [110, 25]
        }),
        zIndexOffset: 10000,
        interactive: false,
        keyboard: false
    });
    
    // Thêm vào map
    try {
        hoangSaOverlay.addTo(mapInstance);
        truongSaOverlay.addTo(mapInstance);
        console.log("✅ Island labels added to map successfully");
        console.log("   - Hoàng Sa marker:", hoangSaOverlay.getLatLng());
        console.log("   - Trường Sa marker:", truongSaOverlay.getLatLng());
    } catch (e) {
        console.error("❌ Error adding island labels to map:", e);
        return;
    }
    
    // Đảm bảo labels luôn hiển thị trên cùng khi map zoom/pan/move
    const ensureLabelsOnTop = function() {
        try {
            // Kiểm tra xem markers có trong map không, nếu không thì thêm lại
            if (mapInstance.hasLayer) {
                if (!mapInstance.hasLayer(hoangSaOverlay)) {
                    console.warn("⚠️ Hoàng Sa marker not in map, re-adding...");
                    hoangSaOverlay.addTo(mapInstance);
                }
                if (!mapInstance.hasLayer(truongSaOverlay)) {
                    console.warn("⚠️ Trường Sa marker not in map, re-adding...");
                    truongSaOverlay.addTo(mapInstance);
                }
            }
            
            // Đảm bảo labels luôn ở trên cùng
            if (hoangSaOverlay && typeof hoangSaOverlay.bringToFront === 'function') {
                hoangSaOverlay.bringToFront();
            }
            if (truongSaOverlay && typeof truongSaOverlay.bringToFront === 'function') {
                truongSaOverlay.bringToFront();
            }
            
            // Debug: Kiểm tra xem markers có visible không
            const bounds = mapInstance.getBounds();
            if (bounds) {
                const hoangSaInBounds = bounds.contains([16.5, 112.0]);
                const truongSaInBounds = bounds.contains([10.0, 114.0]);
                console.log("🏝️ Island labels status:", {
                    hoangSaInView: hoangSaInBounds,
                    truongSaInView: truongSaInBounds,
                    zoom: mapInstance.getZoom(),
                    center: mapInstance.getCenter()
                });
            }
        } catch (e) {
            console.warn("Error ensuring island labels on top:", e);
        }
    };
    
    // Đảm bảo labels luôn hiển thị khi map thay đổi
    mapInstance.on('zoomend moveend viewreset load', ensureLabelsOnTop);
    
    // Đảm bảo labels hiển thị ngay cả khi zoom quá gần
    mapInstance.on('zoom', function() {
        ensureLabelsOnTop();
    });
    
    // Đảm bảo labels hiển thị ngay sau khi map load
    mapInstance.whenReady(function() {
        setTimeout(function() {
            ensureLabelsOnTop();
            // Kiểm tra lại sau 500ms và 1 giây
            setTimeout(ensureLabelsOnTop, 500);
            setTimeout(ensureLabelsOnTop, 1000);
        }, 200);
    });
    
    // Lưu references để có thể truy cập sau và debug
    if (!window.__islandLabels) {
        window.__islandLabels = {};
    }
    window.__islandLabels.hoangSa = hoangSaOverlay;
    window.__islandLabels.truongSa = truongSaOverlay;
    
    // Expose function để có thể gọi lại từ console nếu cần
    window.__ensureIslandLabelsVisible = ensureLabelsOnTop;
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

                        // Reset flags để đảm bảo load được
                        warehousesLoaded = false;
                        warehousesLoading = false;

                        // Cập nhật địa chỉ vào input và warehouseData
                        input.value = addressText;
                        setWarehouseLocation(lat, lng, addressText, true);
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

            // Reset flags để đảm bảo load được
            warehousesLoaded = false;
            warehousesLoading = false;

            // Lấy địa chỉ từ reverse geocode trước rồi mới set location
            reverseGeocode(lat, lng).then((address) => {
                const input = document.getElementById("warehouseAreaInput");
                if (input) input.value = address;
                // Gọi với shouldLoadWarehouses = true để load warehouses ngay
                setWarehouseLocation(lat, lng, address, true);
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
function setWarehouseLocation(lat, lng, addressLine = null, shouldLoadWarehouses = true) {
    console.log("📍 setWarehouseLocation called:", lat, lng, addressLine, "shouldLoadWarehouses:", shouldLoadWarehouses);
    
    // Kiểm tra xem có phải là thay đổi vị trí mới không (TRƯỚC KHI cập nhật warehouseData)
    const isLocationChanged = warehouseData && 
        (Math.abs(warehouseData.lat - lat) > 0.0001 || Math.abs(warehouseData.lng - lng) > 0.0001);
    
    // Nếu là thay đổi vị trí mới (user chọn địa chỉ khác), reset flag để cho phép load lại
    if (isLocationChanged && shouldLoadWarehouses) {
        console.log("🔄 Location changed, resetting flags to allow reload...");
        warehousesLoaded = false;
        warehousesLoading = false; // Reset để cho phép load lại
    }
    
    // Lưu địa chỉ nhận hàng (PickupAddress) - đây là địa chỉ từ input hoặc vị trí hiện tại
    warehouseData = { lat: lat, lng: lng, address: addressLine || "" };

    // Cập nhật input với địa chỉ
    const input = document.getElementById("warehouseAreaInput");
    if (input && addressLine) {
        input.value = addressLine;
    }

    if (!map) {
        console.warn("⚠️ Map is not initialized in setWarehouseLocation!");
        // Chỉ gọi loadNearbyWarehouses nếu được yêu cầu và map chưa sẵn sàng
        if (shouldLoadWarehouses) {
            loadNearbyWarehouses(lat, lng);
        }
        return;
    }

        if (warehouseMarker) {
            warehouseMarker.setLatLng([lat, lng]);
        console.log("📍 Updated existing warehouse marker");
        } else {
        console.log("📍 Creating new warehouse marker");
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
            console.log("📍 Marker dragged to:", newPos.lat, newPos.lng);
            // Khi drag, luôn load lại warehouses
            warehousesLoaded = false;
            warehousesLoading = false; // Reset để cho phép load lại
                loadNearbyWarehouses(newPos.lat, newPos.lng);
            });
        }
        map.setView([lat, lng], 15);
    console.log("📍 Map view set to:", lat, lng);

    // Chỉ gọi loadNearbyWarehouses nếu được yêu cầu
    if (shouldLoadWarehouses) {
        console.log("🔄 Calling loadNearbyWarehouses from setWarehouseLocation...");
    loadNearbyWarehouses(lat, lng);
    }
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

// ============ LOAD NEARBY WAREHOUSES - CHỈ LẤY 3 KHO GẦN NHẤT ============
async function loadNearbyWarehouses(lat, lng) {
    if (!lat || !lng || isNaN(lat) || isNaN(lng)) {
        console.error("❌ loadNearbyWarehouses: Invalid coordinates", lat, lng);
        warehousesLoading = false;
        return;
    }

    // Tránh load nhiều lần cùng lúc (chỉ nếu đang load cùng một vị trí)
    // Nhưng cho phép load lại nếu vị trí đã thay đổi
    if (warehousesLoading) {
        // Kiểm tra xem có phải là vị trí mới không
        const isNewLocation = !warehouseData || 
            Math.abs(warehouseData.lat - lat) > 0.0001 || 
            Math.abs(warehouseData.lng - lng) > 0.0001;
        
        if (!isNewLocation) {
            console.log("⏭️ Already loading warehouses for same location, skipping duplicate call...");
            return;
        } else {
            console.log("🔄 New location detected, will load after current request completes...");
            // Đợi một chút rồi retry
            setTimeout(() => loadNearbyWarehouses(lat, lng), 500);
            return;
        }
    }

    warehousesLoading = true;
    console.log("🔍 Loading 3 nearest warehouses for:", lat, lng);

    // Hiển thị loading trong danh sách kho
    const listContainer = document.getElementById("warehouseList");
    if (listContainer) {
        listContainer.innerHTML = '<div style="padding: 20px; text-align: center; color: #667eea;">🔄 Đang tìm 3 kho gần nhất...</div>';
    }

    try {
        // CHỈ LẤY 3 KHO GẦN NHẤT
        const url = `/Quote/NearbyWarehouses?lat=${lat}&lng=${lng}&take=3`;
        console.log("🌐 Fetching URL:", url);

        const response = await fetch(url);
        console.log("📡 Response status:", response.status, response.statusText);

        if (!response.ok) {
            const errorText = await response.text();
            console.error("❌ API Error Response:", errorText);
            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }

        const warehouses = await response.json();
        console.log("📦 Received warehouses from API:", warehouses);
        console.log("📦 Total warehouses:", warehouses?.length);

        if (!warehouses || !Array.isArray(warehouses)) {
            console.error("❌ Invalid warehouses data:", warehouses);
            if (listContainer) {
                listContainer.innerHTML = '<div style="padding: 15px; text-align: center; color: #e74c3c;">❌ Dữ liệu kho không hợp lệ.</div>';
            }
            warehousesLoading = false;
            return;
        }

        if (warehouses.length === 0) {
            console.log("⚠️ No warehouses found nearby");
            if (listContainer) {
                listContainer.innerHTML = `
                    <div style="padding: 20px; text-align: center; color: #666;">
                        <div style="font-size: 48px; margin-bottom: 10px;">📍</div>
                        <div style="font-size: 16px; font-weight: 600; margin-bottom: 5px;">Không tìm thấy kho nào gần đây</div>
                        <div style="font-size: 14px; color: #999;">Vui lòng thử tìm kiếm vị trí khác</div>
                    </div>
                `;
            }
            warehousesLoading = false;
            return;
        }

        nearbyWarehouses = warehouses;
        warehousesLoaded = true;
        warehousesLoading = false;

        console.log(`✅ Loaded ${warehouses.length} warehouse(s), rendering...`);

        // Render danh sách kho NGAY LẬP TỨC
        renderWarehouseList(warehouses);

        // Hiển thị kho trên bản đồ
        if (map) {
        displayWarehousesOnMap(warehouses);
        } else {
            console.warn("⚠️ Map not ready yet, will retry...");
            // Retry với timeout
            let attempts = 0;
            const retryInterval = setInterval(() => {
                attempts++;
                if (map && map._loaded) {
                    console.log("✅ Map ready, displaying warehouses...");
                    displayWarehousesOnMap(warehouses);
                    clearInterval(retryInterval);
                } else if (attempts >= 10) {
                    console.error("❌ Map still not ready after 10 attempts");
                    clearInterval(retryInterval);
                }
            }, 300);
        }
    } catch (error) {
        warehousesLoading = false;
        console.error("❌ Error loading warehouses:", error);
        if (listContainer) {
            listContainer.innerHTML = `
                <div style="padding: 20px; text-align: center; color: #e74c3c;">
                    <div style="font-size: 32px; margin-bottom: 10px;">⚠️</div>
                    <div style="font-size: 16px; font-weight: 600; margin-bottom: 5px;">Không thể tải danh sách kho</div>
                    <div style="font-size: 14px; margin-bottom: 10px;">${error.message}</div>
                    <button onclick="loadNearbyWarehouses(${lat}, ${lng})" style="padding: 8px 16px; background: #667eea; color: white; border: none; border-radius: 6px; cursor: pointer;">🔄 Thử lại</button>
                </div>
            `;
        }
    }
}

function renderWarehouseList(warehouses) {
    console.log("📋 renderWarehouseList called with:", warehouses?.length, "warehouse(s)");

    const listContainer = document.getElementById("warehouseList");
    if (!listContainer) {
        console.error("❌ warehouseList element not found!");
        return;
    }

    console.log("✅ warehouseList element found, rendering...");
    listContainer.innerHTML = "";

    if (!warehouses || warehouses.length === 0) {
        console.warn("⚠️ No warehouses to render");
        listContainer.innerHTML = `
            <div style="padding: 20px; text-align: center; color: #666;">
                <div style="font-size: 48px; margin-bottom: 10px;">📦</div>
                <div style="font-size: 16px; font-weight: 600;">Không tìm thấy kho nào gần đây</div>
            </div>
        `;
        return;
    }

    console.log(`📋 Rendering ${warehouses.length} warehouse(s) to list...`);

    // Thêm tiêu đề cho danh sách
    const headerDiv = document.createElement("div");
    headerDiv.style.cssText = "padding: 15px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; border-radius: 12px 12px 0 0; margin-bottom: 10px;";
    headerDiv.innerHTML = `
        <div style="font-size: 18px; font-weight: 700; margin-bottom: 5px;">🏪 ${warehouses.length} Kho Gần Nhất</div>
        <div style="font-size: 13px; opacity: 0.9;">Nhấn vào kho để xem chi tiết và chọn</div>
    `;
    listContainer.appendChild(headerDiv);

    warehouses.forEach((warehouse, index) => {
        // ASP.NET Core mặc định serialize theo PascalCase
        const warehouseId = warehouse.Id ?? warehouse.id;
        const warehouseName = warehouse.Name ?? warehouse.name ?? "Kho";
        const distanceKm = warehouse.distanceKm ?? warehouse.DistanceKm ?? 0;
        const address = warehouse.full ?? warehouse.Full ?? warehouse.addressLine ?? warehouse.AddressLine ?? "";
        const storeName = warehouse.StoreName ?? warehouse.storeName ?? "";

        console.log(`📋 Rendering warehouse ${index + 1}:`, {
            id: warehouseId,
            name: warehouseName,
            distance: distanceKm.toFixed(2) + " km",
            address: address
        });

        const item = document.createElement("div");
        item.className = "warehouse-item";
        item.dataset.warehouseId = warehouseId;

        // Thêm badge cho thứ hạng (Top 1, 2, 3)
        const rankBadge = index === 0 ? '🥇' : index === 1 ? '🥈' : '🥉';

        // Check if this is the selected warehouse
        if (selectedWarehouse) {
            const selectedId = selectedWarehouse.Id ?? selectedWarehouse.id;
            if (selectedId === warehouseId) {
                item.classList.add("selected");
            }
        }

        item.innerHTML = `
            <div style="display: flex; align-items: flex-start; gap: 12px;">
                <div style="font-size: 32px; line-height: 1;">${rankBadge}</div>
                <div style="flex: 1;">
                    <div class="warehouse-name" style="font-size: 16px; font-weight: 700; margin-bottom: 4px;">${warehouseName}</div>
                    <div class="warehouse-address" style="font-size: 13px; color: #666; margin-bottom: 8px;">${address}</div>
                    <div class="warehouse-info" style="display: flex; justify-content: space-between; align-items: center;">
                        <div style="font-size: 13px; color: #667eea;">📦 ${storeName}</div>
                        <div class="warehouse-distance" style="background: linear-gradient(135deg, #27ae60 0%, #2ecc71 100%); color: white; padding: 4px 10px; border-radius: 12px; font-size: 12px; font-weight: 700;">📍 ${distanceKm.toFixed(2)} km</div>
                    </div>
                </div>
            </div>
        `;

        item.addEventListener("click", function () {
            selectWarehouse(warehouse);
        });

        listContainer.appendChild(item);
    });

    // Thêm footer với gợi ý
    const footerDiv = document.createElement("div");
    footerDiv.style.cssText = "padding: 12px; background: #f8f9fa; border-radius: 0 0 12px 12px; text-align: center; color: #666; font-size: 13px; margin-top: 10px;";
    footerDiv.innerHTML = `💡 <strong>Gợi ý:</strong> Kho gần nhất sẽ giúp tiết kiệm chi phí vận chuyển`;
    listContainer.appendChild(footerDiv);
}

// ============ SET WAREHOUSE LOCATION - TỰ ĐỘNG LOAD 3 KHO GẦN NHẤT ============
function setWarehouseLocation(lat, lng, addressLine = null, shouldLoadWarehouses = true) {
    console.log("📍 setWarehouseLocation called:", lat, lng, addressLine, "shouldLoadWarehouses:", shouldLoadWarehouses);

    // Lưu địa chỉ nhận hàng (PickupAddress)
    warehouseData = { lat: lat, lng: lng, address: addressLine || "" };

    // Cập nhật input với địa chỉ
    const input = document.getElementById("warehouseAreaInput");
    if (input && addressLine) {
        input.value = addressLine;
    }

    if (!map) {
        console.warn("⚠️ Map is not initialized in setWarehouseLocation!");
        // Vẫn load warehouses ngay cả khi map chưa sẵn sàng
        if (shouldLoadWarehouses) {
            // Reset flag để cho phép load lại
            warehousesLoaded = false;
            warehousesLoading = false;
            loadNearbyWarehouses(lat, lng);
        }
        return;
    }

    if (warehouseMarker) {
        warehouseMarker.setLatLng([lat, lng]);
        console.log("📍 Updated existing warehouse marker");
    } else {
        console.log("📍 Creating new warehouse marker");
        warehouseMarker = L.marker([lat, lng], {
            draggable: true,
            icon: L.divIcon({
                className: "custom-marker",
                html: '<div style="background:#f26722;width:36px;height:36px;border-radius:50%;border:4px solid white;box-shadow:0 4px 16px rgba(242,103,34,0.7);display:flex;align-items:center;justify-content:center;font-size:20px;">📍</div>',
                iconSize: [36, 36],
                iconAnchor: [18, 18],
            }),
        })
            .addTo(map)
            .bindPopup("📍 Địa chỉ nhận hàng<br><small>(Kéo để di chuyển)</small>");

        warehouseMarker.on("dragend", async function (e) {
            const newPos = e.target.getLatLng();
            warehouseData.lat = newPos.lat;
            warehouseData.lng = newPos.lng;
            const newAddress = await reverseGeocode(newPos.lat, newPos.lng);
            warehouseData.address = newAddress;
            const input = document.getElementById("warehouseAreaInput");
            if (input) input.value = newAddress;
            console.log("📍 Marker dragged to:", newPos.lat, newPos.lng);

            // Khi drag, reset và load lại 3 kho gần nhất
            warehousesLoaded = false;
            warehousesLoading = false;
            selectedWarehouse = null; // Reset kho đã chọn
            loadNearbyWarehouses(newPos.lat, newPos.lng);
        });
    }

    map.setView([lat, lng], 14); // Zoom level 14 để nhìn rõ hơn
    console.log("📍 Map view set to:", lat, lng);

    // Load 3 kho gần nhất nếu được yêu cầu
    if (shouldLoadWarehouses) {
        // Reset flags để load lại
        warehousesLoaded = false;
        warehousesLoading = false;
        loadNearbyWarehouses(lat, lng);
    }
}

// ============ DISPLAY WAREHOUSES ON MAP - CHỈ 3 KHO ============
function displayWarehousesOnMap(warehouses) {
    console.log("🗺️ displayWarehousesOnMap called with:", warehouses?.length, "warehouse(s)");

    if (!map) {
        console.error("❌ Map is not initialized! Cannot display warehouses.");
        return;
    }

    console.log("✅ Map is ready, proceeding to display warehouses...");

    // Remove old warehouse markers (keep warehouseMarker)
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

    console.log(`Displaying ${warehouses.length} warehouse(s) on map`);

    const bounds = [];
    let markersAdded = 0;

    warehouses.forEach((warehouse, index) => {
        let lat = warehouse.Latitude ?? warehouse.latitude ?? warehouse.lat ?? warehouse.Lat;
        let lng = warehouse.Longitude ?? warehouse.longitude ?? warehouse.lng ?? warehouse.Lng;

        if (lat == null || lng == null) {
            console.warn(`⚠️ Warehouse ${index} missing coordinates`);
            return;
        }

        const latNum = parseFloat(lat);
        const lngNum = parseFloat(lng);

        if (isNaN(latNum) || isNaN(lngNum)) {
            console.error(`Warehouse ${index} has invalid coordinates:`, lat, lng);
            return;
        }

        const warehouseName = warehouse.Name ?? warehouse.name ?? "Kho";
        const warehouseId = warehouse.Id ?? warehouse.id;
        const distanceKm = warehouse.distanceKm ?? warehouse.DistanceKm ?? 0;
        const address = warehouse.full ?? warehouse.Full ?? warehouse.addressLine ?? warehouse.AddressLine ?? "";
        const storeName = warehouse.StoreName ?? warehouse.storeName ?? "";

        // Badge cho thứ hạng
        const rankBadge = index === 0 ? '🥇' : index === 1 ? '🥈' : '🥉';

        console.log(`📍 Adding marker ${index + 1}:`, warehouseName, "at", latNum, lngNum);

        bounds.push([latNum, lngNum]);

        try {
            const marker = L.marker([latNum, lngNum], {
                icon: L.divIcon({
                    className: "warehouse-marker",
                    html: `<div style="background:linear-gradient(135deg, #667eea 0%, #764ba2 100%);color:white;padding:10px 14px;border-radius:20px;font-size:14px;font-weight:700;box-shadow:0 4px 16px rgba(102,126,234,0.6);white-space:nowrap;border:3px solid white;">
                            ${rankBadge} ${warehouseName}
                          </div>`,
                    iconSize: [180, 45],
                    iconAnchor: [90, 22],
                }),
            }).addTo(map);

            markersAdded++;

            const popupContent = `
                <div style="min-width:220px;">
                    <div style="display: flex; align-items: center; gap: 8px; margin-bottom: 8px;">
                        <span style="font-size: 24px;">${rankBadge}</span>
                        <h4 style="margin:0;color:#667eea;">${warehouseName}</h4>
                    </div>
                    ${storeName ? `<p style="margin:4px 0;font-size:13px;color:#666;">📦 ${storeName}</p>` : ""}
                    <p style="margin:4px 0;font-size:12px;color:#888;">${address}</p>
                    <p style="margin:8px 0 0 0;font-size:13px;">
                        <strong style="color:#27ae60;">📍 ${distanceKm.toFixed(2)} km</strong>
                    </p>
                    <button onclick="selectWarehouseFromMap('${warehouseId}')" style="margin-top:10px;padding:8px 14px;background:#667eea;color:white;border:none;border-radius:8px;cursor:pointer;font-size:13px;width:100%;font-weight:600;">
                        ✅ Chọn kho này
                    </button>
                </div>
            `;
            marker.bindPopup(popupContent);

            marker.options.warehouseId = warehouseId?.toString();
            marker.options.warehouse = warehouse;

            marker.on("click", function () {
                selectWarehouse(warehouse);
                marker.openPopup();
            });
        } catch (error) {
            console.error(`Error adding marker for warehouse ${index}:`, error);
        }
    });

    console.log(`✅ Added ${markersAdded} marker(s) to map`);

    // Fit map view
    if (bounds.length > 0) {
        if (warehouseMarker) {
            const markerLatLng = warehouseMarker.getLatLng();
            bounds.push([markerLatLng.lat, markerLatLng.lng]);
        }

        try {
            console.log(`🗺️ Fitting bounds for ${bounds.length} locations`);
            map.fitBounds(bounds, {
                padding: [60, 60],
                maxZoom: 14,
            });
            console.log("✅ Map bounds fitted successfully");
        } catch (e) {
            console.error("❌ Error fitting bounds:", e);
        }
    }
}

// Export functions for debugging
window.bookingDebug = {
    map: () => map,
    nearbyWarehouses: () => nearbyWarehouses,
    selectedWarehouse: () => selectedWarehouse,
    warehouseData: () => warehouseData,
    loadNearbyWarehouses: loadNearbyWarehouses,
    reloadWarehouses: () => {
        if (warehouseData && warehouseData.lat && warehouseData.lng) {
            warehousesLoaded = false;
            warehousesLoading = false;
            loadNearbyWarehouses(warehouseData.lat, warehouseData.lng);
    } else {
            console.warn("No location set yet");
    }
}
};

// Helper function để chọn kho từ map popup
function selectWarehouseFromMap(warehouseId) {
    const warehouse = nearbyWarehouses.find((w) => {
        const wId = w.Id ?? w.id;  // ASP.NET Core mặc định PascalCase
        return wId && wId.toString() === warehouseId.toString();
    });
    if (warehouse) {
        selectWarehouse(warehouse);
    } else {
        console.warn("⚠️ Warehouse not found with ID:", warehouseId);
    }
}

function renderWarehouseList(warehouses) {
    console.log("📋 renderWarehouseList called with:", warehouses?.length, "warehouses");
    
    const listContainer = document.getElementById("warehouseList");
    if (!listContainer) {
        console.error("❌ warehouseList element not found!");
        return;
    }

    console.log("✅ warehouseList element found, rendering...");
    listContainer.innerHTML = "";

    if (!warehouses || warehouses.length === 0) {
        console.warn("⚠️ No warehouses to render");
        listContainer.innerHTML =
            '<div style="padding: 15px; text-align: center; color: #666;">Không tìm thấy kho nào gần đây.</div>';
        return;
    }

    console.log(`📋 Rendering ${warehouses.length} warehouses to list...`);

    warehouses.forEach((warehouse, index) => {
        // ASP.NET Core mặc định serialize theo PascalCase
        const warehouseId = warehouse.Id ?? warehouse.id;
        const warehouseName = warehouse.Name ?? warehouse.name ?? "Kho";
        const distanceKm = warehouse.distanceKm ?? warehouse.DistanceKm ?? 0;
        const address = warehouse.full ?? warehouse.Full ?? warehouse.addressLine ?? warehouse.AddressLine ?? "";
        const storeName = warehouse.StoreName ?? warehouse.storeName ?? "";
        
        console.log(`📋 Rendering warehouse ${index + 1}:`, {
            id: warehouseId,
            name: warehouseName,
            distance: distanceKm,
            address: address
        });
        
        const item = document.createElement("div");
        item.className = "warehouse-item";
        item.dataset.warehouseId = warehouseId;

        // Check if this is the selected warehouse
        if (selectedWarehouse) {
            const selectedId = selectedWarehouse.Id ?? selectedWarehouse.id;  // ASP.NET Core mặc định PascalCase
            if (selectedId === warehouseId) {
                item.classList.add("selected");
            }
        }

        item.innerHTML = `
            <div class="warehouse-name">${warehouseName}</div>
            <div class="warehouse-address">${address}</div>
            <div class="warehouse-info">
                <div>📦 ${storeName}</div>
                <div class="warehouse-distance">${distanceKm.toFixed(2)} km</div>
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

    // ASP.NET Core mặc định serialize theo PascalCase
    const warehouseId = warehouse.Id ?? warehouse.id;
    const warehouseName = warehouse.Name ?? warehouse.name ?? "";
    if (warehouseIdInput && warehouseId) {
        warehouseIdInput.value = warehouseId.toString();
    }
    if (warehouseNameDisplay) {
        warehouseNameDisplay.value = warehouseName;
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
                
                // Tạo tooltip với thông tin size chi tiết
                const widthM = slot.widthM || slot.WidthM || 0;
                const lengthM = slot.lengthM || slot.LengthM || 0;
                const heightM = slot.heightM || slot.HeightM || 0;
                const volumeM3 = slot.volumeM3 || slot.VolumeM3 || (widthM * lengthM * heightM);
                const areaM2 = (widthM * lengthM).toFixed(2);
                
                let sizeInfo = "";
                if (widthM > 0 && lengthM > 0 && heightM > 0) {
                    sizeInfo = `Kích thước: ${widthM}m × ${lengthM}m × ${heightM}m\nDiện tích: ${areaM2} m²\nThể tích: ${volumeM3.toFixed(2)} m³`;
                } else if (slot.size) {
                    sizeInfo = `Size: ${slot.size}`;
                } else {
                    sizeInfo = "Size: N/A";
                }
                
                const priceInfo = slot.basePricePerHour ? `${Number(slot.basePricePerHour).toLocaleString('vi-VN')} đ/h` : "N/A";
                slotEl.title = `Slot: ${slot.code || "N/A"}\n${sizeInfo}\nGiá: ${priceInfo}`;

                // Chỉ cho phép click nếu slot available (không blocked, occupied, hoặc reserved)
                if (statusClass === "available") {
                    slotEl.addEventListener("click", function () {
                        toggleSlotSelection(slotEl, slot);
                    });
                } else if (statusClass === "reserved") {
                    // Slot reserved hiển thị tooltip thông tin
                    slotEl.title = `Slot: ${slot.code || "N/A"}\n${sizeInfo}\nGiá: ${priceInfo}\n⚠️ Đang được giữ chỗ`;
                    slotEl.style.cursor = "not-allowed";
                } else if (statusClass === "occupied") {
                    slotEl.title = `Slot: ${slot.code || "N/A"}\n${sizeInfo}\nGiá: ${priceInfo}\n⚠️ Đang sử dụng`;
                    slotEl.style.cursor = "not-allowed";
                } else if (statusClass === "blocked") {
                    slotEl.title = `Slot: ${slot.code || "N/A"}\n${sizeInfo}\nGiá: ${priceInfo}\n🚫 Đã khóa`;
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
        warehouseId = selectedWarehouse.Id ?? selectedWarehouse.id;  // ASP.NET Core mặc định PascalCase
        warehouseName = selectedWarehouse.Name ?? selectedWarehouse.name ?? "";
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
        // Định dạng ảnh được chấp nhận
        const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp', 'image/gif'];
        const allowedExtensions = ['.jpg', '.jpeg', '.png', '.webp', '.gif'];
        
        // Kiểm tra từng file
        const validFiles = [];
        let hasInvalidFile = false;
        let invalidFileNames = [];
        
        Array.from(files).forEach((file, index) => {
            // Kiểm tra định dạng file
            const fileExtension = '.' + file.name.split('.').pop().toLowerCase();
            const isValidType = allowedTypes.includes(file.type) || allowedExtensions.includes(fileExtension);
            
            if (!isValidType) {
                hasInvalidFile = true;
                invalidFileNames.push(file.name);
                console.error(`❌ File không hợp lệ: ${file.name} (${file.type || 'unknown type'})`);
            } else {
                validFiles.push(file);
            }
        });
        
        // Nếu có file không hợp lệ, hiển thị toast và clear input
        if (hasInvalidFile) {
            const invalidNames = invalidFileNames.join(', ');
            showToast(`⚠️ Định dạng ảnh không hợp lệ!\n\nFile không hợp lệ: ${invalidNames}\n\nVui lòng chọn file ảnh có định dạng: JPG, JPEG, PNG, WEBP hoặc GIF.`, 'error', 6000);
            input.value = ''; // Clear input
            if (previewContainer) {
                previewContainer.innerHTML = "";
                previewContainer.style.display = "none";
            }
            return;
        }
        
        console.log(`📷 ${validFiles.length} image(s) selected:`);
        
        // Hiển thị preview cho tất cả các ảnh hợp lệ
        validFiles.forEach((file, index) => {
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

        // Gọi AI để phân tích tất cả ảnh hợp lệ và tự động điền vào bảng
        await analyzeImageAndFillItems(validFiles);
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
    // Collect data from form
    const warehouseAreaInput = document.getElementById("warehouseAreaInput");
    const pickupAddressText =
        warehouseAreaInput?.value?.trim() || warehouseData?.address || "";
    
    const warehouseIdInput = document.getElementById("warehouseIdInput");
    const warehouseIdValue = warehouseIdInput?.value?.trim();
    
    const startDate = document.getElementById("storageStartDate").value;
    const endDate = document.getElementById("storageEndDate").value;
    const customerName = document.getElementById("customerName")?.value?.trim();
    const customerPhone = document.getElementById("customerPhone")?.value?.trim();
    const customerEmail = document.getElementById("customerEmail")?.value?.trim();
    
    // Validate email nếu có nhập
    if (customerEmail && customerEmail.length > 0) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(customerEmail)) {
            showToast("⚠️ Email không hợp lệ! Vui lòng nhập đúng định dạng email (ví dụ: example@email.com)", 'error', 5000);
            const emailError = document.getElementById("customerEmailError");
            if (emailError) {
                emailError.textContent = "Email không hợp lệ! Vui lòng nhập đúng định dạng email (ví dụ: example@email.com)";
                emailError.style.display = "block";
            }
            const emailInput = document.getElementById("customerEmail");
            if (emailInput) {
                emailInput.classList.add("error");
            }
            const bookBtn = document.getElementById("bookBtn");
            if (bookBtn) {
                bookBtn.disabled = false;
                bookBtn.textContent = "Đặt hàng";
            }
            return;
        }
    }

    // Collect items
    const items = [];
    const rows = document.querySelectorAll("#itemsTableBody > tr");
    rows.forEach((row) => {
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
    formData.append("PickupAddress.RecipientName", customerName || "");
    formData.append("PickupAddress.RecipientPhone", customerPhone || "");

    // Gửi WarehouseId (ưu tiên) để tìm warehouse chính xác
    const finalWarehouseId = selectedWarehouse?.Id ?? selectedWarehouse?.id ?? warehouseIdValue;  // ASP.NET Core mặc định PascalCase
    
    // Validate: Phải chọn kho trước khi submit
    if (!finalWarehouseId || !selectedWarehouse) {
        showToast("⚠️ Vui lòng chọn kho từ danh sách trước khi đặt hàng!", 'error', 5000);
        const bookBtn = document.getElementById("bookBtn");
        if (bookBtn) {
            bookBtn.disabled = false;
            bookBtn.textContent = "Đặt hàng";
        }
        return;
    }

    // WarehouseArea - Địa chỉ kho đã chọn (nơi lưu trữ)
    const warehouseAreaLine =
        selectedWarehouse?.full ||
        selectedWarehouse?.addressLine ||
        selectedWarehouse?.AddressLine ||
        selectedWarehouse?.name ||
        "Kho đã chọn";
    const warehouseLat =
        selectedWarehouse?.latitude ||
        selectedWarehouse?.Latitude ||
        selectedWarehouse?.lat ||
        selectedWarehouse?.Lat;
    const warehouseLng =
        selectedWarehouse?.longitude ||
        selectedWarehouse?.Longitude ||
        selectedWarehouse?.lng ||
        selectedWarehouse?.Lng;
    
    if (finalWarehouseId) {
        formData.append("WarehouseId", finalWarehouseId.toString());
    }

    formData.append("WarehouseArea.AddressLine", warehouseAreaLine);
    formData.append("WarehouseArea.Latitude", warehouseLat);
    formData.append("WarehouseArea.Longitude", warehouseLng);
    formData.append("WarehouseArea.RecipientName", customerName || "");
    formData.append("WarehouseArea.RecipientPhone", customerPhone || "");
    formData.append("StorageStartDate", startDate || "");
    formData.append("StorageEndDate", endDate || "");
    formData.append("CustomerFullName", customerName || "");
    formData.append("CustomerPhone", customerPhone || "");
    if (customerEmail) {
        formData.append("CustomerEmail", customerEmail);
    }
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
        // Validate image format trước khi submit
        const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp', 'image/gif'];
        const allowedExtensions = ['.jpg', '.jpeg', '.png', '.webp', '.gif'];
        const file = productImageInput.files[0];
        const fileExtension = '.' + file.name.split('.').pop().toLowerCase();
        const isValidType = allowedTypes.includes(file.type) || allowedExtensions.includes(fileExtension);
        
        if (!isValidType) {
            showToast(`⚠️ Định dạng ảnh không hợp lệ!\n\nFile: ${file.name}\n\nVui lòng chọn file ảnh có định dạng: JPG, JPEG, PNG, WEBP hoặc GIF.`, 'error', 6000);
            const bookBtn = document.getElementById("bookBtn");
            if (bookBtn) {
                bookBtn.disabled = false;
                bookBtn.textContent = "Đặt hàng";
            }
            return;
        }
        
        formData.append("productImage", file);
        hasImage = true;
        console.log(
            "📷 Product image will be uploaded:",
            file.name,
            `(${(file.size / 1024).toFixed(2)} KB)`
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
            console.log("WarehouseId:", finalWarehouseId);
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

                    showToast(`${result.message}\n\n📦 Mã đơn hàng: ${result.orderId}`, 'success', 6000);

                    // Redirect to success page
                    if (result.orderId) {
                        window.location.href =
                            "/Booking/Success?Id=" + encodeURIComponent(result.orderId);
                    }
                }
            } else {
                // Lỗi từ server - có thể là validation errors từ ModelState
                // Chuyển đổi message tiếng Anh sang tiếng Việt
                let errorMessage = result?.message || result?.detail || `Lỗi ${response.status}: ${response.statusText}`;
                
                // Sửa các message tiếng Anh thường gặp
                if (errorMessage.includes("The AddressLine field is required") || errorMessage.includes("AddressLine field is required")) {
                    errorMessage = "Địa chỉ là bắt buộc";
                } else if (errorMessage.includes("WarehouseId") || errorMessage.includes("Vui lòng chọn kho") || errorMessage.includes("chọn kho")) {
                    errorMessage = "Vui lòng chọn kho từ danh sách";
                } else if (errorMessage.includes("field is required")) {
                    errorMessage = errorMessage.replace(/The (\w+) field is required/gi, "Trường $1 là bắt buộc");
                    errorMessage = errorMessage.replace(/(\w+) field is required/gi, "Trường $1 là bắt buộc");
                    // Xử lý WarehouseId riêng
                    if (errorMessage.includes("WarehouseId") || errorMessage.includes("Kho")) {
                        errorMessage = "Vui lòng chọn kho từ danh sách";
                    }
                }
                console.error("Order submission failed:", {
                    status: response.status,
                    result: result,
                    responseText: result,
                });

                // Xử lý validation errors từ server
                if (result?.errors && Array.isArray(result.errors)) {
                    // Chuyển đổi tất cả message sang tiếng Việt
                    const validationErrors = result.errors.map(e => {
                        let msg = e.Message || e.message || e;
                        // Chuyển các message tiếng Anh thường gặp sang tiếng Việt
                        if (msg.includes("The AddressLine field is required") || msg.includes("AddressLine field is required")) {
                            msg = "Địa chỉ là bắt buộc";
                        } else if (msg.includes("Email") && (msg.includes("invalid") || msg.includes("không hợp lệ") || msg.includes("hợp lệ"))) {
                            msg = "Email không hợp lệ! Vui lòng nhập đúng định dạng email (ví dụ: example@email.com)";
                        } else if (msg.includes("CustomerEmail") && msg.includes("required")) {
                            msg = "Email là bắt buộc";
                        } else if (msg.includes("WarehouseId") || msg.includes("Vui lòng chọn kho")) {
                            msg = "Vui lòng chọn kho từ danh sách";
                        } else if (msg.includes("field is required")) {
                            msg = msg.replace(/The (\w+) field is required/gi, "Trường $1 là bắt buộc");
                            msg = msg.replace(/(\w+) field is required/gi, "Trường $1 là bắt buộc");
                            // Xử lý WarehouseId riêng
                            if (msg.includes("WarehouseId") || msg.includes("Kho")) {
                                msg = "Vui lòng chọn kho từ danh sách";
                            }
                        }
                        return msg;
                    }).join('\n');
                    showToast(`Có lỗi validation:\n\n${validationErrors}`, 'error', 8000);
                    
                    // Highlight các trường có lỗi
                    result.errors.forEach(error => {
                        const fieldName = error.Field || error.field;
                        if (fieldName) {
                            // Map field name to input ID
                            let inputId = '';
                            let errorElementId = '';
                            if (fieldName.includes('CustomerFullName')) {
                                inputId = 'customerName';
                                errorElementId = 'customerNameError';
                            }
                            else if (fieldName.includes('CustomerPhone')) {
                                inputId = 'customerPhone';
                                errorElementId = 'customerPhoneError';
                            }
                            else if (fieldName.includes('CustomerEmail')) {
                                inputId = 'customerEmail';
                                errorElementId = 'customerEmailError';
                            }
                            else if (fieldName.includes('StorageStartDate')) {
                                inputId = 'storageStartDate';
                                errorElementId = 'storageStartDateError';
                            }
                            else if (fieldName.includes('StorageEndDate')) {
                                inputId = 'storageEndDate';
                                errorElementId = 'storageEndDateError';
                            }
                            else if (fieldName.includes('WarehouseId')) inputId = 'warehouseIdInput';
                            else if (fieldName.includes('PickupAddress')) inputId = 'warehouseAreaInput';
                            
                            if (inputId) {
                                const input = document.getElementById(inputId);
                                if (input) {
                                    input.classList.add('error');
                                    input.focus();
                                }
                            }
                            
                            // Hiển thị error message bên dưới input
                            if (errorElementId) {
                                const errorElement = document.getElementById(errorElementId);
                                if (errorElement) {
                                    let errorMsg = error.Message || error.message || "Lỗi validation";
                                    // Chuyển message sang tiếng Việt nếu cần
                                    if (errorMsg.includes("Email") && (errorMsg.includes("invalid") || errorMsg.includes("không hợp lệ") || errorMsg.includes("hợp lệ"))) {
                                        errorMsg = "Email không hợp lệ! Vui lòng nhập đúng định dạng email (ví dụ: example@email.com)";
                                    }
                                    errorElement.textContent = errorMsg;
                                    errorElement.style.display = "block";
                                }
                            }
                        }
                    });
                } else {
                    showToast(
                        `${errorMessage}\n\nVui lòng kiểm tra lại:\n- Địa chỉ nhận hàng đã nhập chưa?\n- Đã chọn kho chưa?\n- Đã nhập ít nhất một món đồ chưa?`,
                        'error',
                        8000
                    );
                }

                bookBtn.textContent = originalText;
                bookBtn.disabled = false;
            }
        } catch (error) {
            console.error("Error submitting order:", error);
            console.error("Error stack:", error.stack);
            showToast(
                `Không thể kết nối đến máy chủ!\n\nChi tiết: ${error.message}\n\nVui lòng kiểm tra kết nối và thử lại.`,
                'error',
                8000
            );
            bookBtn.textContent = originalText;
            bookBtn.disabled = false;
        }
    }
}

// Hiển thị bảng báo giá chi tiết
function showQuoteBreakdown(quote, quotationId) {
    // Debug: Log dữ liệu AI để kiểm tra
    console.log("🔍 Quote data for breakdown:", {
        requiredVolumeM3: quote.requiredVolumeM3,
        requiredAreaM2: quote.requiredAreaM2,
        analysisDetails: quote.analysisDetails ? quote.analysisDetails.substring(0, 100) + "..." : null,
        itemEstimates: quote.itemEstimates,
        geminiAnalysisAvailable: quote.geminiAnalysisAvailable
    });
    
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
                    
                    <!-- Yêu cầu tính toán từ AI - hiển thị diện tích, thể tích ước tính và giải thích cách sắp xếp -->
                    ${(quote.requiredVolumeM3 != null && quote.requiredVolumeM3 > 0) || 
                       (quote.requiredAreaM2 != null && quote.requiredAreaM2 > 0) || 
                       (quote.analysisDetails && quote.analysisDetails.trim().length > 0)
            ? `
                    <div style="margin-bottom: 24px; padding: 16px; background: #e3f2fd; border-radius: 8px; border-left: 4px solid #2196f3;">
                        <h3 style="margin: 0 0 12px; color: #333; font-size: 18px;">📊 Ước tính không gian cần thiết (từ AI)</h3>
                        
                        ${quote.requiredVolumeM3 != null && quote.requiredVolumeM3 > 0
                ? `<div style="margin: 8px 0; padding: 10px; background: white; border-radius: 6px;">
                    <p style="margin: 0; font-size: 16px; font-weight: 600; color: #1976d2;">
                        <strong>📦 Thể tích ước tính cần thiết:</strong> ${formatNumber(quote.requiredVolumeM3)} m³
                    </p>
                </div>`
                : ""
            }
                        
                        ${quote.requiredAreaM2 != null && quote.requiredAreaM2 > 0
                ? `<div style="margin: 8px 0; padding: 10px; background: white; border-radius: 6px;">
                    <p style="margin: 0; font-size: 16px; font-weight: 600; color: #1976d2;">
                        <strong>📐 Diện tích ước tính cần thiết:</strong> ${formatNumber(quote.requiredAreaM2)} m²
                    </p>
                </div>`
                : ""
            }
                        
                        ${quote.analysisDetails && quote.analysisDetails.trim().length > 0
                ? `<div style="margin-top: 12px; padding: 12px; background: white; border-radius: 6px; font-size: 14px; color: #555; line-height: 1.6;">
                    <h4 style="margin: 0 0 8px; font-size: 15px; color: #333; font-weight: 600;">💡 Giải thích cách sắp xếp:</h4>
                    <div style="white-space: pre-wrap;">${quote.analysisDetails.replace(/\n/g, "<br>")}</div>
                </div>`
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
                                <td style="padding: 8px 0; font-weight: 600;">Phí slot: <span style="color: #667eea; font-size: 16px;">(1)</span></td>
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
                            `
            : ""
        }
                            <tr style="border-bottom: 2px solid #4caf50;">
                                <td style="padding: 8px 0; font-weight: 600;">Tổng phí dịch vụ: <span style="color: #667eea; font-size: 16px;">(2)</span></td>
                                <td style="text-align: right; padding: 8px 0; font-weight: 600;">${formatCurrency(
                    quote.totalAddonPrice || 0
                )}</td>
                            </tr>
                            <tr style="border-bottom: 1px solid #ddd;">
                                <td style="padding: 8px 0;">VAT (${quote.vatRate || 10
        }%): <span style="color: #667eea; font-size: 16px;">(3)</span></td>
                                <td style="text-align: right; padding: 8px 0;">${formatCurrency(
            quote.vatAmount || 0
        )}</td>
                            </tr>
                        </table>
                        <div style="margin-top: 16px; padding: 16px; background: #4caf50; color: white; border-radius: 8px;">
                            <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 8px;">
                                <span style="font-size: 20px; font-weight: 700;">THÀNH TIỀN:</span>
                                <span style="font-size: 24px; font-weight: 700;">${formatCurrency(
            quote.totalAmount || 0
        )}</span>
                            </div>
                            <div style="text-align: center; margin-top: 8px; padding-top: 8px; border-top: 1px solid rgba(255,255,255,0.3); font-size: 16px; opacity: 0.9;">
                                <span>(1) + (2) + (3) = ${formatCurrency(quote.totalAmount || 0)}</span>
                            </div>
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

            // Redirect đến màn hình Quản lý Báo giá - tab "Đã chỉnh sửa"
            window.location.href = "/Quotation/Index?status=revised";
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
        "🧊 Kho mát": 5000, // VND/ngày
        "💧 Chống ẩm": 3000, // VND/ngày
        "🔒 An ninh cao": 4000, // VND/ngày
        "🛡️ Bảo hiểm hàng hóa": 10000, // VND (một lần)
        "🏢 Kho có thang máy": 2000, // VND/ngày
        "📹 Giám sát 24/7": 6000, // VND/ngày
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

// Initialize email validation
function initEmailValidation() {
    const emailInput = document.getElementById("customerEmail");
    const emailError = document.getElementById("customerEmailError");
    
    if (emailInput) {
        // Clear error khi user nhập lại
        emailInput.addEventListener('input', function() {
            if (emailError) {
                emailError.style.display = 'none';
            }
            emailInput.classList.remove('error');
        });
        
        // Validate khi blur (rời khỏi field)
        emailInput.addEventListener('blur', function() {
            const email = emailInput.value?.trim();
            if (email && email.length > 0) {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(email)) {
                    if (emailError) {
                        emailError.textContent = "Email không hợp lệ! Vui lòng nhập đúng định dạng email (ví dụ: example@email.com)";
                        emailError.style.display = "block";
                    }
                    emailInput.classList.add('error');
                } else {
                    if (emailError) {
                        emailError.style.display = 'none';
                    }
                    emailInput.classList.remove('error');
                }
            } else {
                // Nếu để trống, clear error (vì email là optional)
                if (emailError) {
                    emailError.style.display = 'none';
                }
                emailInput.classList.remove('error');
            }
        });
    }
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
