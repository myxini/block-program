namespace Myxini.Recognition
{
	public interface IBlock
	{
		Command CommandIdentification { get; }
		BlockParameter Parameter { get; }
	}
}
