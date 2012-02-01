using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Ploeh.SemanticComparison
{
    internal class ProxyGenerator<TSource, TDestination>
    {
        private readonly TSource value;
        private readonly SemanticComparer<TSource, TDestination> comparer;

        internal ProxyGenerator(TSource value, SemanticComparer<TSource, TDestination> comparer)
        {
            this.value = value;
            this.comparer = comparer;
        }

        internal TSource OverrideEquals()
        {
            ModuleBuilder mb = ProxyGenerator<TSource, TDestination>.BuildModule("SemanticComparisonProxies");
            TypeBuilder type = ProxyGenerator<TSource, TDestination>.BuildType(mb);

            var proxy = (TSource)Activator.CreateInstance(
                this.BuildMethodEquals(type)
                    .CreateType());
            this.CopyPropertiesTo(proxy);
            return proxy;
        }

        private static ModuleBuilder BuildModule(string name)
        {
            ModuleBuilder mb = AppDomain.CurrentDomain
                .DefineDynamicAssembly(
                    new AssemblyName(name), AssemblyBuilderAccess.RunAndSave)
                .DefineDynamicModule(name, name + ".dll");
            return mb;
        }

        private static TypeBuilder BuildType(ModuleBuilder mb)
        {
            TypeBuilder type = mb.DefineType(
                typeof(Likeness<,>).Namespace + ".Proxies." + typeof(TSource).Name + "Proxy",
                TypeAttributes.Public,
                typeof(TSource));
            return type;
        }

        private TypeBuilder BuildMethodEquals(TypeBuilder type)
        {
            MethodBuilder method = type.DefineMethod(
                "Equals",
                MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig);

            FieldBuilder equalsHasBeenCalled = type.DefineField(
                "equalsHasBeenCalled" + Guid.NewGuid().ToString().Replace("-", ""),
                typeof(bool),
                FieldAttributes.Private);

            MethodInfo objectEquals = typeof(object).GetMethod(
                "Equals",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[]
                {
                    typeof(object) 
                },
                null);

            ConstructorInfo semanticComparerConstructor = this.comparer
                .GetType()
                .GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { },
                null);

            MethodInfo semanticComparerEquals = this.comparer
                .GetType()
                .GetMethod(
                "Equals",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[]
                {
                    typeof(object),
                    typeof(object)
                },
                null);

            // Setting return type
            method.SetReturnType(typeof(bool));

            // Adding parameters
            method.SetParameters(typeof(object));

            // Parameter obj
            method.DefineParameter(1, ParameterAttributes.None, "obj");

            ILGenerator gen = method.GetILGenerator();

            // Preparing locals
            gen.DeclareLocal(typeof(bool));
            gen.DeclareLocal(typeof(bool));

            // Preparing labels
            Label label1 = gen.DefineLabel();
            Label label2 = gen.DefineLabel();

            // Writing body
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
            gen.Emit(OpCodes.Newobj, semanticComparerConstructor);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Call, semanticComparerEquals);
            gen.Emit(OpCodes.Stloc_0);
            gen.Emit(OpCodes.Br_S, label2);
            gen.MarkLabel(label2);
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);

            return type;
        }

        private void CopyPropertiesTo(TSource proxy)
        {
            Type type = this.value.GetType();

            while (type != null)
            {
                FieldInfo[] fields = type.GetFields(
                      BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                foreach (FieldInfo fi in fields)
                {
                    fi.SetValue(proxy, fi.GetValue(this.value));
                }

                type = type.BaseType;
            }
        }
    }
}
