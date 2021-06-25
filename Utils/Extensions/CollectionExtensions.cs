using System;
using System.Collections.Generic;

namespace Build1.PostMVC.Utils.Extensions
{
    public static class CollectionExtensions
    {
        private static readonly Random _random = new Random();
        
        public static T Random<T>(this T[] list)
        {
            return list.Length == 0 ? default : list[_random.Next(list.Length)];
        }
        
        public static T Random<T>(this IList<T> list)
        {
            return list.Count == 0 ? default : list[_random.Next(list.Count)];
        }
    }
}