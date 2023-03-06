using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LinkedList
{
	public class ListSerializer
	{
        private delegate string[] GetFormatArgs();

        private ListRand listRand;
        private int randNodesCount;

		public void Serialize(FileStream stream, ListRand list)
        {
			listRand = list;
            randNodesCount = 0;

            var data = BuildSerializeData();
            byte[] info = new UTF8Encoding(true).GetBytes(data);
            stream.Write(info, 0, info.Length);
        }

		private string BuildSerializeData()
        {
            Dictionary<ListNode, int> nodesWIdx = new();
            List<ListNode> randomNodes = new();

            var curNode = listRand.Head;
            string data = "[";

            for (int i = 0; i < listRand.Count; i++)
            {
                if (curNode.Rand != null)
                    randomNodes.Add(curNode.Rand);

                AddNodeDataTo(ref data, curNode, i);
                nodesWIdx[curNode] = i;
                curNode = curNode.Next;
            }
            data += "]";

            AddRandNodesTo(ref data, () => GetRandomNodesIdxs(randomNodes, nodesWIdx));
            return data;
        }

        private void AddNodeDataTo(ref string data, ListNode node, int idx)
        {
            data += "{{"
                + $"Prev:{(node.Prev == null ? "null" : idx - 1)},"
                + $"Next:{(node.Next == null ? "null" : idx + 1)},"
                + $"Rand:{GetNodeRandData(node)},"
                + $"Data:'{node.Data}'"
                + "}},";
        }

        private string GetNodeRandData(ListNode node)
        {
            string randData;
            if (node.Rand == null)
                randData = "null";
            else
            {
                randData = "{" + $"{randNodesCount}" + "}";
                randNodesCount++;
            }
            return randData;
        }

        private string[] GetRandomNodesIdxs(List<ListNode> randomNodes, Dictionary<ListNode, int> nodesWIdx)
        {
            var idxs = new string[randomNodes.Count];
            for (int i = 0; i < randomNodes.Count; i++)
            {
                idxs[i] = $"{nodesWIdx[randomNodes[i]]}";
            }
            return idxs;
        }

        private void AddRandNodesTo(ref string data, GetFormatArgs getFormatArgs)
        {
            if (randNodesCount == 0)
                return;
            var idxs = getFormatArgs?.Invoke();
            data = string.Format(data, idxs);
        }
    }
}
