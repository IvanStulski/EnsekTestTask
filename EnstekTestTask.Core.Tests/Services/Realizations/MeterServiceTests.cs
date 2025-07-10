using EnsekTestTask.Core.Services.Realizations;
using EnsekTestTask.Database;
using EnsekTestTask.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Globalization;

namespace EnstekTestTask.Core.Tests.Services.Realizations;

public class MeterServiceTests
{
    private readonly Mock<ApplicationDbContext> _contextMock = new Mock<ApplicationDbContext>();
    private readonly MeterService _service;

    public MeterServiceTests()
    {
        _service = new MeterService(_contextMock.Object);   
    }

    [Test]
    public async Task UploadMeters_WithValidCsv_AddsMeters()
    {
        // Arrange: Seed Accounts into the database
        _contextMock.Setup(context => context.Accounts).ReturnsDbSet(new List<Account>
        {
            new Account { Id = 1 },
            new Account { Id = 2 }
        });
        _contextMock.Setup(context => context.Meters).ReturnsDbSet(new List<Meter>());

        // Arrange: Valid CSV content
        var csvContent = "1,22/04/2019 09:24,100\n2,23/04/2019 10:30,200";
        var file = CreateFormFile(csvContent);

        // Act
        var response = await _service.UploadMeters(file);

        // Assert
        var meters = await _contextMock.Object.Meters.ToListAsync();
        Assert.That(response.Successful, Is.EqualTo(2), "Response should indicate 2 successful entries.");
        Assert.That(response.WithErrors, Is.EqualTo(0), "Response should have no errors.");
    }

    [Test]
    public async Task UploadMeters_WithInvalidCsv_SkipsInvalidRows()
    {
        // Arrange: Seed Accounts into the database
        _contextMock.Setup(context => context.Accounts).ReturnsDbSet(new List<Account>
        {
            new Account { Id = 1 }
        });
        _contextMock.Setup(context => context.Meters).ReturnsDbSet(new List<Meter>());

        // Arrange: Mix of valid and invalid rows
        var csvContent = "1,22/04/2019 09:24,100\nINVALID,INVALID,INVALID\n1,INVALID DATA";
        var file = CreateFormFile(csvContent);

        // Act
        var response = await _service.UploadMeters(file);

        // Assert
        var meters = await _contextMock.Object.Meters.ToListAsync();
        Assert.That(response.Successful, Is.EqualTo(1), "Response should indicate 1 successful entry.");
        Assert.That(response.WithErrors, Is.EqualTo(2), "Response should indicate 2 invalid rows.");
    }

    [Test]
    public async Task UploadMeters_WithDuplicateMeters_SkipsDuplicates()
    {
        // Arrange: Seed Accounts and existing Meters
        _contextMock.Setup(context => context.Accounts).ReturnsDbSet(new List<Account>
        {
            new Account { Id = 1 }
        });
        _contextMock.Setup(context => context.Meters).ReturnsDbSet(new List<Meter>
        {
            new Meter
            {
                AccountId = 1,
                MeterReadingDateTime = DateTime.Parse("22/04/2019 09:24", new CultureInfo("en-GB")),
                MeterReadValue = 100
            }
        });

        // Arrange: CSV content with duplicate meter
        var csvContent = "1,22/04/2019 09:24,100\n1,23/04/2019 10:30,200";
        var file = CreateFormFile(csvContent);

        // Act
        var response = await _service.UploadMeters(file);

        // Assert
        var meters = await _contextMock.Object.Meters.ToListAsync();
        Assert.That(response.WithErrors, Is.EqualTo(1), "Response should indicate 1 duplicate row skipped.");
        Assert.That(response.Successful, Is.EqualTo(1), "Response should indicate 1 successful entry.");
    }

    [Test]
    public async Task UploadMeters_WithCompletelyInvalidCsv_ReturnsErrors()
    {
        // Arrange: CSV content with completely invalid rows
        _contextMock.Setup(context => context.Accounts).ReturnsDbSet(new List<Account>());
        _contextMock.Setup(context => context.Meters).ReturnsDbSet(new List<Meter>());
        var csvContent = "INVALID,INVALID,INVALID\nINVALID AGAIN,INVALID TOO";
        var file = CreateFormFile(csvContent);

        // Act
        var response = await _service.UploadMeters(file);

        // Assert
        var meters = await _contextMock.Object.Meters.ToListAsync();
        Assert.That(meters.Count, Is.EqualTo(0), "No meters should be added for invalid rows.");
        Assert.That(response.Successful, Is.EqualTo(0), "Response should indicate no successful entries.");
        Assert.That(response.WithErrors, Is.EqualTo(2), "Response should indicate all rows as errors.");
    }

    [Test]
    public async Task UploadMeters_WithEmptyFile_ReturnsZeroCounts()
    {
        // Arrange: Empty CSV content
        var file = CreateFormFile(string.Empty);
        _contextMock.Setup(context => context.Accounts).ReturnsDbSet(new List<Account>());
        _contextMock.Setup(context => context.Meters).ReturnsDbSet(new List<Meter>());

        // Act
        var response = await _service.UploadMeters(file);

        // Assert
        var meters = await _contextMock.Object.Meters.ToListAsync();
        Assert.That(response.Successful, Is.EqualTo(0), "Response should indicate no successful entries.");
        Assert.That(response.WithErrors, Is.EqualTo(0), "Response should indicate no errors.");
    }

    private IFormFile CreateFormFile(string content)
    {
        var fileMock = new Mock<IFormFile>();

        // Convert string content to memory stream
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.FileName).Returns("file.csv");
        fileMock.Setup(f => f.Length).Returns(stream.Length);

        return fileMock.Object;
    }
}
