
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace BlushingPenguin.JsonPath.Test
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class APIRulesTests
    {
        [Test]
        public void AylienRulesTests()
        {
            const string fileName = "aylien-response.json";
            var apiDoc = OpenDocument(fileName);

            var ruleNames = new string[]
            {
                "keywords", "persons", "organizations", "locations", "concepts", "categories", "hashtags"
            };

            var rules = ruleNames.ToDictionary(
                o => o,
                o => apiDoc.SelectToken($"rules.{o}").Value.GetString()
                );

            var concepts = apiDoc.ParseMany(
                "results[?(@.endpoint == 'concepts')].result.concepts.*.surfaceForms[*]",
                e => new
                {
                    @string = e.SelectToken("string")?.GetString(),
                    score= e.SelectToken("score")?.GetSingle()
                }).ToList();

            foreach (var ruleKey in rules.Keys)
            {
                var rule = rules[ruleKey];

                object value = ruleKey.EndsWith("s") ?
                    (object)apiDoc.SelectTokens(rule).Select(o => o.GetString()).ToList() :
                    (object)apiDoc.SelectToken(rule)?.GetString();

            }
        }

        [Test]
        public void IBMWatsonRulesTests()
        {
            const string fileName = "watson-response.json";
            var apiDoc = OpenDocument(fileName);

            var ruleNames = new string[]
            {
                "keywords", "persons", "organizations", "locations", "concepts", "categories"
            };

            var rules = ruleNames.ToDictionary(
                o => o,
                o => apiDoc.SelectToken($"rules.{o}").Value.GetString()
                );

            foreach (var ruleKey in rules.Keys)
            {
                var rule = rules[ruleKey];

                object value = ruleKey.EndsWith("s") ?
                    (object)apiDoc.SelectTokens(rule).Select(o => o.GetString()).ToList() :
                    (object)apiDoc.SelectToken(rule)?.GetString();

            }
        }


        JsonDocument OpenDocument(string fileName) =>
            JsonDocument.Parse(ReadRulesFile(fileName));

        string ReadRulesFile(string fileName) =>
            File.ReadAllText(fileName);
    }
}
