using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_part_project.Class
{
    [Table("tbTrades")]
    public class Trade
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("CoinType")]
    public string CoinType { get; set; }

    [Column("Worth")]
    public int Worth { get; set; }

    [Column("DateCreated")]
    public DateTime DateCreated { get; set; }

    [Column("IdTrader")]
    public int IdTrader { get; set; }

    [ForeignKey("IdTrader")]
    [JsonIgnore]
    public Trader? Trader { get; set; }
}

}

