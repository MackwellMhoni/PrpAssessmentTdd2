using PrpAssessmentTdd2.Models;
using PrpAssessmentTdd2.RepositoryInterfaces;
using System.Collections.Generic;

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
			var invoices = await _invoiceRepository.GetInvoicesByDateRangeAsync(start, end, false);


			if (invoices.Count == 0 || invoices == null)
			{
				return null;
			}

			var aggregate = new Dictionary<string, decimal>();
			foreach (var invoice in invoices)
			{
				foreach (var lineItem in invoice.LineItems)
				{
					decimal subTotal = lineItem.Quantity * lineItem.UnitPrice;

					if (aggregate.ContainsKey(lineItem.ProductCode))
					{
						aggregate[lineItem.ProductCode] += subTotal; //ads to exii sting value with same code
					}
					else
					{
						aggregate[lineItem.ProductCode] = subTotal; // adds new code and value
					}
				}
			}
			return aggregate.OrderByDescending(i => i.Value).FirstOrDefault().Key;
		}

		public async Task<List<string>> GetHighRiskCustomersAsync(DateTime start, DateTime end)
		{
			var invoices = await _invoiceRepository.GetInvoicesByDateRangeAsync(start, end, false);

			var customer_Names = new List<string>();
			foreach (var inv in invoices)
			{
				customer_Names.Add(inv.Customer.Name);
			}
			customer_Names.Sort();
			return customer_Names;
		}
	}
}
