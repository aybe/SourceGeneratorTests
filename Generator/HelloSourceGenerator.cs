using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Generator;

[Generator]
public class HelloSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var assemblySymbols = context.Compilation.SourceModule.ReferencedAssemblySymbols;
        var assemblySymbol = assemblySymbols.Single(s => s.Name.StartsWith("Library"));
        var assemblyMembers = assemblySymbol.GlobalNamespace.GetMembers();

        foreach (var namespaceOrTypeSymbol in assemblyMembers)
        {
            var symbols = namespaceOrTypeSymbol.GetMembers();

            foreach (var symbol in symbols)
            {
                if (symbol is INamedTypeSymbol namedTypeSymbol)
                {
                    var members = namedTypeSymbol.GetMembers();

                    foreach (var member in members)
                    {
                        if (member is IMethodSymbol methodSymbol)
                        {
                            var attributes = methodSymbol.GetAttributes();

                            foreach (var attribute in attributes)
                            {
                                var s = attribute.AttributeClass?.ToString();
                                if (s == "Library.TestAttribute")
                                {
                                    Console.WriteLine(member);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }
}