namespace Myxini.Recognition
{
	public class MicroSwitchBlock : SensorBlock
	{
		MicroSwitchBlock(bool state) : base(Command.MicroSwitch, new BlockParameter(new int[]{state ? 1 : 0}))
		{

		}
	}
}
