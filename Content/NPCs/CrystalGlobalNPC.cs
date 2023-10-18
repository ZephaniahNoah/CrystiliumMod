using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystiliumMod.Content.NPCs
{
	public class CrystalGlobalNPC : GlobalNPC
	{
		/*	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
			{
				if (player.GetModPlayer<CrystalPlayer>().ZoneCrystal)
				{
					spawnRate = (int)(spawnRate * 0.65f);
					maxSpawns = (int)(maxSpawns * 1.45f);
				}
			}*/

		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.GetModPlayer<CrystalPlayer>().ZoneCrystal)
			{
				pool.Clear(); //remove ALL spawns here
				pool.Add(ModContent.NPCType<CrystalElemental>(), 4f); // a modded enemy
				pool.Add(ModContent.NPCType<CrystalArcher>(), 10f); // a modded enemy
				pool.Add(ModContent.NPCType<CrystalSlime>(), 10f); // a modded enemy
				pool.Add(ModContent.NPCType<CrystalZombie>(), 8f); // a modded enemy

				if (Main.hardMode)
				{
					pool.Add(ModContent.NPCType<CrystalMimic>(), 0.1f); // another modded enemy
					pool.Add(ModContent.NPCType<GeodeMonster>(), 13f); // another modded enemy
				}
			}
		}

		public override void ModifyShop(NPCShop shop)
		{
			Player player = Main.LocalPlayer;
			if (shop.NpcType == NPCID.Cyborg)
			{
				if (player.HasItem(ModContent.ItemType<Items.Weapons.Shatterocket>()))
				{
					shop.Add(ModContent.ItemType<Items.RPC>());
				}
			}
		}
	}
}