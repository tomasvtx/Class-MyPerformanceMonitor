namespace MyPerformanceMonitor.Model
{
    /// <summary>
    /// Třída pro generování systémového reportu.
    /// </summary>
    public struct SystemReport
    {
        /// <summary>
        /// Získá nebo nastaví jméno uživatele.
        /// </summary>
        public string Uzivatel {  get; set; }

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
}
