using ClientPortal.Application.DTOs;
using ClientPortal.Application.Features.Commands.ChangeFeatureDescription;
using ClientPortal.Application.Features.Commands.ChangeFeaturePriority;
using ClientPortal.Application.Features.Commands.ChangeFeatureStatus;
using ClientPortal.Application.Features.Commands.CreateFeature;
using ClientPortal.Application.Features.Commands.RenameFeature;
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

    [HttpPatch("{id}/priority")]
    public async Task<ActionResult<FeatureDto>> ChangeFeaturePriority([FromRoute] Guid id, ChangeFeaturePriorityRequest request)
    {
        if (!Enum.IsDefined(typeof(FeaturePriority), request.NewPriority))
            return BadRequest("Invalid priority");
        
        var command = new ChangeFeaturePriorityCommand()
        {
            Id = id,
            NewPriority = request.NewPriority
        };
        FeatureDto feature = await mediator.Send(command);
        
        return Ok(feature);
    }

    [HttpPatch("{id}/name")]
    public async Task<ActionResult<FeatureDto>> RenameFeature([FromRoute] Guid id, RenameFeatureRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewName))
        {
            return BadRequest("New name must be provided");
        }
        var command = new RenameFeatureCommand()
        {
            Id = id,
            NewName = request.NewName
        };
        FeatureDto feature = await mediator.Send(command);
        return Ok(feature);
    }
    
    [HttpPatch("{id}/description")]
    public async Task<ActionResult<FeatureDto>> ChangeFeatureDescription([FromRoute] Guid id, ChangeFeatureDescriptionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewDescription))
        {
            return BadRequest("New name must be provided");
        }
        var command = new ChangeFeatureDescriptionCommand
        {
            Id = id,
            NewDescription = request.NewDescription
        };
        FeatureDto feature = await mediator.Send(command);
        return Ok(feature);
    }
}