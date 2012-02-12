using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            var args = new object[] { comparer };
            return (TClass)Activator.CreateInstance(builder.CreateType(), args);
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
                TypeAttributes.NotPublic,
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
                  FieldAttributes.Private | FieldAttributes.InitOnly
                );
            return field;
        }

        private static void BuildConstructors<TClass>(TypeBuilder type, FieldInfo comparer)
        {
            ConstructorInfo baseConstructor =
                (from ci in typeof(TClass).GetConstructors(
                     BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)
                 orderby ci.GetParameters().Length ascending
                 select ci).First();

            List<Type> parameterTypes =
                (from pi in baseConstructor.GetParameters()
                 select pi.ParameterType).ToList();
            parameterTypes.Add(typeof(IEqualityComparer));
            
            ConstructorBuilder constructor = type.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig,
                CallingConventions.Standard, 
                parameterTypes.ToArray());

            for (int position = 1; position <= parameterTypes.Count; position++)
            {
                constructor.DefineParameter(position, ParameterAttributes.None, "arg" + position);
            }

            ILGenerator gen = constructor.GetILGenerator();
            
            for (int position = 0; position < parameterTypes.Count; position++)
            {
                if (position == 0)
                {
                    gen.Emit(OpCodes.Ldarg_0);
                }
                else if (position == 1)
                {
                    gen.Emit(OpCodes.Ldarg_1);
                }
                else if (position == 2)
                {
                    gen.Emit(OpCodes.Ldarg_2);
                }
                else if (position == 3)
                {
                    gen.Emit(OpCodes.Ldarg_3);
                }
                else
                {
                    gen.Emit(OpCodes.Ldarg_S, position);
                }
            }

            gen.Emit(OpCodes.Call, baseConstructor);
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Nop);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_S, parameterTypes.Count);
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
