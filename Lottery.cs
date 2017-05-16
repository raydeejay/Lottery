using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ModLoader;

namespace Lottery {
    // The Mod class
    class Lottery : Mod {
        public static Dictionary<string, PrizeNode> prizeTables = new Dictionary<string, PrizeNode>();

        public Lottery() {
            Properties = new ModProperties() {
                Autoload = true
            };

            Lottery.AddBuiltinTables();
        }

        // Add a prizes table to the dictionary
        public static void AddTable(string name, double weight, Func<bool> predicate, PrizeNode[] entries) {
            Lottery.prizeTables[name] = new PrizeNode(weight, predicate).AddChildren(entries);
        }

        public static void AddTable(string name, double weight, PrizeNode[] entries) {
            Lottery.AddTable(name, weight, () => true, new PrizeNode[] {});
        }

        public static void AddTable(string name, double weight) {
            Lottery.AddTable(name, weight, new PrizeNode[] {});
        }

        private static void AddBuiltinTables() {
            Lottery.AddTable("easymode", 0.33, () => true, BuiltinTables.BuildEasymode());
            Lottery.AddTable("hardmode", 0.33, () => Main.hardMode, BuiltinTables.BuildHardmode());
        }

        // Call interface for modders

        // Create a table
        //   Call(name, weight, predicate);

        // Add a fixed amount to a table
        //   Call(name, weight, ID, amount);

        // Add a variable amount to a table
        //   Call(name, weight, ID, min, max);

        public override object Call(params object[] args) {
            PrizeNode table;
            Prize prize;

            switch(args.Length) {
                case 2:
                    Lottery.AddTable((string) args[0], (double) args[1], (Func<bool>) args[2], new PrizeNode[] {});
                    break;

                case 3:
                    Lottery.AddTable((string) args[0], (double) args[1], (Func<bool>) args[2], new PrizeNode[] {});
                    break;

                case 4:
                    table = Lottery.prizeTables[(string) args[0]];
                    prize = new Prize((double) args[1], (int) args[2], (int) args[3]);
                    table.AddChildren(prize);
                    break;

                case 5:
                    table = Lottery.prizeTables[(string) args[0]];
                    prize = new Prize((double) args[1], (int) args[2], (int) args[3], (int) args[4]);
                    table.AddChildren(prize);
                    break;
            }

            return this;
        }
    }

    // Hook for dropping the Lottery Ticket
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
