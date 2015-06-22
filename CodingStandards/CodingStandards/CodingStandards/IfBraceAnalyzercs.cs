using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CodingStandards
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfBraceAnalyzercs : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "IfBraceAnalyzercs";
        internal static readonly LocalizableString Title = "IfBraceAnalyzercs Title";
        internal static readonly LocalizableString MessageFormat = "IfBraceAnalyzercs '{0}'";
        internal const string Category = "IfBraceAnalyzercs Category";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
        }
    }
}