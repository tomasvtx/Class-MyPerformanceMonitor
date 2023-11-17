using SystemMonitor;

namespace MyPerformanceMonitor
{
    /// <summary>
    /// Třída pro získání informací o počítači a uživateli.
    /// </summary>
    public class ComputerInfoHelper
    {
        private SystemData systemData;

        /// <summary>
        /// Inicializuje novou instanci třídy ComputerInfoHelper.
        /// </summary>
        public ComputerInfoHelper()
        {
            systemData = new SystemData();
        }

        /// <summary>
        /// Získá název výrobce a model počítače.
        /// </summary>
        /// <returns>Název výrobce a model počítače jako řetězec.</returns>
        public string GetPCModel() => $"{systemData?.QueryComputerSystem("manufacturer")}, {systemData.QueryComputerSystem("model")}";

        /// <summary>
        /// Získá identifikátor procesoru.
        /// </summary>
        /// <returns>Identifikátor procesoru jako řetězec.</returns>
        public string GetProcessorIdentifier() => systemData?.QueryEnvironment("%PROCESSOR_IDENTIFIER%");

        /// <summary>
        /// Získá jméno aktuálního uživatele na systému.
        /// </summary>
        /// <returns>Jméno aktuálního uživatele na systému jako řetězec.</returns>
        public string GetUsername() => $"User: {systemData?.QueryComputerSystem("username")}";
    }
}
