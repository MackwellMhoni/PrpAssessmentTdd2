using Castle.Core.Resource;
using Moq;
using NuGet.Frameworks;
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
			
            // Act
			var total = await _service.GetTotalInvoiceValueAsync(start, end);

			// Assert
			Assert.Equal(250m, total);
		}

        [Fact]
        public async Task FilterBy_Customer_Name_Return_InvoiceValue()
        {
			// Arrange
			var start = new DateTime(2025, 4, 1);
			var end = new DateTime(2025, 4, 30);
            var customerName = "Servest";


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
            Assert.Equal(200m, result);

		}

		[Fact]
		public async Task FilterBy_Customer_Name_Return_Sum_Multiple_InvoiceValue()
		{
			// Arrange
			var start = new DateTime(2025, 4, 1);
			var end = new DateTime(2025, 4, 30);
			var customerName = "Servest";


			var allInvoices = new List<Invoice>()
			{
				 new Invoice
				{
					IssueDate = new DateTime(2025, 4, 10),
					TotalAmount = 350m,
					Customer = new Party { Name = "Servest"}
				},
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
				 new Invoice
				{
					IssueDate = new DateTime(2025, 4, 15),
					TotalAmount = 150m,
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
			Assert.Equal(700m, result);

		}

		[Fact]
		public async Task No_Line_Items_Exist()
		{
			//Arrange
			var start = new DateTime(2025, 4, 5);
			var end = new DateTime(2025, 4, 15);
			var allInvoices = new List<Invoice>();

			_repositoryMock
				.Setup(r => r.GetInvoicesByDateRangeAsync(
					start, end, false))
				.ReturnsAsync(allInvoices);

			// Act
			var result = await _service.GetMostSoldProductCodeAsync(start, end);

			// Assert
			Assert.Equal(null, result);

		}

		[Fact]
		public async Task Entry_of_one_Line_Item_()
		{
			//Arrange
			var start = new DateTime(2025, 4, 5);
			var end = new DateTime(2025, 4, 15);

			var allInvoices = new List<Invoice>()
			{
				new Invoice
				{
					LineItems = new List<InvoiceLineItem>()
					{
						new InvoiceLineItem
						{
							ProductCode = "PO1",
						}
					}
				}
			};

			_repositoryMock
				.Setup(r => r.GetInvoicesByDateRangeAsync(start, end, false))
				.ReturnsAsync(allInvoices);

			//Act 
			var result = await _service.GetMostSoldProductCodeAsync(start, end);

			//Assert
			Assert.Equal("PO1", result);

		}

		[Fact]
		public async Task Entry_of_1_Line_Items_Return_the_code_with_the_highest_value()
		{
			//Arrange
			var start = new DateTime(2025, 4, 5);
			var end = new DateTime(2025, 4, 15);

			var allInvoices = new List<Invoice>()
			{
				new Invoice
				{
					LineItems = new List<InvoiceLineItem>()
					{
						new InvoiceLineItem
						{
							ProductCode = "PO1",
							Quantity = 2,
							UnitPrice = 100
						},
						new InvoiceLineItem
						{
							ProductCode = "PO2",
							Quantity = 3,
							UnitPrice = 150
						}
					}
				}
			};

			_repositoryMock
				.Setup(r => r.GetInvoicesByDateRangeAsync(start, end, false))
				.ReturnsAsync(allInvoices);

			//Act 
			var result = await _service.GetMostSoldProductCodeAsync(start, end);

			//Assert
			Assert.Equal("PO2", result);

		}
	}
}