using System.Collections.Generic;

namespace Myxini.Detection.Raw
{
	class IBoard
	{
		List<ILine> LineList { get; protected set; }
		List<IBlock> BlockList { get; protected set; }
	}
}
