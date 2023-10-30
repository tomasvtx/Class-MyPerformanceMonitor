using System;
using System.Diagnostics;
using System.Management;

namespace SystemMonitor
{
    /// <summary>
    /// Tato tøída slouí k získávání systémovıch dat pro SystemMonitor.
    /// </summary>
    internal class SystemData
    {
        /// <summary>
        /// Konstruktor tøídy SystemData inicializuje instanci a pøipraví poèáteèní hodnoty pro sledování vıkonu síovıch rozhraní.
        /// </summary>
        internal SystemData()
        {
            PerformanceCounterCategory cat = new PerformanceCounterCategory("Network Interface");
            InstanceNames = cat.GetInstanceNames();

            _netRecvCounters = new PerformanceCounter[InstanceNames.Length];
            for (int i = 0; i < InstanceNames.Length; i++)
                _netRecvCounters[i] = new PerformanceCounter();

            _netSentCounters = new PerformanceCounter[InstanceNames.Length];
            for (int i = 0; i < InstanceNames.Length; i++)
                _netSentCounters[i] = new PerformanceCounter();

            CompactFormat = false;
        }


        /// <summary>
        /// Získá nebo nastaví hodnotu indikující, zda se má vıstupní formát dat komprimovat.
        /// </summary>
        bool compactFormat;
        internal bool CompactFormat
        {
            get { return compactFormat; }
            set { compactFormat = value; }
        }

        /// <summary>
        /// Získá hodnotu procesorového vytíení.
        /// </summary>
        /// <returns>Procesorové vytíení v procentech.</returns>
        internal double GetProcessorData()
        {
            double d = GetCounterValue(_cpuCounter, "Processor", "% Processor Time", "_Total");
            return d;
        }

        /// <summary>
        /// Získá data o vyuití pamìti.
        /// </summary>
        /// <returns>Data o vyuití pamìti ve formátu textu.</returns>
        internal string GetMemoryVData()
        {
            string str;
            double d = GetCounterValue(_memoryCounter, "Memory", "% Committed Bytes In Use", null);
            str = d.ToString("F") + "% (";

            d = GetCounterValue(_memoryCounter, "Memory", "Committed Bytes", null);
            str += FormatBytes(d) + " / ";

            d = GetCounterValue(_memoryCounter, "Memory", "Commit Limit", null);
            return str + FormatBytes(d) + ") ";
        }

        /// <summary>
        /// Získá data o vyuití fyzické pamìti.
        /// </summary>
        /// <returns>Data o vyuití fyzické pamìti ve formátu textu.</returns>
        internal string GetMemoryPData()
        {
            string s = QueryComputerSystem("totalphysicalmemory");
            double totalPhysicalMemory = Convert.ToDouble(s);

            double d = GetCounterValue(_memoryCounter, "Memory", "Available Bytes", null);
            d = totalPhysicalMemory - d;

            s = CompactFormat ? "%" : "% (" + FormatBytes(d) + " / " + FormatBytes(totalPhysicalMemory) + ")";
            d /= totalPhysicalMemory;
            d *= 100;
            return CompactFormat ? ((int)d).ToString() + s : d.ToString("F") + s;
        }


        /// <summary>
        /// Reprezentuje rùzné typy pøístupu tıkající se disku.
        /// </summary>
        internal enum DiskData
        {
            /// <summary>
            /// Ètení a zápis
            /// </summary>
            ReadAndWrite,

            /// <summary>
            /// Ètení a zápis
            /// </summary>
            Read,

            /// <summary>
            /// Ètení a zápis
            /// </summary>
            Write,

            /// <summary>
            /// Èas
            /// </summary>
            Time
        }


        /// <summary>
        /// Získá data tıkající se disku na základì zvoleného typu dat.
        /// </summary>
        /// <param name="dd">Typ dat disku.</param>
        /// <returns>Hodnota dat disku odpovídající zvolenému typu.</returns>
        internal double GetDiskData(DiskData dd)
        {
            return dd == DiskData.Read ?
                GetCounterValue(_diskReadCounter, "PhysicalDisk", "Disk Read Bytes/sec", "_Total") :
                dd == DiskData.Write ?
                GetCounterValue(_diskWriteCounter, "PhysicalDisk", "Disk Write Bytes/sec", "_Total") :
                dd == DiskData.Time ?
                GetCounterValue(_diskWriteCounter, "PhysicalDisk", "% Disk Time", "_Total") :
                dd == DiskData.ReadAndWrite ?
                GetCounterValue(_diskReadCounter, "PhysicalDisk", "Disk Read Bytes/sec", "_Total") +
                GetCounterValue(_diskWriteCounter, "PhysicalDisk", "Disk Write Bytes/sec", "_Total") :
                0;
        }


        /// <summary>
        /// Reprezentuje typy síovıch dat, která lze získat.
        /// </summary>
        internal enum NetData
        {
            /// <summary>
            /// Pøijato a odesláno
            /// </summary>
            ReceivedAndSent,

            /// <summary>
            /// Pøijato
            /// </summary>
            Received,

            /// <summary>
            /// Pøijato
            /// </summary>
            Sent
        }


        /// <summary>
        /// Získá data o síti na základì zvoleného typu.
        /// </summary>
        /// <param name="nd">Typ síovıch dat k získání.</param>
        /// <returns>Hodnota síovıch dat.</returns>
        internal double GetNetData(NetData nd)
        {
            if (InstanceNames.Length == 0)
                return 0;

            double d = 0;
            for (int i = 0; i < InstanceNames.Length; i++)
            {
                d += nd == NetData.Received ?
                    GetCounterValue(_netRecvCounters[i], "Network Interface", "Bytes Received/sec", InstanceNames[i]) :
                    nd == NetData.Sent ?
                    GetCounterValue(_netSentCounters[i], "Network Interface", "Bytes Sent/sec", InstanceNames[i]) :
                    nd == NetData.ReceivedAndSent ?
                    GetCounterValue(_netRecvCounters[i], "Network Interface", "Bytes Received/sec", InstanceNames[i]) +
                    GetCounterValue(_netSentCounters[i], "Network Interface", "Bytes Sent/sec", InstanceNames[i]) :
                    0;
            }

            return d;
        }

        /// <summary>
        /// Vıètovı typ reprezentující jednotky pro formátování bytù.
        /// </summary>
        internal enum Unit
        {
            /// <summary>
            /// bajty
            /// </summary>
            B,

            /// <summary>
            /// kilobajty
            /// </summary>
            KB,

            /// <summary>
            /// megabajty
            /// </summary>
            MB,

            /// <summary>
            /// gigabajty
            /// </summary>
            GB,

            /// <summary>
            /// nepodporovaná jednotka
            /// </summary>
            ER,
        }
        /// <summary>
        /// Formátuje velikost vstupních bytù na základì zvolenıch jednotek.
        /// </summary>
        /// <param name="bytes">Velikost v bytech k formátování.</param>
        /// <returns>Formátovanı øetìzec reprezentující velikost s jednotkou.</returns>
        internal string FormatBytes(double bytes)
        {
            int unit = 0;
            while (bytes > 1024)
            {
                bytes /= 1024;
                ++unit;
            }

            string formattedString = CompactFormat ? ((int)bytes).ToString() : bytes.ToString("F") + " ";
            return formattedString + ((Unit)unit).ToString();
        }


        /// <summary>
        /// Provádí dotaz na systémové údaje na základì zadaného typu.
        /// </summary>
        /// <param name="type">Typ údajù k získání (napø. "totalphysicalmemory").</param>
        /// <returns>Øetìzec reprezentující hodnotu získanou z poèítaèového systému.</returns>
        internal string QueryComputerSystem(string type)
        {
            string str = null;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                str = objMgmt[type].ToString();
            }
            return str;
        }

        /// <summary>
        /// Rozšiøuje promìnné prostøedí a vrací hodnotu pro zadanı typ.
        /// </summary>
        /// <param name="type">Typ promìnného prostøedí k rozšíøení (napø. "%TEMP%").</param>
        /// <returns>Øetìzec reprezentující rozšíøenou hodnotu promìnného prostøedí.</returns>
        internal string QueryEnvironment(string type)
        {
            return Environment.ExpandEnvironmentVariables(type);
        }

        /// <summary>
        /// Získá informace o logickıch diskech a jejich dostupném volném prostoru.
        /// </summary>
        /// <returns>Øetìzec obsahující informace o jednotlivıch logickıch discích a jejich dostupném volném prostoru.</returns>
        internal string LogicalDisk()
        {
            string diskSpace = string.Empty;
            object device, space;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                device = objMgmt["DeviceID"];       // Napøíklad "C:"
                if (null != device)
                {
                    space = objMgmt["FreeSpace"];   // Napøíklad "10.32 GB" nebo "5.87 GB"
                    if (null != space)
                        diskSpace += device.ToString() + FormatBytes(double.Parse(space.ToString())) + ", ";
                }
            }

            diskSpace = diskSpace.Substring(0, diskSpace.Length - 2);
            return diskSpace;
        }

        /// <summary>
        /// Získá hodnotu vıkonnostního èítaèe na základì specifikovanıch parametrù.
        /// </summary>
        /// <param name="vıkonnostníÈítaè">Instance tøídy PerformanceCounter.</param>
        /// <param name="kategorieNázev">Název kategorie vıkonnostního èítaèe.</param>
        /// <param name="názevÈítaèe">Název vıkonnostního èítaèe.</param>
        /// <param name="názevInstance">Název instance vıkonnostního èítaèe.</param>
        /// <returns>Hodnota získaná z vıkonnostního èítaèe.</returns>
        double GetCounterValue(PerformanceCounter vıkonnostníÈítaè, string kategorieNázev, string názevÈítaèe, string názevInstance)
        {
            vıkonnostníÈítaè.CategoryName = kategorieNázev;
            vıkonnostníÈítaè.CounterName = názevÈítaèe;
            vıkonnostníÈítaè.InstanceName = názevInstance;
            return vıkonnostníÈítaè.NextValue();
        }

        PerformanceCounter _memoryCounter = new PerformanceCounter();
        PerformanceCounter _cpuCounter = new PerformanceCounter();
        PerformanceCounter _diskReadCounter = new PerformanceCounter();
        PerformanceCounter _diskWriteCounter = new PerformanceCounter();

        string[] InstanceNames;
        PerformanceCounter[] _netRecvCounters;
        PerformanceCounter[] _netSentCounters;
    }
    internal delegate void OnLogicalDiskProc(string s);
}
