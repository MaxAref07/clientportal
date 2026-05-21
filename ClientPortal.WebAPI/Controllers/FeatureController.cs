using ClientPortal.Application.DTOs;
using ClientPortal.Application.Features.Commands.CreateFeature;
using ClientPortal.Application.Features.Queries.GetFeatureById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateFeature(CreateFeatureCommand command)
    {
        FeatureDto feature = await mediator.Send(command);
        return CreatedAtAction(nameof(GetFeatureById), new { id = feature.Id }, feature);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FeatureDto>> GetFeatureById(Guid id)
    {
        var query = new GetFeatureByIdQuery(id);
        FeatureDto responseFeature = await mediator.Send(query);
        return Ok(responseFeature);
    }
}