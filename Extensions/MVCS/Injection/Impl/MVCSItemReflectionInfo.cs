using System;
using System.Collections.Generic;
using System.Reflection;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;
using Build1.PostMVC.Utils.Reflection;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Impl
{
    internal sealed class MVCSItemReflectionInfo : IMVCSItemReflectionInfo
    {
        public IList<IInjectionInfo> Injections       { get; private set; }
        public IList<MethodInfo>     PostConstructors { get; private set; }
        public IList<MethodInfo>     PreDestroys      { get; private set; }

        public IReflectionInfo Build(Type type)
        {
            Injections = GetInjects(type);
            PostConstructors = GetMethodList<PostConstruct>(type);
            PreDestroys = GetMethodList<PreDestroy>(type);
            return this;
        }

        /*
         * Static.
         */

        private static List<IInjectionInfo> GetInjects(Type type)
        {
            var members = type.FindMembers(MemberTypes.Property,
                                           BindingFlags.FlattenHierarchy |
                                           BindingFlags.SetProperty |
                                           BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Instance,
                                           null,
                                           null);

            var properties = new List<IInjectionInfo>();
            foreach (var member in members)
            {
                var injections = member.GetCustomAttributes(typeof(Inject), true);
                if (injections.Length > 0)
                    properties.Add(new InjectionInfo(member as PropertyInfo, injections[0] as Inject));
            }

            return properties;
        }

        private static List<MethodInfo> GetMethodList<T>(IReflect type) where T : Attribute
        {
            var methods = type.GetMethods(BindingFlags.FlattenHierarchy |
                                          BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance |
                                          BindingFlags.InvokeMethod);

            var methodList = new List<MethodInfo>();
            foreach (var method in methods)
            {
                if (method.GetCustomAttributes(typeof(T), true).Length > 0)
                    methodList.Add(method);
            }

            return methodList;
        }
    }
}