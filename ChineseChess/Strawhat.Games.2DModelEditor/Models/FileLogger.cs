/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 04/13/2013
 * Time: 12:08
 */
using System;
using System.Diagnostics;
using System.IO;

namespace Strawhat.Games._2DModelEditor.Models
{
	/// <summary>
	/// Description of FileLogger.
	/// </summary>
	public class FileLogger : TraceListener
	{
		public override void WriteLine(string message)
		{
			var file = CheckLogFile();
			using (StreamWriter sw = file.AppendText())
			{
				sw.WriteLine(message);
			}
		}
		
		public override void Write(string message)
		{
			var file = CheckLogFile();
			using (StreamWriter sw = file.AppendText())
			{
				sw.Write(message);
			}
		}
		
		private FileInfo CheckLogFile()
		{
			string currentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
			string logFileName = currentDir + Path.DirectorySeparatorChar + "Logs\\Log.log";
			string logDirName = currentDir + Path.DirectorySeparatorChar + "Logs";
			if (!Directory.Exists(logDirName))
			{
				Directory.CreateDirectory(logDirName);
			}
			
			if (!File.Exists(logFileName))
			{
				using (File.Create(logFileName));
			}
			
			FileInfo file = new FileInfo(logFileName);
			if (file.Length >= 0x300000)
			{
				file.CopyTo(string.Format("Logs\\Log{0}.log", DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")), true);
				using (var stream = file.OpenWrite())
				{
					stream.Write(new byte[]{}, 0, 0);
				}
			}
			
			return file;
		}
	}
}
