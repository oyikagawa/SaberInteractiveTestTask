using LinkedList;
using System.Collections.Generic;
using System.IO;

namespace SaberInteractiveTestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @".\test.json";
            var sList = CreateSerializeData(out List<ListNode> nodesList);
            AddRandomNode(nodesList);

            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                {
                    sList.Serialize(fs);
                    fs.Seek(0, SeekOrigin.Begin);
                    CreateDeserializeData(fs);
                }
                return;
            }

            using (FileStream fs = File.OpenRead(path))
            {
                CreateDeserializeData(fs);
            }
        }

        private static ListRand CreateSerializeData(out List<ListNode> nodesList)
        {
            int count = 5;
            var list = new ListRand();
            nodesList = new List<ListNode>(count);

            for (int i = 0; i < count; i++)
            {
                nodesList.Add(new ListNode());
            }
            for (int i = 0; i < count; i++)
            {
                nodesList[i].Next = i + 1 > count - 1 ? null : nodesList[i + 1];
                nodesList[i].Prev = i - 1 < 0 ? null : nodesList[i - 1];
                nodesList[i].Data = $"test{i}";
            }

            list.Head = nodesList[0];
            list.Tail = nodesList[^1];
            list.Count = nodesList.Count;

            return list;
        }

        private static void AddRandomNode(List<ListNode> nodesList)
        {
            nodesList[1].Rand = nodesList[1];
            nodesList[2].Rand = nodesList[4];
            nodesList[3].Rand = nodesList[0];
        }

        private static void CreateDeserializeData(FileStream fs)
        {
            var dList = new ListRand();
            dList.Deserialize(fs);
        }
    }
}
