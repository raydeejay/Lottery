using System.Collections.Generic;
using System.Linq;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lottery.Items {
    public class BoosterPack : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Booster Pack");
            Tooltip.SetDefault("Get a boost!");
        }

        public override void SetDefaults() {
            item.width = 32;
            item.height = 16;
            item.value = 0;
            item.maxStack = 999;
            item.rare = 3;
            item.autoReuse = false;
            item.consumable = true;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.useTurn = true;
        }

        public override bool UseItem(Player player)
        {
            player.velocity.Y = (float) Main.rand.Next(-10, -7);
            Main.PlaySound(SoundID.DoubleJump);
            for (int i = Main.rand.Next(2, 7); i > 0; --i) {
                Dust.NewDust(player.Center, 16, 16, DustID.Smoke);
            }
            return true;
        }
    }
}
