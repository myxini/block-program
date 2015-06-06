namespace Myxini.Detection
{
	public struct BlockParameter
	{
		BlockParameter(int level, int[] value)
		{
			Value_ = new int[value.Length];
			Level_ = level;
			value.CopyTo(this.Value_, 0);
		}

		public int Level
		{
			get { return Level_; }
			private set { Level_ = value; }
		}

		public int Value(int index)
		{
			return this.Value_[index];
		}

		public int ValueLength()
		{
			return this.Value_.Length;
		}

		private int Level_;
		private int[] Value_;
	}
}
