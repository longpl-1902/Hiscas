using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hicas.Revit.Setting
{
    public class SystemSetting
    {
        public SystemLibrary Library { get; set; }

        public static SystemSetting Instance => GetInstance();

        private static SystemSetting GetInstance()
        {
            string jsonPath = Path.Combine(Assembly.GetExecutingAssembly().Location, "..", "Setting", "uPVC.json");

            string jsonText = File.ReadAllText(jsonPath);

            return new SystemSetting
            {
                Library = JsonConvert.DeserializeObject<SystemLibrary>(jsonText)
            };
        }
    }
}
