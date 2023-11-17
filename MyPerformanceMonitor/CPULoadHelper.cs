using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using SystemMonitor;

namespace MyPerformanceMonitor
{
    /// <summary>
    /// Třída pro získání informací o zatížení CPU (procesoru).
    /// </summary>
    public class CPULoadHelper
    {
        private PerformanceCounter cpuCounter;

        /// <summary>
        /// Inicializuje novou instanci třídy CPULoadHelper.
        /// </summary>
        public CPULoadHelper()
        {
        }

        /// <summary>
        /// Získá zatížení CPU jako desetinné číslo (double).
        /// </summary>
        /// <returns>Zatížení CPU jako desetinné číslo (procentuální hodnota).</returns>
        public double GetCPULoadAsDouble()
        {
            if (cpuCounter == null)
            {
                // Vytvoření instance PerformanceCounter pro měření vytížení CPU
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            }

            float cpuUsage = cpuCounter.NextValue();

            // Připočteme krátkou pauzu pro aktualizaci hodnoty
            return cpuUsage;
        }

        /// <summary>
        /// Získá zatížení CPU jako řetězec (string).
        /// </summary>
        /// <returns>Zatížení CPU jako řetězec (procentuální hodnota s jednotkou '%').</returns>
        public string GetCPULoadAsString() => $"{GetCPULoadAsDouble():F}%";
    }

}
