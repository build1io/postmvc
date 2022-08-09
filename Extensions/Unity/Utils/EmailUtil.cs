using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Utils
{
    public static class EmailUtil
    {
        public static void MailTo(string email, string subject)
        {
            Application.OpenURL($"mailto:{email}?subject={URLUtil.Escape(subject)}");
        }
        
        public static void MailTo(string email, string subject, string body)
        {
            Application.OpenURL($"mailto:{email}?subject={URLUtil.Escape(subject)}&body={URLUtil.Escape(body)}");
        }
    }
}