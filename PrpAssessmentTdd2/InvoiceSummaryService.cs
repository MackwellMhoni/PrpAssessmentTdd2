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
            return 0;
        }
    }
}
