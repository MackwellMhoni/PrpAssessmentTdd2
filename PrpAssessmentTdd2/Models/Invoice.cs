namespace PrpAssessmentTdd2.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
        public Party Supplier { get; set; } = new();
        public Party Customer { get; set; } = new();
        public List<InvoiceLineItem> LineItems { get; set; } = new();        
        public string? Reference { get; set; }                    
        public string? Notes { get; set; }
        public string? PaymentTerms { get; set; }                 
        public DateTime? PaidDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }        
        public decimal AmountDue => TotalAmount - (PaidDate.HasValue ? TotalAmount : 0);
    }

    public enum InvoiceStatus
    {
        Draft,
        Issued,
        Paid,
        Overdue,
        Cancelled
    }

    public class Party
    {
        public string Name { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string? VatNumber { get; set; }          // Tax/VAT/Company registration
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public Address Address { get; set; } = new();
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string? Suburb { get; set; }
        public string City { get; set; } = string.Empty;
        public string? Province { get; set; }           // Gauteng, Western Cape, etc.
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = "South Africa";
    }

    public class InvoiceLineItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int LineNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ProductCode { get; set; }
        public decimal Quantity { get; set; } = 1m;
        public decimal UnitPrice { get; set; }
        public decimal VatPercentage { get; set; }
        public decimal Subtotal => Quantity * UnitPrice;
        public decimal VatAmount => Subtotal * (VatPercentage / 100m);
        public decimal Total => Subtotal + VatAmount;
    }

}
