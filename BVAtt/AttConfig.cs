using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BVAtt.PluginConfig
{
    public class Config : IRocketPluginConfiguration
    {
        [XmlArrayItem(elementName: "AttID")]
        public List<ushort> BlacklistAttachments;

        [XmlArrayItem(elementName: "WeaponID")]
        public List<ushort> BlacklistWeapons;
        public void LoadDefaults()
        {
            BlacklistAttachments = new List<ushort>()
            {
                0,
                0
            };
            BlacklistWeapons = new List<ushort>()
            {
                0,
                0
            };

        }
    }
}
