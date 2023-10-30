using System;
using System.Diagnostics;
using System.Management;

namespace SystemMonitor
{
    /// <summary>
    /// Tato t��da slou�� k z�sk�v�n� syst�mov�ch dat pro SystemMonitor.
    /// </summary>
    internal class SystemData
    {
        /// <summary>
        /// Konstruktor t��dy SystemData inicializuje instanci a p�iprav� po��te�n� hodnoty pro sledov�n� v�konu s�ov�ch rozhran�.
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
        /// Z�sk� nebo nastav� hodnotu indikuj�c�, zda se m� v�stupn� form�t dat komprimovat.
        /// </summary>
        bool compactFormat;
        internal bool CompactFormat
        {
            get { return compactFormat; }
            set { compactFormat = value; }
        }

        /// <summary>
        /// Z�sk� hodnotu procesorov�ho vyt�en�.
        /// </summary>
        /// <returns>Procesorov� vyt�en� v procentech.</returns>
        internal double GetProcessorData()
        {
            double d = GetCounterValue(_cpuCounter, "Processor", "% Processor Time", "_Total");
            return d;
        }

        /// <summary>
        /// Z�sk� data o vyu�it� pam�ti.
        /// </summary>
        /// <returns>Data o vyu�it� pam�ti ve form�tu textu.</returns>
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
        /// Z�sk� data o vyu�it� fyzick� pam�ti.
        /// </summary>
        /// <returns>Data o vyu�it� fyzick� pam�ti ve form�tu textu.</returns>
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
        /// Reprezentuje r�zn� typy p��stupu t�kaj�c� se disku.
        /// </summary>
        internal enum DiskData
        {
            /// <summary>
            /// �ten� a z�pis
            /// </summary>
            ReadAndWrite,

            /// <summary>
            /// �ten� a z�pis
            /// </summary>
            Read,

            /// <summary>
            /// �ten� a z�pis
            /// </summary>
            Write,

            /// <summary>
            /// �as
            /// </summary>
            Time
        }


        /// <summary>
        /// Z�sk� data t�kaj�c� se disku na z�klad� zvolen�ho typu dat.
        /// </summary>
        /// <param name="dd">Typ dat disku.</param>
        /// <returns>Hodnota dat disku odpov�daj�c� zvolen�mu typu.</returns>
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
        /// Reprezentuje typy s�ov�ch dat, kter� lze z�skat.
        /// </summary>
        internal enum NetData
        {
            /// <summary>
            /// P�ijato a odesl�no
            /// </summary>
            ReceivedAndSent,

            /// <summary>
            /// P�ijato
            /// </summary>
            Received,

            /// <summary>
            /// P�ijato
            /// </summary>
            Sent
        }


        /// <summary>
        /// Z�sk� data o s�ti na z�klad� zvolen�ho typu.
        /// </summary>
        /// <param name="nd">Typ s�ov�ch dat k z�sk�n�.</param>
        /// <returns>Hodnota s�ov�ch dat.</returns>
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
        /// V��tov� typ reprezentuj�c� jednotky pro form�tov�n� byt�.
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
            /// nepodporovan� jednotka
            /// </summary>
            ER,
        }
        /// <summary>
        /// Form�tuje velikost vstupn�ch byt� na z�klad� zvolen�ch jednotek.
        /// </summary>
        /// <param name="bytes">Velikost v bytech k form�tov�n�.</param>
        /// <returns>Form�tovan� �et�zec reprezentuj�c� velikost s jednotkou.</returns>
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
        /// Prov�d� dotaz na syst�mov� �daje na z�klad� zadan�ho typu.
        /// </summary>
        /// <param name="type">Typ �daj� k z�sk�n� (nap�. "totalphysicalmemory").</param>
        /// <returns>�et�zec reprezentuj�c� hodnotu z�skanou z po��ta�ov�ho syst�mu.</returns>
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
        /// Roz�i�uje prom�nn� prost�ed� a vrac� hodnotu pro zadan� typ.
        /// </summary>
        /// <param name="type">Typ prom�nn�ho prost�ed� k roz���en� (nap�. "%TEMP%").</param>
        /// <returns>�et�zec reprezentuj�c� roz���enou hodnotu prom�nn�ho prost�ed�.</returns>
        internal string QueryEnvironment(string type)
        {
            return Environment.ExpandEnvironmentVariables(type);
        }

        /// <summary>
        /// Z�sk� informace o logick�ch diskech a jejich dostupn�m voln�m prostoru.
        /// </summary>
        /// <returns>�et�zec obsahuj�c� informace o jednotliv�ch logick�ch disc�ch a jejich dostupn�m voln�m prostoru.</returns>
        internal string LogicalDisk()
        {
            string diskSpace = string.Empty;
            object device, space;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                device = objMgmt["DeviceID"];       // Nap��klad "C:"
                if (null != device)
                {
                    space = objMgmt["FreeSpace"];   // Nap��klad "10.32 GB" nebo "5.87 GB"
                    if (null != space)
                        diskSpace += device.ToString() + FormatBytes(double.Parse(space.ToString())) + ", ";
                }
            }

            diskSpace = diskSpace.Substring(0, diskSpace.Length - 2);
            return diskSpace;
        }

        /// <summary>
        /// Z�sk� hodnotu v�konnostn�ho ��ta�e na z�klad� specifikovan�ch parametr�.
        /// </summary>
        /// <param name="v�konnostn���ta�">Instance t��dy PerformanceCounter.</param>
        /// <param name="kategorieN�zev">N�zev kategorie v�konnostn�ho ��ta�e.</param>
        /// <param name="n�zev��ta�e">N�zev v�konnostn�ho ��ta�e.</param>
        /// <param name="n�zevInstance">N�zev instance v�konnostn�ho ��ta�e.</param>
        /// <returns>Hodnota z�skan� z v�konnostn�ho ��ta�e.</returns>
        double GetCounterValue(PerformanceCounter v�konnostn���ta�, string kategorieN�zev, string n�zev��ta�e, string n�zevInstance)
        {
            v�konnostn���ta�.CategoryName = kategorieN�zev;
            v�konnostn���ta�.CounterName = n�zev��ta�e;
            v�konnostn���ta�.InstanceName = n�zevInstance;
            return v�konnostn���ta�.NextValue();
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
