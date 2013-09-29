using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ploeh.VisitReflect.UnitTest
{
    public class Scenario
    {
        [Fact]
        public void DumpAssemblyAndTypesAndMethods()
        {
            var assemblyElements = new CompositeReflectionElement(
                new AssemblyElement(this.GetType().Assembly),
                new AssemblyElement(typeof(TestTypeFoundation.UnguardedMethodHost).Assembly));

            Console.WriteLine(assemblyElements
                .Accept(new AssemblyAndTypeAndMethodPrinter())
                .Value
                .Aggregate((x, y) => x + Environment.NewLine + y));
        }

        class AssemblyAndTypeAndMethodPrinter : ReflectionVisitor<IList<string>>
        {
            private readonly List<string> printedStrings = new List<string>();
            private int indent;

            public override IList<string> Value
            {
                get { return this.printedStrings; }
            }

            public override IReflectionVisitor<IList<string>> EnterAssembly(AssemblyElement assemblyElement)
            {
                printedStrings.Add(string.Format("{0}Assembly: {1}",
                    "".PadLeft(indent), assemblyElement.Assembly));
                indent++;
                return this;
            }

            public override IReflectionVisitor<IList<string>> ExitAssembly(AssemblyElement assemblyElement)
            {
                indent--;
                return this;
            }

            public override IReflectionVisitor<IList<string>> EnterType(TypeElement typeElement)
            {
                var t = typeElement.Type;
                printedStrings.Add(string.Format("{0}{1}{2}{3}{4}.{5}",
                    "".PadLeft(indent),
                    t.IsAbstract ? "public " : "",
                    t.IsAbstract ? "abstract " : "",
                    t.IsInterface ? "interface " : "class ",
                    t.Namespace,
                    t.Name));
                indent++;
                return this;
            }

            public override IReflectionVisitor<IList<string>> ExitType(TypeElement typeElement)
            {
                indent--;
                return this;
            }

            public override IReflectionVisitor<IList<string>> EnterMethod(MethodInfoElement methodInfoElement)
            {
                printedStrings.Add(string.Format("{0}{1}.{2}()",
                    "".PadLeft(indent),
                    methodInfoElement.MethodInfo.ReflectedType,
                    methodInfoElement.MethodInfo.Name));

                indent++;
                return this;
            }

            public override IReflectionVisitor<IList<string>> ExitMethod(MethodInfoElement methodInfoElement)
            {
                indent--;
                return this;
            }
        }
    }
}
