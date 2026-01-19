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
    }
}