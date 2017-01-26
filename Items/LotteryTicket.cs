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

        public PrizeNode AddChildren(params Prize[] children)
        {
            this.children.AddRange(children);
            return this;
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
        static PrizeNode easymodePrizeTree = BuildEasymodePrizeTree();
        static PrizeNode hardmodePrizeTree = BuildHardmodePrizeTree();

        static PrizeNode BuildEasymodePrizeTree () {
            PrizeNode moneyPrizes = new PrizeNode (0.40);
            moneyPrizes.AddChildren(new Prize (0.60, ItemID.SilverCoin, 1, 99),
                                    new Prize (0.35, ItemID.GoldCoin, 1, 99),
                                    new Prize (0.05, ItemID.PlatinumCoin, 1));

            PrizeNode cratePrizes = new PrizeNode (0.25);
            cratePrizes.AddChildren(new Prize (0.50, ItemID.WoodenCrate, 1),
                                    new Prize (0.30, ItemID.IronCrate, 1),
                                    new Prize (0.20, ItemID.GoldenCrate, 1));

            PrizeNode orePrizes = new PrizeNode (0.16);
            orePrizes.AddChildren(new Prize (0.1, ItemID.TinOre, 1, 10),
                                  new Prize (0.1, ItemID.CopperOre, 1, 10),
                                  new Prize (0.1, ItemID.TungstenOre, 1, 10),
                                  new Prize (0.1, ItemID.SilverOre, 1, 10),
                                  new Prize (0.1, ItemID.GoldOre, 1, 10),
                                  new Prize (0.1, ItemID.PlatinumOre, 1, 10),
                                  new Prize (0.1, ItemID.DemoniteOre, 1, 10),
                                  new Prize (0.1, ItemID.CrimtaneOre, 1, 10));
            PrizeNode gemPrizes = new PrizeNode (0.2);
            gemPrizes.AddChildren(new Prize (0.17, ItemID.Emerald, 1, 7),
                                  new Prize (0.17, ItemID.Amethyst, 1, 7),
                                  new Prize (0.16, ItemID.Sapphire, 1, 7),
                                  new Prize (0.16, ItemID.Topaz, 1, 7),
                                  new Prize (0.14, ItemID.Diamond, 1, 7),
                                  new Prize (0.14, ItemID.Ruby, 1, 7),
                                  new Prize (0.04, ItemID.Amber, 1, 7));
            orePrizes.AddChildren(gemPrizes);

            PrizeNode ammoPrizes = new PrizeNode (0.16);
            ammoPrizes.AddChildren(new Prize (0.4, ItemID.MusketBall, 50),
                                   new Prize (0.4, ItemID.WoodenArrow, 50),
                                   new Prize (0.2, ItemID.RocketI, 5));

            PrizeNode junkPrizes = new PrizeNode (0.03);
            junkPrizes.AddChildren(new Prize (1.00, ItemID.FishingSeaweed, 1, 3));

            PrizeNode root = new PrizeNode (0.33);
            root.AddChildren(moneyPrizes, cratePrizes, orePrizes, ammoPrizes, junkPrizes);

            return root;
        }

        static PrizeNode BuildHardmodePrizeTree () {
            PrizeNode moneyPrizes = new PrizeNode (0.40);
            moneyPrizes.AddChildren(new Prize (0.30, ItemID.SilverCoin, 1, 99),
                                    new Prize (0.55, ItemID.GoldCoin, 1, 99),
                                    new Prize (0.15, ItemID.PlatinumCoin, 1, 5));

            PrizeNode cratePrizes = new PrizeNode (0.25);
            cratePrizes.AddChildren(new Prize (0.20, ItemID.WoodenCrate, 1, 5),
                                    new Prize (0.45, ItemID.IronCrate, 1, 3),
                                    new Prize (0.35, ItemID.GoldenCrate, 1));

            PrizeNode orePrizes = new PrizeNode (0.16);
            orePrizes.AddChildren(new Prize (0.1, ItemID.TinOre, 7, 20),
                                  new Prize (0.1, ItemID.CopperOre, 7, 20),
                                  new Prize (0.1, ItemID.TungstenOre, 7, 20),
                                  new Prize (0.1, ItemID.SilverOre, 7, 20),
                                  new Prize (0.1, ItemID.GoldOre, 7, 20),
                                  new Prize (0.1, ItemID.PlatinumOre, 7, 20),
                                  new Prize (0.1, ItemID.DemoniteOre, 7, 20),
                                  new Prize (0.1, ItemID.CrimtaneOre, 7, 20));
            PrizeNode gemPrizes = new PrizeNode (0.2);
            gemPrizes.AddChildren(new Prize (0.17, ItemID.Emerald, 5, 15),
                                  new Prize (0.17, ItemID.Amethyst, 5, 15),
                                  new Prize (0.16, ItemID.Sapphire, 5, 15),
                                  new Prize (0.16, ItemID.Topaz, 5, 15),
                                  new Prize (0.14, ItemID.Diamond, 5, 15),
                                  new Prize (0.14, ItemID.Ruby, 5, 15),
                                  new Prize (0.04, ItemID.Amber, 5, 15));
            orePrizes.AddChildren(gemPrizes);

            PrizeNode ammoPrizes = new PrizeNode (0.16);
            ammoPrizes.AddChildren(new Prize (0.4, ItemID.MusketBall, 100),
                                   new Prize (0.4, ItemID.WoodenArrow, 100),
                                   new Prize (0.2, ItemID.RocketI, 25));

            PrizeNode junkPrizes = new PrizeNode (0.03);
            junkPrizes.AddChildren(new Prize (1.00, ItemID.FishingSeaweed, 1, 3));

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
            Prize result = NPC.downedMoonlord ? hardmodePrizeTree.Roll()
                : Main.hardMode ? hardmodePrizeTree.Roll()
                : easymodePrizeTree.Roll();
            resultType = result.item;
            resultStack = result.amount;
        }
    }
}
