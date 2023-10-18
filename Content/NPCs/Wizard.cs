using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystiliumMod.Content.NPCs
{
	public class Wizard : GlobalNPC
	{
		public override void ModifyShop(NPCShop shop)
		{
			if (shop.NpcType == NPCID.Wizard)
			{
				shop.Add(ModContent.ItemType<Items.CrystalBottle>());
			}
		}
	}
}