using Microsoft.AspNetCore.Mvc;
using TestApbdGroupA.Serivces;

namespace TestApbdGroupA.Controllers;

public class AppointmentsController : ControllerBase
{   
    private readonly IApoinmentSerivce _apoinmentService;

    public AppointmentsController(IApoinmentSerivce apoinmentSerivce)
    {
        _apoinmentService = apoinmentSerivce;
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApoiinments(int id)
    {
        var visit = await _apoinmentService.GetApoinmentsByIdAsync(id);
        if (visit == null)
            return NotFound();
        return Ok(visit);
    }
}