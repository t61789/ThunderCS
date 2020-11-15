using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class Platform
    {
        public static Platforms CurPlatform;

        static Platform()
        {
            ResolveCurPlatform();
        }

        private static void ResolveCurPlatform()
        {
            CurPlatform = 0;
#if UNITY_EDITOR
            CurPlatform &= Platforms.Editor;
#endif
#if UNITY_ANDROID
            CurPlatform &= Platforms.Android;
#endif
#if UNITY_STANDALONE
            CurPlatform &= Platforms.Standalone;
#endif
        }
    }

    [Flags]
    public enum Platforms
    {
        Standalone = 0x1,
        Editor = 0x2,
        Android = 0x4
    }
}
