using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_part_project
{
    [Table("tbTrades")]
    public class Trade
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("CoinType")]
        public string CoinType { get; set; }
        [Column("Worth")]
        public int Worth { get; set; }
        [Column("DateCreated")]
        public DateTime DateCreated { get; set; }

        public Trade(int id, string coinType, int worth, DateTime dateCreated)
        {
            Id = id;
            CoinType = coinType;
            Worth = worth;
            DateCreated = dateCreated;
        }
    }
}

