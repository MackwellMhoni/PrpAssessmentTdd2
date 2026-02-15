using Castle.Core.Resource;
using Moq;
using PrpAssessmentTdd2.Models;
using PrpAssessmentTdd2.RepositoryInterfaces;

namespace PrpAssessmentTdd.Tests
{
    public class InvoiceSummaryServiceTests
    {
        private readonly Mock<IInvoiceRepository> _repositoryMock;
        private readonly InvoiceSummaryService _service;

        public InvoiceSummaryServiceTests()
        {
            _repositoryMock = new Mock<IInvoiceRepository>();
            _service = new InvoiceSummaryService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetTotalInvoiceValueAsync_ReturnsZero_WhenNoInvoicesInRange()
        {
            // Arrange
            var start = new DateTime(2025, 4, 1);
            var end = new DateTime(2025, 4, 30);

            _repositoryMock
                .Setup(r => r.GetInvoicesByDateRangeAsync(
                    start, end, false))
                .ReturnsAsync(new List<Invoice>());

            // Act
            var total = await _service.GetTotalInvoiceValueAsync(start, end);

            // Assert
            Assert.Equal(0m, total);
        }

		[Fact]
		public async Task GetTotalInvoiceValueAsync_ReturnsValueOf_2_Invoices()
		{
			// Arrange
			var start = new DateTime(2025, 4, 1);
			var end = new DateTime(2025, 4, 30);

            var allInvoices = new List<Invoice>()
            {
                new Invoice { TotalAmount = 200m },
                new Invoice { TotalAmount = 50m },
            };

			_repositoryMock
				.Setup(r => r.GetInvoicesByDateRangeAsync(
					start, end, false))
				.ReturnsAsync(allInvoices);

            var service = new InvoiceSummaryService(_repositoryMock.Object);
			
            // Act
			var total = await service.GetTotalInvoiceValueAsync(start, end);

			// Assert
			Assert.Equal(250m, total);
		}

        [Fact]
        public async Task FilterBy_Customer_Name_Return_InvoiceValue()
        {
			// Arrange
			var start = new DateTime(2025, 4, 1);
			var end = new DateTime(2025, 4, 30);
            var customerName = "Coca Cola";


            var allInvoices = new List<Invoice>()
            {
                new Invoice
                {
					IssueDate = new DateTime(2025, 4, 10),
                    TotalAmount = 165m,
                    Customer = new Party { Name = "Coca Cola"}
                },
				 new Invoice
				{
					IssueDate = new DateTime(2025, 4, 10),
					TotalAmount = 200m,
					Customer = new Party { Name = "Servest"}
				},
			};

            _repositoryMock
				.Setup(r => r.GetInvoicesByDateRangeAsync(
					start, end, false))
				.ReturnsAsync(allInvoices);


            //Act
			var result = await _service.GetTotalSalesByCustomerAsync(start, end, customerName);

            //Assert
            Assert.Equal(165m, result);

		}
	}
}