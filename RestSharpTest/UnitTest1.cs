using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace RestSharpTest
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public double Salary { get; set; }
    }

    [TestClass]
    public class UnitTest1
    {

        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }

        [TestMethod]
        public void OnCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(15, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.id + "Name: " + item.name + "Salary: " + item.Salary);
            }
        }

        [TestMethod]
        public void GivenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Clark");
            jObjectbody.Add("Salary", "15000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual(15000, dataResponse.Salary);
        }

        [TestMethod]
        public void GivenMultipleEmployee_OnPost_ShouldReturnCountOfEmployees()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject []jObjectbody = new JObject[2];
            JObject obj = new JObject();
            obj.Add("name", "Baba Ka Dhaba");
            obj.Add("Salary", "15000");
            jObjectbody[0] = obj;
            obj = new JObject();
            obj.Add("name", "Swad Official");
            obj.Add("Salary", "16000");
            jObjectbody[1] = obj;
            var json = JsonConvert.SerializeObject(jObjectbody);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            request.AddJsonBody(jObjectbody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            Employee []dataResponse = JsonConvert.DeserializeObject<Employee[]>(response.Content);
            Assert.AreEqual("Baba Ka Dhaba", dataResponse[0].name);
            Assert.AreEqual(15000, dataResponse[0].Salary);
            Assert.AreEqual("Swad Official", dataResponse[1].name);
            Assert.AreEqual(16000, dataResponse[1].Salary);
        }
    }
}
