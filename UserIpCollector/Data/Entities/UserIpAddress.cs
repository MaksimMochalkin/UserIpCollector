namespace UserIpCollector.Data.Entities
{
    using Newtonsoft.Json;

    public class UserIpAddress
    {
        public long Id { get; set; }

        public string IpAddress { get; set; }

        public DateTime ConnectedAt { get; set; }

        public long UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
