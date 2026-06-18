using ClientPortal.Application.Projects.Commands.ChangeProjectDescription;
using ClientPortal.Application.Projects.Commands.ChangeProjectScopeFeatures;
using ClientPortal.Application.Projects.Commands.CreateProject;
using ClientPortal.Application.Projects.Commands.DeleteProject;
using ClientPortal.Application.Projects.Commands.RenameProject;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Application.Projects.Queries.GetProjectById;
using ClientPortal.Application.Projects.Queries.GetProjects;
using ClientPortal.WebAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectCommand command)
    {
        ProjectDto responseProject = await mediator.Send(command);
        return CreatedAtAction(nameof(GetProjectById), new { id = responseProject.Id }, responseProject);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById(Guid id)
    {
        var query = new GetProjectByIdQuery(id);
        ProjectDto? responseProject = await mediator.Send(query);
        if (responseProject == null)
            return NotFound();
        return Ok(responseProject);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAllProjects()
    {
        var query = new GetProjectsQuery();
        var projects = await mediator.Send(query);
        return Ok(projects);
    }
    
    [HttpPatch("{id}/name")]
    public async Task<ActionResult<ProjectDto>> RenameProject([FromRoute] Guid id, RenameProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewName))
            return BadRequest("New name must be provided");

        var command = new RenameProjectCommand()
        {
            Id = id,
            NewName = request.NewName
        };
        
        ProjectDto project = await mediator.Send(command);
        return Ok(project);
    }
    
    [HttpPatch("{id}/description")]
    public async Task<ActionResult<ProjectDto>> ChangeProjectDescription([FromRoute] Guid id, ChangeProjectDescriptionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewDescription))
            return BadRequest("New description must be provided");

        var command = new ChangeProjectDescriptionCommand()
        {
            Id = id,
            NewDescription = request.NewDescription
        };

        ProjectDto project = await mediator.Send(command);
        return Ok(project);
    }

    [HttpPatch("{id}/scope")]
    public async Task<ActionResult<ProjectDto>> ChangeProjectScopeFeatures([FromRoute] Guid id,
        ChangeProjectScopeFeaturesRequest request)
    {
        if (request.NewScopeFeatures < 1)
            return BadRequest("New feature scope must be greater than 0");

        var command = new ChangeProjectScopeFeaturesCommand()
        {
            Id = id,
            NewScopeFeatures = request.NewScopeFeatures
        };
        
        ProjectDto project = await mediator.Send(command);
        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var command = new DeleteProjectCommand()
        {
            Id = id
        };
        
        await mediator.Send(command);
        
        return NoContent();
    }
}