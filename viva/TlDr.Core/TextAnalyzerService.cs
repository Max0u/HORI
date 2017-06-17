using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ProjectOxford.Text.Topic;
using Microsoft.ProjectOxford.Text.KeyPhrase;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace TlDr.Core
{
    public class TextAnalyzerService
    {

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
        /// 
        public static List<string> Start(string fileName)
        {
            string txtJson = string.Empty;
            List<string> result = new List<string>();
            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                List<Script> jsonFromTxt = JsonConvert.DeserializeObject<List<Script>>(json);
                foreach (var item in jsonFromTxt)
                {
                   txtJson += item.Sentence;
                }
                result = TextAnalyzerService.KeyWord(txtJson);
            }
            return result;
        }
        
        public static List<string> KeyWord(string txt)
        {
            Console.WriteLine(txt);
            List<string> arr = new List<string>();
            var apiKey = "3f99cd2db7e8405c85fced07a31756d5";

            var document = new KeyPhraseDocument()
            {
                Id = "a026ef6b-9136-4935-a428-6258ebdc87f1",
                Text = txt,
                Language = "en"
            };

            var request = new KeyPhraseRequest();
            request.Documents.Add(document);

            var client = new KeyPhraseClient(apiKey);

            var response = client.GetKeyPhrases(request);

            foreach (var doc in response.Documents)
            {
                //Console.WriteLine("Document Id: {0}", doc.Id);

                foreach (var keyPhrase in doc.KeyPhrases)
                {
                    arr.Add(keyPhrase);
                }
                //Console.WriteLine();
            }
            


            return arr;
        }
    }
}

