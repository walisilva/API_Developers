using API_Developers.Business;
using API_Developers.Model;
using Microsoft.AspNetCore.Mvc;

namespace API_Developers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeveloperController : ControllerBase
{
    private readonly ILogger<DeveloperController> _logger;
    private IDeveloperBusiness? _developerBusiness;

    public DeveloperController(ILogger<DeveloperController> logger
        , IDeveloperBusiness developerService)
    {
        _logger = logger;
        _developerBusiness = developerService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_developerBusiness.FindAll());
    }

    [HttpGet("{id}")]
    public IActionResult Get(long id)
    {
        var developer = _developerBusiness.FindById(id);
        if (developer == null) return NotFound();
        return Ok(developer);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Developer developer)
    {
        if (developer == null) return BadRequest();
        return Ok(_developerBusiness.Create(developer));
    }

    [HttpPut]
    public IActionResult Put([FromBody] Developer developer)
    {
        if (developer == null) return BadRequest();
        return Ok(_developerBusiness.Update(developer));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        _developerBusiness.Delete(id);
        return NoContent();
    }
}

