namespace Myxini.Detection
{
	public interface IBlock
	{
		Command CommandIdentification { get; }
		BlockParameter Parameter { get; }
	}
}
