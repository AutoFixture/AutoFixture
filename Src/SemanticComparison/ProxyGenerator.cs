using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Ploeh.SemanticComparison
{
    internal class ProxyGenerator<TSource>
    {
        private const string assemblyName = "SemanticComparisonGeneratedAssembly";

        internal TSource OverrideEquals(TSource value, IEqualityComparer comparer)
        {
            TypeBuilder builder = BuildType(BuildModule(BuildAssembly(assemblyName)));
            FieldBuilder equals = BuildFieldComparer(builder);

            BuildConstructors(builder, equals);
            BuildMethodEquals(builder, BuildFieldEqualsHasBeenCalled(builder), equals);

            var proxy = (TSource)Activator.CreateInstance(
                builder.CreateType(),
                new object[] { comparer });
            CopyProperties(source: value, destination: proxy);
            return proxy;
        }

        private static AssemblyBuilder BuildAssembly(string assemblyName)
        {
            return AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndSave);
        }

        private static ModuleBuilder BuildModule(AssemblyBuilder ab)
        {
            return ab.DefineDynamicModule(assemblyName, assemblyName + ".dll");
        }

        private static TypeBuilder BuildType(ModuleBuilder mb)
        {
            TypeBuilder type = mb.DefineType(
                typeof(TSource).Name + "Proxy" + Guid.NewGuid().ToString().Replace("-", ""),
                TypeAttributes.Public,
                typeof(TSource));
            return type;
        }

        private static FieldBuilder BuildFieldEqualsHasBeenCalled(TypeBuilder type)
        {
            FieldBuilder field = type.DefineField(
                "equalsHasBeenCalled",
                typeof(Boolean),
                  FieldAttributes.Private
                );
            return field;
        }

        private static FieldBuilder BuildFieldComparer(TypeBuilder type)
        {
            FieldBuilder field = type.DefineField(
                "comparer",
                typeof(IEqualityComparer),
                  FieldAttributes.Private
                );
            return field;
        }

        private static MethodBuilder BuildConstructors(TypeBuilder type, FieldInfo comparer)
        {
            var methodAttributes = MethodAttributes.Public| MethodAttributes.HideBySig;
            MethodBuilder method = type.DefineMethod(".ctor", methodAttributes);
            ConstructorInfo ctor = typeof(TSource).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { },
                null
                );

            method.SetReturnType(typeof(void));
            method.SetParameters(typeof(IEqualityComparer));
            method.DefineParameter(1, ParameterAttributes.None, "comparer");
            
            ILGenerator gen = method.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Call, ctor);
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stfld, comparer);
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ret);

            return method;
        }

        private static MethodBuilder BuildMethodEquals(TypeBuilder type, FieldInfo equalsHasBeenCalled, FieldInfo comparer)
        {
            var methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;
            MethodBuilder method = type.DefineMethod("Equals", methodAttributes);

            MethodInfo objectEquals = typeof(object).GetMethod(
                "Equals",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(object) },
                null
                );

            MethodInfo equalityComparerEquals = typeof(IEqualityComparer).GetMethod(
                "Equals",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(object), typeof(object) },
                null
                );
            
            method.SetReturnType(typeof(bool));
            method.SetParameters(typeof(object));
            method.DefineParameter(1, ParameterAttributes.None, "obj");
            
            ILGenerator gen = method.GetILGenerator();
            gen.DeclareLocal(typeof(Boolean));
            gen.DeclareLocal(typeof(Boolean));
            
            Label label1 = gen.DefineLabel();
            Label label2 = gen.DefineLabel();
            
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, equalsHasBeenCalled);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ceq);
            gen.Emit(OpCodes.Stloc_1);
            gen.Emit(OpCodes.Ldloc_1);
            gen.Emit(OpCodes.Brtrue_S, label1);
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Call, objectEquals);
            gen.Emit(OpCodes.Stloc_0);
            gen.Emit(OpCodes.Br_S, label2);
            gen.MarkLabel(label1);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldc_I4_1);
            gen.Emit(OpCodes.Stfld, equalsHasBeenCalled);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, comparer);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Callvirt, equalityComparerEquals);
            gen.Emit(OpCodes.Stloc_0);
            gen.Emit(OpCodes.Br_S, label2);
            gen.MarkLabel(label2);
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);
            
            return method;
        }

        private static void CopyProperties(TSource source, TSource destination)
        {
            Type type = source.GetType();

            while (type != null)
            {
                FieldInfo[] fields = type.GetFields(
                      BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                foreach (FieldInfo fi in fields)
                {
                    fi.SetValue(destination, fi.GetValue(source));
                }

                type = type.BaseType;
            }
        }
    }
}
