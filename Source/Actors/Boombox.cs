
namespace Celeste64;

public class Boombox : Actor, IHaveModels, IPickup, IHaveSprites, ICastPointShadow
{
	public SkinnedModel Model;
	public float PointShadowAlpha { get; set; } = 1.0f;
	public string Event;

	private float tCooldown = 0.0f;
	private float tWiggle = 0.2f;
	private float spinSpeed = 0.35f;
	private float wiggleMult = 0.0f;

	public Boombox(string ev)
	{
		Event = ev;
		LocalBounds = new BoundingBox(Vec3.Zero, 3);
		Model = new(Assets.Models["boombox"]);
		foreach (var mat in Model.Materials)
			mat.Color = Color.White * 0.50f;
	}

	public float PickupRadius => 15;

	public bool IsCollected => false;

	public override void Added()
	{
		LocalBounds = new BoundingBox(Vec3.Zero, 8);
		// in case you spawn on it
		tCooldown = 1.0f;
	}

	public void SetCooldown()
	{
		tCooldown = 3.5f;
	}

	public override void Update()
	{
		PointShadowAlpha = IsCollected ? 0.5f : 1.0f;
		Calc.Approach(ref tCooldown, 0, Time.Delta);
		Calc.Approach(ref tWiggle, 0, Time.Delta / 0.7f);
	}

	public void CollectModels(List<(Actor Actor, Model Model)> populate)
	{
		var wiggle = 1 + MathF.Sin(tWiggle * MathF.Tau * 2) * .8f * tWiggle;

		Model.Transform = Model.Transform =
			Matrix.CreateScale(Vec3.One * 2.5f * wiggle) *
			Matrix.CreateTranslation(Vec3.UnitZ * MathF.Sin(World.GeneralTimer * wiggleMult) * 2) *
			Matrix.CreateRotationZ(World.GeneralTimer * spinSpeed);

		populate.Add((this, Model));
	}

	public void Pickup(Player player)
	{
		if (!IsCollected && tCooldown <= 0.0f && !Game.Instance.IsMidTransition)
		{
			UpdateOffScreen = true;
			SetCooldown();
			World.HitStun = 0.05f;
			Game.Instance.Music.Stop();
			Audio.Play(Sfx.sfx_cassette_enter, Position);
			Game.Instance.Music = Audio.Play($"event:/music/{Event}", Position);
			tWiggle = 1.0f;
			spinSpeed = 2.0f;
			wiggleMult = 1.75f;
			foreach (var mat in Model.Materials)
				mat.Color = Color.White;
		}
	}

	public void CollectSprites(List<Sprite> populate)
	{
		if (tWiggle > 0)
		{
			populate.Add(Sprite.CreateBillboard(World, Position, "ring", tWiggle * tWiggle * 40, Color.White * .4f) with { Post = true });
			populate.Add(Sprite.CreateBillboard(World, Position, "ring", tWiggle * 50, Color.White * .4f) with { Post = true });
		}
	}
}
