using EnsekTestTask.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EnsekTestTask.Controllers;

[ApiController]
[Route("[controller]")]
public class MetersController : ControllerBase
{
    private readonly IMeterService _service;

    public MetersController(IMeterService service)
    {
        _service = service;
    }

    [HttpPost("meter-reading-uploads")]
    public async Task<IActionResult> UploadMeters(/*[FileExtensions(Extensions = ".csv")]*/ IFormFile file)
    {
        var result = await _service.UploadMeters(file);

        return Ok(result);
    }
}
