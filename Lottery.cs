using Terraria;
using Terraria.ModLoader;

namespace Lottery {
    class Lottery : Mod {
        public Lottery() {
            Properties = new ModProperties() {
                Autoload = true
            };
        }
    }

    public class LotteryGlobalNPC : GlobalNPC {
        public override void NPCLoot(NPC npc) {
            if (Main.rand.NextDouble() <= 0.05) {
                Item.NewItem((int) npc.position.X, (int) npc.position.Y,
                             npc.width, npc.height,
                             mod.ItemType("LotteryTicket"),
                             Main.rand.Next(1, 3));
            }
        }
    }
}
