
namespace Myxini.Recognition
{
	public class PSDSensorBlock : SensorBlock
	{
		public PSDSensorBlock(int distance) : base(Command.PSD, new BlockParameter(new int[]{distance, 0}))
		{

		}
	}
}
