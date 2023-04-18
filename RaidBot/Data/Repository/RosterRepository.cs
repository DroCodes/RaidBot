using RaidBot.entities;
using RaidBot.Util;

namespace RaidBot.Data.Repository;

public class RosterRepository : IRosterRepository
{
    private readonly DataContext _context;
    private readonly ILogger _logger;

    RosterRepository(DataContext ctx, ILogger logger)
    {
        _context = ctx;
        _logger = logger;
    }
}