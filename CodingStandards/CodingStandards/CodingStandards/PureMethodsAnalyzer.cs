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
    public class PureMethodsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PureMethodsAnalyzer";
        internal static readonly LocalizableString Title = "Ignored Method return code";
        internal static readonly LocalizableString MessageFormat = "The return value from '{0}' is being ignored";
        internal const string Category = "CodingStandards";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node;

            // Rule:  Look at the syntax only first. It will make our analyzer faster,
            // because the compiler does not do as much work.
            var memberAccessExpr = invocationExpression.Expression as MemberAccessExpressionSyntax;

            // Likely, we'll use this a lot because it's how we get information in the model
            var memberSymbol = context.SemanticModel.GetSymbolInfo(memberAccessExpr)
                .Symbol as IMethodSymbol;
            if (memberSymbol.ReturnsVoid)
            {
                // This method returns nothing. short circuit and exit
            }
        }
    }
}