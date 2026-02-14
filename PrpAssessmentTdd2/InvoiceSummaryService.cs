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
    }
}
