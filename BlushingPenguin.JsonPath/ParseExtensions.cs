#region Author Info

/*
 
 Additional Extensions that instantiate objects, authored by Ken Granderson (https://kengranderson.com)
 
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace BlushingPenguin.JsonPath
{
    public static class ParseExtensions
    {
        public static T Parse<T>(this JsonElement document, string path, Func<JsonElement?, T> factory, bool errorWhenNoMatch = false)
        {
            if (factory == null)
            {
                throw new JsonException("null factory passed to Parse!");
            }

            return factory(document.SelectToken(path, errorWhenNoMatch));
        }

        public static IEnumerable<T> ParseMany<T>(this JsonElement document, string path, Func<JsonElement, T> factory, bool errorWhenNoMatch = false)
        {
            if (factory == null)
            {
                throw new JsonException("null factory passed to ParseMany!");
            }

            return document.SelectTokens(path, errorWhenNoMatch).Select(o => factory(o));
        }

        public static T Parse<T>(this JsonDocument document, string path, Func<JsonElement?, T> factory, bool errorWhenNoMatch = false) =>
            document.RootElement.Parse(path, factory, errorWhenNoMatch);

        public static IEnumerable<T> ParseMany<T>(this JsonDocument document, string path, Func<JsonElement, T> factory, bool errorWhenNoMatch = false) =>
            document.RootElement.ParseMany(path, factory, errorWhenNoMatch);
    }
}
