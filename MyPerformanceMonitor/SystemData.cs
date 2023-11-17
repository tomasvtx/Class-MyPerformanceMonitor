using System;
using System.Diagnostics;
using System.Management;
using static MyPerformanceMonitor.Model.Enum;

namespace SystemMonitor
{
    internal class SystemData
    {
        private PerformanceCounter _memoryCounter;
        private PerformanceCounter _diskReadCounter;
        private PerformanceCounter _diskWriteCounter;

        private string[] _instanceNames;
        private PerformanceCounter[] _netRecvCounters;
        private PerformanceCounter[] _netSentCounters;

        internal SystemData()
        {
            InitializeCounters();
        }

        /// <summary>
        /// Metoda pro inicializaci v˝konnostnÌch ËÌtaË˘
        /// </summary>
        private void InitializeCounters()
        {
            _memoryCounter = InitializeCounter("Memory", "Available Bytes");
            _diskReadCounter = InitializeCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            _diskWriteCounter = InitializeCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");

            PerformanceCounterCategory cat = new PerformanceCounterCategory("Network Interface");
            _instanceNames = cat.GetInstanceNames();

            _netRecvCounters = new PerformanceCounter[_instanceNames.Length];
            _netSentCounters = new PerformanceCounter[_instanceNames.Length];

            for (int i = 0; i < _instanceNames.Length; i++)
            {
                _netRecvCounters[i] = new PerformanceCounter();
                _netSentCounters[i] = new PerformanceCounter();
            }
        }

        /// <summary>
        /// Metoda pro inicializaci v˝konnostnÌho ËÌtaËe s moûnostÌ dalöÌ konfigurace
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="counterName"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        private PerformanceCounter InitializeCounter(string categoryName, string counterName, string instanceName = null)
        {
            var counter = new PerformanceCounter(categoryName, counterName, instanceName);
            // DalöÌ konfigurace, pokud je pot¯eba
            return counter;
        }

        /// <summary>
        /// Metoda pro zÌsk·nÌ hodnoty z v˝konnostnÌho ËÌtaËe
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="categoryName"></param>
        /// <param name="counterName"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        internal double GetCounterValue(PerformanceCounter counter, string categoryName, string counterName, string instanceName)
        {
            counter.CategoryName = categoryName;
            counter.CounterName = counterName;
            counter.InstanceName = instanceName;
            return counter.NextValue();
        }

        /// <summary>
        /// Metoda pro zÌsk·nÌ informacÌ o vyuûitÌ pamÏti v procentech a v bytech
        /// </summary>
        /// <returns></returns>
        internal string GetMemoryDataV()
        {
            double committedBytesInUse = GetCounterValue(_memoryCounter, "Memory", "% Committed Bytes In Use", null);
            string str = committedBytesInUse.ToString("F") + "% (";

            double committedBytes = GetCounterValue(_memoryCounter, "Memory", "Committed Bytes", null);
            str += FormatBytes(committedBytes) + " / ";

            double commitLimit = GetCounterValue(_memoryCounter, "Memory", "Commit Limit", null);
            return str + FormatBytes(commitLimit) + ") ";
        }

        /// <summary>
        /// Metoda pro zÌsk·nÌ informacÌ o vyuûitÌ fyzickÈ pamÏti v procentech a v bytech
        /// </summary>
        /// <returns></returns>
        internal string GetMemoryDataP()
        {
            double totalPhysicalMemory = GetTotalPhysicalMemory();
            double availableBytes = GetCounterValue(_memoryCounter, "Memory", "Available Bytes", null);
            double usedPhysicalMemory = totalPhysicalMemory - availableBytes;

            string s = "% (" + FormatBytes(usedPhysicalMemory) + " / " + FormatBytes(totalPhysicalMemory) + ")";
            double percentageUsed = (usedPhysicalMemory / totalPhysicalMemory) * 100;
            return percentageUsed.ToString("F") + s;
        }

        /// <summary>
        /// Metoda pro zÌsk·nÌ celkovÈ fyzickÈ pamÏti
        /// </summary>
        /// <returns></returns>
        private double GetTotalPhysicalMemory()
        {
            string totalPhysicalMemoryString = QueryComputerSystem("totalphysicalmemory");
            return Convert.ToDouble(totalPhysicalMemoryString);
        }

        /// <summary>
        /// Metoda pro zÌsk·nÌ informacÌ o vyuûitÌ disku (ËtenÌ, z·pis, Ëas, ËtenÌ a z·pis)
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        internal double GetDiskData(DiskData dd)
        {
            double result = 0;

            switch (dd)
            {
                case DiskData.Read:
                    result = GetCounterValue(_diskReadCounter, "PhysicalDisk", "Disk Read Bytes/sec", "_Total");
                    break;
                case DiskData.Write:
                    result = GetCounterValue(_diskWriteCounter, "PhysicalDisk", "Disk Write Bytes/sec", "_Total");
                    break;
                case DiskData.Time:
                    result = GetCounterValue(_diskWriteCounter, "PhysicalDisk", "% Disk Time", "_Total");
                    break;
                case DiskData.ReadAndWrite:
                    result = GetCounterValue(_diskReadCounter, "PhysicalDisk", "Disk Read Bytes/sec", "_Total") +
                             GetCounterValue(_diskWriteCounter, "PhysicalDisk", "Disk Write Bytes/sec", "_Total");
                    break;
            }

            return result;
        }

        /// <summary>
        /// Metoda pro zÌsk·nÌ informacÌ o sÌùovÈm provozu (p¯ijat·, odeslan·, p¯ijat· a odeslan·)
        /// </summary>
        /// <param name="nd"></param>
        /// <returns></returns>
        internal double GetNetData(NetData nd)
        {
            if (_instanceNames.Length == 0)
                return 0;

            double totalData = 0;
            for (int i = 0; i < _instanceNames.Length; i++)
            {
                double data = 0;

                switch (nd)
                {
                    case NetData.Received:
                        data = GetCounterValue(_netRecvCounters[i], "Network Interface", "Bytes Received/sec", _instanceNames[i]);
                        break;
                    case NetData.Sent:
                        data = GetCounterValue(_netSentCounters[i], "Network Interface", "Bytes Sent/sec", _instanceNames[i]);
                        break;
                    case NetData.ReceivedAndSent:
                        data = GetCounterValue(_netRecvCounters[i], "Network Interface", "Bytes Received/sec", _instanceNames[i]) +
                               GetCounterValue(_netSentCounters[i], "Network Interface", "Bytes Sent/sec", _instanceNames[i]);
                        break;
                }

                totalData += data;
            }

            return totalData;
        }

        /// <summary>
        /// Metoda pro zÌsk·nÌ informacÌ o logick˝ch discÌch
        /// </summary>
        /// <returns></returns>
        internal string LogicalDisk()
        {
            string diskSpace = string.Empty;
            object device, space;
            ManagementObjectSearcher objCS = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            foreach (ManagementObject objMgmt in objCS.Get())
            {
                device = objMgmt["DeviceID"];
                if (null != device)
                {
                    space = objMgmt["FreeSpace"];
                    if (null != space)
                        diskSpace += device.ToString() + FormatBytes(double.Parse(space.ToString())) + ", ";
                }
            }

            diskSpace = diskSpace.TrimEnd(',', ' ');
            return diskSpace;
        }

        /// <summary>
        /// Metoda pro form·tov·nÌ byt˘ na Ëiteln˝ ¯etÏzec
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        internal string FormatBytes(double bytes)
        {
            int unit = 0;
            while (bytes > 1024)
            {
                bytes /= 1024;
                ++unit;
            }

            string formattedString = bytes.ToString("F") + " ";
            return formattedString + ((Unit)unit).ToString();
        }

        /// <summary>
        /// Metoda pro dotaz na systÈmovÈ informace
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
        /// Metoda pro dotaz na informace o prost¯edÌ
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal string QueryEnvironment(string type)
        {
            return Environment.ExpandEnvironmentVariables(type);
        }
    }

    /// <summary>
    /// Deleg·t pro ud·losti spojenÈ s logick˝mi disky
    /// </summary>
    /// <param name="s"></param>
    internal delegate void OnLogicalDiskProc(string s);
}
