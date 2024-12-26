namespace GESTIONCOMMANDES.enums
{
    public enum TypePaiement
    {
        OM,
        WAVE,
        ESPECES,
        CHEQUE
    }
    public static class TypePaiementHelper
    {
        public static TypePaiement? GetValue(string value)
        {
            foreach (TypePaiement g in Enum.GetValues(typeof(TypePaiement)))
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