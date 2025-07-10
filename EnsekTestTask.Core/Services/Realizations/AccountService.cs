using EnsekTestTask.Core.Services.Abstractions;
using EnsekTestTask.Database;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace EnsekTestTask.Core.Services.Realizations;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext _context;

    public AccountService(ApplicationDbContext context)
    {
        _context = context;        
    }

    public async Task<ParseAccountsResponse> ParseAccounts(byte[] file)
    {
        using var memoryStream = new MemoryStream(file);
        using var parser = new TextFieldParser(memoryStream);

        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        while (!parser.EndOfData)
        {
            string[] fields = parser.ReadFields();
        }

        return new ParseAccountsResponse();
    }
}
