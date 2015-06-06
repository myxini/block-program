using System.Collections.Generic;

namespace Myxini.Detection.Raw
{
	class BlockArray
	{
		/// <summary>
		/// ブロックを追加します
		/// </summary>
		/// <param name="block">追加したいブロック</param>
		void Add(IBlock block)
		{
			this.Blocks.Add(block);
		}

		/// <summary>
		/// ブロックを取得します
		/// </summary>
		/// <param name="index">取得したいブロックのインデックス</param>
		/// <returns>指定したインデックスのブロック</returns>
		IBlock Get(int index)
		{
			return this.Blocks[index];
		}

		/// <summary>
		/// 現在保存されているブロックの数
		/// </summary>
		int Count { get { return this.Blocks.Count; } }

		List<IBlock> Blocks = new List<IBlock>();
	}
}
