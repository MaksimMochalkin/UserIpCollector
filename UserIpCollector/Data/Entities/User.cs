namespace UserIpCollector.Data.Entities
{
    public class User
    {
        public long Id { get; set; }

        public ICollection<UserIpAddress> IpAddresses { get; set; } = new List<UserIpAddress>();
    }
}
