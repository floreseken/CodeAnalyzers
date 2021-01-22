using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using VerifyCS = CodeAnalyzers.Test.CSharpAnalyzerVerifier<CodeAnalyzers.NetFrameworkVersionAnalyzer>;

namespace CodeAnalyzers.Test
{
    [TestClass]
    public class NetFrameworkVersionAnalyzerUnitTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }


        [TestMethod]
        public async Task TestAgain()
        {
            var test = new Mock<AnalysisContext>();

            var myAnalyzer = new NetFrameworkVersionAnalyzer();
            myAnalyzer.Initialize(test.Object);


        }

    }
}
