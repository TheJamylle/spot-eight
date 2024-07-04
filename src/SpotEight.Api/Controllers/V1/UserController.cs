using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpotEight.Core.Application.UseCases.V1.User.GetAll;

namespace SpotEight.Api.Controllers.V1;

/// <summary>
/// 
/// </summary>
[ApiVersion("1.0", Deprecated = true)]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : ApiControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("All")]
    public async Task<ActionResult<GetAllUserResponse>> GetAll()
    {
        var result = await _mediator.Send(new GetAllRequest());

        return Ok(result);
    }
}