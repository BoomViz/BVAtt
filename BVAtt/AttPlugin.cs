using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using BVAtt.PluginConfig;
using Logger = Rocket.Core.Logging.Logger;
using SDG.Unturned;

namespace BVAtt
{
    public partial class AttPlugin : RocketPlugin<Config>
    {
        public static AttPlugin Instance;
        public static Config Config;

        public override void LoadPlugin()
        {
            base.LoadPlugin();
            Instance = this;
            Config = Configuration.Instance;
            Logger.Log("BVAtt успешно загружен.");
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "BVAtt_Fail_Gun", "Вы не можете установить обвес на это оружие." },
            { "BVAtt_Fail_Item", "Вы не можете установить этот предмет." },
            { "BVAtt_Fail_Mag", "У вас нет прав, чтобы установить магазин." },
            { "BVAtt_Fail_Blacklist", "Этот обвес запрещен." },
            { "BVAtt_Attached", "{0} ({1}) Установлен на ваше оружие." },
        };

        public override void UnloadPlugin(PluginState state = PluginState.Unloaded)
        {
            base.UnloadPlugin();
            Logger.Log("BVAtt успешно выгружен.");
            Instance = null;
        }
    }
}