﻿using System.Collections.Generic;

namespace Myxini.Detection.Raw
{
	class LineList
	{
		void Add(ILine block)
		{
			this.Lines.Add(block);
		}

		ILine Get(int index)
		{
			return this.Lines[index];
		}

		List<ILine> Lines;
	}
}
