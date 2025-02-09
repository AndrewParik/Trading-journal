using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_part_project
{
    [Table("tbTraders")]
    public class Trader
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Column("LastName")]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public List<Trade> ?Trades { get; set; }
        public string PassWord { get; set; }

        public Trader(int id, string firstName, string lastName, string passWord)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PassWord = passWord;
        }
    }
}

