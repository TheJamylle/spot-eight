using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SpotEight.Api.Controllers;

/// <summary>
/// Controlador base para API
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    /// <summary>
    /// 
    /// </summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
