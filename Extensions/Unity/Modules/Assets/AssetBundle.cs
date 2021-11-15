using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public sealed class AssetBundle
    {
        internal bool                    IsLoaded => Bundle != null;
        internal UnityEngine.AssetBundle Bundle   { get; private set; }

        public readonly Enum     id;
        public readonly string   name;
        public readonly string[] atlasesNames;

        public AssetBundle(Enum id, string name, params string[] atlasesNames)
        {
            this.id = id;
            this.name = name;
            this.atlasesNames = atlasesNames;
        }

        internal void SetBundle(UnityEngine.AssetBundle bundle)
        {
            Bundle = bundle;
        }

        public override string ToString()
        {
            return name;
        }
    }
}