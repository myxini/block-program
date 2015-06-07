namespace Myxini.Recognition
{
	public struct BlockParameter
	{
		BlockParameter(int[] parameter)
		{
			this.Value_ = parameter;
		}

		public int Value(int index)
		{
			return this.Value_[index];
		}

		public int ValueLength()
		{
			return this.Value_.Length;
		}

		private int[] Value_;
	}
}
