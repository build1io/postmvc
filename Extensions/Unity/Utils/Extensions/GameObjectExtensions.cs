namespace Build1.PostMVC.Extensions.Unity.Utils.Extensions
{
    public static class GameObjectExtensions
    {
        public static UnityEngine.GameObject GetFirstActiveChild(this UnityEngine.GameObject gameObject)
        {
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (child.gameObject.activeSelf)
                    return child.gameObject;
            }
            return null;
        }
    }
}