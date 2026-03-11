using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission10_LaTour.Models;

namespace Mission10_LaTour.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BowlingLeagueController : ControllerBase
{
    private BowlingLeagueContext _bowlingcontext;

    public BowlingLeagueController(BowlingLeagueContext temp)
    {
        _bowlingcontext = temp;
    }

    [HttpGet(Name = "GetBowlingLeague")]
    public IEnumerable<BowlerWithTeamNameDto> Get()
    {
        var bowlingList = _bowlingcontext.Bowlers
            .Where(b => b.TeamId == 1 || b.TeamId == 2)
            .Select(b => new BowlerWithTeamNameDto(
                b.BowlerId,
                b.BowlerLastName,
                b.BowlerFirstName,
                b.BowlerMiddleInit,
                b.BowlerAddress,
                b.BowlerCity,
                b.BowlerState,
                b.BowlerZip,
                b.BowlerPhoneNumber,
                b.Team!.TeamName
            ))
            .ToList();
        
        return bowlingList;
    }
}

public record BowlerWithTeamNameDto(
    int BowlerId,
    string? BowlerLastName,
    string? BowlerFirstName,
    string? BowlerMiddleInit,
    string? BowlerAddress,
    string? BowlerCity,
    string? BowlerState,
    string? BowlerZip,
    string? BowlerPhoneNumber,
    string TeamName
);
