namespace Qorrect.Integration.Models
{
    public class DTOLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class DTOTokenResponse
    {
        public string token { get; set; }
        public object privatetoken { get; set; }
    }
}
