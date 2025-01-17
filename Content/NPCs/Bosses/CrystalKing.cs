using CrystiliumMod.Content.Items.Weapons;
using CrystiliumMod.Content.Projectiles.CrystalKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;

namespace CrystiliumMod.Content.NPCs.Bosses
{
	[AutoloadBossHead]
	public class CrystalKing : ModNPC
	{
		private int timer = 0;
		private int timer2 = 0;
		private int timer3 = 0;
		private int timer4 = 0;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crystal King");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.width = 184;
			NPC.height = 170;
			NPC.damage = 87;
			NPC.defense = 58;
			NPC.lifeMax = 46500;
			NPC.HitSound = SoundID.NPCHit5;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 60000f;
			NPC.knockBackResist = 0f;
			Music = MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/CrystalKing");
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.boss = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				CrystiliumMod.SpawnCondition,
				new FlavorTextBestiaryInfoElement("Mods.CrystiliumMod.Bestiary.CrystalKing"),
				// new FlavorTextBestiaryInfoElement("The ancient soul of a tyrant king, who became an elemental lich to prolong his reign. In life, he was very fond of swords."),
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0)
			{
				//spawn all gores once
				for (int i = 1; i <= 10; i++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("KingGore" + i).Type);
				}
			}
		}

		public override void AI()
		{
			// TODO: Huge multiplayer syncing issues.

			NPC.TargetClosest(true);
			NPC.spriteDirection = NPC.direction;
			Player player = Main.player[NPC.target];
			if (!player.active || player.dead)
			{
				NPC.TargetClosest(false);
				NPC.velocity.Y = -50;
				timer = 0;
				timer2 = 0;
				timer3 = 0;
				timer4 = 0;
			}
			timer++;
			if (timer <= 900)
			{
				timer2++;
			}
			if (timer <= 900)
			{
				timer3++;
			}
			if (timer >= 900)
			{
				timer4++;
			}

			//start of movement
			if (timer == 3 || timer == 100 || timer == 200 || timer == 300 || timer == 400 || timer == 500)
			{
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				NPC.velocity.Y = direction.Y * 12f;
				NPC.velocity.X = direction.X * 12f;
			}

			if (timer == 75 || timer == 175 || timer == 275 || timer == 375 || timer == 475)
			{
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				NPC.velocity.Y = direction.Y * 6f;
				NPC.velocity.X = direction.X * 6f;
			}

			if (timer >= 600 && timer <= 900)
			{
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				NPC.velocity.Y = direction.Y * 8f;
				NPC.velocity.X = direction.X * 8f;
			}

			if (timer >= 900)
			{
				NPC.velocity.Y = 0;
				NPC.velocity.X = 0;
				if (Main.rand.Next(70) == 0)
				{
					NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<CrystalCultist>());
				}
			}

			//end of move
			if (timer == 1100)
			{
				timer = 0;
			}

			if (timer2 == 50)
			{
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, direction.X * 10f, direction.Y * 10f, ModContent.ProjectileType<Slasher>(), 50, 1, Main.myPlayer, 0, 0);
				timer2 = 0;
			}

			if (timer3 == 225)
			{
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction.X *= 17f;
				direction.Y *= 17f;
				timer3 = 0;
				int amountOfProjectiles = Main.rand.Next(10, 15);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-150, 150) * 0.01f;
					float B = (float)Main.rand.Next(-150, 150) * 0.01f;
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<CultistFire>(), 60, 1, Main.myPlayer, 0, 0);
				}
			}

			if (timer4 >= 25 && Main.expertMode)
			{
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				Vector2 newVect = direction.RotatedBy(System.Math.PI / 13);
				Vector2 newVect2 = direction.RotatedBy(-System.Math.PI / 13);
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, direction.X * 20f, direction.Y * 20f, ModContent.ProjectileType<Kingwave>(), 55, 1, Main.myPlayer, 0, 0);
				if (NPC.life <= 46500)
				{
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, newVect.X * 20f, newVect.Y * 20f, ModContent.ProjectileType<Kingwave>(), 55, 1, Main.myPlayer, 0, 0);
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, newVect2.X * 20f, newVect2.Y * 20f, ModContent.ProjectileType<Kingwave>(), 55, 1, Main.myPlayer, 0, 0);
				}
				timer4 = 0;
			}

			if (timer4 >= 50 && !Main.expertMode)
			{
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				Vector2 newVect = direction.RotatedBy(System.Math.PI / 20);
				Vector2 newVect2 = direction.RotatedBy(-System.Math.PI / 20);
				Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, direction.X * 20f, direction.Y * 20f, ModContent.ProjectileType<Projectiles.CrystalKing.Kingwave>(), 50, 1, Main.myPlayer, 0, 0);
				if (NPC.life <= 23250)
				{
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, newVect.X * 20f, newVect.Y * 20f, ModContent.ProjectileType<Kingwave>(), 50, 1, Main.myPlayer, 0, 0);
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, newVect2.X * 20f, newVect2.Y * 20f, ModContent.ProjectileType<Kingwave>(), 50, 1, Main.myPlayer, 0, 0);
				}

				timer4 = 0;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			int frameWidth = 184; // I'm just hardcoding this, since this is the frame width of one frame along the X axis.
			NPC.spriteDirection = NPC.direction;

			// Now if you want to animate, you can do:
			NPC.frameCounter++;
			if (NPC.frameCounter >= 4)
			{
				NPC.frame.Y += frameHeight;
				if (NPC.frame.Y >= 1360)
				{
					NPC.frame.Y = 0;
					NPC.frame.X = (NPC.frame.X + frameWidth) % (2 * frameWidth);
				}

				NPC.frameCounter = 0;
			}

			NPC.frame.Width = frameWidth;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D drawTexture = TextureAssets.Npc[NPC.type].Value;
			Vector2 origin = new((drawTexture.Width / 2) * 0.5F, (drawTexture.Height / Main.npcFrameCount[NPC.type]) * 0.5F);

			Vector2 drawPos = new(
				NPC.position.X - screenPos.X + (NPC.width / 2) - (TextureAssets.Npc[NPC.type].Value.Width / 2) * NPC.scale / 2f + origin.X * NPC.scale,
				NPC.position.Y - screenPos.Y + NPC.height - TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4f + origin.Y * NPC.scale + NPC.gfxOffY);

			SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(drawTexture, drawPos, NPC.frame, Color.White, NPC.rotation, origin, NPC.scale, effects, 0);

			return false; // We return false, since we don't want vanilla drawing to execute.
		}

		public override void OnKill()
		{
			if (!CrystalWorld.downedCrystalKing)
			{
				CrystalWorld.downedCrystalKing = true;
				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.WorldData);
			}
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.CrystalBag>()));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.KingTrophy>(), 10));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Armor.CrystalMask>(), 10));

			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.CrystiliumBar>(), 1, 13, 20));

			int[] lootTable = {
				ModContent.ItemType<Cryst>(),
				ModContent.ItemType<Callandor>(),
				ModContent.ItemType<QuartzSpear>(),
				ModContent.ItemType<ShiningTrigger>(),
				ModContent.ItemType<Slamborite>(),
				ModContent.ItemType<Shimmer>(),
				ModContent.ItemType<Shatterocket>(),
				ModContent.ItemType<RoyalShredder>() };
			notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, lootTable));

		}
    }
}