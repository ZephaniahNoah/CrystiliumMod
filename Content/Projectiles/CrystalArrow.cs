using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CrystiliumMod.Content.Projectiles
{
	public class CrystalArrow : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(82);
			Projectile.damage = 10;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				for (int h = 0; h < 2; h++)
				{
					Vector2 vel = new(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 5f;
					Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center.X, Projectile.Center.Y, vel.X, vel.Y, Mod.Find<ModProjectile>("ShatterEnemy" + (1 + Main.rand.Next(0, 3))).Type, Projectile.damage, 0, Main.myPlayer);
				}
			}
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				for (int h = 0; h < 7; h++)
				{
					Vector2 vel = new(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 5f;
					Projectile.NewProjectile(Projectile.GetSource_Death(), (Projectile.Center.X - 30) + Main.rand.Next(60), (Projectile.Center.Y - 30) + Main.rand.Next(60), vel.X, vel.Y, Mod.Find<ModProjectile>("Shatter" + (1 + Main.rand.Next(0, 3))).Type, Projectile.damage - 6, 0, Main.myPlayer);
				}
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				for (int h = 0; h < 14; h++)
				{
					Vector2 vel = new(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 5f;
					Projectile.NewProjectile(Projectile.GetSource_Death(), (Projectile.Center.X - 30) + Main.rand.Next(60), (Projectile.Center.Y - 30) + Main.rand.Next(60), vel.X, vel.Y, Mod.Find<ModProjectile>("ShatterEnemy" + (1 + Main.rand.Next(0, 3))).Type, Projectile.damage - 6, 0, Main.myPlayer);
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}