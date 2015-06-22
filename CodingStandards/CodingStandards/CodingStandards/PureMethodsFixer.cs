﻿using System;
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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PureMethodsFixer)), Shared]
    public class PureMethodsFixer : CodeFixProvider
    {
        // TODO: Replace with actual diagnostic id that should trigger this fix.

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(PureMethodsAnalyzer.DiagnosticId); }
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
                .OfType<ExpressionStatementSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create("Assign method return to new variable", c => AssignMethodReturnToNewVariable(context.Document, statement, c)),
                diagnostic);

            context.RegisterCodeFix(
                CodeAction.Create("Assign method return to first parameter", c => AssignMethodReturnToFirstParameter(context.Document, statement, c)),
                diagnostic);
        }

        private Task<Document> AssignMethodReturnToFirstParameter(Document document, ExpressionStatementSyntax statement, CancellationToken c)
        {
            throw new NotImplementedException();
        }

        private async Task<Document> AssignMethodReturnToNewVariable(Document document, ExpressionStatementSyntax statement, CancellationToken c)
        {
            var rValueExpr = statement.ToString();

            var declaration = SyntaxFactory.ParseStatement($"var returnValue = {rValueExpr}");

            var formattedDeclaration = declaration.WithTriviaFrom(statement);
            // Replace the old statement with the block:
            var root = await document.GetSyntaxRootAsync();
            var newRoot = root.ReplaceNode((SyntaxNode)statement, formattedDeclaration);

            var newDocument = document.WithSyntaxRoot(newRoot);
            return newDocument;

        }
    }
}