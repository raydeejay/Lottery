using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.UI.Chat;

using Terraria.ModLoader;

namespace Lottery.Items {
    public class ItoriusStar : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Itorius' Star");
            Tooltip.SetDefault("Twinkle it!");
        }

        // maybe consume one star to gain X frames of "battery"
        // or die after N frames of use, and make them craftable with stars

        public override void SetDefaults() {
            item.width = 22;
            item.height = 24;
            item.value = 10000;
            item.maxStack = 1;
            item.rare = 3;
            item.autoReuse = true;
            item.consumable = false;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = 4;
            item.useTurn = true;
        }

        public override void HoldItem(Player player) {
            // Main.PlaySound(SoundID.MaxMana);
            Lighting.AddLight(Main.MouseWorld, 0.7f, 0.6f, 0f);
            Lighting.AddLight(player.Center, 0.7f, 0.6f, 0f);

            if(Main.rand.Next(10) == 0) {
                for (int i = Main.rand.Next(2, 7); i > 0; --i) {
                    Dust.NewDust(Main.MouseWorld, 16, 16, DustID.AncientLight);
                }
            }
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 99);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
