using SystemMonitor;
using static MyPerformanceMonitor.Model.Enum;

namespace MyPerformanceMonitor
{
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
        public double GetReceivedDataAsDouble() => systemData.GetNetData(NetData.Received);

        /// <summary>
        /// Získá množství přijatých síťových dat jako řetězec.
        /// </summary>
        /// <returns>Přijatá síťová data jako řetězec (formátované bajty).</returns>
        public string GetReceivedDataAsString() => systemData.FormatBytes(systemData.GetNetData(NetData.Received));

        /// <summary>
        /// Získá množství odeslaných síťových dat jako desetinné číslo (double).
        /// </summary>
        /// <returns>Odeslaná síťová data jako desetinné číslo.</returns>
        public double GetSentDataAsDouble() => systemData.GetNetData(NetData.Sent);

        /// <summary>
        /// Získá množství odeslaných síťových dat jako řetězec.
        /// </summary>
        /// <returns>Odeslaná síťová data jako řetězec (formátované bajty).
        /// </returns>
        public string GetSentDataAsString() => systemData.FormatBytes(systemData.GetNetData(NetData.Sent));

        /// <summary>
        /// Získá množství přijatých a odeslaných síťových dat jako desetinné číslo (double).
        /// </summary>
        /// <returns>Přijatá a odeslaná síťová data jako desetinné číslo.</returns>
        public double GetReceivedAndSentDataAsDouble() => systemData.GetNetData(NetData.ReceivedAndSent);

        /// <summary>
        /// Získá množství přijatých a odeslaných síťových dat jako řetězec.
        /// </summary>
        /// <returns>Přijatá a odeslaná síťová data jako řetězec (formátované bajty).
        /// </returns>
        public string GetReceivedAndSentDataAsString() => systemData.FormatBytes(systemData.GetNetData(NetData.ReceivedAndSent));
    }

}
