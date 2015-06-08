namespace Myxini.Recognition
{
	public interface IBlock
	{
		bool IsControlBlock { get; }
		Command CommandIdentification { get; }
		BlockParameter Parameter { get; }
	}
}
