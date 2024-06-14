
namespace Celeste64;

public class Granny : NPC
{
	public const string DIALOG_ID = "GRANNY";

	public Granny() : base(Assets.Models["granny"])
	{
		Model.Transform = Matrix.CreateScale(3) * Matrix.CreateTranslation(0, 0, -1.5f);
		InteractHoverOffset = new Vec3(0, -2, 16);
		InteractRadius = 32;
		CheckForDialog(DIALOG_ID);
	}

	public override void Interact(Player player)
	{
		World.Add(new Cutscene(Conversation));
	}

	private CoEnumerator Conversation(Cutscene cs)
	{
		yield return Co.Run(cs.MoveToDistance(World.Get<Player>(), Position.XY(), 16));
		yield return Co.Run(cs.FaceEachOther(World.Get<Player>(), this));

		int index = Save.CurrentRecord.GetFlag(DIALOG_ID) + 1;
		yield return Co.Run(cs.Say(Loc.Lines($"{DIALOG_ID}{index}")));
		Save.CurrentRecord.IncFlag(DIALOG_ID);
		CheckForDialog(DIALOG_ID);
	}
}
