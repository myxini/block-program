namespace Myxini.Recognition
{
	public class BlockParameter
	{
		public BlockParameter(int[] parameter)
		{
            this.Value_ = parameter;
		}

        public BlockParameter() { }

		public int Value(int index)
		{
			return this.Value_[index];
		}

		public int ValueLength()
		{
			return this.Value_.Length;
		}

		private int[] Value_ = new int[] { };
	}
}
