using System;
using System.Collections.Generic;
using System.Text;

namespace MyPerformanceMonitor.Model
{
    internal class Enum
    {
        /// <summary>
        /// Reprezentuje různé typy přístupu týkající se disku.
        /// </summary>
        internal enum DiskData
        {
            /// <summary>
            /// Čtení a zápis
            /// </summary>
            ReadAndWrite,

            /// <summary>
            /// Čtení a zápis
            /// </summary>
            Read,

            /// <summary>
            /// Čtení a zápis
            /// </summary>
            Write,

            /// <summary>
            /// Čas
            /// </summary>
            Time
        }

        /// <summary>
        /// Reprezentuje typy síťových dat, která lze získat.
        /// </summary>
        internal enum NetData
        {
            /// <summary>
            /// Přijato a odesláno
            /// </summary>
            ReceivedAndSent,

            /// <summary>
            /// Přijato
            /// </summary>
            Received,

            /// <summary>
            /// Přijato
            /// </summary>
            Sent
        }


        /// <summary>
        /// Výčtový typ reprezentující jednotky pro formátování bytů.
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
    }
}
