
namespace BasicAPI.Models
{
    public class User
    {
        public required PersonName Name { get; set; }
        public required LoginInfo Login { get; set; }
    }

    public class PersonName
    {
        public required string Title { get; set; }
        public required string First { get; set; }
        public required string Last { get; set; }
    }

    public class  LoginInfo
    {
        public required string UUID { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Salt { get; set; }
        public required string MD5 { get; set; }
        public required string SHA1 { get; set; }
        public required string SHA256 { get; set; }
    }
}
