using androkat.application.Interfaces;
using AutoMapper;

namespace androkat.infrastructure.DataManager;

public class BaseRepository
{
    protected readonly AndrokatContext _ctx;
    protected readonly IClock _clock;
    protected readonly IMapper _mapper;

    public BaseRepository(AndrokatContext ctx, IClock clock, IMapper mapper)
    {
        _ctx = ctx;
        _clock = clock;
        _mapper = mapper;
    }
}