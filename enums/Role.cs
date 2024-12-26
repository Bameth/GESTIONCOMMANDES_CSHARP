namespace GESTIONCOMMANDES.enums
{
    public enum Role
    {
        RS,
        ADMIN,
        COMPTABLE,
        LIVREUR,
        CLIENT,
    }
    public static class RoleHelper
    {
        public static Role? GetValue(string value)
        {
            foreach (Role g in Enum.GetValues(typeof(Role)))
            {
                if (string.Equals(g.ToString(), value, StringComparison.OrdinalIgnoreCase))
                {
                    return g;
                }
            }
            return null;
        }
    }

}