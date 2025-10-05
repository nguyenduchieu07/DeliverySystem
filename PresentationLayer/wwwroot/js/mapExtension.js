function initMap()
{
    geocoder = new google.maps.Geocoder();

    const defaultLatLng = { lat: 21.0278, lng: 105.8342 }; // Hà Nội
map = new google.maps.Map(document.getElementById("map"), {
            center: defaultLatLng,
            zoom: 13,
            mapTypeControl: false,
            streetViewControl: false
        });

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
    geocoder.geocode({ location: { lat, lng } }, (results, status) => {
        if (status === "OK" && results && results.length)
        {
            const r = results[0];
            document.getElementById("autocomplete").value = r.formatted_address || "";
            fillAddressFromComponents(r.address_components, r.formatted_address);
        }
    });
}

function setVal(id, val) { document.getElementById(id).value = val || ""; }
</ script >
< script src = "https://maps.googleapis.com/maps/api/js?key=@(Context.RequestServices.GetService<IConfiguration>()?["GoogleMaps: ApiKey"])&libraries=places&callback=initMap" async defer></script>