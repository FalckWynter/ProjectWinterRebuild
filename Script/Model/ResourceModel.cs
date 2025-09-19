using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlentyFishFramework
{
    public class ResourceModel : AbstractModel
    {
        public UserConfig userConfig;
        public FilePathConfig filePathConfig;
        protected override void OnInit()
        {
        }

    }
    public class UserConfig
    {
        public string language = "ChineseSimple"; // ƒ¨»œ”Ô—‘
    }

    public class FilePathConfig
    {
        public string localizationRoot;
        public string cultureFileName;
    }
}