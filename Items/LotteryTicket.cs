using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Lottery.Items {
    internal class Prize {
        public double percent = -1;
        public int item = ItemID.None;
        public int min = 0;
        public int max = 0;
        public int amount = 0;

        public Prize (double percent = -1, int item = ItemID.None, int min = 0, int max = 0) {
            this.percent = percent;
            this.item = item;
            this.min = min;
            this.max = max;
        }

        public virtual Prize Roll() {
            this.amount = (this.max == 0) ? this.min : Main.rand.Next(this.min, this.max + 1);
            return this;
        }
    }

    internal class PrizeNode : Prize {
        public List<Prize> children = new List<Prize>();

        public PrizeNode (double percent) {
            this.percent = percent;
        }

        public void AddChildren(params Prize[] children)
        {
            this.children.AddRange(children);
        }

        public override Prize Roll() {
            double chance = Main.rand.NextDouble();
            foreach (Prize child in this.children) {
                if (chance < child.percent) {
                    return child.Roll();
                } else {
                    chance -= child.percent;
                }
            }
            // either there were no children,
            // or somehow we managed to run out of them
            return this;
        }
    }

    public class LotteryTicket : ModItem {
        static PrizeNode prizeTree = BuildPrizesTree();

        static PrizeNode BuildPrizesTree () {
            // 40% money
            //     60% silver 1-99 coins
            //     35% gold 1-20 coins
            //      5% platinum 1-5 coins
            PrizeNode moneyPrizes = new PrizeNode (0.40);
            moneyPrizes.AddChildren(new Prize (0.60, ItemID.SilverCoin, 1, 99),
                                    new Prize (0.35, ItemID.GoldCoin, 1, 99),
                                    new Prize (0.05, ItemID.PlatinumCoin, 1, 5));

            // 25% crate
            //     50% wooden
            //     30% iron
            //     10% golden
            //     10% themed
            PrizeNode cratePrizes = new PrizeNode (0.25);
            cratePrizes.AddChildren(new Prize (0.50, ItemID.WoodenCrate, 1),
                                    new Prize (0.30, ItemID.IronCrate, 1),
                                    new Prize (0.20, ItemID.GoldenCrate, 1));

            // 16% ore (1-10 units)
            //     20% tin/copper
            //     20% tungsten/silver
            //     20% gold/platinum
            //     20% demonite/crimtane
            //     20% random gem (weighted)
            PrizeNode orePrizes = new PrizeNode (0.16);
            orePrizes.AddChildren(new Prize (0.1, ItemID.TinOre, 1, 10),
                                  new Prize (0.1, ItemID.CopperOre, 1, 10),
                                  new Prize (0.1, ItemID.TungstenOre, 1, 10),
                                  new Prize (0.1, ItemID.SilverOre, 1, 10),
                                  new Prize (0.1, ItemID.GoldOre, 1, 10),
                                  new Prize (0.1, ItemID.PlatinumOre, 1, 10),
                                  new Prize (0.1, ItemID.DemoniteOre, 1, 10),
                                  new Prize (0.1, ItemID.CrimtaneOre, 1, 10));
            // gems
            PrizeNode gemPrizes = new PrizeNode (0.2);
            gemPrizes.AddChildren(new Prize (0.17, ItemID.Emerald, 1, 7),
                                  new Prize (0.17, ItemID.Amethyst, 1, 7),
                                  new Prize (0.16, ItemID.Sapphire, 1, 7),
                                  new Prize (0.16, ItemID.Topaz, 1, 7),
                                  new Prize (0.14, ItemID.Diamond, 1, 7),
                                  new Prize (0.14, ItemID.Ruby, 1, 7),
                                  new Prize (0.04, ItemID.Amber, 1, 7));
            orePrizes.AddChildren(gemPrizes);

            // 16% random non-regular ammo (1d10 * 100 units, cap at 999)
            //     40% arrows
            //     40% bullets
            //     20% rockets
            PrizeNode ammoPrizes = new PrizeNode (0.16);
            ammoPrizes.AddChildren(new Prize (0.4, ItemID.MusketBall, 100),
                                   new Prize (0.4, ItemID.WoodenArrow, 100),
                                   new Prize (0.2, ItemID.RocketI, 25));

            //  3% junk
            PrizeNode junkPrizes = new PrizeNode (0.03);
            junkPrizes.AddChildren(new Prize (1.00, ItemID.FishingSeaweed, 1, 3));

            // finally add evertyhing to the root node, 1/3 chance of a prize
            PrizeNode root = new PrizeNode (0.33);
            root.AddChildren(moneyPrizes, cratePrizes, orePrizes, ammoPrizes, junkPrizes);

            return root;
        }

        public override void SetDefaults() {
            item.name = "Lottery Ticket";
            item.toolTip = "A chance to win!";
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
            Prize result = prizeTree.Roll();
            resultType = result.item;
            resultStack = result.amount;
        }
    }
}
