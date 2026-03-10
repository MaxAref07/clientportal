using ClientPortal.Application.Projects.Commands.CreateProject;
using ClientPortal.Application.Projects.DTOs;
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
        return Ok(responseProject);
    }
}