namespace DataAccessLayer.Constants;

public class KycDocumentConstant
{
    public static readonly Dictionary<string, (bool Required, string Description)> Types =
        new()
        {
            // --- Store Information ---
            // { "StoreName", (true, "The display name of the store.") },
            // { "LegalName", (false, "The registered legal name of the business or company.") },
            { "LicenseNumber", (false, "The business license number, if applicable.") },
            { "TaxNumber", (false, "The tax identification number (TIN) of the store.") },
            { "ID", (false, "The ID card of owner.") },

            // --- Address & Location ---
            // { "AddressLine", (true, "Street address of the store (house number and street name).") },
            // { "Ward", (false, "Ward or commune where the store is located.") },
            // { "District", (false, "District or county where the store is located.") },
            // { "City", (true, "City or province where the store is located.") },
            // { "Latitude", (true, "Latitude coordinate selected from the map.") },
            // { "Longitude", (true, "Longitude coordinate selected from the map.") }
        };

        public class DocumentType
        {
            public string Key { get; set; } = "";
            public string Label { get; set; } = "";
        }

        public static readonly List<DocumentType> TypesWithLabel = new()
        {
            new() { Key = "LicenseNumber", Label = "Giấy phép kinh doanh" },
            new() { Key = "TaxNumber", Label = "Giấy chứng nhận thuế" },
            new() { Key = "ID", Label = "CMND/CCCD chủ sở hữu" }
        };
}