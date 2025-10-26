
    document.addEventListener("DOMContentLoaded", function () {
  // ====== GUARDS ======
  const mapEl = document.getElementById("map");
    if (!mapEl || !window.L) return;

    // ====== INIT MAP ======
    const map = L.map('map').setView([21.0278, 105.8342], 12);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap contributors'
  }).addTo(map);

    // ====== MARKER ======
    let marker;
    function setMarker(latlng, name) {
    if (marker) map.removeLayer(marker);
    marker = L.marker(latlng, {draggable: true }).addTo(map);
    if (name) marker.bindPopup(name).openPopup();
    setLatLngInputs(latlng);
    marker.on('dragend', (e) => {
      const p = e.target.getLatLng();
    setLatLngInputs(p);
    fetchReverseGeocode(p.lat, p.lng, /*writeAddressLine*/ true);
    });
  }

    function setLatLngInputs(latlng) {
    const lat = document.getElementById("lat");
    const lng = document.getElementById("lng");
    if (lat) lat.value = Number(latlng.lat).toFixed(6);
    if (lng) lng.value = Number(latlng.lng).toFixed(6);
  }

    // ====== SAFE INIT GEOCODER (với fallback) ======
    function initGeocoder(retry = 0) {
    if (!L.Control || !L.Control.Geocoder) {
      if (retry < 15) return setTimeout(() => initGeocoder(retry + 1), 200);
    console.warn("Leaflet Control.Geocoder chưa sẵn sàng.");
    return;
    }

    // Fallback provider nếu thiếu nominatim()
    if (!L.Control.Geocoder.nominatim) {
        L.Control.Geocoder.nominatim = function () {
            return {
                geocode: function (query, cb) {
                    fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)}`)
                        .then(r => r.json())
                        .then(d => cb(d.map(i => {
                            // boundingbox: [south, north, west, east]
                            const south = parseFloat(i.boundingbox?.[0] ?? i.lat);
                            const north = parseFloat(i.boundingbox?.[1] ?? i.lat);
                            const west = parseFloat(i.boundingbox?.[2] ?? i.lon);
                            const east = parseFloat(i.boundingbox?.[3] ?? i.lon);
                            return {
                                name: i.display_name,
                                center: L.latLng(parseFloat(i.lat), parseFloat(i.lon)),
                                bbox: L.latLngBounds(L.latLng(south, west), L.latLng(north, east))
                            };
                        })));
                },
                reverse: function (loc, _scale, cb) {
                    fetch(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${loc.lat}&lon=${loc.lng}`)
                        .then(r => r.json())
                        .then(d => cb([{
                            name: d.display_name,
                            center: L.latLng(parseFloat(d.lat), parseFloat(d.lon))
                        }]));
                }
            };
        };
    }

    L.Control.geocoder({
        geocoder: L.Control.Geocoder.nominatim(),
    placeholder: '🔍 Tìm kiếm địa chỉ...',
    defaultMarkGeocode: false
    })
    .on('markgeocode', function (e) {
      const p = e.geocode.center;
    setMarker(p, e.geocode.name);
    map.setView(p, 15);
    fetchReverseGeocode(p.lat, p.lng, /*writeAddressLine*/ true);
    })
    .addTo(map);
  }
    initGeocoder();

    // Click trên map → đặt marker + reverse
    map.on('click', function (e) {
        setMarker(e.latlng, 'Vị trí đã chọn');
    fetchReverseGeocode(e.latlng.lat, e.latlng.lng, /*writeAddressLine*/ true);
  });

    // ====== REVERSE GEOCODE ======
    function fetchReverseGeocode(lat, lng, writeAddressLine) {
        fetch(`https://nominatim.openstreetmap.org/reverse?lat=${lat}&lon=${lng}&format=json`)
            .then(res => res.json())
            .then(data => {
                const addr = data.address || {};
                const cityEl = document.getElementById("city");
                const districtEl = document.getElementById("district");
                const wardEl = document.getElementById("ward");
                const addressLineEl = document.getElementById("addressLine");

                const city = addr.city || addr.town || addr.state || '';
                const district = addr.district || addr.county || addr.suburb || '';
                const ward = addr.ward || addr.village || addr.town || '';

                if (cityEl) cityEl.value = city;
                if (districtEl) districtEl.value = district;
                if (wardEl) wardEl.value = ward;

                if (writeAddressLine && addressLineEl) {
                    // Ưu tiên display_name; fallback sang chuỗi ghép
                    addressLineEl.value = data.display_name || [ward, district, city].filter(Boolean).join(", ");
                }
            })
            .catch(() => console.log("Không lấy được thông tin địa chỉ"));
  }

    // ====== SLOT TABLE (3D + giá + lease + blocked) ======
    const slotTableBody = document.querySelector("#slotTable tbody");
    const slots = [];

  const el = (id) => document.getElementById(id);
  const esc = (v) => String(v ?? "").replaceAll("&","&amp;").replaceAll("<","&lt;").replaceAll('"',"&quot;");
  const dash = (v) => (v === null || v === undefined || v === "" || Number.isNaN(v)) ? "—" : v;

    const addBtn = document.getElementById("btnAddSlot");
    if (addBtn) {
        addBtn.addEventListener("click", function () {
            const code = (el("slotCode")?.value || "").trim();
            const row = parseInt(el("slotRow")?.value || "0", 10);
            const col = parseInt(el("slotCol")?.value || "0", 10);
            const h = parseFloat(el("slotH")?.value || "0");
            const l = parseFloat(el("slotL")?.value || "0");
            const w = parseFloat(el("slotW")?.value || "0");
            const price = parseFloat(el("slotPrice")?.value || "0");
            const status = (el("slotStatus")?.value || "4").trim();
            if (!code) return alert("Nhập mã slot!");
            if (row <= 0 || col <= 0) return alert("Row/Col phải > 0");
            if (h <= 0 || l <= 0 || w <= 0) return alert("Kích thước H/L/W phải > 0 khi không Blocked");
            const dupCode = slots.some(s => s.Code.toLowerCase() === code.toLowerCase());
            if (dupCode) return alert("Code đã tồn tại trong danh sách tạm.");
            const dupPos = slots.some(s => s.Row === row && s.Col === col);
            if (dupPos) return alert("Vị trí (Row,Col) đã tồn tại trong danh sách tạm.");

            slots.push({
                Code: code, Row: row, Col: col, HeightM: h, LengthM: l, WidthM: w,
                BasePricePerHour: price, Status: status
            });

            renderSlots();
            clearSlotInputs();
        });
  }

    function renderSlots() {
    if (!slotTableBody) return;
    slotTableBody.innerHTML = "";

    slots.forEach((s, i) => {
      const tr = document.createElement("tr");
    const price = Number(s.BasePricePerHour || 0);
    tr.innerHTML = `
    <td><input type="hidden" name="slots[${i}].Code" value="${esc(s.Code)}" />${esc(s.Code)}</td>
    <td><input type="hidden" name="slots[${i}].Row" value="${esc(s.Row)}" />${dash(s.Row)}</td>
    <td><input type="hidden" name="slots[${i}].Col" value="${esc(s.Col)}" />${dash(s.Col)}</td>
    <td><input type="hidden" name="slots[${i}].HeightM" value="${esc(s.HeightM)}" />${dash(s.HeightM)}</td>
    <td><input type="hidden" name="slots[${i}].LengthM" value="${esc(s.LengthM)}" />${dash(s.LengthM)}</td>
    <td><input type="hidden" name="slots[${i}].WidthM" value="${esc(s.WidthM)}" />${dash(s.WidthM)}</td>
    <td><input type="hidden" name="slots[${i}].BasePricePerHour" value="${esc(price)}" />${price.toLocaleString('vi-VN')}</td>
    <td><input type="hidden" name="slots[${i}].Status" value="${esc(s.Status)}" />${esc(s.Status)}</td>
    <td><button type="button" class="btn danger" data-index="${i}" aria-label="Xóa">🗑</button></td>
    `;
    slotTableBody.appendChild(tr);
    });

    // remove handlers
    slotTableBody.querySelectorAll('button[data-index]').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const idx = parseInt(e.currentTarget.getAttribute('data-index'), 10);
            if (!Number.isNaN(idx)) {
                slots.splice(idx, 1);
                renderSlots();
            }
        });
    });
  }

    function clearSlotInputs() {
    const set = (id, v) => { const x = el(id); if (x) x.value = v; };
    set("slotCode", "");
    set("slotRow", ""); set("slotCol", "");
    set("slotH", ""); set("slotL", ""); set("slotW", "");
    set("slotPrice", "");
    set("slotStatus", "0");
  }

    // Khởi tạo marker ban đầu (nếu có sẵn lat/lng trong form)
    const lat0 = parseFloat(document.getElementById("lat")?.value || "0");
    const lng0 = parseFloat(document.getElementById("lng")?.value || "0");
    if (!Number.isNaN(lat0) && !Number.isNaN(lng0) && (lat0 !== 0 || lng0 !== 0)) {
    const p = L.latLng(lat0, lng0);
    map.setView(p, 14);
    setMarker(p, "Vị trí hiện tại");
    fetchReverseGeocode(p.lat, p.lng, /*writeAddressLine*/ false);
  }
});
