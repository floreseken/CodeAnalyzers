using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace CodeAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NetFrameworkVersionAnalyzer : DiagnosticAnalyzer
    {
        private readonly string[] _allowedFrameworkVersions = { "2.1", "3.1" };

        private const string CoreFrameworkString = ".NETCoreApp,Version=v";
        private const string DiagnosticId = "MyRule0001";
        private const string Title = "Application framework not allowed";
        private const string MessageFormat = "The application '{0}' has the attribute: '{1}', that does not match with the supported framework versions: '{2}'";
        private const string Description = "Right now we are only supporting applications targeting a LTS .NETCore version.";
        private const string HelpLink = "https://dotnet.microsoft.com/platform/support/policy/dotnet-core";
        private const string Category = "CustomRules.Maintenability";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Error,
            true,
            description: Description,
            helpLinkUri: HelpLink);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterCompilationAction(AnalyzeMethod);
        }

        private void AnalyzeMethod(CompilationAnalysisContext context)
        {
            var attrs = context.Compilation.Assembly.GetAttributes()
                .FirstOrDefault(attr => attr.ToString().Contains("TargetFrameworkAttribute"));

            if (attrs != null)
            {
                var attrString = attrs.ToString();

                var found = _allowedFrameworkVersions
                    .Select(version => $"{CoreFrameworkString}{version}")
                    .Any(pattern => attrString.Contains(pattern));

                if (!found)
                {
                    var diagnostic = Diagnostic.Create(
                        Rule,
                        Location.None,
                        context.Compilation.AssemblyName,
                        attrString,
                        _allowedFrameworkVersions.Aggregate((result, next) => $"{next}, {result}"));

                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
