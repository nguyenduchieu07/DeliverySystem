public class AddressOptionVM
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Full { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}