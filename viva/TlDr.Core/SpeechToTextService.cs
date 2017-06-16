using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlDr.Core
{
    /// <summary>
    /// 
    /// - Faire les calls API pour Récupérer les phrases
    /// - Après chaque phrase ( . ? ! ) 
    ///     - Demander le Stop du recorder (renvoit le WaveName)
    ///     - Sauvegarder dans un fichier text : [date de debut] + la phrase + [date de fin + WaveName]
    /// - Relancer les call API
    /// 
    /// 
    /// 
    /// FileSample.txt : 
    /// 
    /// [01/01/2017 09:00:00]
    /// Ceci est une phrase.
    /// [01/01/2017 09:01:00 - Wave001.wav]
    /// [01/01/2017 09:01:00]
    /// Et ca c'est une Autre phrase de la même personne
    /// [01/01/2017 09:01:00 - Wave002.wav]
    /// [01/01/2017 09:01:00]
    /// Mais ca c'est une phrase d'une AUTRE personne
    /// [01/01/2017 09:01:00 - Wave003.wav]
    /// 
    /// 
    /// </summary>
    public class SpeechToTextService
    {
        /// <summary>
        /// Ecrire la date de debut dans le fichier
        /// Calls API + Loop.
        /// Appelle le stop() des qu'il a une phrase
        /// </summary>
        public static void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ecrit la phrase dans le fichier 
        /// Demande d'arreter le record (recup du fichier Wave)
        /// Ecrit la date de fin + WaveName
        /// </summary>
        /// <returns>FileName.txt</returns>
        public static string Stop()
        {
            throw new NotImplementedException();
        }
    }
}
