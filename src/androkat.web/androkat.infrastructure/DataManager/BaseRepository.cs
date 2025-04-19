using androkat.application.Interfaces;
using AutoMapper;

namespace androkat.infrastructure.DataManager;

public class BaseRepository
{
    protected readonly AndrokatContext Ctx;
    protected readonly IClock Clock;
    protected readonly IMapper Mapper;

    protected BaseRepository(AndrokatContext ctx, IClock clock, IMapper mapper)
    {
        Ctx = ctx;
        Clock = clock;
        Mapper = mapper;
    }    
}