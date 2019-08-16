using System;
using Newtonsoft.Json.Linq;

namespace sqlmap_automation
{
	class Program
	{
		static void Main(string[] args)
		{
			string host = args[0];
			int port = int.Parse(args[1]);
			using (SqlmapSession session = new SqlmapSession(host, port))
			{
				string response = session.ExecuteGet("/task/new");
				JToken token = JObject.Parse(response);
				string taskId = token.SelectToken("taskid").ToString();

				Console.WriteLine("New task ID: " + taskId);
				Console.WriteLine("Deleting task:" + taskId);

				response = session.ExecuteGet("/task/" + taskId + "/delete");
				token = JObject.Parse(response);
				bool isSuccessful = (bool) token.SelectToken("success");

				Console.WriteLine("Delete successful: " + isSuccessful);
			}
		}
	}
}