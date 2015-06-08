using System.Collections.Generic;

namespace Myxini.Recognition
{
	public class BlockNode
	{
		public BlockNode(IBlock block)
		{
			this.Node = block;
		}
		
		public void AddPreviousNode(BlockNode block)
		{
			this.Previous.Add(block);
		}

		public void AddNextNode(BlockNode block)
		{
			this.Next.Add(block);
		}

		public IBlock Node { get; protected set; }
		public BlockNodeList Previous { get; private set; }
		public BlockNodeList Next { get; private set; }
	}
}
