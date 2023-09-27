using System;
using System.Collections.Generic;
using System.Reflection;
using Build1.PostMVC.Core.MVCS.Injection.Api;

namespace Build1.PostMVC.Core.MVCS.Injection.Impl
{
    public sealed class MVCSItemReflectionInfo
    {
        public IList<IInjectionInfo>                Injections  { get; private set; }
        public IDictionary<Type, IList<MethodInfo>> MethodInfos { get; private set; }

        public void SetInjectionInfos(IList<IInjectionInfo> infos)
        {
            Injections = infos;
        }

        public void AddMethodInfos<T>(IList<MethodInfo> infos) where T : Attribute
        {
            MethodInfos ??= new Dictionary<Type, IList<MethodInfo>>();
            MethodInfos.Add(typeof(T), infos);
        }

        public IList<MethodInfo> GetMethodInfos<T>() where T : Attribute
        {
            if (MethodInfos != null && MethodInfos.TryGetValue(typeof(T), out var infos))
                return infos;
            return null;
        }
        
        /*
         * Static.
         */
        
        public static List<IInjectionInfo> GetInjects(Type type)
        {
            var members = type.FindMembers(MemberTypes.Property,
                                           BindingFlags.FlattenHierarchy |
                                           BindingFlags.SetProperty |
                                           BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Instance,
                                           null,
                                           null);

            List<IInjectionInfo> properties = null;

            foreach (var member in members)
            {
                var injections = member.GetCustomAttributes(typeof(Inject), true);
                if (injections.Length == 0)
                    continue;

                properties ??= new List<IInjectionInfo>();
                properties.Add(new InjectionInfo((PropertyInfo)member, (Inject)injections[0]));
            }

            return properties;
        }

        public static List<MethodInfo> GetMethodList<T>(IReflect type) where T : Attribute
        {
            var methods = type.GetMethods(BindingFlags.FlattenHierarchy |
                                          BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance |
                                          BindingFlags.InvokeMethod);

            List<MethodInfo> methodList = null;

            foreach (var method in methods)
            {
                if (method.GetCustomAttributes(typeof(T), true).Length <= 0)
                    continue;

                methodList ??= new List<MethodInfo>();
                methodList.Add(method);
            }

            return methodList;
        }
    }
}