using androkat.application.Interfaces;
using androkat.domain;
using AutoMapper;

namespace androkat.infrastructure.DataManager;

public class AdminRepository : BaseRepository, IAdminRepository
{
    public AdminRepository(AndrokatContext ctx, IClock clock, IMapper mapper) : base(ctx, clock, mapper)
    {
    }
}