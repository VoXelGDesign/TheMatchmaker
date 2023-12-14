using Client.MyAccount.Models;

namespace Client.MyAccount
{
    public interface IMyAccount
    {
        public Task<UserAccountInfo?> GetUserAccountInfo();
        public Task<UserAccountInfo?> UpdateUserAccountInfo(UserAccountInfo info);

        public Task<RocketLeagueRank?> GetRocketLeagueRank();
        public Task<RocketLeagueRank?> UpdateRocketLeagueRank(RocketLeagueRank rank);
    }
}
