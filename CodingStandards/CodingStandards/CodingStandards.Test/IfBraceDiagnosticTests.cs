using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHelper;

namespace CodingStandards.Test
{
    [TestClass]
    public class IfBraceDiagnosticTests : CodeFixVerifier
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
        public void IfStatementWithoutBraces()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b1 = true;

            if (b1)
                Console.WriteLine(""b1"");
        }
    }
}";
            var expected = new DiagnosticResult
            {
                Id = IfBraceAnalyzercs.DiagnosticId,
                Message = string.Format("Code: '{0}' is not surrounded by braces", "true clause"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 17, 17)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b1 = true;

            if (b1)
            {
                Console.WriteLine(""b1"");
            }
        }
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic finds already corrected code.
        [TestMethod]
        public void IfStatementWithBraces()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b2 = true;

            if (b2)
            {
                Console.WriteLine(""b2"");
            }
        }
    }
}";
            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void IfElseStatementWithoutBraces()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b3 = true;

            if (b3)
                Console.WriteLine(""b3"");
            else
                Console.WriteLine(""not b3"");
        }
    }
}";
            var expectedTrue = new DiagnosticResult
            {
                Id = IfBraceAnalyzercs.DiagnosticId,
                Message = string.Format("Code: '{0}' is not surrounded by braces", "true clause"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 17, 17)
                        }
            };
            var expectedFalse = new DiagnosticResult
            {
                Id = IfBraceAnalyzercs.DiagnosticId,
                Message = string.Format("Code: '{0}' is not surrounded by braces", "false (else) clause"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 19, 17)
                        }
            };

            VerifyCSharpDiagnostic(test, expectedTrue, expectedFalse);

            var fixtest = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b3 = true;

            if (b3)
            {
                Console.WriteLine(""b3"");
            }
            else
            {
                Console.WriteLine(""not b3"");
            }
        }
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        [TestMethod]
        public void ElseStatementWithoutBraces()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b4 = true;

            if (b4)
            {
                Console.WriteLine(""b4"");
            }
            else
                Console.WriteLine(""not b4"");
        }
    }
}";
            var expectedFalse = new DiagnosticResult
            {
                Id = IfBraceAnalyzercs.DiagnosticId,
                Message = string.Format("Code: '{0}' is not surrounded by braces", "false (else) clause"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 21, 17)
                        }
            };

            VerifyCSharpDiagnostic(test, expectedFalse);

            var fixtest = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b4 = true;

            if (b4)
            {
                Console.WriteLine(""b4"");
            }
            else
            {
                Console.WriteLine(""not b4"");
            }
        }
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        [TestMethod]
        public void IfWithoutBracesElseStatementWithBraces()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b5 = true;

            if (b5)
                Console.WriteLine(""b5"");
            else
            {
                Console.WriteLine(""not b5"");
            }
        }
    }
}";
            var expectedTrue = new DiagnosticResult
            {
                Id = IfBraceAnalyzercs.DiagnosticId,
                Message = string.Format("Code: '{0}' is not surrounded by braces", "true clause"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 17, 17)
                        }
            };
            VerifyCSharpDiagnostic(test, expectedTrue);

            var fixtest = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b5 = true;

            if (b5)
            {
                Console.WriteLine(""b5"");
            }
            else
            {
                Console.WriteLine(""not b5"");
            }
        }
    }
}";
            VerifyCSharpFix(test, fixtest);
        }

        //Diagnostic finds already corrected code.
        [TestMethod]
        public void IfElseStatementWithBraces()
        {
            var test = @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCode
{
    class Program
    {
        static void Main(string[] args)
        {
            bool b6 = true;

            if (b6)
            {
                Console.WriteLine(""b6"");
            }
            else
            {
                Console.WriteLine(""not b6"");
            }
        }
    }
}";
            VerifyCSharpDiagnostic(test);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new IfBraceFixer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new IfBraceAnalyzercs();
        }
    }
}
