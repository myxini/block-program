using System.Collections.Generic;

namespace Myxini.Detection.Raw
{
	class LineArray
	{
		void Add(ILine block)
		{
			this.Lines.Add(block);
		}

		ILine Get(int index)
		{
			return this.Lines[index];
		}

		List<ILine> Lines = new List<ILine>();
	}
}
