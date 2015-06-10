using System.Collections.Generic;

namespace Myxini.Recognition.Raw
{
	class BlockArray
	{
		/// <summary>
		/// ブロックを追加します
		/// </summary>
		/// <param name="block">追加したいブロック</param>
		void Add(IRawBlock block)
		{
			this.Blocks.Add(block);
		}

		/// <summary>
		/// ブロックを取得します
		/// </summary>
		/// <param name="index">取得したいブロックのインデックス</param>
		/// <returns>指定したインデックスのブロック</returns>
		IRawBlock Get(int index)
		{
			return this.Blocks[index];
		}

		/// <summary>
		/// 現在保存されているブロックの数
		/// </summary>
		int Count { get { return this.Blocks.Count; } }

		List<IRawBlock> Blocks = new List<IRawBlock>();
	}
}
