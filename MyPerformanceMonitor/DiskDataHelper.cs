using SystemMonitor;
using static MyPerformanceMonitor.Model.Enum;

namespace MyPerformanceMonitor
{
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
        public double GetDiskReadDataAsDouble() => systemData.GetDiskData(DiskData.Read);

        /// <summary>
        /// Získá množství přečtených dat z disku jako řetězec.
        /// </summary>
        /// <returns>Přečtená data z disku jako řetězec (formátované bajty).</returns>
        public string GetDiskReadDataAsString() => systemData.FormatBytes(systemData.GetDiskData(DiskData.Read));

        /// <summary>
        /// Získá množství zapsaných dat na disk jako desetinné číslo (double).
        /// </summary>
        /// <returns>Zapsaná data na disk jako desetinné číslo.</returns>
        public double GetDiskWriteDataAsDouble() => systemData.GetDiskData(DiskData.Write);

        /// <summary>
        /// Získá množství zapsaných dat na disk jako řetězec.
        /// </summary>
        /// <returns>Zapsaná data na disk jako řetězec (formátované bajty).</returns>
        public string GetDiskWriteDataAsString() => systemData.FormatBytes(systemData.GetDiskData(DiskData.Write));

        /// <summary>
        /// Získá množství přečtených a zapsaných dat na disk jako desetinné číslo (double).
        /// </summary>
        /// <returns>Přečtená a zapsaná data na disk jako desetinné číslo.</returns>
        public double GetDiskReadAndWriteDataAsDouble() => systemData.GetDiskData(DiskData.ReadAndWrite);

        /// <summary>
        /// Získá čas aktivního používání disku jako desetinné číslo (double).
        /// </summary>
        /// <returns>Čas aktivního používání disku jako desetinné číslo.</returns>
        public double GetDiskTimeAsDouble() => systemData.GetDiskData(DiskData.Time);

        /// <summary>
        /// Získá množství přečtených a zapsaných dat na disk jako řetězec.
        /// </summary>
        /// <returns>Přečtená a zapsaná data na disk jako řetězec (formátované bajty).</returns>
        public string GetDiskReadAndWriteDataAsString() => systemData.FormatBytes(systemData.GetDiskData(DiskData.ReadAndWrite));
    }

}
