namespace GESTIONCOMMANDES.enums
{
    public enum StatutPaiement
    {
        PAYEE,
        NON_PAYEE
    }
    public static class StatutPaiementHelper
    {
        public static StatutPaiement? GetValue(string value)
        {
            foreach (StatutPaiement g in Enum.GetValues(typeof(StatutPaiement)))
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