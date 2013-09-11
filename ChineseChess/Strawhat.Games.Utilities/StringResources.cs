using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Strawhat.Games.Utilities
{
    public class StringResources
    {
#if DEBUG
        public static readonly string _FOLDERS_TEMP_ = "Temp";
        public static readonly string _FOLDERS_CONFIGS_ = "Configs";
        public static readonly string _FOLDERS_RESOURCES_ = "Resources";
        public static readonly string _FOLDERS_LOGS_ = "Logs";
        public static readonly string _FOLDERS_RESOURCES_VIDEO_ = "Resources\\Videos";
        public static readonly string _FOLDERS_RESOURCES_MODELS_ = "Resources\\Models";
        public static readonly string _FOLDERS_RESOURCES_SOUNDS_ = "Resources\\Sounds";
#else
        public const string _FOLDERS_TEMP_ = "Temp";
        public const string _FOLDERS_CONFIG_ = "Config";
        public const string _FOLDERS_RESOURCES_ = "Resources";
        public const string string _FOLDERS_LOGS_ = "Logs";
        public const string _FOLDERS_RESOURCES_VIDEO_ = "Resources\\Videos";
        public const string _FOLDERS_RESOURCES_MODELS_ = "Resources\\Models";
        public const string _FOLDERS_RESOURCES_SOUNDS_ = "Resources\\Sounds";
#endif
    }
}
