using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using UnityEngine;

namespace DarkClearInventory
{
    public class CommandCi : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "ci";
        public string Help => "";
        public string Syntax => "/ci [name/self]";
        public List<string> Aliases => new List<string>() { "clearinventory", "cleari" };
        public List<string> Permissions => new List<string>() { "ci.other", "ci" };

        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length > 1)
            {
                UnturnedChat.Say(player, ClearInventory.Instance.Translate("clear_invalid_command", Syntax), Color.yellow);
                return;
            }
            UnturnedPlayer toPlayer = player;
            if (command.Length == 1 && command[0].ToLower() != "self")
            {
                if (!R.Permissions.HasPermission(player, "ci.other") && !player.IsAdmin)
                {
                    UnturnedChat.Say(player, ClearInventory.Instance.Translate("clear_not_have_permission_other"), Color.red);
                    return;
                }
                toPlayer = UnturnedPlayer.FromName(command[0]);
            }
            bool cleared = ClearInventory.Instance.ClearPlayerInventory(toPlayer);
            if (!cleared)
            {
                UnturnedChat.Say(player, ClearInventory.Instance.Translate("clear_failed", toPlayer.CharacterName), Color.red);
                return;
            }
            UnturnedChat.Say(player, ClearInventory.Instance.Translate("clear_successfully", toPlayer.CharacterName), Color.green);
        }
    }
}
