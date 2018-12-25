using Inchecker.Entities;
using System.Linq;

namespace Inchecker.Common.Settings
{
    public  class SettingManager
    {
        public static int LogRetention
        {
            get
            {
                return ReadSetting(nameof(LogRetention));
            }
            set
            {
                SaveSetting(nameof(LogRetention), value);
            }
        }

        private static int ReadSetting(string name)
        {
            using (var ctx = new IncheckerDbCtx())
            {
                return ctx.Settings.First(n => n.Name == name).Value;
            }
        }

        private static bool SaveSetting(string name, int value)
        {
            using (var ctx = new IncheckerDbCtx())
            {
                ctx.Settings.First(n => n.Name == name).Value = value;
                ctx.SaveChanges();
                return true;
            }
        }
    }
}
