using System;
using System.Diagnostics;

// This project is for testing how to use Process class to call cmd.
namespace process_class_playground
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Now, we're going to start a cmd process...\n");

			Process cmdProcess = new Process();

			string osPlatform = Environment.OSVersion.Platform.ToString();
			string osVersion = Environment.OSVersion.Version.ToString();
			Console.WriteLine("Your machine runs on " + osPlatform);
			// Console.WriteLine(osVersion);

			cmdProcess.StartInfo.FileName = "java";

//			switch (osPlatform)
//			{
//				case "Unix":
//					cmdProcess.StartInfo.FileName = "terminal.app";
//					break;
//				case "Windows":
//					cmdProcess.StartInfo.FileName = "cmd.exe";
//					break;
//				default:
//					Console.WriteLine("error.");
//					break;
//			}

			cmdProcess.StartInfo.Arguments = "-version";
			cmdProcess.Start();
			cmdProcess.StandardOutput.ReadToEnd();
			cmdProcess.WaitForExit();
			cmdProcess.Close();
		}
	}
}