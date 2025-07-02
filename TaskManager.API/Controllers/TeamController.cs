using Microsoft.AspNetCore.Mvc;
using TaskManager.Business;
using TaskManager.Entities.DTOs;

namespace TaskManager.API.Controllers;

// API/Controllers/TeamController.cs
[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamDto dto)
    {
        await _teamService.CreateTeamAsync(dto);
        return Ok("Takım oluşturuldu.");
    }

    [HttpPost("invite")]
    public async Task<IActionResult> InviteMember([FromBody] InviteMemberDto dto)
    {
        await _teamService.InviteMemberAsync(dto);
        return Ok("Davet gönderildi.");
    }
}
