using Contracts.ApiContracts.UserAccountInfo.Requests;
using Contracts.ApiContracts.UserAccountInfo.Responses;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Requests;
using Contracts.ApiContracts.UserGameRanks.RocketLeagueRank.Responses;

namespace Client.MyAccount
{
    public interface IMyAccount
    {
        public Task<GetUserAccountInfoResponse?> GetUserAccountInfo();
        public Task<UpdateUserAccountInfoResponse?> UpdateUserAccountInfo(UpdateUserAccountInfoRequest info);

        public Task<GetRocketLeagueRankResponse?> GetRocketLeagueRank();
        public Task<UpdateRocketLeagueRankResponse?> UpdateRocketLeagueRank(UpdateRocketLeagueRankRequest rank);
    }
}
