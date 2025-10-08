document.addEventListener("DOMContentLoaded", function () {
    // ====== INIT MAP ======
    const map = L.map('map').setView([21.0278, 105.8342], 12);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
    }).addTo(map);

    // ====== SAFE INIT GEOCODER ======
    function initGeocoder() {
        // Nếu Control.Geocoder chưa load, chờ 200ms rồi thử lại
        if (!L.Control || !L.Control.Geocoder) {
            console.warn("Geocoder chưa sẵn sàng, thử lại...");
            return setTimeout(initGeocoder, 200);
        }

        // Nếu provider nominatim chưa có, tạo provider thủ công
        if (!L.Control.Geocoder.nominatim) {
            L.Control.Geocoder.nominatim = function () {
                return {
                    geocode: function (query, cb) {
                        fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)}`)
                            .then(r => r.json())
                            .then(d => cb(d.map(i => ({
                                name: i.display_name,
                                center: L.latLng(i.lat, i.lon),
                                bbox: L.latLngBounds(
                                    L.latLng(i.boundingbox[0], i.boundingbox[2]),
                                    L.latLng(i.boundingbox[1], i.boundingbox[3])
                                )
                            }))));
                    },
                    reverse: function (loc, scale, cb) {
                        fetch(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${loc.lat}&lon=${loc.lng}`)
                            .then(r => r.json())
                            .then(d => cb([{
                                name: d.display_name,
                                center: L.latLng(d.lat, d.lon)
                            }]));
                    }
                };
            };
        }

        // Tạo control geocoder
        const geocoderControl = L.Control.geocoder({
            geocoder: L.Control.Geocoder.nominatim(),
            placeholder: '🔍 Tìm kiếm địa chỉ...',
            defaultMarkGeocode: false
        })
            .on('markgeocode', function (e) {
                const latlng = e.geocode.center;
                const name = e.geocode.name;
                setMarker(latlng, name);
                fetchReverseGeocode(latlng.lat, latlng.lng);
            })
            .addTo(map);
    }

    initGeocoder();

    // ====== MARKER HANDLER ======
    let marker;
    function setMarker(latlng, name) {
        if (marker) map.removeLayer(marker);
        marker = L.marker(latlng).addTo(map).bindPopup(name).openPopup();
        document.getElementById("lat").value = latlng.lat.toFixed(6);
        document.getElementById("lng").value = latlng.lng.toFixed(6);
        document.getElementById("addressLine").value = name;
    }

    map.on('click', function (e) {
        setMarker(e.latlng, `Vị trí đã chọn`);
        fetchReverseGeocode(e.latlng.lat, e.latlng.lng);
    });

    // ====== REVERSE GEOCODE ======
    function fetchReverseGeocode(lat, lng) {
        fetch(`https://nominatim.openstreetmap.org/reverse?lat=${lat}&lon=${lng}&format=json`)
            .then(res => res.json())
            .then(data => {
                const addr = data.address || {};
                const city = document.getElementById("city").value = addr.city || addr.state || '';
                const district = document.getElementById("district").value = addr.county || addr.suburb || '';
                const ward = document.getElementById("ward").value = addr.village || addr.town || addr.suburb || '';
                document.getElementById("addressLine").value = data.display_name;
            })
            .catch(() => console.log("Không lấy được thông tin địa chỉ"));
    }

    // ====== SLOT TABLE ======
    const slotTable = document.querySelector("#slotTable tbody");
    const slots = [];

    document.getElementById("btnAddSlot").addEventListener("click", function () {
        const code = document.getElementById("slotCode").value.trim();
        const size = document.getElementById("slotSize").value.trim();
        const status = document.getElementById("slotStatus").value;

        if (!code) return alert("Nhập mã slot!");

        slots.push({ Code: code, Size: size, Status: status });
        renderSlots();
        clearSlotInputs();
    });

    function renderSlots() {
        slotTable.innerHTML = "";
        slots.forEach((s, i) => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td><input type="hidden" name="slots[${i}].Code" value="${s.Code}" />${s.Code}</td>
                <td><input type="hidden" name="slots[${i}].Size" value="${s.Size}" />${s.Size || '-'}</td>
                <td><input type="hidden" name="slots[${i}].Status" value="${s.Status}" />${s.Status}</td>
                <td><button type="button" class="btn" onclick="removeSlot(${i})">🗑</button></td>`;
            slotTable.appendChild(tr);
        });
    }

    window.removeSlot = function (index) {
        slots.splice(index, 1);
        renderSlots();
    };

    function clearSlotInputs() {
        document.getElementById("slotCode").value = "";
        document.getElementById("slotSize").value = "";
        document.getElementById("slotStatus").value = "Available";
    }
});
const addr = data.address || {};
const city = document.getElementById("city").value = addr.city || addr.state || '';
const district = document.getElementById("district").value = addr.county || addr.suburb || '';
const ward = document.getElementById("ward").value = addr.village || addr.town || addr.suburb || '';
document.getElementById("addressLine").value = city + ', ' + district + ', ' + ward;