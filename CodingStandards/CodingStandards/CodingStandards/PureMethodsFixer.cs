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

            var memberAccess = root.FindNode(context.Span)?.FirstAncestorOrSelf<MemberAccessExpressionSyntax>();

            var receiver = memberAccess.Expression;
            if (receiver.IsKind(SyntaxKind.IdentifierName))
            {
                context.RegisterCodeFix(
                CodeAction.Create("Assign method return to receiver",
                c => AssignMethodReturnToReceiverAsync(context, context.Document, statement, 
                receiver.ToString(), c)),
                diagnostic);
            } else // static method
            {
                var invocation = (statement
                    .ChildNodes().First() as InvocationExpressionSyntax);
                var firstArgument = invocation.ArgumentList.Arguments.First();
                context.RegisterCodeFix(
                CodeAction.Create("Assign method return to first parameter",
                c => AssignMethodReturnToReceiverAsync(context, context.Document, statement,
                firstArgument.ToString(), c)),
                diagnostic);

            }
        }

        private async Task<Document> AssignMethodReturnToReceiverAsync(CodeFixContext context, Document document, ExpressionStatementSyntax statement, string receiverName, CancellationToken c)
        {
            var rValueExpr = statement.ToString();
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            var memberAccess = root.FindNode(context.Span)?.FirstAncestorOrSelf<MemberAccessExpressionSyntax>();

            var declaration = SyntaxFactory
                .ParseStatement($"{receiverName} = {rValueExpr}");

            var formattedDeclaration = declaration.WithTriviaFrom(statement);
            // Replace the old statement with the block:
            var newRoot = root.ReplaceNode((SyntaxNode)statement, formattedDeclaration);

            var newDocument = document.WithSyntaxRoot(newRoot);
            return newDocument;

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