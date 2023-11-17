using MyPerformanceMonitor.Model;
using System;
using SystemMonitor;

namespace MyPerformanceMonitor
{
    /// <summary>
    /// Hlavní třída programu pro sledování výkonu systému.
    /// </summary>
    public partial class Program
    {
        /// <summary>
        /// Inicializace jednotlivých pomocných tříd
        /// </summary>
        private static CPULoadHelper loadHelper;
        private static SystemReport systemReport;
        private static ComputerInfoHelper computerInfoHelper;
        private static MemorySwapHelper memorySwapHelper;
        private static DiskDataHelper diskDataHelper;
        private static NetworkDataHelper networkDataHelper;

        static Program()
        {
            // Inicializace jednotlivých pomocných tříd
            computerInfoHelper = new ComputerInfoHelper();
            memorySwapHelper = new MemorySwapHelper();
            loadHelper = new CPULoadHelper();
            diskDataHelper = new DiskDataHelper();
            networkDataHelper = new NetworkDataHelper();
        }

        /// <summary>
        /// Získává a vytváří hlášení o systému se shromážděnými informacemi o výkonu.
        /// </summary>
        /// <returns>Objekt SystemReport obsahující informace o systému.</returns>
        public static SystemReport GetSystemReport()
        {
            // Vytvoření instance SystemReport pro shromáždění informací o systému.
            systemReport = new SystemReport
            {
                // Získání a uložení informace o aktuálním uživateli systému.
                Uzivatel = computerInfoHelper.GetUsername(),

                // Získání a uložení informace o využití paměti swap (swap file) v systému.
                MemorySwapFileString = memorySwapHelper?.GetMemorySwapFileAsString(),

                // Získání a uložení informace o modelu počítače.
                ModelPC = computerInfoHelper?.GetPCModel(),

                // Získání a uložení informace o zatížení CPU (procesoru) v systému.
                CPU = loadHelper?.GetCPULoadAsString(),

                // Získání a uložení informace o zápisu a čtení z disku spolu s časem aktivního používání disku.
                DiskWriteRead = $"{diskDataHelper?.GetDiskWriteDataAsString()} {diskDataHelper?.GetDiskReadDataAsString()} {diskDataHelper?.GetDiskTimeAsDouble()}",

                // Získání a uložení informace o množství přijatých a odeslaných síťových dat.
                NetSentAndReceived = networkDataHelper?.GetReceivedAndSentDataAsString()
            };
            // Vracení naplněného objektu SystemReport s informacemi o systému.
            return systemReport;
        }
    }
}
