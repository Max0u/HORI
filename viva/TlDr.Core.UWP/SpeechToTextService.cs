using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TlDr.Core.UWP
{
    public class Script
    {
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string WaveName { get; set; }
        public string Person { get; set; }
        public string Sentence { get; set; }
    }

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
    /// ta
    /// 
    /// </summary>
    //public class SpeechToTextService
    //{
    //    public const string SubscriptionKey = "e8501d935cab4d59a66b8bf2f504992f";
    //    public const string FileName = "transcript_{0}.txt";

    //    private string _sentence = string.Empty;
    //    private int i = 0;
    //    private Script _script = null;
    //    private List<Script> _scripts = null;

    //    private RecorderService _wave = new RecorderService();

    //    /// <summary>
    //    /// Ecrire la date de debut dans le fichier
    //    /// Calls API.
    //    /// Appelle le stop() des qu'il a une phrase
    //    /// </summary>
    //    public async void Start()
    //    {
    //        i++;
    //        _scripts = new List<Script>();

    //        //StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
    //        //_sampleFile = await storageFolder.CreateFileAsync(string.Format(FileName, i.ToString("000")), CreationCollisionOption.ReplaceExisting);
    //        //string json = await FileIO.ReadTextAsync(_sampleFile);
    //        string fileNamePath = string.Format(FileName, i.ToString("000"));
    //        if (File.Exists(fileNamePath))
    //        {
    //            string json = File.ReadAllText(fileNamePath);
    //            _scripts = JsonConvert.DeserializeObject<List<Script>>(json);
    //            _scripts = _scripts ?? new List<Script>();
    //        }
    //        else
    //        {
    //            FileStream fs = File.Create(fileNamePath);
    //            fs.Dispose();
    //        }

    //        InitData();

    //        if (_micClient == null)
    //        {
    //            CreateMicrophoneRecoClient();
    //        }

    //        _micClient.StartMicAndRecognition();
    //    }

    //    private void InitData()
    //    {
    //        _script = new Script();
    //        _script.DateDebut = DateTime.Now;
    //        _wave.Start();
    //    }

    //    /// <summary>
    //    /// Ecrit la phrase dans le fichier 
    //    /// Demande d'arreter le record (recup du fichier Wave)
    //    /// Ecrit la date de fin + WaveName
    //    /// </summary>
    //    /// <returns>FileName.txt</returns>
    //    public async Task<string> Stop()
    //    {
    //        string num = i.ToString("000");
    //        if (_script.Sentence == null)
    //            return string.Format(FileName, num);

    //        _script.DateFin = DateTime.Now;
    //        _script.WaveName = await _wave.Stop();

    //        string fileNamePath = string.Format(FileName, i.ToString("000"));
    //        string jsonInit = File.ReadAllText(fileNamePath);
    //        _scripts = JsonConvert.DeserializeObject<List<Script>>(jsonInit);
    //        _scripts = _scripts ?? new List<Script>();
    //        _scripts.Add(_script);

    //        InitData();

    //        string json = JsonConvert.SerializeObject(_scripts);
    //        //await FileIO.WriteTextAsync(_sampleFile, json);
    //        File.WriteAllText(string.Format(FileName, num), json);
    //        //i++;
    //        return string.Format(FileName, num);
    //    }

    //    /// <summary>
    //    /// Called when a final response is received;
    //    /// </summary>
    //    /// <param name="sender">The sender.</param>
    //    /// <param name="e">The <see cref="SpeechResponseEventArgs"/> instance containing the event data.</param>
    //    private void OnMicDictationResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
    //    {
    //        WriteLine("--- OnMicDictationResponseReceivedHandler ---");
    //        if (e.PhraseResponse.RecognitionStatus == RecognitionStatus.EndOfDictation ||
    //            e.PhraseResponse.RecognitionStatus == RecognitionStatus.DictationEndSilenceTimeout)
    //        {
    //            // we got the final result, so it we can end the mic reco.  No need to do this
    //            // for dataReco, since we already called endAudio() on it as soon as we were done
    //            // sending all the data.
    //            _micClient.EndMicAndRecognition();
    //        }

    //        WriteResponseResult(e);

    //        if(e?.PhraseResponse?.Results != null)
    //            _script.Sentence = e?.PhraseResponse?.Results?.OrderByDescending(ph => ph.Confidence)?.FirstOrDefault()?.DisplayText;
    //        Stop();
    //    }

    //    #region Core

    //    /// <summary>
    //    /// The data recognition client
    //    /// </summary>
    //    private DataRecognitionClient _dataClient;

    //    /// <summary>
    //    /// The microphone client
    //    /// </summary>
    //    private MicrophoneRecognitionClient _micClient;

    //    #region Events

    //    /// <summary>
    //    /// Implement INotifyPropertyChanged interface
    //    /// </summary>
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    #endregion Events

    //    /// <summary>
    //    /// Gets the default locale.
    //    /// </summary>
    //    /// <value>
    //    /// The default locale.
    //    /// </value>
    //    private string DefaultLocale
    //    {
    //        get { return "en-US"; }
    //    }

    //    /// <summary>
    //    /// Raises the System.Windows.Window.Closed event.
    //    /// </summary>
    //    protected void Close()
    //    {
    //        if (null != _dataClient)
    //        {
    //            _dataClient.Dispose();
    //        }

    //        if (null != _micClient)
    //        {
    //            _micClient.Dispose();
    //        }

    //    }

    //    /// <summary>
    //    /// Creates a new microphone reco client without LUIS intent support.
    //    /// </summary>
    //    private void CreateMicrophoneRecoClient()
    //    {
    //        _micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(
    //            SpeechRecognitionMode.LongDictation,
    //            DefaultLocale,
    //            SubscriptionKey);

    //        // Event handlers for speech recognition results
    //        _micClient.OnPartialResponseReceived += OnPartialResponseReceivedHandler;
    //        _micClient.OnResponseReceived += OnMicDictationResponseReceivedHandler;

    //        _micClient.OnConversationError += OnConversationErrorHandler;
    //    }

    //    /// <summary>
    //    /// Writes the response result.
    //    /// </summary>
    //    /// <param name="e">The <see cref="SpeechResponseEventArgs"/> instance containing the event data.</param>
    //    private void WriteResponseResult(SpeechResponseEventArgs e)
    //    {
    //        if (e.PhraseResponse.Results.Length == 0)
    //        {
    //            WriteLine("No phrase response is available.");
    //        }
    //        else
    //        {
    //            WriteLine("********* Final n-BEST Results *********");
    //            for (int i = 0; i < e.PhraseResponse.Results.Length; i++)
    //            {
    //                WriteLine(
    //                    "[{0}] Confidence={1}, Text=\"{2}\"",
    //                    i,
    //                    e.PhraseResponse.Results[i].Confidence,
    //                    e.PhraseResponse.Results[i].DisplayText);
    //            }

    //            WriteLine();
    //        }
    //    }

    //    /// <summary>
    //    /// Called when a partial response is received.
    //    /// </summary>
    //    /// <param name="sender">The sender.</param>
    //    /// <param name="e">The <see cref="PartialSpeechResponseEventArgs"/> instance containing the event data.</param>
    //    private void OnPartialResponseReceivedHandler(object sender, PartialSpeechResponseEventArgs e)
    //    {
    //        WriteLine("--- Partial result received by OnPartialResponseReceivedHandler() ---");
    //        WriteLine("{0}", e.PartialResult);
    //        WriteLine();
    //    }

    //    /// <summary>
    //    /// Called when an error is received.
    //    /// </summary>
    //    /// <param name="sender">The sender.</param>
    //    /// <param name="e">The <see cref="SpeechErrorEventArgs"/> instance containing the event data.</param>
    //    private void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
    //    {
    //        WriteLine("--- Error received by OnConversationErrorHandler() ---");
    //        WriteLine("Error code: {0}", e.SpeechErrorCode.ToString());
    //        WriteLine("Error text: {0}", e.SpeechErrorText);
    //        WriteLine();
    //    }

    //    /// <summary>
    //    /// Writes the line.
    //    /// </summary>
    //    private void WriteLine()
    //    {
    //        WriteLine(string.Empty);
    //    }

    //    /// <summary>
    //    /// Writes the line.
    //    /// </summary>
    //    /// <param name="format">The format.</param>
    //    /// <param name="args">The arguments.</param>
    //    private void WriteLine(string format, params object[] args)
    //    {
    //        var formattedStr = string.Format(format, args);
    //        Trace.WriteLine(formattedStr);
    //    }

    //    /// <summary>
    //    /// Helper function for INotifyPropertyChanged interface 
    //    /// </summary>
    //    /// <typeparam name="T">Property type</typeparam>
    //    /// <param name="caller">Property name</param>
    //    private void OnPropertyChanged<T>([CallerMemberName]string caller = null)
    //    {
    //        var handler = PropertyChanged;
    //        if (handler != null)
    //        {
    //            handler(this, new PropertyChangedEventArgs(caller));
    //        }
    //    }

    //    private void Reset()
    //    {
    //        // Reset everything
    //        if (_micClient != null)
    //        {
    //            _micClient.EndMicAndRecognition();
    //            _micClient.Dispose();
    //            _micClient = null;
    //        }

    //        if (_dataClient != null)
    //        {
    //            _dataClient.Dispose();
    //            _dataClient = null;
    //        }
    //    }
    //    #endregion
    //}
}
