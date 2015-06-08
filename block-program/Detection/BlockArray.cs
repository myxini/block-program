using System.Collections.Generic;

namespace Myxini.Recognition
{
	class BlockArray
	{
		/// <summary>
		/// ブロックを追加する
		/// </summary>
		/// <param name="block">追加するブロック</param>
		public void Add(IBlock block)
		{
			this.Blocks.Add(block);
		}

		/// <summary>
		/// ブロックを取得する
		/// </summary>
		/// <param name="index">取得したいブロックの番号</param>
		/// <returns></returns>
		public IBlock Get(int index)
		{
			return this.Blocks[index];
		}
		
		/// <summary>
		/// 保存されているブロックの数
		/// </summary>
		public int Count 
		{
			get { return this.Blocks.Count; }
		}

		private List<IBlock> Blocks = new List<IBlock>();
	}
}
