using System.Linq.Expressions;
using System.Reflection;

namespace Infraestrutura.Utils
{
    public static class WReflectionUtils
    {
        #region Hierarquia

        public static Type GetInterfaceClass(Type toCheck)
        {
            Type[] interfs = toCheck.GetInterfaces();

            if (interfs.Length > 0)
                return interfs[0];
            return null;
        }

        public static Type GetInterfaceSubclass(Type toCheck)
        {
            Type[] interfs = toCheck.GetInterfaces();

            if (interfs.Length > 0)
                return interfs[interfs.Length - 1];
            return null;
        }

        public static bool ImplementsGenericInterface(Type generic, Type toCheck)
        {
            return generic.GetInterfaces()
                   .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == toCheck);
        }

        public static bool ImplementsInterface(Type tipo, Type toCheck)
        {
            return tipo.GetInterfaces()
                    .Any(x => x == toCheck);
        }

        public static bool IsSubclassOfGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic.FullName.StartsWith(cur.FullName ?? string.Empty))
                    return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        #endregion Hierarquia

        #region Collections

        public static void AddToCollection(object collection, object item)
        {
            MethodInfo meth = collection?.GetType()
                .GetMethods()
                .FirstOrDefault(mi => mi.Name == "Add");

            if (meth == null)
                return;

            ParameterInfo[] pa = meth.GetParameters();
            if (pa.Length == 1 && item.GetType() == pa[0].ParameterType)
                meth.Invoke(collection, new object[] { item });
        }

        public static void ClearCollection(object collection)
        {
            MethodInfo meth = collection?.GetType()
                .GetMethods()
                .FirstOrDefault(mi => mi.Name == "Clear");

            if (meth == null)
                return;

            meth.Invoke(collection, Array.Empty<object>());
        }

        public static Type GetCollectionElementType(Type type)
        {
            if (type.GenericTypeArguments.Length == 1)
            {
                return type.GenericTypeArguments[0];
            }

            return null;
        }

        #endregion Collections

        #region Auxiliares

        /// Obtenção de propriedades
        public static readonly BindingFlags DefaultFlags = BindingFlags.IgnoreCase |
                                                           BindingFlags.Public |
                                                           BindingFlags.Instance;

        //public static object Create(IAppContext pContext, Type tipo)
        //{
        //    object ret = null;
        //    TypeInfo tipoInfo = tipo.GetTypeInfo();
        //    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        //    ConstructorInfo[] cia = tipoInfo.GetConstructors(flags);
        //    if (cia.Length > 0)
        //    {
        //        ConstructorInfo ci = cia[0];
        //        ParameterInfo[] parms = ci.GetParameters();
        //        var parma = new List<object>();

        //        foreach (ParameterInfo parm in parms)
        //        {
        //            object o = pContext.Provider.GetRequiredService(parm.ParameterType);
        //            parma.Add(o);
        //        }

        //        ret = ci.Invoke(parma.ToArray());
        //    }
        //    return ret;
        //}

        //public static T Create<T>(IAppContext pContext)
        //{
        //    return (T)Create (pContext, typeof(T));
        //}

        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            Type tipo = obj?.GetType();
            var info = tipo?.GetProperties(DefaultFlags).FirstOrDefault(p => p.Name == propertyName);
            if (info?.CanWrite ?? false)
                info?.SetValue(obj, value);
        }

        public static PropertyInfo GetPropertyInfo(this Type tipo, string propertyName)
        {
            PropertyInfo ret;
            ret = tipo.GetProperties(DefaultFlags).FirstOrDefault(p => p.Name == propertyName);
            return ret; // tipo.GetProperty(propertyName, DefaultFlags);
        }

        public static bool HasAttribute<TAttribute>(this Type tipo)
            where TAttribute : Attribute
        {
            var a = tipo.CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(TAttribute));
            return a != null;
        }

        #endregion Auxiliares

        #region Expressions
        public static Expression GetExpressionForProperty(string fullName, PropertyInfo prop, ParameterExpression param)
        {
            Expression body = null;
            var sm = prop.GetSetMethod();
            if (sm == null)
            {
                var mi = prop.DeclaringType.GetMethod(prop.Name + "AsExpression");
                if (mi != null && mi.IsStatic)
                    body = mi.Invoke(null, new object[] { param }) as Expression;
            }

            if (body == null)
            {
                body = param;
                foreach (var member in fullName.Split('.'))
                    body = Expression.PropertyOrField(body, member);
            }

            return body;
        }
        #endregion Expressions
    }
}
