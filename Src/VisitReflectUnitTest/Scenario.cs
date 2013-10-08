using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ploeh.TestTypeFoundation;
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
                new AssemblyElement(typeof(TestTypeFoundation.UnguardedMethodHost).Assembly),
                new TypeElement(this.GetType()),
                new TypeElement(typeof(UnguardedMethodHost)),
                new MethodInfoElement((MethodInfo)MethodBase.GetCurrentMethod()),
                new MethodInfoElement(typeof(UnguardedMethodHost).GetMethods().First()));

            Console.WriteLine(assemblyElements
                .Accept(new AssemblyAndTypeAndMethodPrinter())
                .Value
                .Aggregate((x, y) => x + Environment.NewLine + y));
        }

        [Fact]
        public void DumpHierachicalAssemblyAndTypesAndMethods()
        {
            var assemblyElements = new CompositeHierarchicalReflectionElement(
                new AssemblyElement(this.GetType().Assembly),
                new AssemblyElement(typeof(TestTypeFoundation.UnguardedMethodHost).Assembly));

            Console.WriteLine(assemblyElements
                .Accept(new HierarchicalAssemblyAndTypeAndMethodPrinter())
                .Value
                .Aggregate((x, y) => x + Environment.NewLine + y));
        }

        class AssemblyAndTypeAndMethodPrinter : ReflectionVisitor<IList<string>>
        {
            private readonly IList<string> observations = new List<string>();

            public override IList<string> Value
            {
                get { return this.observations; }
            }

            public override IReflectionVisitor<IList<string>> Visit(AssemblyElement assemblyElement)
            {
                observations.Add(AssemblyToString(assemblyElement.Assembly));
                return this;
            }

            public override IReflectionVisitor<IList<string>> Visit(TypeElement typeElement)
            {
                observations.Add(ClassToString(typeElement.Type));
                return this;
            }

            public override IReflectionVisitor<IList<string>> Visit(MethodInfoElement methodInfoElement)
            {
                observations.Add(MethodToString(methodInfoElement.MethodInfo));
                return this;
            }
        }

        class HierarchicalAssemblyAndTypeAndMethodPrinter : HierarchicalReflectionVisitor<IList<string>>
        {
            private readonly List<string> printedStrings = new List<string>();
            private int indent;

            public override IList<string> Value
            {
                get { return this.printedStrings; }
            }

            private void AddIndented(string s)
            {
                printedStrings.Add("".PadLeft(indent) + s);
            }

            public override IHierarchicalReflectionVisitor<IList<string>> EnterAssembly(AssemblyElement assemblyElement)
            {
                AddIndented(AssemblyToString(assemblyElement.Assembly));
                indent++;
                return this;
            }

            public override IHierarchicalReflectionVisitor<IList<string>> ExitAssembly(AssemblyElement assemblyElement)
            {
                indent--;
                return this;
            }

            public override IHierarchicalReflectionVisitor<IList<string>> EnterType(TypeElement typeElement)
            {
                AddIndented(ClassToString(typeElement.Type));
                indent++;
                return this;
            }

            public override IHierarchicalReflectionVisitor<IList<string>> ExitType(TypeElement typeElement)
            {
                indent--;
                return this;
            }

            public override IHierarchicalReflectionVisitor<IList<string>> EnterMethod(MethodInfoElement methodInfoElement)
            {
                AddIndented(MethodToString(methodInfoElement.MethodInfo));
                indent++;
                return this;
            }

            public override IHierarchicalReflectionVisitor<IList<string>> ExitMethod(MethodInfoElement methodInfoElement)
            {
                indent--;
                return this;
            }
        }

        private static string AssemblyToString(Assembly a)
        {
            return string.Format("Assembly: {0}", a);
        }

        private static string MethodToString(MethodInfo m)
        {
            return string.Format("{0}{1}{2}()",
                m.IsPublic ? "public " : (m.IsPrivate ? "private " : m.IsAssembly ? "internal " : " "),
                (m.ReturnType == typeof(void) ? "void" : m.ReturnType.Name) + " ",
                m.Name);
        }

        private static string ClassToString(Type t)
        {
            return string.Format("{0}{1}{2}{3}.{4}",
                t.IsPublic ? "public " : (t.Attributes.HasFlag(TypeAttributes.NestedAssembly) ? "internal " : "private "),
                t.IsAbstract ? "abstract " : "",
                t.IsInterface ? "interface " : "class ",
                t.Namespace,
                t.Name);
        }

    }
}
