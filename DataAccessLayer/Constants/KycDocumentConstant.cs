namespace DataAccessLayer.Constants;

public class KycDocumentConstant
{
    public static readonly string LicenseNumberKey = "LicenseNumber";
    public static readonly string TaxNumberKey = "TaxNumber";
    public static readonly string IDKey = "ID";


    public static readonly Dictionary<string, (bool Required, string Description)> Types =
        new()
        {
            // --- Store Information ---
            // { "StoreName", (true, "The display name of the store.") },
            // { "LegalName", (false, "The registered legal name of the business or company.") },
            { LicenseNumberKey, (false, "The business license number, if applicable.") },
            { TaxNumberKey, (false, "The tax identification number (TIN) of the store.") },
            { IDKey, (false, "The ID card of owner.") },

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

        public static readonly List<DocumentType> TypesWithLabelObject = new()
        {
            new() { Key = LicenseNumberKey, Label = "Giấy phép kinh doanh" },
            new() { Key = TaxNumberKey, Label = "Giấy chứng nhận thuế" },
            new() { Key = IDKey, Label = "CMND/CCCD chủ sở hữu" }
        };
        public static readonly Dictionary<string, string> TypesWithLabelDict = new()
        {
            {   LicenseNumberKey,  "Giấy phép kinh doanh" },
            {   TaxNumberKey, "Giấy chứng nhận thuế" },
            {   IDKey,  "CMND/CCCD chủ sở hữu" }
        };
}