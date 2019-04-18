namespace notebook.Helpers
{
    public class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "member";
                public const string Moderator = "moderator";
                public const string Admin = "admin";
            }
        }
    }
}