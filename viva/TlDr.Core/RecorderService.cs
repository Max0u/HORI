using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlDr.Core
{
    public class RecorderService
    {
        /// <summary>
        /// Lance le record de la Wave
        /// </summary>
        public static void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Arrete le record de la Wave
        /// Envoit le fichier WaveFileName a RecognitionSpeakerService
        /// </summary>
        /// <returns>WaveFileName</returns>
        public static string Stop()
        {
            return "tot";
        }
    }
}
