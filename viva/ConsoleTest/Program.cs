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
        static SpeechToTextService _speechToTextService;
        static RecorderService _wave;
        static void Main(string[] args)
        {
            _speechToTextService = new SpeechToTextService();
            _speechToTextService.Start();
            Console.Read();
            //while(true)
            //{
            //    Loop();
            //}
        }

        private static async void Loop()
        {
            RecorderService wave = new RecorderService();

            string command = Console.ReadLine();

            switch (command)
            {
                case "Start":
                    wave.Start(); // Lancer le record de la Wav
                    _speechToTextService.Start(); // Lance le speech to text
                    break;
                case "Stop":
                    string fileNameWaveJson = await wave.Stop(); // Lancer le record de la Wav
                    string fileNameTxt = await _speechToTextService.Stop(); // Lance le speech to text

                    //TextAnalyzerService.Analyzed -= OnTextAnalysed;
                    //TextAnalyzerService.Analyzed += OnTextAnalysed;
                    //Task doNotAwait = TextAnalyzerService.Start(fileNameTxt, fileNameWaveJson);
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
