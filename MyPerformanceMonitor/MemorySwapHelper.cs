using SystemMonitor;

namespace MyPerformanceMonitor
{
    /// <summary>
    /// Třída pro získání informací o využití paměti swap (swap file).
    /// </summary>
    public class MemorySwapHelper
    {
        private SystemData systemData;

        /// <summary>
        /// Inicializuje novou instanci třídy MemorySwapHelper.
        /// </summary>
        public MemorySwapHelper() => systemData = new SystemData();

        /// <summary>
        /// Získá využití paměti swap jako řetězec.
        /// </summary>
        /// <returns>Využití paměti swap jako řetězec (procentuální hodnota).</returns>
        public string GetMemorySwapFileAsString() => systemData?.GetMemoryDataV();

        /// <summary>
        /// Získá využití paměti swap jako desetinné číslo (double).
        /// </summary>
        /// <returns>Využití paměti swap jako desetinné číslo (procentuální hodnota).</returns>
        public double GetMemorySwapFileAsDouble()
        {
            string data = systemData?.GetMemoryDataV();
            if (data.Contains("%"))
            {
                int percentIndex = data?.IndexOf("%") ?? 0;
                if (double.TryParse(data?.Substring(0, percentIndex), out double result))
                {
                    return result;
                }
            }
            return 0.0; // Návratová hodnota v případě chyby.
        }
    }

}
