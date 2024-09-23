namespace UserIpCollector.Abstractions.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
        IUserIpAdresesRepository UserIpAddressRepository { get; }
    }
}
