using System;
using System.Collections.Generic;
using System.Reflection;
using Build1.PostMVC.Core.MVCS.Injection.Api;

namespace Build1.PostMVC.Core.MVCS.Injection.Impl
{
    public class MVCSItemReflectionInfo : IMVCSItemReflectionInfo
    {
        public IList<IInjectionInfo> Injections       { get; }
        public IList<MethodInfo>     PostConstructors { get; }
        public IList<MethodInfo>     PreDestroys      { get; }

        internal MVCSItemReflectionInfo(IList<IInjectionInfo> injectionInfos, IList<MethodInfo> postConstructors, IList<MethodInfo> preDestroys)
        {
            Injections = injectionInfos;
            PostConstructors = postConstructors;
            PreDestroys = preDestroys;
        }

        /*
         * Static.
         */

        internal static List<IInjectionInfo> GetInjects(Type type)
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

        internal static List<MethodInfo> GetMethodList<T>(IReflect type) where T : Attribute
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