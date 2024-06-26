using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using BVAtt.Translate;

namespace BVAtt.Command
{
    public class AttCommand : IRocketCommand
    {
        readonly static EItemType[] Types = new EItemType[] { EItemType.BARREL, EItemType.GRIP, EItemType.SIGHT, EItemType.TACTICAL, EItemType.MAGAZINE };
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "Att";

        public string Help => "Позволяет установить обвес на оружие.";

        public string Syntax => "/Att [Название обвеса/ID]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "BV.Att" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer Player = (UnturnedPlayer)caller;
            if (command.Length > 0)
            {
                if (Player.Player.equipment.state == null || Player.Player.equipment.state.Length < 12 || Player.Player.equipment.asset == null || Player.Player.equipment.asset.type != EItemType.GUN)
                {
                    UnturnedChat.Say(caller, "BVAtt_Fail_Gun".Translate());
                    return;
                }

                ItemAsset Item = null;

                if (ushort.TryParse(command[0], out ushort ItemID))
                {
                    Asset SelectAsset = Assets.find(EAssetType.ITEM, ItemID);
                    if (SelectAsset != null && typeof(ItemAsset).IsAssignableFrom(SelectAsset.GetType()) && Types.Contains(((ItemAsset)SelectAsset).type))
                    {
                        Item = (ItemAsset)SelectAsset;
                    }
                }

                if (Item == null)
                {
                    ItemAsset[] Ast = Assets.find(EAssetType.ITEM).Where(x => typeof(ItemAsset).IsAssignableFrom(x.GetType()) &&
                        Types.Contains(((ItemAsset)x).type) &&
                        ((ItemAsset)x).itemName.ToLower().Contains(command[0].ToLower()))
                        .Cast<ItemAsset>()
                        .ToArray();
                    if (Ast.Length != 0) Item = Ast[0];
                }


                if (AttPlugin.Config.BlacklistWeapons.Contains(Player.Player.equipment.itemID))
                {
                    UnturnedChat.Say(caller, "BVAtt_Fail_Blacklist".Translate());
                    return;
                }

                if (Item != null)
                {
                    if (AttPlugin.Config.BlacklistAttachments.Contains(Item.id))
                    {
                        UnturnedChat.Say(caller, "BVAtt_Fail_Blacklist".Translate());
                        return;
                    }

                    if (Item.type == EItemType.MAGAZINE && !Player.HasPermission("BV.AttMag"))
                    {
                        UnturnedChat.Say(caller, "BVAtt_Fail_Mag".Translate());
                        return;
                    }

                    byte pos = 255;
                    if (Item.type == EItemType.SIGHT)
                    {
                        pos = 0;
                    }
                    else if (Item.type == EItemType.TACTICAL)
                    {
                        pos = 2;
                    }
                    else if (Item.type == EItemType.GRIP)
                    {
                        pos = 4;
                    }
                    else if (Item.type == EItemType.BARREL)
                    {
                        pos = 6;
                        Player.Player.equipment.state[16] = 100;
                    }
                    else if (Item.type == EItemType.MAGAZINE)
                    {
                        pos = 8;
                        Player.Player.equipment.state[10] = Item.amount;
                    }
                    if (pos == 255) return;
                    byte[] ID = BitConverter.GetBytes(Item.id);
                    Array.Copy(ID, 0, Player.Player.equipment.state, pos, 2);
                    Player.Player.equipment.sendUpdateState();
                    UnturnedChat.Say(caller, "BVAtt_Attached".Translate(Item.itemName, Item.id));
                }
                else
                {
                    UnturnedChat.Say(caller, "BVAtt_Fail_Item".Translate());
                }
            }
            else
            {
                UnturnedChat.Say(caller, Syntax);
            }
        }
    }
}