using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using CodingStandards;

namespace CodingStandards.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentLibrary
{
    public class Student
    {
        public string firstName;
        public string lastName;
        public string GetFormattedName()
        {
            var name = firstName;
            name.PadRight(20);
            string.Concat(name, lastName);
            name.PadRight(40);
            return name;
        }
    }
}
";
            var expected = new DiagnosticResult[]
            {
            new DiagnosticResult{
                Id = PureMethodsAnalyzer.DiagnosticId,
                Message = String.Format("The return value from '{0}' is being ignored", "PadRight"),
                Severity = DiagnosticSeverity.Info,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 17, 13)
                        }
            },
            new DiagnosticResult{
                Id = PureMethodsAnalyzer.DiagnosticId,
                Message = String.Format("The return value from '{0}' is being ignored", "Concat"),
                Severity = DiagnosticSeverity.Info,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 18, 13)
                        }
            },
            new DiagnosticResult{
                Id = PureMethodsAnalyzer.DiagnosticId,
                Message = String.Format("The return value from '{0}' is being ignored", "PadRight"),
                Severity = DiagnosticSeverity.Info,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 19, 13)
                        }
            }
            };

            VerifyCSharpDiagnostic(test, expected);
            return;

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";
            VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new PureMethodsFixer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new PureMethodsAnalyzer();
        }
    }
}