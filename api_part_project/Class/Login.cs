using System.Text.Json.Serialization;

namespace api_part_project
{
    public class Login
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
        [JsonPropertyName("pass_word")]
        public string PassWord { get; set; }

        public Login(string userName, string passWord)
        {
            UserName = userName;
            PassWord = passWord;
        }
    }
}
