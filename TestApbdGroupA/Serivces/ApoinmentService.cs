using TestApbdGroupA.Models;
using TestApbdGroupA.Reopisotries;

namespace TestApbdGroupA.Serivces;

public class ApoinmentService : IApoinmentSerivce
{
    private readonly IApoinmentRepository _repository;

    public ApoinmentService(IApoinmentRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ApoinmentDto> GetApoinmentsByIdAsync(int id)
    {
        return await _repository.GetApoinmnetsDetailsAsync(id);
    }
}