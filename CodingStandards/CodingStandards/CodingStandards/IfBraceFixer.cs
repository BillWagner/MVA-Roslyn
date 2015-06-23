using System;
using System.Composition;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace CodingStandards
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IfBraceFixer)), Shared]
    public class IfBraceFixer : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(IfBraceAnalyzercs.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document
                .GetSyntaxRootAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var statement = root.FindToken(diagnosticSpan.Start)
                .Parent.AncestorsAndSelf()
                .OfType<StatementSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create("Enclose clause in { }", c => MakeBlockAsync(context.Document, statement, c)),
                diagnostic);
        }

        private async Task<Document> MakeBlockAsync(Document document, StatementSyntax ifChildStatement, CancellationToken c)
        {
            var block = SyntaxFactory.Block(ifChildStatement);
            // Replace the old statement with the block:
            var root = await document.GetSyntaxRootAsync();
            var newRoot = root.ReplaceNode((SyntaxNode)ifChildStatement, block);

            var newDocument = document.WithSyntaxRoot(newRoot);
            return newDocument;
        }
    }
}