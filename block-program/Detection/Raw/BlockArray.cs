using System.Collections.Generic;

namespace Myxini.Detection.Raw
{
	class BlockArray
	{
		void Add(IBlock block)
		{
			this.Blocks.Add(block);
		}

		IBlock Get(int index)
		{
			return this.Blocks[index];
		}

		List<IBlock> Blocks = new List<IBlock>();
	}
}
