// ==========================
// Quote Page – Front Script
// ==========================

// Configuration
const CONFIG = {
    VAT_RATE: 0.1,
    FREE_DAYS: 2,
    VALIDITY_HOURS: 48,
    DEFAULT_CENTER: [10.8231, 106.6297],
    DEFAULT_ZOOM: 12,
    API_BASE: '' // Empty for same domain
};

// State
let map;
let userMarker;
let warehouseMarkers = [];
let selectedWarehouse = null;
let selectedSlots = [];
let userLocation = null;
let currentQuotationId = null;

// Cache
const slotsCache = new Map();

// Helpers
async function fetchJson(url, options = {}) {
    const res = await fetch(CONFIG.API_BASE + url, options);
    if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
    return res.json();
}

function esc(s) {
    return String(s ?? '').replace(/[&<>"']/g, m =>
        ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m])
    );
}

function formatCurrency(amount) {
    const n = Number(amount ?? 0);
    return new Intl.NumberFormat('vi-VN').format(Math.round(n)) + ' đ';
}

function calculateDaysExclusive(start, end) {
    const s = new Date(start);
    const e = new Date(end);
    if (isNaN(s) || isNaN(e)) return 0;
    const diff = e - s; // ms
    if (diff <= 0) return 0;
    return Math.ceil(diff / (1000 * 60 * 60 * 24));
}

// DOM Ready
document.addEventListener('DOMContentLoaded', function () {
    initMap();
    initializeFormListeners();
    loadNearbyWarehouses();
});

// Map
function initMap() {
    map = L.map('map').setView(CONFIG.DEFAULT_CENTER, CONFIG.DEFAULT_ZOOM);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
        maxZoom: 19
    }).addTo(map);
}

// Load nearby warehouses or search
async function loadNearbyWarehouses(lat = null, lng = null) {
    try {
        let url = '/api/warehouses';
        if (lat != null && lng != null) {
            url = `/api/warehouses/nearby?lat=${lat}&lng=${lng}&radiusKm=50`;
        }
        const warehouses = await fetchJson(url);
        displayWarehousesOnMap(warehouses);
        renderWarehouseList(warehouses);
    } catch (error) {
        console.error('Error loading warehouses:', error);
        alert('Không thể tải danh sách kho!');
    }
}

function displayWarehousesOnMap(warehouses) {
    warehouseMarkers.forEach(wm => map.removeLayer(wm.marker));
    warehouseMarkers = [];

    const bounds = [];
    warehouses.forEach(warehouse => {
        if (warehouse.lat == null || warehouse.lng == null) return;

        const marker = L.marker([warehouse.lat, warehouse.lng], {
            icon: L.divIcon({
                className: 'custom-marker',
                html: `<div style="background:#667eea;color:#fff;padding:8px 12px;border-radius:20px;font-weight:700;box-shadow:0 2px 10px rgba(0,0,0,.3);white-space:nowrap;">
                        🏪 ${esc(warehouse.name)}
                       </div>`,
                iconSize: [150, 40],
                iconAnchor: [75, 40]
            })
        });

        marker.bindPopup(getWarehousePopupEl(warehouse));
        marker.on('click', () => selectWarehouse(warehouse));
        marker.addTo(map);

        warehouseMarkers.push({ warehouse, marker });
        bounds.push([warehouse.lat, warehouse.lng]);
    });

    if (bounds.length) {
        try { map.fitBounds(bounds, { padding: [30, 30] }); } catch { }
    }
}

function getWarehousePopupEl(warehouse) {
    const distanceHtml = warehouse.distanceKm
        ? `<div style="color:#666;margin:5px 0;">📍 ${Number(warehouse.distanceKm).toFixed(1)} km</div>`
        : '';

    const html = `
        <div style="padding:10px;min-width:200px;">
            <div style="font-weight:700;font-size:1.1rem;color:#667eea;margin-bottom:5px;">${esc(warehouse.name)}</div>
            <div style="color:#666;margin-bottom:10px;">${esc(warehouse.address || warehouse.city || '')}</div>
            ${distanceHtml}
            <div style="display:flex;justify-content:space-between;margin-bottom:10px;">
                <span style="color:#27ae60;font-weight:600;">${formatCurrency(warehouse.minPricePerHour || 5000)}/h</span>
                <span style="color:#666;">${warehouse.availableSlots}/${warehouse.totalSlots} trống</span>
            </div>
            <button id="popupSelectBtn" style="width:100%;padding:10px;background:#667eea;color:#fff;border:none;border-radius:8px;cursor:pointer;font-weight:600;">✅ Chọn kho này</button>
        </div>
    `;
    const container = document.createElement('div');
    container.innerHTML = html;
    setTimeout(() => {
        const btn = container.querySelector('#popupSelectBtn');
        if (btn) btn.addEventListener('click', () => selectWarehouse(warehouse));
    }, 0);
    return container;
}

// Render list
function renderWarehouseList(warehouses) {
    const list = document.getElementById('warehouseList');
    list.innerHTML = '';

    if (!warehouses || warehouses.length === 0) {
        list.innerHTML = '<div style="padding: 20px; text-align: center; color: #999;">Không tìm thấy kho nào</div>';
        return;
    }

    warehouses.forEach(warehouse => {
        const item = document.createElement('div');
        item.className = 'warehouse-item';
        if (selectedWarehouse && selectedWarehouse.id === warehouse.id) {
            item.classList.add('selected');
        }

        const distanceHtml = warehouse.distanceKm
            ? `<span class="warehouse-distance">📍 ${Number(warehouse.distanceKm).toFixed(1)} km</span>`
            : '';

        item.innerHTML = `
            <div class="warehouse-name">${esc(warehouse.name)}</div>
            <div class="warehouse-address">${esc(warehouse.address || warehouse.city || '')}</div>
            <div class="warehouse-info">
                <div>
                    <div style="font-weight:700;color:#27ae60;">${formatCurrency(warehouse.minPricePerHour || 5000)}/h</div>
                    <div style="font-size:0.85rem;color:#666;">${warehouse.availableSlots}/${warehouse.totalSlots} slot trống</div>
                </div>
                ${distanceHtml}
            </div>
        `;

        item.onclick = () => {
            selectWarehouse(warehouse);
            if (warehouse.lat != null && warehouse.lng != null) {
                map.setView([warehouse.lat, warehouse.lng], 14);
            }
        };
        list.appendChild(item);
    });
}

// Search location
function searchLocation() {
    const query = document.getElementById('locationInput').value.trim();
    if (!query) return alert('Vui lòng nhập địa chỉ!');

    if (!L.Control || !L.Control.Geocoder) {
        alert('Không có plugin geocoder. Vui lòng tải lại trang.');
        return;
    }
    const geocoder = L.Control.Geocoder.nominatim();
    geocoder.geocode(query, (results) => {
        if (results && results.length > 0) {
            const result = results[0];
            setUserLocation(result.center.lat, result.center.lng);
        } else {
            alert('Không tìm thấy địa chỉ!');
        }
    });
}

// Current location
function getCurrentLocation() {
    if (!navigator.geolocation) return alert('Trình duyệt không hỗ trợ Geolocation!');
    navigator.geolocation.getCurrentPosition(
        (position) => setUserLocation(position.coords.latitude, position.coords.longitude),
        (error) => { alert('Không thể lấy vị trí hiện tại!'); console.error(error); }
    );
}

function setUserLocation(lat, lng) {
    userLocation = { lat, lng };
    if (userMarker) map.removeLayer(userMarker);

    userMarker = L.marker([lat, lng], {
        icon: L.divIcon({
            className: 'user-marker',
            html: `<div style="background:#e74c3c;color:#fff;padding:8px 12px;border-radius:20px;font-weight:bold;box-shadow:0 2px 10px rgba(0,0,0,.3);">📍 Vị trí của bạn</div>`,
            iconSize: [120, 40],
            iconAnchor: [60, 40]
        })
    }).addTo(map);

    map.setView([lat, lng], 13);
    loadNearbyWarehouses(lat, lng);
}

// Select warehouse
function selectWarehouse(warehouse) {
    selectedWarehouse = warehouse;
    document.getElementById('warehouseIdInput').value = warehouse.id;
    document.getElementById('warehouseNameDisplay').value = warehouse.name;
    document.getElementById('estWarehouse').textContent = warehouse.name;
    if (warehouse.distanceKm) {
        document.getElementById('estDistance').textContent = Number(warehouse.distanceKm).toFixed(1) + ' km';
    } else {
        document.getElementById('estDistance').textContent = '--';
    }
    renderWarehouseList(warehouseMarkers.map(wm => wm.warehouse));
}

// Show warehouse slots (with cache)
async function showWarehouseSlots() {
    if (!selectedWarehouse) return alert('⚠️ Vui lòng chọn kho trước!');
    const wid = selectedWarehouse.id;

    let slots = slotsCache.get(wid);
    try {
        if (!slots) {
            document.getElementById('warehouseGrid').innerHTML = '<div style="grid-column:1/-1;padding:12px;">Đang tải sơ đồ kho…</div>';
            slots = await fetchJson(`/api/warehouses/${wid}/slots`);
            slotsCache.set(wid, slots);
        }
        renderWarehouseGrid(slots);
        document.getElementById('warehouseSlotSection').classList.add('show');
        document.getElementById('warehouseSlotSection').scrollIntoView({ behavior: 'smooth' });
    } catch (error) {
        console.error('Error loading slots:', error);
        alert('Không thể tải sơ đồ kho!');
    }
}

// Render grid
function renderWarehouseGrid(slots) {
    const grid = document.getElementById('warehouseGrid');
    if (!slots || !slots.length) {
        grid.style.gridTemplateColumns = 'repeat(1, 1fr)';
        grid.innerHTML = '<div>Kho chưa cấu hình ô lưu.</div>';
        return;
    }
    const maxRow = Math.max(...slots.map(s => s.row));
    const maxCol = Math.max(...slots.map(s => s.col));

    grid.style.gridTemplateColumns = `repeat(${maxCol}, 1fr)`;
    grid.innerHTML = '';

    for (let row = 1; row <= maxRow; row++) {
        for (let col = 1; col <= maxCol; col++) {
            const slot = slots.find(s => s.row === row && s.col === col);
            if (!slot) {
                const emptyDiv = document.createElement('div');
                grid.appendChild(emptyDiv);
                continue;
            }
            const slotDiv = document.createElement('div');
            slotDiv.className = `warehouse-slot ${slot.status}`;
            if (selectedSlots.some(s => s.id === slot.id)) slotDiv.classList.add('selected');

            slotDiv.innerHTML = `
                <div>${esc(slot.code)}</div>
                <div style="font-size:0.7rem;">${esc(slot.size)}</div>
                <div style="font-size:0.65rem;">${formatCurrency(slot.basePricePerHour)}/h</div>
            `;
            slotDiv.onclick = () => toggleSlotSelection(slot);
            grid.appendChild(slotDiv);
        }
    }
    updateEstimation();
}

// Toggle slot selection (no refetch)
function toggleSlotSelection(slot) {
    if (slot.status !== 'available') return alert('Slot này không khả dụng!');
    const i = selectedSlots.findIndex(s => s.id === slot.id);
    if (i >= 0) selectedSlots.splice(i, 1);
    else selectedSlots.push(slot);

    const slots = slotsCache.get(selectedWarehouse.id) || [];
    renderWarehouseGrid(slots);
    updateSelectedSlotsSummary();
}

function updateSelectedSlotsSummary() {
    const summary = document.getElementById('selectedSlotsSummary');
    if (selectedSlots.length === 0) {
        summary.innerHTML = '';
        return;
    }
    let html = '<h4 style="color:#856404;">📦 Slot đã chọn:</h4>';
    selectedSlots.forEach(slot => {
        html += `
            <div style="background:white;padding:10px;border-radius:5px;margin:5px 0;display:flex;justify-content:space-between;">
                <span><strong>${esc(slot.code)}</strong> - ${esc(slot.size)} (${Number(slot.volumeM3)}m³)</span>
                <button onclick="removeSlot('${slot.id}')" style="background:#dc3545;color:white;border:none;padding:5px 10px;border-radius:5px;cursor:pointer;">✕</button>
            </div>`;
    });
    summary.innerHTML = html;
}

function removeSlot(slotId) {
    selectedSlots = selectedSlots.filter(s => s.id !== slotId);
    const slots = slotsCache.get(selectedWarehouse.id) || [];
    renderWarehouseGrid(slots);
    updateSelectedSlotsSummary();
}

function toggleSlotSection() {
    document.getElementById('warehouseSlotSection').classList.toggle('show');
}

// Estimation
function updateEstimation() {
    const startDate = document.getElementById('startDate').value;
    const endDate = document.getElementById('endDate').value;

    const totalDays = calculateDaysExclusive(startDate, endDate);
    if (totalDays <= 0) {
        document.getElementById('estDays').textContent = '0 ngày';
        document.getElementById('estSlotCount').textContent = '0';
        document.getElementById('estVolume').textContent = '0 m³';
        document.getElementById('estSlotFee').textContent = formatCurrency(0);
        document.getElementById('estAddons').textContent = formatCurrency(0);
        document.getElementById('estSubtotal').textContent = formatCurrency(0);
        document.getElementById('estVAT').textContent = formatCurrency(0);
        document.getElementById('estTotal').textContent = formatCurrency(0);
        return;
    }

    const chargeableDays = Math.max(0, totalDays - CONFIG.FREE_DAYS);
    const hours = chargeableDays * 24;

    let totalSlotFee = 0;
    let totalVolume = 0;
    selectedSlots.forEach(slot => {
        totalSlotFee += Number(slot.basePricePerHour) * hours;
        totalVolume += Number(slot.volumeM3);
    });

    let addonsTotal = 0;
    document.querySelectorAll('.checkbox-item input[type="checkbox"]').forEach(checkbox => {
        if (checkbox.checked) {
            const addonValue = Number(checkbox.value);
            if (checkbox.id === 'coldStorage') {
                addonsTotal += addonValue * totalVolume * chargeableDays;
            } else {
                addonsTotal += addonValue;
            }
        }
    });

    const subtotal = totalSlotFee + addonsTotal;
    const vat = subtotal * CONFIG.VAT_RATE;
    const total = subtotal + vat;

    document.getElementById('estSlotCount').textContent = selectedSlots.length;
    document.getElementById('estVolume').textContent = totalVolume.toFixed(1) + ' m³';
    document.getElementById('estDays').textContent = `${totalDays} ngày`;
    document.getElementById('estSlotFee').textContent = formatCurrency(totalSlotFee);
    document.getElementById('estAddons').textContent = formatCurrency(addonsTotal);
    document.getElementById('estSubtotal').textContent = formatCurrency(subtotal);
    document.getElementById('estVAT').textContent = formatCurrency(vat);
    document.getElementById('estTotal').textContent = formatCurrency(total);
}

// Form listeners
function initializeFormListeners() {
    ['startDate', 'endDate'].forEach(id => {
        const el = document.getElementById(id);
        if (el) el.addEventListener('change', updateEstimation);
    });

    document.querySelectorAll('.checkbox-item input[type="checkbox"]').forEach(checkbox => {
        checkbox.addEventListener('change', updateEstimation);
    });

    const form = document.getElementById('quoteForm');
    if (form) form.addEventListener('submit', handleFormSubmit);
}

// Submit form
async function handleFormSubmit(e) {
    e.preventDefault();

    if (!selectedWarehouse) return alert('⚠️ Vui lòng chọn kho!');
    if (selectedSlots.length === 0) return alert('⚠️ Vui lòng chọn ít nhất 1 slot!');

    const name = document.getElementById('customerName').value.trim();
    const phone = document.getElementById('customerPhone').value.trim();
    if (!name || !phone) return alert('Vui lòng nhập họ tên và SĐT!');

    const addons = [];
    document.querySelectorAll('.checkbox-item input[type="checkbox"]').forEach(checkbox => {
        if (checkbox.checked) {
            addons.push({
                id: checkbox.id,
                name: (checkbox.nextElementSibling?.textContent || checkbox.id).split('+')[0].trim(),
                value: Number(checkbox.value),
                isPerM3: checkbox.id === 'coldStorage'
            });
        }
    });

    const payload = {
        storeId: selectedWarehouse.storeId,
        warehouseId: selectedWarehouse.id,
        startDate: document.getElementById('startDate').value,
        endDate: document.getElementById('endDate').value,
        slotIds: selectedSlots.map(s => s.id),
        addons,
        customerName: name,
        customerPhone: phone
    };
    console.log(payload);
    const submitBtn = e.submitter || document.querySelector('#quoteForm button[type=submit]');
    if (submitBtn) { submitBtn.disabled = true; submitBtn.textContent = 'Đang tính…'; }

    try {
        const result = await fetchJson('/Quote/Calculate', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });
        currentQuotationId = result.quotationId;
        showBreakdownPage(result);
    } catch (err) {
        console.error(err);
        alert('Không thể tính giá! Vui lòng thử lại.');
    } finally {
        if (submitBtn) { submitBtn.disabled = false; submitBtn.textContent = '📊 Nhận báo giá chi tiết'; }
    }
}

// Breakdown page
function showBreakdownPage(result) {
    const breakdownCard = document.querySelector('.breakdown-card');

    let slotsHtml = '';
    (result.slots || []).forEach(slot => {
        slotsHtml += `
            <div class="breakdown-item">
                <span>Slot ${esc(slot.code)} (${esc(slot.size)} - ${Number(slot.volumeM3)}m³):</span>
                <span>${formatCurrency(slot.totalFee)}</span>
            </div>`;
    });

    let addonsHtml = '';
    (result.addons || []).forEach(addon => {
        addonsHtml += `
            <div class="breakdown-item">
                <span>${esc(addon.name)}:</span>
                <span>${formatCurrency(addon.price)}</span>
            </div>`;
    });

    const validUntil = result.validUntil
        ? new Date(result.validUntil)
        : new Date(Date.now() + CONFIG.VALIDITY_HOURS * 60 * 60 * 1000);

    breakdownCard.innerHTML = `
        <div class="breakdown-header">
            <h2>📄 Báo giá chi tiết</h2>
            <p>Mã báo giá: <strong>${esc(result.quoteCode || '')}</strong></p>
            <p>Ngày tạo: <strong>${result.createdAt ? new Date(result.createdAt).toLocaleString('vi-VN') : new Date().toLocaleString('vi-VN')}</strong></p>
        </div>

        <div class="breakdown-section">
            <h3>👤 Thông tin khách hàng</h3>
            <div class="breakdown-item"><span>Họ tên:</span><span>${esc(result.customer?.name || '')}</span></div>
            <div class="breakdown-item"><span>Số điện thoại:</span><span>${esc(result.customer?.phone || '')}</span></div>
        </div>

        <div class="breakdown-section">
            <h3>📦 Thông tin lưu kho</h3>
            <div class="breakdown-item"><span>Kho:</span><span>${esc(result.warehouse?.name || '')}</span></div>
            ${result.distanceKm ? `<div class="breakdown-item"><span>Khoảng cách:</span><span>${Number(result.distanceKm).toFixed(1)} km</span></div>` : ''}
            <div class="breakdown-item"><span>Số slot:</span><span>${(result.slots || []).length} slot</span></div>
            ${slotsHtml}
            <div class="breakdown-item"><span>Thời gian:</span><span>${Number(result.totalDays)} ngày (tính phí ${Number(result.chargeableDays)} ngày)</span></div>
        </div>

        <div class="breakdown-section">
            <h3>💵 Chi tiết tính giá</h3>
            <div class="breakdown-item"><span>Phí slot:</span><span>${formatCurrency(result.totalSlotFee)}</span></div>
            ${addonsHtml}
            <div class="breakdown-item" style="background:#f8f9fa;padding:15px;border-radius:8px;">
                <strong>Tạm tính (chưa VAT):</strong><strong>${formatCurrency(result.subtotal)}</strong>
            </div>
            <div class="breakdown-item"><span>VAT (10%):</span><span>${formatCurrency(result.vatAmount)}</span></div>
        </div>

        <div class="breakdown-total">
            <div style="display:flex;justify-content:space-between;font-size:1.8rem;font-weight:700;">
                <span>THÀNH TIỀN:</span>
                <span>${formatCurrency(result.total)}</span>
            </div>
        </div>

        <div style="text-align:center;color:#e74c3c;font-weight:600;margin:20px 0;padding:15px;background:#fee;border-radius:10px;">
            ⏰ Báo giá có hiệu lực đến: <strong>${validUntil.toLocaleString('vi-VN')}</strong>
        </div>

        <div class="action-buttons">
            <button class="action-btn secondary" onclick="handleTempReservation()">🕐 Giữ chỗ tạm (2h)</button>
            <button class="action-btn primary" onclick="handleAcceptQuote()">✅ Chấp nhận báo giá</button>
            <button class="action-btn outline" onclick="showNegotiateModal()">💬 Yêu cầu chỉnh giá</button>
            <button class="action-btn outline" onclick="showFeedbackModal()">💬 Viết đánh giá</button>
        </div>

        <button class="action-btn outline" onclick="backToForm()" style="margin-top:20px;width:100%;">← Quay lại chỉnh sửa</button>
    `;

    document.getElementById('quotePage').style.display = 'none';
    document.getElementById('breakdownPage').classList.add('active');
    window.scrollTo(0, 0);
}

function backToForm() {
    document.getElementById('quotePage').style.display = 'block';
    document.getElementById('breakdownPage').classList.remove('active');
    window.scrollTo(0, 0);
}

// Reservations & Accept
async function handleTempReservation() {
    if (!currentQuotationId) return;

    try {
        const ok = await fetch(CONFIG.API_BASE + '/Quote/HoldTemp', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                quotationId: currentQuotationId,
                slotIds: selectedSlots.map(s => s.id),
                holdMinutes: 120
            })
        });
        if (ok.ok) alert('✅ Đã giữ chỗ tạm thời trong 2 giờ!');
        else alert('❌ Không thể giữ chỗ!');
    } catch (error) {
        console.error('Error:', error);
        alert('Có lỗi xảy ra!');
    }
}

async function handleAcceptQuote() {
    if (!currentQuotationId) return;
    if (!confirm('Bạn xác nhận chấp nhận báo giá này?')) return;

    try {
        const ok = await fetch(CONFIG.API_BASE + '/Quote/Accept', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ quotationId: currentQuotationId })
        });
        if (ok.ok) alert('✅ Chấp nhận báo giá thành công!\n\nChúng tôi sẽ liên hệ với bạn sớm nhất.');
        else alert('❌ Không thể chấp nhận báo giá!');
    } catch (error) {
        console.error('Error:', error);
        alert('Có lỗi xảy ra!');
    }
}

//Write feedback
function showFeedbackModal() {
    document.getElementById('writeFeedbackModal').classList.add('active');
}
function closeFeedbackModal() {
    document.getElementById('writeFeedbackModal').classList.remove('active');
    document.getElementById('writeFeedbackNote').value = '';
}

// Negotiate
function showNegotiateModal() {
    document.getElementById('negotiateModal').classList.add('active');
}
function closeNegotiateModal() {
    document.getElementById('negotiateModal').classList.remove('active');
    document.getElementById('negotiateNote').value = '';
}
async function submitNegotiate() {
    const note = document.getElementById('negotiateNote').value.trim();
    if (!note) return alert('Vui lòng nhập ghi chú!');

    try {
        const ok = await fetch(CONFIG.API_BASE + '/Quote/RequestRevision', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ quotationId: currentQuotationId, note })
        });
        if (ok.ok) { alert('✅ Đã gửi yêu cầu chỉnh giá!'); closeNegotiateModal(); }
        else alert('❌ Không thể gửi yêu cầu!');
    } catch (error) {
        console.error('Error:', error);
        alert('Có lỗi xảy ra!');
    }
}

// Modal
function closeSlotDetailModal() {
    document.getElementById('slotDetailModal').classList.remove('active');
}
// ---- CONFIG (có thể reuse CONFIG hiện có) ----
const EST_CFG = {
    VAT_RATE: 0.10,     // 10%
    FREE_DAYS: 2        // miễn phí 2 ngày đầu
};

// ---- STATE TỐI THIỂU (nếu đã có thì dùng state sẵn có) ----
window.selectedSlots = window.selectedSlots || [];     // mỗi slot: { basePricePerHour, volumeM3, ... }
window.selectedWarehouse = window.selectedWarehouse || null;

// ---- HELPERS ----
function toNumber(v) { return Number(v || 0); }
function vnd(n) { return new Intl.NumberFormat('vi-VN').format(Math.round(toNumber(n))) + ' đ'; }
function daysExclusive(start, end) {
    const s = new Date(start), e = new Date(end);
    if (isNaN(s) || isNaN(e)) return 0;
    const diff = e - s;
    if (diff <= 0) return 0;
    return Math.ceil(diff / (1000 * 60 * 60 * 24));
}

// ---- CORE: TÍNH TOÁN + CẬP NHẬT UI ----
function recalcEstimation() {
    const startDate = document.getElementById('startDate')?.value;
    const endDate = document.getElementById('endDate')?.value;

    const totalDays = daysExclusive(startDate, endDate);
    const chargeableDays = Math.max(0, totalDays - EST_CFG.FREE_DAYS);
    const hours = chargeableDays * 24;

    // Tính phí slot & tổng m3
    let slotFee = 0;
    let totalM3 = 0;
    for (const s of selectedSlots) {
        slotFee += toNumber(s.basePricePerHour) * hours;
        totalM3 += toNumber(s.volumeM3);
    }

    // Tính addons
    let addons = 0;
    document.querySelectorAll('.checkbox-item input[type="checkbox"]').forEach(cb => {
        if (!cb.checked) return;
        const val = toNumber(cb.value);
        if (cb.id === 'coldStorage') {
            // tính theo m³/ngày
            addons += val * totalM3 * chargeableDays;
        } else {
            // phí một lần
            addons += val;
        }
    });

    const subtotal = slotFee + addons;
    const vat = subtotal * EST_CFG.VAT_RATE;
    const total = subtotal + vat;

    // ---- ĐỔ VÀO CARD ----
    document.getElementById('estWarehouse').textContent = selectedWarehouse?.name || 'Chưa chọn';
    document.getElementById('estDistance').textContent = (selectedWarehouse?.distanceKm != null)
        ? `${Number(selectedWarehouse.distanceKm).toFixed(1)} km` : '--';
    document.getElementById('estSlotCount').textContent = selectedSlots.length;
    document.getElementById('estVolume').textContent = `${totalM3.toFixed(1)} m³`;
    document.getElementById('estDays').textContent = `${totalDays} ngày`;
    document.getElementById('estSlotFee').textContent = vnd(slotFee);
    document.getElementById('estAddons').textContent = vnd(addons);
    document.getElementById('estSubtotal').textContent = vnd(subtotal);
    document.getElementById('estVAT').textContent = vnd(vat);
    document.getElementById('estTotal').textContent = vnd(total);
}

// ---- GẮN SỰ KIỆN TỰ TÍNH ----
function initEstimationAutoCalc() {
    // đổi ngày → tính lại
    ['startDate', 'endDate'].forEach(id => {
        const el = document.getElementById(id);
        if (el) el.addEventListener('change', recalcEstimation);
    });
    // tick addon → tính lại
    document.querySelectorAll('.checkbox-item input[type="checkbox"]').forEach(cb => {
        cb.addEventListener('change', recalcEstimation);
    });

    // Nếu app của bạn có các điểm sau, nhớ gọi recalcEstimation():
    // - sau khi chọn kho:
    //     selectedWarehouse = warehouse; recalcEstimation();
    // - sau khi chọn/bỏ slot:
    //     selectedSlots.push(...) / splice(...); recalcEstimation();

    recalcEstimation(); // tính lần đầu
}

// ---- Gọi khi DOM sẵn sàng ----
document.addEventListener('DOMContentLoaded', initEstimationAutoCalc);

// ---- Ví dụ: hook vào chỗ chọn kho/slot có sẵn của bạn ----
// function selectWarehouse(wh) { selectedWarehouse = wh; /* ... UI ... */ recalcEstimation(); }
// function toggleSlotSelection(slot) { /* thêm/bớt vào selectedSlots */ recalcEstimation(); }
// function removeSlot(id) { /* lọc selectedSlots */ recalcEstimation(); }
