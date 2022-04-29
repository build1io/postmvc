namespace Build1.PostMVC.Utils.Extensions
{
    public static class FileSizeExtensions
    {
        public static float ToMb(this long bytes)
        {
            return bytes / 1048576F;
        }
        
        public static float ToMb(this ulong bytes)
        {
            return bytes / 1048576F;
        }
        
        public static float ToMb(this float bytes)
        {
            return bytes / 1048576F;
        }
    }
}