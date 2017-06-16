using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlDr.Core
{
    public class RecognitionSpeakerService
    {
        /// <summary>
        /// Envoit de la Wave + Guids (call api)
        /// Recup des Names
        /// Ecriture d'un fichier JSON 
        /// 
        /// JSON :
        /// {
        ///     {WaveName  : "Wave001.wav", Personns : "Antho"}
        ///     {WaveName  : "Wave002.wav", Personns : "Antho"}
        ///     {WaveName  : "Wave003.wav", Personns : "Junior"}
        /// }
        /// 
        /// </summary>
        public void Start(string waveFileName)
        {
            throw new NotImplementedException();
        }
    }
}
