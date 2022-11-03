using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.Repositories.Abstracts;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _platformRepo;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepo platformRepo, IMapper mapper)
    {
        _platformRepo = platformRepo;
        _mapper = mapper;
    }

    /// <summary>
    /// This action gives the platforms list
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public IActionResult GetPlatforms()
    {
        var platforms = _platformRepo.GetAllPlatforms();
        var result = _mapper.Map<List<PlatformReadDto>>(platforms);
        return Ok(result);
    }


    /// <summary>
    /// This action gives only a platform.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name ="GetPlatformById")]
    public IActionResult GetPlatform(int id)
    {
        var platform = _platformRepo.GetPlatformById(id);
        if (platform != null)
        {
            var result = _mapper.Map<PlatformReadDto>(platform);
            return Ok(result);
        }

        return NotFound($"{id}'s platform not found!");
    }

    /// <summary>
    /// This action adds a new platform
    /// </summary>
    /// <param name="createDto"></param>
    /// <returns></returns>
    [HttpPost("Create")]
    public IActionResult CreatePlatform([FromBody] PlatformCreateDto createDto)
    {
        Platform platform = _mapper.Map<Platform>(createDto);
        _platformRepo.CreatePlatform(platform);
        _platformRepo.SaveChanges();

        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        return CreatedAtRoute("GetPlatformById", new { id = platformReadDto.Id }, platformReadDto);

    }
}

