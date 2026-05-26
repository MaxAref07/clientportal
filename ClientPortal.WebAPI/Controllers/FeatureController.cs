using ClientPortal.Application.DTOs;
using ClientPortal.Application.Features.Commands.ChangeFeatureStatus;
using ClientPortal.Application.Features.Commands.CreateFeature;
using ClientPortal.Application.Features.Queries.GetFeatureById;
using ClientPortal.Application.Features.Queries.GetFeaturesByProjectIdQuery;
using ClientPortal.Domain.Enums;
using ClientPortal.WebAPI.Models;
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
    
    [HttpGet]
    public async Task<ActionResult<List<FeatureDto>>> GetFeaturesByProjectId([FromQuery] Guid projectId)
    {
        var query = new GetFeaturesByProjectIdQuery(projectId);
        List<FeatureDto> responseFeatures = await mediator.Send(query);
        return Ok(responseFeatures);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<FeatureDto>> ChangeFeatureStatus([FromRoute] Guid id, ChangeFeatureStatusRequest request)
    {
        var command = new ChangeFeatureStatusCommand
        {
            Id = id,
            NewStatus = request.NewStatus
        };
        FeatureDto feature = await mediator.Send(command);
        
        return Ok(feature);
    }
}