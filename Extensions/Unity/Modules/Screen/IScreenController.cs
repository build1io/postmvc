using System.Collections.Generic;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screen
{
    public interface IScreenController
    {
        void Initialize(IEnumerable<Screen> screens);
        void Show(Screen screen);
    }
}