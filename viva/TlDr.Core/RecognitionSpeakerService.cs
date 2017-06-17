using Microsoft.CognitiveServices.SpeechRecognition;
using Microsoft.ProjectOxford.SpeakerRecognition;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract.Identification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlDr.Core
{

    public class RecognitionSpeakerService
    {
        private const string Key = "63b6ab24c15f4e72a1949bcdaba4ecad";
        private Dictionary<Guid, string> Guids = new Dictionary<Guid, string>{
            {new Guid("3ce9e16f-3fd8-4038-adee-b42efadd0935"), "Max" },
            //{new Guid("b8614233-5dd5-4d6d-9342-6adaac7790c2"), "Junior" },
            //{new Guid(""), "Diana" },
            //{new Guid(""), "Jeff" },
            { new Guid("752d5bae-88cd-4ec1-bbcf-5450f2c261b5"), "Antho" },
        };

        private SpeakerIdentificationServiceClient _serviceClient;
        private List<Script> _wavePersons;
        int i = 1; // TODO : prendre celui du SpeechToText

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
        public async void Start(string waveFileName)
        {
            Tuple<Guid, Microsoft.ProjectOxford.SpeakerRecognition.Contract.Confidence> result = await CallApi(waveFileName);

            Trace.WriteLine("#### ");
            if (result?.Item2 == Microsoft.ProjectOxford.SpeakerRecognition.Contract.Confidence.Low
                || result?.Item1 == Guid.Empty)
            {
                result = new Tuple<Guid, Microsoft.ProjectOxford.SpeakerRecognition.Contract.Confidence>(Guid.Empty, Microsoft.ProjectOxford.SpeakerRecognition.Contract.Confidence.High);
            }

            string fileNamePath = string.Format(SpeechToTextService.FileName, i.ToString("000"));
            if (File.Exists(fileNamePath))
            {
                string jsonRead = File.ReadAllText(fileNamePath);
                _wavePersons = JsonConvert.DeserializeObject<List<Script>>(jsonRead);
            }
            else
            {
                FileStream fs = File.Create(fileNamePath);
                fs.Dispose();
            }
            _wavePersons = _wavePersons ?? new List<Script>();

            string personne = result.Item1 == Guid.Empty ? "Unknown" : Guids[result.Item1];
            Trace.WriteLine("############################ " + waveFileName + " : " + fileNamePath + " ( " + personne + " )");
            IEnumerable<Script> scripts = _wavePersons.Where(o => o.WaveName == waveFileName);
            foreach (var item in _wavePersons)
            {
                if(item.WaveName == waveFileName)
                {
                    Trace.WriteLine("############################ wav " + item.WaveName + " ( " + personne + " )");
                    item.Person = personne;
                }
            }

            string jsonWrite = JsonConvert.SerializeObject(_wavePersons);
            File.WriteAllText(fileNamePath, jsonWrite);
        }

        private async Task<Tuple<Guid, Microsoft.ProjectOxford.SpeakerRecognition.Contract.Confidence>> CallApi(string waveFileName)
        {
            _serviceClient = new SpeakerIdentificationServiceClient(Key);

            try
            {
                OperationLocation processPollingLocation;
                using (Stream audioStream = File.OpenRead(waveFileName))
                {
                    processPollingLocation = await _serviceClient.IdentifyAsync(audioStream, Guids.Select(o => o.Key).ToArray(), true);
                }

                IdentificationOperation identificationResponse = null;
                int numOfRetries = 10;
                TimeSpan timeBetweenRetries = TimeSpan.FromSeconds(5.0);
                while (numOfRetries > 0)
                {
                    await Task.Delay(timeBetweenRetries);
                    identificationResponse = await _serviceClient.CheckIdentificationStatusAsync(processPollingLocation);

                    if (identificationResponse.Status == Status.Succeeded)
                    {
                        break;
                    }
                    else if (identificationResponse.Status == Status.Failed)
                    {
                        throw new IdentificationException(identificationResponse.Message);
                    }
                    numOfRetries--;
                }
                if (numOfRetries <= 0)
                {
                    throw new IdentificationException("Identification operation timeout.");
                }

                Guid IdProfile = identificationResponse.ProcessingResult.IdentifiedProfileId;
                Microsoft.ProjectOxford.SpeakerRecognition.Contract.Confidence confidence = identificationResponse.ProcessingResult.Confidence;
                return new Tuple<Guid, Microsoft.ProjectOxford.SpeakerRecognition.Contract.Confidence>(IdProfile, confidence);
            }
            catch (IdentificationException ex)
            {

                Trace.WriteLine("IdentificationException : " + ex);
                return null;
            }
            catch (Exception ex)
            {

                Trace.WriteLine("Exception : " + ex);
                return null;
            }
        }
    }
}
