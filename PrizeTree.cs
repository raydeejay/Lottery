using System;
using System.Collections.Generic;
using System.Linq;

using Terraria;
using Terraria.ID;

namespace Lottery {
    // Represents a leaf in the prize tree, holding a number of
    // possibilities for the actual prize obtained
    public class Prize {
        public double percent = -1;
        public int item = ItemID.None;
        public int min = 0;
        public int max = 0;
        public int amount = 0;
        public Func<bool> predicate;

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

    // Extends Prizes to represent a Node in the tree, which contains
    // one or more PrizeNodes and/or Prize leafs
    public class PrizeNode : Prize {
        public List<Prize> children = new List<Prize>();

        public PrizeNode (double percent, Func<bool> predicate) {
            this.percent = percent;
            this.predicate = predicate;
        }

        public PrizeNode (double percent) : this(percent, () => true) { }
        public PrizeNode () : this(1) { }

        public PrizeNode AddChildren(params Prize[] children)
        {
            this.children.AddRange(children);
            return this;
        }

        public override Prize Roll() {
            // sum all the chances of the children together
            double totalWeight = (from entry in this.children select entry.percent).Sum();

            // then multiply this by the upper bound (+1 ?)
            double chance = Main.rand.NextDouble() * totalWeight;

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
}
