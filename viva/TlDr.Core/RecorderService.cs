using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using System.Threading;

namespace TlDr.Core
{
	public class RecorderService
	{
		public static WaveInEvent waveSource = null;
		public static WaveFileWriter waveFile = null;
		public int i = 0;
		public const string fileName = "wave_{0}.wav";
        private bool _isReadToGo = true;
        private RecognitionSpeakerService _sendWave;

        /// <summary>
        /// Lance le record de la Wave
        /// </summary>
        public async void Start()
		{
            if(_sendWave == null)
            {
                _sendWave = RecognitionSpeakerService.Current;
            }

            while (!_isReadToGo)
            {
                await Task.Delay(100);
            }
            i++;
		    waveSource = new WaveInEvent();
			waveSource.WaveFormat = new WaveFormat(16000, 1);

			waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(OnDataAvailable);
			waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(OnRecordingStopped);

            waveFile = new WaveFileWriter(string.Format(fileName,i.ToString("000")), waveSource.WaveFormat);

            waveSource.StartRecording();
		}

		/// <summary>
		/// Arrete le record de la Wave
		/// Envoit le fichier WaveFileName a RecognitionSpeakerService
		/// </summary>
		/// <returns>WaveFileName</returns>
		public async Task<string> Stop()
        {
            _isReadToGo = false;

            waveSource.StopRecording();
            string fileName = waveFile.Filename;

            while (!_isReadToGo)
            {
                await Task.Delay(100);
            }
            _sendWave.Start(fileName);
            return fileName;
        }

        private static void OnDataAvailable(object sender, WaveInEventArgs e)
		{
			if (waveFile != null)
			{
				waveFile.Write(e.Buffer, 0, e.BytesRecorded);
				waveFile.Flush();
			}
		}

		private void OnRecordingStopped(object sender, StoppedEventArgs e)
		{
			if (waveSource != null)
			{
				waveSource.Dispose();
				waveSource = null;
			}

			if (waveFile != null)
			{
				waveFile.Dispose();
				waveFile = null;
			}

            _isReadToGo = true;
		}
	}
}


