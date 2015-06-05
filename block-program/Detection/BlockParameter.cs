namespace Myxini.Detection
{
	struct BlockParameter
	{
		BlockParameter(int level, int[] value)
		{
			this.Level = level;
			this.Value = new int[value.Length];
			value.CopyTo(this.Value, 0);
		}

		public int Level { get; private set;}
		public int[] Value { get; private set; }
	}
}
