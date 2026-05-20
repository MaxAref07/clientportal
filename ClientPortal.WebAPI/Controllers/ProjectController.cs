using ClientPortal.Application.Projects.Commands.CreateProject;
using ClientPortal.Application.Projects.DTOs;
using ClientPortal.Application.Projects.Queries.GetProjectById;
using ClientPortal.Application.Projects.Queries.GetProjects;
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
}