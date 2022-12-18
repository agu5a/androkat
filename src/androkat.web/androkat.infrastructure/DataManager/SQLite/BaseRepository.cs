using AutoMapper;

namespace androkat.infrastructure.DataManager.SQLite;

public class BaseRepository
{
    protected readonly IMapper _mapper;

    public BaseRepository(IMapper mapper)
    {
        _mapper = mapper;
    }
}