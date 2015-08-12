using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Neo4jClient;
using Neo4jClient.Cypher;
using NUnit.Framework;

namespace helloNeo4j
{
    [TestFixture(Description = "Movie Graph Intro, cf.: http://neo4j.com/docs/stable/cypherdoc-movie-database.html")]
    [Ignore, Explicit]
    class Neo4JLearningTests : IDisposable
    {
        private static readonly IGraphClient Client = GetGraphClient();

        [Test]
        public void TestConnect()
        {
            Client.ServerVersion.Should().BeGreaterOrEqualTo(new Version(2, 2));
        }

        static IGraphClient GetGraphClient(string endpoint = "http://neo4j:admin@localhost:7474/db/data")
        {
            // https://github.com/Readify/Neo4jClient/wiki/connecting
            var client = new GraphClient(new Uri(endpoint));
            client.Connect();

            //SetupTestData(client);


            return client;
        }

        private static void SetupTestData(ICypherGraphClient client)
        {
            /*
            // https://github.com/Readify/Neo4jClient/wiki/cypher-examples#create-a-user-only-if-they-dont-already-exist
            var newUser = new User {Id = 456, Name = "Jim"};
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
            */


            // movie data base
            var queryText = File.ReadAllText("CreateMovieDb.cypher");
            ExecuteCypher(client, queryText);
        }

        private static void ExecuteCypher(ICypherGraphClient client, string queryText)
        {
            // https://github.com/Readify/Neo4jClient/wiki/cypher#manual-queries-highly-discouraged
            var query = new CypherQuery(queryText, new Dictionary<string, object>(), CypherResultMode.Projection);
            ((IRawGraphClient)client).ExecuteCypher(query);
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            //MATCH(a: Person),(m:Movie) OPTIONAL MATCH (a) -[r1] - (), (m) -[r2] - () DELETE a, r1, m, r2
            var query = Client.Cypher
                .Match("(a: Person),(m:Movie)")
                .OptionalMatch("(a) -[r1] - (), (m) -[r2] - ()")
                .Delete("a, r1, m, r2");
            query.ExecuteWithoutResults();
        }
    }
}
