using System;
using System.Collections.Generic;

namespace sqlmap_automation
{
	class Program
	{
		static void Main(string[] args)
		{
//			string host = args[0];
//			int port = Int32.Parse(args[1]);
			
			// hard-code host & port for sqlmap api
			using (SqlmapManager mngr = new SqlmapManager(new SqlmapSession("127.0.0.1", 8775)))
			{
				string taskId = mngr.NewTask();
				Dictionary<string, object> options = mngr.GetOptions(taskId);

				Console.WriteLine("Created task: " + taskId + "\n");

//				foreach (var pair in options)
//					Console.WriteLine("Key: " + pair.Key + "\t:: Value: " + pair.Value);

				// set options for the scan
				options["url"] = args[0];
				options["flushSession"] = true; // start a new scan for the same target, ignoring the former scans.
				options["cookie"] = args[1];

				// start scan
				mngr.StartTask(taskId, options);

				SqlmapStatus stat = mngr.GetScanStatus(taskId);
				while (stat.Status != "terminated")
				{
					System.Threading.Thread.Sleep(new TimeSpan(0, 0, 10));
					stat = mngr.GetScanStatus(taskId);
				}

				Console.WriteLine("Scan finished! \n\n Printing log...");

				// print log
				List<SqlmapLogItem> logItems = mngr.GetLog(taskId);
				foreach (SqlmapLogItem item in logItems)
					Console.WriteLine(item.Message);

				// Delete task and check the result.
				Console.WriteLine("\nDeleting task...");
				try
				{
					Console.WriteLine("Delete successful: " + mngr.DeleteTask(taskId));
				}
				catch (Exception e)
				{
					Console.WriteLine("Error: " + e);
					throw;
				}
				finally
				{
					Console.WriteLine("\nProcess Ends");
				}
			}
		}
	}
}