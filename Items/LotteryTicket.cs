using System.Collections.Generic;
using System.Linq;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lottery.Items {
    public class LotteryTicket : ModItem {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Lottery Ticket");
            Tooltip.SetDefault("A chance to win!");
        }

        public override void SetDefaults() {
            item.width = 32;
            item.height = 16;
            item.value = 0;
            item.maxStack = 999;
            item.rare = 3;
            item.autoReuse = true;
            item.consumable = true;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.useTurn = true;
            ItemID.Sets.ExtractinatorMode[item.type] = item.type;
        }

        public override void ExtractinatorUse(ref int resultType, ref int resultStack) {
            // the tables are assembled together according to their predicates' value
            PrizeNode pool = new PrizeNode();
            pool.AddChildren((from table
                              in Lottery.prizeTables
                              where table.Value.predicate()
                              select table.Value.children)
                             .SelectMany(x => x)
                             .ToArray());

            // then Roll() is invoked
            Prize result = pool.Roll();
            resultType = result.item;
            resultStack = result.amount;
        }
    }
}
