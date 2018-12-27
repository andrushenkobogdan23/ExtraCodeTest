using Microsoft.AspNetCore.Identity;

namespace EnergySuite.SelfService.Identity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class MyUser : IdentityUser<int>, IMyUserCustomization
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }


    public class MyRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }


    public interface IMyUserCustomization
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
    }

}