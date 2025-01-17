using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using CrystiliumMod.Content.Buffs;

namespace CrystiliumMod
{
	public class CrystalPlayer : ModPlayer
	{
		public bool ZoneCrystal = false;
		public float critDmgMult = 1f;
		public bool CrystalAcc = false;
		public int constantDamage = 0;
		public float percentDamage = 0f;
		public float defenseEffect = -1f;
		public bool crystalCharm = false;
		public int crystalCharmStacks = 0;

		public bool crystalFountain = false;

		public override void ResetEffects()
		{
			critDmgMult = 1f;
			constantDamage = 0;
			percentDamage = 0f;
			defenseEffect = -1f;
			CrystalAcc = false;
			crystalCharm = false;
		}

		public override void OnHurt(Player.HurtInfo info)
		{
			if (CrystalAcc)
			{
				SoundEngine.PlaySound(SoundID.Item27, Player.position);
				for (int h = 0; h < 20; h++)
				{
					Vector2 vel = new(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 5f;
					// TODO: We need to track the item reposible for toggling CrystalAcc somehow.
					Projectile.NewProjectile(Player.GetSource_Misc("CrystiliumMod:CrystalAcc"), Player.Center.X, Player.Center.Y, vel.X, vel.Y, Mod.Find<ModProjectile>("Shatter" + (1 + Main.rand.Next(0, 3))).Type, 20, 0, Player.whoAmI);
				}
			}
		}

		private void ApplyCritBonus(ref NPC.HitModifiers modifiers)
		{
			if (modifiers.CritDamage.Base > 0)
			{
				modifiers.FinalDamage *= critDmgMult;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			ApplyCritBonus(ref modifiers);
		}

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
		{
			ApplyCritBonus(ref modifiers);
		}

		// public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
		// {
		// 	ApplyCritBonus(ref damage, ref crit);
		// }

		// public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
		// {
		// 	ApplyCritBonus(ref damage, ref crit);
		// }

		private void UpdateCharmBuff(NPC npc)
		{
			//only do this stuff if wearing accessory
			if (crystalCharm)
			{
				//add buff, update stacks
				int buffIdx = Player.FindBuffIndex(ModContent.BuffType<CrystalCharm>());
				if (buffIdx < 0)
				{
					Player.AddBuff(ModContent.BuffType<CrystalCharm>(), 120);
					crystalCharmStacks = 1;
					//1/3 chance to increase stack each hit
				}
				else if (crystalCharmStacks < 25 && Main.rand.Next(2) == 0)
				{
					crystalCharmStacks += 1;
				}

				//reset buff time
				if (buffIdx > -1)
				{
					Player.buffTime[buffIdx] = 120;
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			UpdateCharmBuff(target);
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			UpdateCharmBuff(target);
		}

		public override void ModifyHurt(ref Player.HurtModifiers modifiers)
		{
			if (constantDamage > 0 || percentDamage > 0f)
			{
				int damageFromPercent = (int)(Player.statLifeMax2 * percentDamage);
				//damage = Math.Max(constantDamage, damageFromPercent);
				//customDamage = true;
				modifiers.FinalDamage.Base = Math.Max(constantDamage, damageFromPercent);
			}
			else if (defenseEffect >= 0f)
			{
				if (Main.expertMode)
				{
					defenseEffect *= 1.5f;
				}
				modifiers.FinalDamage.Base -= (int)(Player.statDefense * defenseEffect);
				if (modifiers.FinalDamage.Base < 0)
				{
					modifiers.FinalDamage.Base = 1;
				}
				//customDamage = true;
			}
			constantDamage = 0;
			percentDamage = 0f;
			defenseEffect = -1f;
		}

		public override void PreUpdateBuffs()
		{
			if (crystalFountain)
			{
				Player.AddBuff(ModContent.BuffType<CrystalHealing>(), 2);
			}
		}

		public override void PostUpdateBuffs()
		{
			if (Player.FindBuffIndex(ModContent.BuffType<CrystalCharm>()) < 0)
			{
				crystalCharmStacks = 0;
			}
		}
	}
}