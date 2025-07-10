using EnsekTestTask.Core.Models;
using Microsoft.AspNetCore.Http;

namespace EnsekTestTask.Core.Services.Abstractions;

public interface IMeterService
{
    Task<ParseMetersResponse> UploadMeters(IFormFile file);
}