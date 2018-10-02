using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmulatorDemo;
using EmulatorDemo.Models;
using System.Threading.Tasks;
using System.Linq;

namespace EmulatorDemoTests
{
    [TestClass]
    public class CosmosDBTest
    {
        private string _EndpointUrl = null;
        private string _AuthKey = null;
        private string _DatabaseName = null;
        private string _CollectionName = null;

        public TestContext TestContext { get; set; }

        public CosmosDBTest()
        {
        }

        [TestInitialize()]
        public void CosmosDBInitialize()
        {
            this._EndpointUrl = TestContext.Properties["endpoint"].ToString();
            this._AuthKey = TestContext.Properties["authKey"].ToString();
            this._DatabaseName = TestContext.Properties["database"].ToString();
            this._CollectionName = TestContext.Properties["collection"].ToString();
            DocumentDBRepository<Person>.Initialize(this._EndpointUrl, this._AuthKey, this._DatabaseName, this._CollectionName);
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }


        [TestMethod]
        public async Task TestInsertDocuments()
        {
            var document = await DocumentDBRepository<Person>.CreateItemAsync(new Person
            {
                Age = 38,
                FirstName = "Andreas",
                LastName = "Pollak"
            });
            Assert.IsNotNull(document);
            Assert.IsFalse(string.IsNullOrEmpty(document.Id));
            var person = (await DocumentDBRepository<Person>.GetItemsAsync(p => p.LastName == "Pollak")).FirstOrDefault();
            Assert.IsNotNull(person);
            Assert.IsTrue(person.FirstName == "Andreas");
            Assert.IsTrue(person.LastName == "Pollak");
            Assert.IsTrue(person.Age == 38);
            await DocumentDBRepository<Person>.DeleteItemAsync(document.Id);
        }
    }
}
