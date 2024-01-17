using Domain.Users.User;

namespace Domain.Users.UserQueueInfos;

public sealed class UserQueueInfo
{
    public UserId UserId { get; private set; }
    public UserQueueStatus Status {  get; private set; }
    public DateTime LastChangeDate { get; private set; } = DateTime.UtcNow;

    public UserQueueInfo(UserId userId)
    {
        UserId = userId;
        Status = UserQueueStatus.NotInQueue;
    }

    public void SetStatusInQueue(DateTime changeDate)
    {
        if (LastChangeDate > changeDate)
            return;

        Status = UserQueueStatus.InQueue;
        LastChangeDate = changeDate;
    }

    public void SetStatusNotInQueue(DateTime changeDate)
    {
        if (LastChangeDate > changeDate)
            return;

        Status = UserQueueStatus.NotInQueue;
        LastChangeDate = changeDate;
    }
    public void SetStatusInLobby(DateTime changeDate)
    {
        if (LastChangeDate > changeDate)
            return;

        Status = UserQueueStatus.InLobby;
        LastChangeDate = changeDate;
    }
}
