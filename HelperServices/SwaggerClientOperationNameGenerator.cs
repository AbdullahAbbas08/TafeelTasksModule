using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HelperServices
{
    public class SwaggerClientOperationNameGenerator : IOperationNameGenerator
    {
        public bool SupportsMultipleClients { get; } = true;

        /// <summary>Converts the path to an operation name.</summary>
        /// <param name="path">The HTTP path.</param>
        /// <returns>The operation name.</returns>
        public static string ConvertPathToName(string path)
        {
            var name = Regex.Replace(path, @"\{.*?\}", "")
                .Split('/', '-', '_')
                .Where(part => !part.Contains("{") && !string.IsNullOrWhiteSpace(part))
                .Aggregate("", (current, part) =>
                {
                    part.Replace("get", "Get");
                    part.Replace("post", "Post");
                    part.Replace("delete", "Delete");
                    part.Replace("update", "Update");
                    return (current + CapitalizeFirst(part));
                });

            if (string.IsNullOrEmpty(name))
            {
                name = "Index"; // Root path based operation?
            }

            return name;
        }

        /// <summary>Capitalizes first letter.</summary>
        /// <param name="name">The name to capitalize.</param>
        /// <returns>Capitalized name.</returns>
        internal static string CapitalizeFirst(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            var capitalized = name;
            return char.ToUpperInvariant(capitalized[0]) + (capitalized.Length > 1 ? capitalized.Substring(1) : "");
        }



        public string GetClientName(SwaggerDocument document, string path, SwaggerOperationMethod httpMethod, SwaggerOperation operation)
        {
            return string.Empty;
        }

        public string GetOperationName(SwaggerDocument document, string path, SwaggerOperationMethod httpMethod, SwaggerOperation operation)
        {
            var operationName = ConvertPathToName(path);
            //var hasNameConflict = document.Paths
            //    .SelectMany(pair => pair.Value.Select(p => new { Path = pair.Key.Trim('/'), HttpMethod = p.Key, Operation = p.Value }))
            //    .Where(op =>
            //        GetClientName(document, op.Path, op.HttpMethod, op.Operation) == GetClientName(document, path, httpMethod, operation) &&
            //        ConvertPathToName(op.Path) == operationName
            //    )
            //    .ToList().Count > 1;

            //if (hasNameConflict)
            //{
            operationName += CapitalizeFirst(httpMethod.ToString());
            //}
            if (string.IsNullOrEmpty(httpMethod.ToString()))
            {
                Console.WriteLine("egtrer");
            }

            return operationName;
        }
    }
}
