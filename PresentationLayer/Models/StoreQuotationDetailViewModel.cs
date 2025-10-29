using DataAccessLayer.Entities;

namespace PresentationLayer.Models;

public class StoreQuotationDetailViewModel
{
    public Quotation Quotation { get; set; }
    public List<Contract> Contracts { get; set; }
}