using System;
using SystemMonitor;

namespace MyPerformanceMonitor
{
    public class Program
	{
		private static SystemData sd = new SystemData();
		static void Main(string[] args)
		{
			//while (true) {
			//	Console.Clear();
			//Console.WriteLine(CPU_LoadString().Result);
			//	await Task.Delay(1000);
		//	}

			Console.ReadKey();
		}

        /// <summary>
        /// Třída pro generování systémového reportu.
        /// </summary>
        public struct SystemReport
        {
            /// <summary>
            /// Získá nebo nastaví jméno uživatele.
            /// </summary>
            public string Uzivatel { get; set; }

            /// <summary>
            /// Získá nebo nastaví informace o CPU.
            /// </summary>
            public string CPU { get; set; }

            /// <summary>
            /// Získá nebo nastaví informace o paměti a swap souborech.
            /// </summary>
            public string MemorySwapFileString { get; set; }

            /// <summary>
            /// Získá nebo nastaví informace o modelu počítače.
            /// </summary>
            public string ModelPC { get; set; }

            /// <summary>
            /// Získá nebo nastaví informace o zápisu a čtení z disku.
            /// </summary>
            public string DiskWriteRead { get; set; }

            /// <summary>
            /// Získá nebo nastaví síťová data pro odeslání a příjem.
            /// </summary>
            public string NetSentAndReceived { get; set; }
        }

        /// <summary>
        /// Získá systémový report obsahující informace o serveru.
        /// </summary>
        /// <returns>Instance třídy SystemReport s informacemi o serveru.</returns>
        public static SystemReport GetSystemReport()
        {
            // Vytvoření instance SystemReport pro shromáždění informací o systému.
            SystemReport systemReport = new SystemReport();

            // Inicializace tříd pro získání různých informací o systému.
            ComputerInfoHelper computerInfoHelper = new ComputerInfoHelper();
            MemorySwapHelper memorySwapHelper = new MemorySwapHelper();
            CPULoadHelper loadHelper = new CPULoadHelper();
            DiskDataHelper diskDataHelper = new DiskDataHelper();
            NetworkDataHelper networkDataHelper = new NetworkDataHelper();

            try
            {
                // Získání a uložení informace o aktuálním uživateli systému.
                systemReport.Uzivatel = computerInfoHelper.GetUsername();
            }
            catch (Exception ex)
            {
                // Pokud se nepodaří získat informaci, uložení chybového hlášení.
                systemReport.Uzivatel = ex?.Message;
            }

            try
            {
                // Získání a uložení informace o využití paměti swap (swap file) v systému.
                systemReport.MemorySwapFileString = memorySwapHelper?.GetMemorySwapFileAsString();
            }
            catch (Exception ex)
            {
                // Pokud se nepodaří získat informaci, uložení chybového hlášení.
                systemReport.MemorySwapFileString = ex?.Message;
            }

            try
            {
                // Získání a uložení informace o modelu počítače.
                systemReport.ModelPC = computerInfoHelper?.GetPCModel();
            }
            catch (Exception ex)
            {
                // Pokud se nepodaří získat informaci, uložení chybového hlášení.
                systemReport.ModelPC = ex?.Message;
            }

            try
            {
                // Získání a uložení informace o zatížení CPU (procesoru) v systému.
                systemReport.CPU = loadHelper?.GetCPULoadAsString();
            }
            catch (Exception ex)
            {
                // Pokud se nepodaří získat informaci, uložení chybového hlášení.
                systemReport.CPU = ex?.Message;
            }

            try
            {
                // Získání a uložení informace o zápisu a čtení z disku spolu s časem aktivního používání disku.
                systemReport.DiskWriteRead = $"{diskDataHelper?.GetDiskWriteDataAsString()} {diskDataHelper?.GetDiskReadDataAsString()} {diskDataHelper?.GetDiskTimeAsDouble()}";
            }
            catch (Exception ex)
            {
                // Pokud se nepodaří získat informaci, uložení chybového hlášení.
                systemReport.DiskWriteRead = ex?.Message;
            }

            try
            {
                // Získání a uložení informace o množství přijatých a odeslaných síťových dat.
                systemReport.NetSentAndReceived = networkDataHelper?.GetReceivedAndSentDataAsString();
            }
            catch (Exception ex)
            {
                // Pokud se nepodaří získat informaci, uložení chybového hlášení.
                systemReport.NetSentAndReceived = ex?.Message;
            }

            // Vracení naplněného objektu SystemReport s informacemi o systému.
            return systemReport;

        }

    }

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


    /// <summary>
    /// Třída pro získání informací o zatížení CPU (procesoru).
    /// </summary>
    public class CPULoadHelper
    {
        private SystemData systemData;

        /// <summary>
        /// Inicializuje novou instanci třídy CPULoadHelper.
        /// </summary>
        public CPULoadHelper()
        {
            systemData = new SystemData();
        }

        /// <summary>
        /// Získá zatížení CPU jako desetinné číslo (double).
        /// </summary>
        /// <returns>Zatížení CPU jako desetinné číslo (procentuální hodnota).</returns>
        public double GetCPULoadAsDouble() => systemData.GetProcessorData();

        /// <summary>
        /// Získá zatížení CPU jako řetězec (string).
        /// </summary>
        /// <returns>Zatížení CPU jako řetězec (procentuální hodnota s jednotkou '%').</returns>
        public string GetCPULoadAsString() => $"{GetCPULoadAsDouble():F}%";
    }


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
        public string GetMemorySwapFileAsString() => systemData?.GetMemoryVData();

        /// <summary>
        /// Získá využití paměti swap jako desetinné číslo (double).
        /// </summary>
        /// <returns>Využití paměti swap jako desetinné číslo (procentuální hodnota).</returns>
        public double GetMemorySwapFileAsDouble()
        {
            string data = systemData?.GetMemoryVData();
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


    /// <summary>
    /// Třída pro získání a formátování dat o disku v různých formátech.
    /// </summary>
    public class DiskDataHelper
    {
        private SystemData systemData;

        /// <summary>
        /// Inicializuje novou instanci třídy DiskDataHelper.
        /// </summary>
        public DiskDataHelper() => systemData = new SystemData();

        /// <summary>
        /// Získá množství přečtených dat z disku jako desetinné číslo (double).
        /// </summary>
        /// <returns>Přečtená data z disku jako desetinné číslo.</returns>
        public double GetDiskReadDataAsDouble() => systemData.GetDiskData(SystemData.DiskData.Read);

        /// <summary>
        /// Získá množství přečtených dat z disku jako řetězec.
        /// </summary>
        /// <returns>Přečtená data z disku jako řetězec (formátované bajty).</returns>
        public string GetDiskReadDataAsString() => systemData.FormatBytes(systemData.GetDiskData(SystemData.DiskData.Read));

        /// <summary>
        /// Získá množství zapsaných dat na disk jako desetinné číslo (double).
        /// </summary>
        /// <returns>Zapsaná data na disk jako desetinné číslo.</returns>
        public double GetDiskWriteDataAsDouble() => systemData.GetDiskData(SystemData.DiskData.Write);

        /// <summary>
        /// Získá množství zapsaných dat na disk jako řetězec.
        /// </summary>
        /// <returns>Zapsaná data na disk jako řetězec (formátované bajty).</returns>
        public string GetDiskWriteDataAsString() => systemData.FormatBytes(systemData.GetDiskData(SystemData.DiskData.Write));

        /// <summary>
        /// Získá množství přečtených a zapsaných dat na disk jako desetinné číslo (double).
        /// </summary>
        /// <returns>Přečtená a zapsaná data na disk jako desetinné číslo.</returns>
        public double GetDiskReadAndWriteDataAsDouble() => systemData.GetDiskData(SystemData.DiskData.ReadAndWrite);

        /// <summary>
        /// Získá čas aktivního používání disku jako desetinné číslo (double).
        /// </summary>
        /// <returns>Čas aktivního používání disku jako desetinné číslo.</returns>
        public double GetDiskTimeAsDouble() => systemData.GetDiskData(SystemData.DiskData.Time);

        /// <summary>
        /// Získá množství přečtených a zapsaných dat na disk jako řetězec.
        /// </summary>
        /// <returns>Přečtená a zapsaná data na disk jako řetězec (formátované bajty).</returns>
        public string GetDiskReadAndWriteDataAsString() => systemData.FormatBytes(systemData.GetDiskData(SystemData.DiskData.ReadAndWrite));
    }


    /// <summary>
    /// Třída pro získání a formátování síťových dat v různých formátech.
    /// </summary>
    public class NetworkDataHelper
    {
        private SystemData systemData;

        /// <summary>
        /// Inicializuje novou instanci třídy NetworkDataHelper.
        /// </summary>
        public NetworkDataHelper() => systemData = new SystemData();

        /// <summary>
        /// Získá množství přijatých síťových dat jako desetinné číslo (double).
        /// </summary>
        /// <returns>Přijatá síťová data jako desetinné číslo.</returns>
        public double GetReceivedDataAsDouble() => systemData.GetNetData(SystemData.NetData.Received);

        /// <summary>
        /// Získá množství přijatých síťových dat jako řetězec.
        /// </summary>
        /// <returns>Přijatá síťová data jako řetězec (formátované bajty).</returns>
        public string GetReceivedDataAsString() => systemData.FormatBytes(systemData.GetNetData(SystemData.NetData.Received));

        /// <summary>
        /// Získá množství odeslaných síťových dat jako desetinné číslo (double).
        /// </summary>
        /// <returns>Odeslaná síťová data jako desetinné číslo.</returns>
        public double GetSentDataAsDouble() => systemData.GetNetData(SystemData.NetData.Sent);

        /// <summary>
        /// Získá množství odeslaných síťových dat jako řetězec.
        /// </summary>
        /// <returns>Odeslaná síťová data jako řetězec (formátované bajty).
        /// </returns>
        public string GetSentDataAsString() => systemData.FormatBytes(systemData.GetNetData(SystemData.NetData.Sent));

        /// <summary>
        /// Získá množství přijatých a odeslaných síťových dat jako desetinné číslo (double).
        /// </summary>
        /// <returns>Přijatá a odeslaná síťová data jako desetinné číslo.</returns>
        public double GetReceivedAndSentDataAsDouble() => systemData.GetNetData(SystemData.NetData.ReceivedAndSent);

        /// <summary>
        /// Získá množství přijatých a odeslaných síťových dat jako řetězec.
        /// </summary>
        /// <returns>Přijatá a odeslaná síťová data jako řetězec (formátované bajty).
        /// </returns>
        public string GetReceivedAndSentDataAsString() => systemData.FormatBytes(systemData.GetNetData(SystemData.NetData.ReceivedAndSent));
    }

}
