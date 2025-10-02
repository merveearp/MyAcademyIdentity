using Microsoft.AspNetCore.Identity;

namespace EmailApp.Entities
{
    public class AppUser: IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; } = "~/AdminLTE-3.0.4/dist/img/defaultuser.png";
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
    }
}
