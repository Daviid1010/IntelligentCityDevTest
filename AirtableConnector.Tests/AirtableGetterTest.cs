using AirtableConnector.Core;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AirtableConnector.Tests
{
	public class AirtableGetterTest
	{
		AirtableGetter Getter { get; set; } = new AirtableGetter("apphruxl9mXWH7QJJ", "secrets.json");

		// Logger for Tests
		private readonly ITestOutputHelper _output;

		public AirtableGetterTest(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public async void RetrieveDataFromTableAsyncTest()
		{
			Dictionary<string, List<string>> data = await Getter.RetrieveDataFromTablesAsync("Projects");
			_output.WriteLine($"Records Returned Count: {data.Count}");

			Assert.NotNull(data);
		}

		[Fact]
		public void RetrieveDataFromProjectsTableTest()
		{
			Dictionary<string, IntelligentProject> data = Getter.RetrieveDataFromProjectTable();
			_output.WriteLine("Printing Results");
			foreach (var item in data)
			{
				// logging returned data verifying that it is Project Data
				_output.WriteLine($"Key: {item.Key}");
				var project = item.Value;
				_output.WriteLine($"City:  {project.City}");
                _output.WriteLine($"Cost/Sqf:  {project.CostPerSqft}");
                _output.WriteLine($"Gross Floor Area:  {project.GrossFloorArea}");
                _output.WriteLine($"Site Area:  {project.SiteArea}");
                _output.WriteLine($"Total Cost:  {project.TotalCost}");
            }
			
			// check if dicttionary returned is not empty
			Assert.NotEmpty(data);
		}

		[Fact]
		public void RetrieveDataFromClientsTableTest()
		{
			Dictionary<string, IntelligentClient> data = Getter.RetrieveDataFromClientsTable();

            _output.WriteLine("Printing Results");
            foreach (var item in data)
            {
				// logging returned data to verify this is Client Data being returned
                _output.WriteLine($"Key: {item.Key}");
                var client = item.Value;
                _output.WriteLine($"Name:  {client.Name}");
                _output.WriteLine($"Company:  {client.Company}");
                _output.WriteLine($"Phone:  {client.Phone}");
            }

            Assert.NotEmpty(data);
		}

		[Fact]
		public void QueryProjectsByCityTest()
		{
			Dictionary<string, IntelligentProject> data = Getter.QueryProjectsByCity("Vancouver");
			
            _output.WriteLine("Printing Results");

            _output.WriteLine($"Data Count {data.Count}");
            Assert.True(data.Count == 6);

			foreach (var item in data)
			{
				_output.WriteLine("Verifying that each city is equal to Vancouver");
				var city = item.Value.City;
				Assert.Equal("Vancouver", city);
			}
		}
    }
}
