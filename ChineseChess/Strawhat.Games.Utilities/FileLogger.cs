/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 04/13/2013
 * Time: 12:08
 */
using System;
using System.Diagnostics;
using System.IO;

namespace Strawhat.Games.Utilities
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
		
		private FileInfo CheckLogFile(string logName = "Log")
		{
            string currentDir = GameResourceFile.CURRENT_DIR;
            string logFileName = string.Format("{0}{1}{2}\\{3}.log", currentDir, Path.DirectorySeparatorChar, StringResources._FOLDERS_LOGS_, logName);
			string logDirName = string.Format("{0}{1}{2}", currentDir, Path.DirectorySeparatorChar, StringResources._FOLDERS_LOGS_);
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
                file.CopyTo(string.Format("{2}\\{0}{1}.log", logName, DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss"), StringResources._FOLDERS_LOGS_), true);
				using (var stream = file.OpenWrite())
				{
					stream.Write(new byte[]{}, 0, 0);
				}
			}
			
			return file;
		}
	}
}
