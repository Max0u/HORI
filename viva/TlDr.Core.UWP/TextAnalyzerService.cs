using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlDr.Core.UWP
{
    public class TextAnalyzerService
    {
        public delegate void AnalyzedEvent(object sender, bool succed);
        public static event AnalyzedEvent Analyzed;

        /// <summary>
        /// Faire le matching entre le fileNameTxt ( = resultat du speetchtotext) et le fichier WaveName + Personne 
        /// Reformatage : (garder le textbrut pour l'analyser en même temps)
        /// 
        /// [Antho]Ceci est une phrase.
        /// [Antho]Et ca c'est une Autre phrase de la même personne.
        /// [Junior]Mais ca c'est une phrase d'une AUTRE personne
        /// 
        /// Call API Text Analyser en envoyant tout le text brut
        /// 
        /// When succed Raise event : Analyzed(this, true/false)!
        /// </summary>
        /// <param name="fileNameTxt"></param>
        /// <param name="fileNameWaveJson"></param>
        /// <returns></returns>
        public static Task Start(string fileNameTxt, object fileNameWaveJson)
        {
            throw new NotImplementedException();
        }
    }
}
