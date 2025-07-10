namespace EnsekTestTask.Core.Services.Abstractions;

public interface IAccountService
{
    Task<ParseAccountsResponse> ParseAccounts(byte[] file);
}

public class ParseAccountsResponse
{
    public int Successful { get; set; }
    public int WithErrors { get; set; }
}