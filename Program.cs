using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListSerialize
{
    class Program
    {
        class ListNode
        {
            public ListNode Previous;
            public ListNode Next;
            public ListNode Random; // произвольный элемент внутри списка
            public string Data;
        }
        class ListRandom
        {
            public ListNode Head;
            public ListNode Tail;
            public int Count;
            public void Serialize(Stream s)
            {
            }
            public void Deserialize(Stream s)
            {
            }
            public void Add(ListNode node)
            {
                Tail.Next = node;
                node.Previous = Tail;
                Tail = node;

                var rand = new Random();
                node.Random = Get(rand.Next(Count));
                Count++;
            }
            public ListNode Get(int id)
            {
                if (id > Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                var tempNode = Head;
                for (int i = 0; i<id;i++)
                {
                    tempNode = tempNode.Next;
                }
                return tempNode;
            }

        }
        static void Main(string[] args)
        {
            var node1 = new ListNode() { Data="1"};
            node1.Random = node1;
            var node2 = new ListNode() { Data = "2" };
            var node3 = new ListNode() { Data = "3" };
            var node4 = new ListNode() { Data = "4" };

            var listRabdom = new ListRandom()
            {
                Head = node1,
                Tail = node1,
                Count=1,
            };

            listRabdom.Add(node2);
            listRabdom.Add(node3);
            listRabdom.Add(node4);
            for (int i = 0; i < listRabdom.Count; i++)
            {
                Console.WriteLine( listRabdom.Get(i).Random.Data);
            }
            
            Console.ReadLine();
        }
    }
}
