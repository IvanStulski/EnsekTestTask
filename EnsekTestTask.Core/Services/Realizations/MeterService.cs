using EnsekTestTask.Core.Models;
using EnsekTestTask.Core.Services.Abstractions;
using EnsekTestTask.Database;
using EnsekTestTask.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace EnsekTestTask.Core.Services.Realizations;

public class MeterService : IMeterService
{
    private const int AccountIdIndex = 0;
    private const int MeterReadingDateTimeIndex = 1;
    private const int MeterReadValueIndex = 2;
    private static readonly CultureInfo cultureInfo = new CultureInfo("en-GB");

    private readonly ApplicationDbContext _context;

    public MeterService(ApplicationDbContext context)
    {
        _context = context;        
    }

    public async Task<ParseAccountsResponse> UploadMeters(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var parser = new TextFieldParser(stream);

        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");

        var accountIds = await _context.Accounts.AsNoTracking().Select(x => x.Id).ToListAsync();
        var dbMeters = await _context.Meters.AsNoTracking().ToListAsync();

        var withErrors = 0;
        var newMeters = new List<Meter>();
        
        while (!parser.EndOfData)
        {
            var meter = new Meter();
            bool validationPassed = true;

            string[] fields = parser.ReadFields();
            
            if (!fields.First().StartsWith("AccountId") && 
                long.TryParse(fields[AccountIdIndex], out var accountId)
                 && DateTime.TryParse(fields[MeterReadingDateTimeIndex], cultureInfo, out var readingDateTime)
                 && int.TryParse(fields[MeterReadValueIndex], out var readValue))
            {
                meter.AccountId = accountId;
                meter.MeterReadingDateTime = readingDateTime;
                meter.MeterReadValue = readValue;
            }
            else
            {
                validationPassed = false;
            }

            validationPassed = ValidateMeter(meter, dbMeters, accountIds);

            if (validationPassed)
            {
                newMeters.Add(meter);
            }
            else
            {
                withErrors++;
            }
        }

        await _context.Meters.AddRangeAsync(newMeters);
        await _context.SaveChangesAsync();

        return new ParseAccountsResponse()
        {
            Successful = newMeters.Count,
            WithErrors = withErrors
        };
    }

    private bool ValidateMeter(Meter meter, IEnumerable<Meter> meters, IEnumerable<long> accountIds) =>
        accountIds.Contains(meter.AccountId) && !meters.Contains(meter, new MeterEqulityComparer());
}
