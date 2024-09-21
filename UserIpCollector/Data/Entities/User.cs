using System.ComponentModel.DataAnnotations.Schema;

namespace UserIpCollector.Data.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]  // Отключаем автоинкремент
        public long Id { get; set; }

        public ICollection<UserIpAddress> IpAddresses { get; set; } = new List<UserIpAddress>();
    }
}
