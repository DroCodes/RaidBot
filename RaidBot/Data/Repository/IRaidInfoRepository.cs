namespace RaidBot.Data.Repository;

public interface IRaidInfoRepository
{
    Task<bool> SaveRaidInfo(string raidName, string info);
    Task<bool> SaveRaidTier(string raidName, string tier);
    Task<bool> SaveDateTime(string raidName, DateTime date);
}