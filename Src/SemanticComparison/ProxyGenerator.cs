using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace Ploeh.SemanticComparison
{
    internal static class ProxyGenerator
    {
        private const string assemblyName = "SemanticComparisonGeneratedAssembly";

        internal static TClass OverrideEquals<TClass>(IEqualityComparer comparer)
        {
            TypeBuilder builder = ProxyGenerator.BuildType<TClass>(BuildModule(BuildAssembly(assemblyName)));
            FieldBuilder equals = ProxyGenerator.BuildFieldComparer(builder);

            ProxyGenerator.BuildConstructors<TClass>(builder, equals);
            ProxyGenerator.BuildMethodEquals(builder, BuildFieldEqualsHasBeenCalled(builder), equals);
            ProxyGenerator.BuildMethodGetHashCode<TClass>(builder);

            return (TClass)Activator.CreateInstance(
                builder.CreateType(),
                new object[] { comparer });
        }

        private static AssemblyBuilder BuildAssembly(string name)
        {
            var an = new AssemblyName(name) { Version = Assembly.GetExecutingAssembly().GetName().Version };
            return AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave);
        }

        private static ModuleBuilder BuildModule(AssemblyBuilder ab)
        {
            return ab.DefineDynamicModule(assemblyName, assemblyName + ".dll");
        }

        private static TypeBuilder BuildType<TClass>(ModuleBuilder mb)
        {
            TypeBuilder type = mb.DefineType(
                typeof(TClass).Name + "Proxy" + Guid.NewGuid().ToString().Replace("-", ""),
                TypeAttributes.Public,
                typeof(TClass));
            return type;
        }

        private static FieldBuilder BuildFieldEqualsHasBeenCalled(TypeBuilder type)
        {
            FieldBuilder field = type.DefineField(
                "equalsHasBeenCalled",
                typeof(bool),
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

        private static void BuildConstructors<TClass>(TypeBuilder type, FieldInfo comparer)
        {
            var methodAttributes = MethodAttributes.Public| MethodAttributes.HideBySig;
            MethodBuilder method = type.DefineMethod(".ctor", methodAttributes);
            ConstructorInfo ctor = typeof(TClass).GetConstructor(
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
        }

        private static void BuildMethodGetHashCode<TClass>(TypeBuilder type)
        {
            var methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;
            MethodBuilder method = type.DefineMethod("GetHashCode", methodAttributes);
            method.SetReturnType(typeof(int));

            int derivedGetHashCode = 135;
            MethodInfo getHashCode = typeof(TClass).GetMethod(
                "GetHashCode",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { },
                null
                );

            ILGenerator gen = method.GetILGenerator();
            gen.DeclareLocal(typeof(int));
            
            Label label = gen.DefineLabel();
            
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Call, getHashCode);
            gen.Emit(OpCodes.Ldc_I4, derivedGetHashCode);
            gen.Emit(OpCodes.Add);
            gen.Emit(OpCodes.Stloc_0);
            gen.Emit(OpCodes.Br_S, label);
            gen.MarkLabel(label);
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);
        }

        private static void BuildMethodEquals(TypeBuilder type, FieldInfo equalsHasBeenCalled, FieldInfo comparer)
        {
            var methodAttributes = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;
            MethodBuilder method = type.DefineMethod("Equals", methodAttributes);

            MethodInfo objectEquals = typeof(object).GetMethod(
                "Equals",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(object) },
                null
                );

            MethodInfo equalityComparerEquals = typeof(IEqualityComparer).GetMethod(
                "Equals",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(object), typeof(object) },
                null
                );
            
            method.SetReturnType(typeof(bool));
            method.SetParameters(typeof(object));
            method.DefineParameter(1, ParameterAttributes.None, "obj");
            
            ILGenerator gen = method.GetILGenerator();
            gen.DeclareLocal(typeof(bool));
            gen.DeclareLocal(typeof(bool));
            
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
        }
    }
}
