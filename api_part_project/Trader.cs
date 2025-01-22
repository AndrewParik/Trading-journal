using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_part_project
{
    [Table("tbTraders")]
    public class Trader
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("LastNam")]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Column("Trades")]
        public List<Trade> Trades { get; set; }

        public Trader(int id, string firstName, string lastName, List<Trade> trades)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Trades = trades;
        }
    }
}

