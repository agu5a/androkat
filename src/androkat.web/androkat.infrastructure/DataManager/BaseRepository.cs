using AutoMapper;

namespace androkat.infrastructure.DataManager;

public class BaseRepository
{
    protected readonly IMapper _mapper;

    public BaseRepository(IMapper mapper)
    {
        _mapper = mapper;
    }
}