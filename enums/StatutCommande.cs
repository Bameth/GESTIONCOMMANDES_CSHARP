namespace GESTIONCOMMANDES.enums
{
    public enum StatutCommande
    {
        ENCOURS,
        ENATTENTE,
        PRET_A_LIVRER,
        LIVREE,
        ANNULEE
    }
    public static class StatutHelper
    {
        public static StatutCommande? GetValue(string value)
        {
            foreach (StatutCommande g in Enum.GetValues(typeof(StatutCommande)))
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