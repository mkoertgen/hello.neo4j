using System;
using System.Linq;
using FluentAssertions;
using Neo4jClient;
using NUnit.Framework;

namespace helloNeo4j
{
    [TestFixture]
    class Neo4JLearningTests
    {
        private static readonly IGraphClient Client = GetGraphClient();

        [Test]
        public void TestConnect()
        {
            Client.ServerVersion.Should().BeGreaterOrEqualTo(new Version(2, 2));
        }

        public class Book
        {
            public string Title { get; set; }
            public int Pages { get; set; }
        }

        public class User
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
        }

        [Test(Description = "Get specific user, cf.: https://github.com/Readify/Neo4jClient/wiki/cypher-examples#get-specific-user")]
        public void TestSimpleQuery()
        {
            var jim = Client.Cypher
                .Match("(user:User)")
                .Where((User user) => user.Id == 456)
                .Return(user => user.As<User>())
                .Results.Single();
            jim.Id.Should().Be(456);
        }

        static IGraphClient GetGraphClient(string endpoint = "http://neo4j:admin@localhost:7474/db/data")
        {
            // https://github.com/Readify/Neo4jClient/wiki/connecting
            var client = new GraphClient(new Uri(endpoint));
            client.Connect();

            // https://github.com/Readify/Neo4jClient/wiki/cypher-examples#create-a-user-only-if-they-dont-already-exist
            var newUser = new User { Id = 456, Name = "Jim" };
            client.Cypher
                .Merge("(user:User { Id: {id} })")
                .OnCreate()
                .Set("user = {newUser}")
                .WithParams(new
                {
                    id = newUser.Id,
                    newUser
                })
                .ExecuteWithoutResults();

            return client;
        }
    }
}
