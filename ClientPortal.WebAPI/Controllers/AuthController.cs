using ClientPortal.Application.Auth.Commands.RequestMagicLink;
using ClientPortal.Application.Auth.Commands.VerifyMagicLink;
using ClientPortal.Application.Interfaces;
using ClientPortal.WebAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthController(IMediator mediator, IWebHostEnvironment environment, IJwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost("magic-link")]
    public async Task<IActionResult> RequestMagicLink([FromBody] RequestMagicLinkRequest request)
    {
        var command = new RequestMagicLinkCommand
        {
            Email = request.Email
        };

        var result = await mediator.Send(command);

        if (environment.IsDevelopment())
        {
            return Ok(new RequestMagicLinkResponse { Token = result.Token });
        }

        return Accepted();
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyMagicLink([FromBody] VerifyMagicLinkRequest request)
    {
        var command = new VerifyMagicLinkCommand
        {
            Token = request.Token
        };

        var domainResult = await mediator.Send(command);

        var authToken = jwtTokenService.GenerateToken(
            domainResult.UserId,
            domainResult.Email,
            domainResult.Role);

        return Ok(authToken);
    }
}
