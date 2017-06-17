using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TlDr.Core;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Loop();
            }
        }

        private static async void Loop()
        {
            RecorderService wave = new RecorderService();

            string command = Console.ReadLine();

            switch (command)
            {
                case "Start":
                    wave.Start(); // Lancer le record de la Wav
                    SpeechToTextService.Start(); // Lance le speech to text
                    break;
                case "Stop":
                    string fileNameWaveJson = wave.Stop(); // Lancer le record de la Wav
                    string fileNameTxt = SpeechToTextService.Stop(); // Lance le speech to text

                    TextAnalyzerService.Analyzed -= OnTextAnalysed;
                    TextAnalyzerService.Analyzed += OnTextAnalysed;
                    Task doNotAwait = TextAnalyzerService.Start(fileNameTxt, fileNameWaveJson);
                    break;
                default:
                    break;
            }
        }

        private static void OnTextAnalysed(object sender, bool succed)
        {
            throw new NotImplementedException();
            // TODO : check succeed and cry for Victory !
        }
    }
}
