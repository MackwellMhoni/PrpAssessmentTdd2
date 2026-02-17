using PrpAssessmentTdd2.Models;
using PrpAssessmentTdd2.RepositoryInterfaces;

namespace PrpAssessmentTdd
{
    public class InvoiceSummaryService
    {
        private IInvoiceRepository _invoiceRepository;

		public InvoiceSummaryService(IInvoiceRepository invoiceRepository) 
        {
            _invoiceRepository = invoiceRepository;
        }

		public async Task<decimal> GetTotalInvoiceValueAsync(DateTime start, DateTime end)
        {
           
			var invoices = await _invoiceRepository.GetInvoicesByDateRangeAsync(start, end, false);

			decimal total = 0;
			foreach (var invoice in invoices)
			{
				total += invoice.TotalAmount;
			}
			return total;
		}

		public async Task<decimal> GetTotalSalesByCustomerAsync(DateTime start, DateTime end, string customerName)
		{
			var invoices = await _invoiceRepository.GetInvoicesByDateRangeAsync(start, end, false);

			return invoices
					.Where(i => !string.IsNullOrWhiteSpace(i.Customer.Name) &&
								i.Customer.Name.Equals(customerName, StringComparison.OrdinalIgnoreCase))
					.Sum(i => i.TotalAmount);
		}


		public async Task<string> GetMostSoldProductCodeAsync(DateTime start, DateTime end)
		{
			return null;
		}
	}
}
