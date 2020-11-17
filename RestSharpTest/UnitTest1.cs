using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;

namespace RestSharpTest
{
    /// <summary>
    /// The class is written to store the salary and name of a person
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// A property to store the id of a person
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// A property to store the name of a person
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// A property to store the salary of a person
        /// </summary>
        public double Salary { get; set; }
    }

    /// <summary>
    /// A class to test the address book json server
    /// </summary>
    [TestClass]
    public class UnitTest1
    {

        RestClient client;
        /// <summary>
        /// A method to initialize the restclient object with the json server address
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }

        /// <summary>
        /// This function is used to request the json server to send the data stored
        /// </summary>
        /// <returns>An IRestResponse object that contains the response sent by the json server</returns>
        private IRestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// This test method gets the data stored in the server and prints the output
        /// </summary>
        [TestMethod]
        public void OnCallingGETApi_ReturnEmployeeList()
        {
            IRestResponse response = getEmployeeList();
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(14, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.id + "Name: " + item.name + "Salary: " + item.Salary);
            }
        }

        /// <summary>
        /// This test method "POST's" the data into the json server
        /// </summary>
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

        /// <summary>
        /// This test method "POST's" the data into the json server
        /// </summary>
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
            for(int i=0;i<2;i++)
            {
                request.AddParameter("application/json", jObjectbody[i], ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
                Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
                if(i==0)
                {
                    Assert.AreEqual("Baba Ka Dhaba", dataResponse.name);
                    Assert.AreEqual(15000, dataResponse.Salary);
                }
            }
        }

        /// <summary>
        /// This test method updates a contact's name based on the id
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnPut_ShouldReturnUpdatedEmployee()
        {
            RestRequest request = new RestRequest("/employees/5", Method.PUT);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "lavanya");
            jObjectbody.Add("Salary", "5000");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("lavanya", dataResponse.name);
            Assert.AreEqual(5000, dataResponse.Salary);
        }

        /// <summary>
        /// This test method deletes a contact's name based on the id
        /// </summary>
        [TestMethod]
        public void GivenEmployee_OnDelete_ShouldReturnSuccessStatus()
        {
            RestRequest request = new RestRequest("/employees/4", Method.DELETE);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("name", "Daas");
            jObjectbody.Add("Salary", "500");
            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        }
    }
}
