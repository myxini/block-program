using System.Collections.Generic;

namespace Myxini.Recognition
{
	public class BlockNodeList
	{
		/// <summary>
		/// 新たなノードを追加する
		/// </summary>
		/// <param name="node">追加したいノード</param>
		public void Add(BlockNode node)
		{
			this.NodeList.Add(node);
		}

		/// <summary>
		/// ノードを取得します
		/// </summary>
		/// <param name="index">取得したいノードのインデックス</param>
		/// <returns>取得したノード</returns>
		public BlockNode Get(int index)
		{
			return NodeList[index];
		}

		/// <summary>
		/// 現在保存されているノードの数
		/// </summary>
		public int Count { get { return this.NodeList.Count; } }

		private List<BlockNode> NodeList = new List<BlockNode>();
	}
}
