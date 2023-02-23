using AirtableApiClient;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AirtableConnector.Core
{
	public class AirtableGetter
	{
		public AirtableBase Base { get; set; }

		public AirtableGetter(string baseId, string apiKeyFile)
		{
			Base = new AirtableBase(ReadApiKey(apiKeyFile), baseId);
		}

		// <summary> Function which reads the API key from a json file allowing access to Intelligent Cities instance of Airtable</summary>
		// <param name="apiKeyFile"> directory location of the json file containing the Airtable Secrets Key </param>
		// <param name="key"> Default Airtable, key of the api key value within the file </param>
		// <returns> API Key for Airtable. This should be a string </returns>
		private static string ReadApiKey(string apiKeyFile, string key = "Airtable")
		{
			// Read the entire file
			string json = File.ReadAllText(apiKeyFile);

			// Parse the JSON data into a C# object
			JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			JsonObject data = JsonSerializer.Deserialize<JsonObject>(json, options);
			return data[key].ToString();
		}

		// <summary>Function which gets all records within an Airtable table, for a specific table name</summary>
		// <param name="tablename">The name of the table to be queired. This should a string</param>
		// <returns></returns> A Dictionary where the keys are record IDs and the values are lists of field values for each record </returns>
		public Dictionary<string, List<string>> RetrieveDataFromTables(string tableName)
		{
			Dictionary<string, List<string>> data = new();
			var task= Base.ListRecords(tableName);
			task.Wait();
			var  records = task.Result.Records;
			foreach ( var record in records )
			{
				var key = record.Id;
				var value = new List<string>();
				foreach ( var field in record.Fields )
				{
					value.Add(field.Key + ": " +field.Value.ToString());
				}
				data.Add(key, value);
			}

            return data;
		}

        // <summary>Function which gets all Project Records in Airtable</summary>
        // <returns></returns> A Dictionary where the keys are record IDs and the values are Project objects </returns>
        public Dictionary<string, IntelligentProject> RetrieveDataFromProjectTable()

		{
			Dictionary<string, IntelligentProject> data = new();
			var task = Base.ListRecords("Projects");
			task.Wait();
			var records = task.Result.Records;
			foreach ( var record in records )
			{
				var key = record.Id;
                IntelligentProject project = new IntelligentProject
                {
                    ProjectId = int.Parse(record.Fields["Project Id"].ToString()),
					City = record.Fields["City"].ToString(),
					SiteArea = int.Parse(record.Fields["Site Area"].ToString()),
					CostPerSqft = int.Parse(record.Fields["Cost/Sqft"].ToString()),
					GrossFloorArea = int.Parse(record.Fields["Gross Floor Area"].ToString()),
                    TotalCost = int.Parse(record.Fields["Gross Floor Area"].ToString())
                };
                data.Add(key, project);
            }
			return data;
		}

        // <summary>Function which gets all Clients from the Airtable</summary>
        // <returns></returns> A Dictionary where the keys are record IDs from Airtable and the values are the associated Client for each</returns>
        public Dictionary<string, IntelligentClient> RetrieveDataFromClientsTable()
        {
            Dictionary<string, IntelligentClient> data = new Dictionary<string, IntelligentClient>();
            var task = Base.ListRecords("Clients");
            task.Wait();
            var records = task.Result.Records;
            foreach (var record in records)
            {
                var key = record.Id;
                IntelligentClient project = new IntelligentClient
                {
                    Name = record.Fields["Name"].ToString(),
					Company = record.Fields["Company"].ToString(),
					Phone =  record.Fields["Phone"].ToString(),
                };
                data.Add(key, project);
            }


            return data;
        }

        // <summary>Function which gets all records within the Projects table which have a specific city</summary>
        // <param name="tablename">The name of the city to be quieried in the Projects table. This should a string</param>
        // <returns></returns> A Dictionary where the keys are record IDs and the values are lists of field values for each record. Keys will be the city</returns>
        public Dictionary<string, IntelligentProject> QueryProjectsByCity(string cityName)
		{
			Dictionary<string, IntelligentProject> data = new();
			// filterFormula takes the format of "{field_name} = {field_value}"
			var filterFormula = "{city} = " +  $"'{cityName}'";
			var task = Base.ListRecords(tableName: "Projects", filterByFormula: filterFormula);
			task.Wait();

            var records = task.Result.Records;
            foreach (var record in records)
            {
                var key = record.Id;
                IntelligentProject project = new IntelligentProject 
				{
					ProjectId = int.Parse(record.Fields["Project Id"].ToString()),
					City = record.Fields["City"].ToString(),
					SiteArea = int.Parse(record.Fields["Site Area"].ToString()),
					CostPerSqft = int.Parse(record.Fields["Cost/Sqft"].ToString()),
					GrossFloorArea = int.Parse(record.Fields["Gross Floor Area"].ToString()),
                    TotalCost = int.Parse(record.Fields["Gross Floor Area"].ToString())
                };
				data.Add(key , project);
            }
            return data;
		}
	}
}
