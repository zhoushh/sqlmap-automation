using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sqlmap_automation
{
	public class SqlmapManager : IDisposable
	{
		private SqlmapSession _session = null;

		public SqlmapManager(SqlmapSession session)
		{
			if (session == null)
				throw new ArgumentNullException("session");
			_session = session;
		}

		public string NewTask()
		{
			JToken tkn = JObject.Parse(_session.ExecuteGet("/task/new"));
			return tkn.SelectToken("taskid").ToString();
		}

		public bool DeleteTask(string taskId)
		{
			JToken tkn = JObject.Parse(_session.ExecuteGet("/task/" + taskId + "/delete"));
			return (bool) tkn.SelectToken("success");
		}

		public Dictionary<string, object> GetOptions(string taskId)
		{
			Dictionary<string, object> options = new Dictionary<string, object>();
			JObject tkn = JObject.Parse(_session.ExecuteGet("/option/" + taskId + "/list"));
			tkn = tkn["options"] as JObject;
			foreach (var pair in tkn)
				options.Add(pair.Key, pair.Value);

			return options;
		}

		public bool StartTask(string taskId, Dictionary<string, object> options)
		{
			string json = JsonConvert.SerializeObject(options);
			JToken tkn = JObject.Parse(_session.ExecutePost("/scan/" + taskId + "/start", json));
			return (bool) tkn.SelectToken("success");
		}

		public SqlmapStatus GetScanStatus(string taskId)
		{
			JObject tkn = JObject.Parse(_session.ExecuteGet("/scan/" + taskId + "/status"));
			SqlmapStatus stat = new SqlmapStatus();
			stat.Status = (string) tkn["status"];
			if (tkn["returncode"].Type != JTokenType.Null)
				stat.ReturnCode = (int) tkn["returncode"];

			return stat;
		}

		public List<SqlmapLogItem> GetLog(string taskId)
		{
			JObject tkn = JObject.Parse(_session.ExecuteGet("/scan/" + taskId + "/log"));
			JArray items = tkn["log"] as JArray;
			List<SqlmapLogItem> logItems = new List<SqlmapLogItem>();

			foreach (var item in items)
			{
				SqlmapLogItem i = new SqlmapLogItem();
				i.Message = (string) item["message"];
				i.Level = (string) item["level"];
				i.Time = (string) item["time"];
				logItems.Add(i);
			}

			return logItems;
		}

		public void Dispose()
		{
			_session.Dispose();
			_session = null;
		}
	}
}