﻿using System;
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
        public const string DiagnosticId = "IfBraceAnalyzer";
        internal static readonly LocalizableString Title = "If and else clauses must be surrounded by braces";
        internal static readonly LocalizableString MessageFormat = "Code: '{0}' is not surrounded by braces";
        internal const string Category = "CodingStandards";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
        }
    }
}