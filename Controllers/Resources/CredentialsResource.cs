using FluentValidation.Attributes;

namespace notebook.Controllers.Resources
{
    [Validator(typeof(CredentialsResourceValidator))]
    public class CredentialsResource
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}