function initMap()
{
    geocoder = new google.maps.Geocoder();

    const defaultLatLng = { lat: 21.0278, lng: 105.8342 }; // Hà Nội
map = new google.maps.Map(document.getElementById("map"), {
            center: defaultLatLng,
            zoom: 13,
            mapTypeControl: false,
            streetViewControl: false,
            region: 'VN', // Đảm bảo hiển thị quần đảo Hoàng Sa và Trường Sa là của Việt Nam
            language: 'vi' // Ngôn ngữ tiếng Việt
        });

    // Thêm labels cho quần đảo Hoàng Sa và Trường Sa
    addVietnameseIslandLabelsForGoogleMaps(map);

marker = new google.maps.Marker({
            map,
            position: defaultLatLng,
            draggable: true
        });

// Places Autocomplete
autocomplete = new google.maps.places.Autocomplete(
    document.getElementById("autocomplete"),
            {
                fields: ["address_components", "geometry", "formatted_address"],
                componentRestrictions: { country: ["vn"] }
            }
        );

autocomplete.addListener("place_changed", () => {
    const place = autocomplete.getPlace();
    if (!place.geometry || !place.geometry.location) return;

    map.panTo(place.geometry.location);
    map.setZoom(16);
    marker.setPosition(place.geometry.location);

    const lat = place.geometry.location.lat();
    const lng = place.geometry.location.lng();
    updateLatLng(lat, lng);

    fillAddressFromComponents(place.address_components, place.formatted_address);
});

// Kéo thả marker → reverse geocode
marker.addListener("dragend", () => {
    const pos = marker.getPosition();
    const lat = pos.lat(), lng = pos.lng();
    updateLatLng(lat, lng);
    reverseGeocode(lat, lng);
});

// Nếu đã có Lat/Lng (postback)
const latInput = document.getElementById("Latitude");
const lngInput = document.getElementById("Longitude");
if (latInput.value && lngInput.value)
{
    const p = { lat: parseFloat(latInput.value), lng: parseFloat(lngInput.value) };
map.setCenter(p);
map.setZoom(16);
marker.setPosition(p);
        }
    }

    function updateLatLng(lat, lng)
{
    document.getElementById("Latitude").value = lat.toFixed(6);
    document.getElementById("Longitude").value = lng.toFixed(6);
}

function fillAddressFromComponents(components, formatted)
{
    setVal("AddressLine", formatted || "");
    setVal("Ward", "");
    setVal("District", "");
    setVal("City", "");

    let city = "", district = "", ward = "", route = "", streetNumber = "";

    components.forEach(c => {
        if (c.types.includes("administrative_area_level_1")) city = c.long_name;
        if (c.types.includes("administrative_area_level_2")) district = c.long_name;
        if (c.types.includes("administrative_area_level_3")) ward = c.long_name;
        if (c.types.includes("sublocality_level_1") && !ward) ward = c.long_name;
        if (c.types.includes("route")) route = c.long_name;
        if (c.types.includes("street_number")) streetNumber = c.long_name;
    });

    if (route || streetNumber)
    {
        setVal("AddressLine", `${ streetNumber? streetNumber +" " : ""}${ route}`.trim());
        }
        setVal("City", city);
setVal("District", district);
setVal("Ward", ward);
    }

    function reverseGeocode(lat, lng)
{
    geocoder.geocode({ 
        location: { lat, lng },
        region: 'VN', // Đảm bảo kết quả geocode theo region Việt Nam
        language: 'vi' // Ngôn ngữ tiếng Việt
    }, (results, status) => {
        if (status === "OK" && results && results.length)
        {
            const r = results[0];
            document.getElementById("autocomplete").value = r.formatted_address || "";
            fillAddressFromComponents(r.address_components, r.formatted_address);
        }
    });
}

function setVal(id, val) { document.getElementById(id).value = val || ""; }

// Thêm labels tiếng Việt cho quần đảo Hoàng Sa và Trường Sa (Google Maps)
function addVietnameseIslandLabelsForGoogleMaps(mapInstance) {
    if (!mapInstance || typeof google === "undefined" || !google.maps) return;
    
    // Quần đảo Hoàng Sa (Paracel Islands) - khoảng 16.5°N, 112.0°E
    const hoangSaMarker = new google.maps.Marker({
        position: { lat: 16.5, lng: 112.0 },
        map: mapInstance,
        icon: {
            path: google.maps.SymbolPath.CIRCLE,
            scale: 0, // Ẩn marker icon
            fillOpacity: 0,
            strokeOpacity: 0
        },
        label: {
            text: '🏝️ Quần đảo Hoàng Sa',
            className: 'vietnamese-island-label-google',
            color: '#d32f2f',
            fontSize: '14px',
            fontWeight: 'bold'
        },
        zIndex: 10000
    });
    
    // Quần đảo Trường Sa (Spratly Islands) - khoảng 10.0°N, 114.0°E
    const truongSaMarker = new google.maps.Marker({
        position: { lat: 10.0, lng: 114.0 },
        map: mapInstance,
        icon: {
            path: google.maps.SymbolPath.CIRCLE,
            scale: 0, // Ẩn marker icon
            fillOpacity: 0,
            strokeOpacity: 0
        },
        label: {
            text: '🏝️ Quần đảo Trường Sa',
            className: 'vietnamese-island-label-google',
            color: '#d32f2f',
            fontSize: '14px',
            fontWeight: 'bold'
        },
        zIndex: 10000
    });
    
    // Thêm CSS để style labels
    if (!document.getElementById('vietnamese-island-styles-google')) {
        const style = document.createElement('style');
        style.id = 'vietnamese-island-styles-google';
        style.textContent = `
            .vietnamese-island-label-google {
                background: rgba(255,255,255,0.98) !important;
                padding: 8px 12px !important;
                border-radius: 6px !important;
                border: 3px solid #d32f2f !important;
                box-shadow: 0 4px 8px rgba(0,0,0,0.4) !important;
                white-space: nowrap !important;
                z-index: 10000 !important;
            }
        `;
        document.head.appendChild(style);
    }
}