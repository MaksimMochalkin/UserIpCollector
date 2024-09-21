namespace UserIpCollector.Data.Entities
{
    public class UserIpAddress
    {
        public long Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime ConnectedAt { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
