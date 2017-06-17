using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace TlDr.Core
{
	public class RecorderService
	{
		public static WaveInEvent waveSource = null;
		public static WaveFileWriter waveFile = null;
		public static int i = 1;
		public const string fileName = "wave_{0}.wav";

		/// <summary>
		/// Lance le record de la Wave
		/// </summary>
		public void Start()
		{
		    waveSource = new WaveInEvent();
			waveSource.WaveFormat = new WaveFormat(16000, 1);

			waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(OnDataAvailable);
			waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(OnRecordingStopped);

            waveFile = new WaveFileWriter(string.Format(fileName,"001"), waveSource.WaveFormat);

            waveSource.StartRecording();
		}

		/// <summary>
		/// Arrete le record de la Wave
		/// Envoit le fichier WaveFileName a RecognitionSpeakerService
		/// </summary>
		/// <returns>WaveFileName</returns>
		public string Stop()
		{
			waveSource.StopRecording();
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

		private static void OnRecordingStopped(object sender, StoppedEventArgs e)
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
		}
	}
}

