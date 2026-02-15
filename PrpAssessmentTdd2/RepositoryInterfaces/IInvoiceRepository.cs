using PrpAssessmentTdd2.Models;

namespace PrpAssessmentTdd2.RepositoryInterfaces
{
    /// <summary>
    /// Repository interface for Invoice persistence operations.
    /// Focused on read operations for now (can be extended later).
    /// </summary>
    public interface IInvoiceRepository
    {
        /// <summary>
        /// Retrieves all invoices issued between the specified date range.
        /// </summary>
        /// <param name="startDate">Inclusive start date (usually IssueDate or DueDate depending on business rule)</param>
        /// <param name="endDate">Inclusive end date</param>
        /// <param name="includeLineItems">
        /// When true → includes LineItems collection (potentially expensive query).
        /// When false → returns invoices without line items loaded (faster, lighter).
        /// </param>
        
        /// <returns>List of invoices (with or without line items depending on parameter)</returns>
        Task<IReadOnlyList<Invoice>> GetInvoicesByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            bool includeLineItems = false);

        /// <summary>
        /// Variant that allows filtering by status as well.
        /// Useful for reports like "all overdue invoices in Q4".
        /// </summary>
        Task<IReadOnlyList<Invoice>> GetInvoicesByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            InvoiceStatus? status,
            bool includeLineItems = false);

        /// <summary>
        /// Retrieves a single invoice by its ID, with optional line items loading.
        /// </summary>
        Task<Invoice?> GetByIdAsync(
            Guid invoiceId,
            bool includeLineItems = true);

    }
}
