using System;

namespace notebook.Controllers.Resources
{
    public class CreateUserResource
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}