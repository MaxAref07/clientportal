using ClientPortal.Application.Projects.Commands.CreateProject;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Application.Projects.Queries.GetProjectById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ClientPortal.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectCommand command)
    {
        ProjectDTO responseProject = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProjectById), new { id = responseProject.Id }, responseProject);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> GetProjectById(Guid id)
    {
        var query = new GetProjectByIdQuery(id);
        ProjectDTO? responseProject = await _mediator.Send(query);
        if (responseProject == null)
            return NotFound();
        return Ok(responseProject);
    }
}