using System.Collections.Generic;

namespace Myxini.Recognition.Raw
{
	class LineArray
	{
		/// <summary>
		/// 新たな線を追加する
		/// </summary>
		/// <param name="node">追加したい線</param>
		public void Add(ILine block)
		{
			this.Lines.Add(block);
		}

		/// <summary>
		/// 線を取得する
		/// </summary>
		/// <param name="index">取得したい線のインデックス</param>
		/// <returns>指定したインデックスの線</returns>
		public ILine Get(int index)
		{
			return this.Lines[index];
		}

		/// <summary>
		/// 現在保存されている線の数
		/// </summary>
		public int Count { get { return this.Lines.Count; } }

		private List<ILine> Lines = new List<ILine>();
	}
}
