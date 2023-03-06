using System.IO;

namespace LinkedList
{
    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            var serializer = new ListSerializer();
            serializer.Serialize(s, this);
        }

        public void Deserialize(FileStream s)
        {
            var deserializer = new ListDeserializer();
            deserializer.Deserialize(s, this);
        }
    }
}
