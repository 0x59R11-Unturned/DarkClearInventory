using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;

namespace DarkClearInventory
{
    public class ClearInventory : RocketPlugin
    {
        public static ClearInventory Instance = null;


        protected override void Load()
        {
            Instance = this;
        }
        protected override void Unload()
        {
            
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "clear_successfully", "У {0} инвентарь успешно очищен" },
            { "clear_failed", "Что-то пошло не так. (проверьте консоль)" },
            { "clear_not_have_permission_other", "У вас не достаточно прав." },
            { "clear_invalid_command", "{0}" }
        };


        public bool ClearPlayerInventory(UnturnedPlayer player)
        {
            return ClearItems(player) && ClearClothes(player);
        }


        public bool ClearItems(UnturnedPlayer player)
        {
            bool result = false;
            try
            {
                player.Player.equipment.dequip();
                for (byte page = 0; page < PlayerInventory.PAGES - 1; page++)
                {
                    byte itemCount = player.Player.inventory.getItemCount(page);
                    if (itemCount > 0)
                    {
                        for (byte i = 0; i < itemCount; i++)
                        {
                            player.Player.inventory.removeItem(page, 0);
                        }
                    }
                }
                player.Player.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[] { 0, 0, new byte[0] });
                player.Player.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[] { 1, 0, new byte[0] });
                result = true;
            }
            catch (Exception ex)
            {
                Rocket.Core.Logging.Logger.Log($"There was an error clearing {player.CharacterName}'s inventory.  Here is the error.", ConsoleColor.White);
                Console.Write(ex);
            }
            return result;
        }

        public bool ClearClothes(UnturnedPlayer player)
        {
            bool result = false;
            try
            {
                player.Player.clothing.askWearBackpack(0, 0, new byte[0], true);
                removeCloth(player.Player.inventory);

                player.Player.clothing.askWearGlasses(0, 0, new byte[0], true);
                removeCloth(player.Player.inventory);

                player.Player.clothing.askWearHat(0, 0, new byte[0], true);
                removeCloth(player.Player.inventory);

                player.Player.clothing.askWearMask(0, 0, new byte[0], true);
                removeCloth(player.Player.inventory);

                player.Player.clothing.askWearPants(0, 0, new byte[0], true);
                removeCloth(player.Player.inventory);

                player.Player.clothing.askWearShirt(0, 0, new byte[0], true);
                removeCloth(player.Player.inventory);

                player.Player.clothing.askWearVest(0, 0, new byte[0], true);
                removeCloth(player.Player.inventory);

                result = true;
            }
            catch (Exception ex)
            {
                Rocket.Core.Logging.Logger.Log($"There was an error clearing {player.CharacterName}'s inventory.  Here is the error.", ConsoleColor.White);
                Console.Write(ex);
            }
            return result;
        }


        private void removeCloth(PlayerInventory inventory)
        {
            for (byte b = 0; b < inventory.getItemCount(2); b++)
            {
                inventory.removeItem(2, 0);
            }
        }
    }
}
