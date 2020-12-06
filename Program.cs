using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ListSerialize
{
    class Program
    {
        class ListNode
        {
            public ListNode Previous;
            public ListNode Next;
            public ListNode Random;
            public string Data;
        }

        class ListRandom
        {
            public ListNode Head;
            public ListNode Tail;
            public int Count;

            public ListRandom(ListNode head)
            {
                Head = head;
                Tail = head;
                head.Random = head;
                Count= 1;
            }
            public ListRandom() { }

            public void Serialize(Stream s)
            {
                string serializeStr = "";
                var temp = Head;
                for (int i = 0; i<Count; i++)
                {
                    serializeStr += "\"" + temp.Data + "\"" + "{" + FindIndex(temp.Random) + "}";
                    temp = temp.Next;
                }

                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.WriteLine(serializeStr);
                }
            }

            public void Deserialize(Stream s)
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    var str = sr.ReadToEnd();

                    var data = Regex.Matches(str, "(?<=\")[\\w]+(?!=\")");
                    var randNode = Regex.Matches(str, "(?<={)[\\w]+(?!=})");
                    Count = data.Count;

                    if (Count == 1)
                    {
                        var temp = new ListNode();
                        Head = temp;
                        Tail = temp;
                        temp.Data = data[0].ToString();
                        temp.Random = temp;
                        return;
                    }

                    var nodeArr = new ListNode[Count];
                    for (int i = 0; i<Count; i++)
                        nodeArr[i] = new ListNode();

                    for (int i = 0; i< Count; i++)
                    {
                        nodeArr[i].Data = data[i].ToString();
                        if(i == 0)
                        {
                            nodeArr[i].Next = nodeArr[i + 1];
                        }else if (i==Count-1)
                        {
                            nodeArr[i].Previous = nodeArr[i - 1];
                        }
                        else
                        {
                            nodeArr[i].Next = nodeArr[i + 1];
                            nodeArr[i].Previous = nodeArr[i - 1];
                        }
                        nodeArr[i].Random = nodeArr[Convert.ToInt32(randNode[i].ToString())];
                    }

                    Head = nodeArr[0];
                    Tail = nodeArr[Count - 1];
                }
            }

            public void Add(ListNode node)
            {
                Tail.Next = node;
                node.Previous = Tail;
                Tail = node;

                var rand = new Random();
                node.Random = Get(rand.Next(Count+1));
                Count++;
            }

            public ListNode Get(int id)
            {
                if (id > Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                var tempNode = Head;
                for (int i = 0; i < id; i++)
                {
                    tempNode = tempNode.Next;
                }
                return tempNode;
            }

            public int FindIndex(ListNode node)
            {
                var temp = Head;
                for(int i = 0; i < Count;i++)
                {
                    if(temp==node)
                    {
                        return i;
                    }
                    else
                    {
                        temp = temp.Next;
                    }
                }
                return -1;
            }
        }
        static void Main(string[] args)
        {
            var node1 = new ListNode() { Data = "1" };
            var node2 = new ListNode() { Data = "2" };
            var node3 = new ListNode() { Data = "3" };
            var node4 = new ListNode() { Data = "4" };

            var firstList = new ListRandom(node1);

            firstList.Add(node2);
            firstList.Add(node3);
            firstList.Add(node4);

            using (FileStream fs = new FileStream("list.txt", FileMode.Create))
            {
                firstList.Serialize(fs);
            }

            var secondList = new ListRandom();
            using (FileStream fs = new FileStream("list.txt", FileMode.Open))
            {
                secondList.Deserialize(fs);
            }

            Console.WriteLine("firstList \t secondList");
            for (int i=0;i<firstList.Count; i++)
            {
                Console.WriteLine("data:{0} \t {1}", firstList.Get(i).Data,secondList.Get(i).Data);
                Console.WriteLine("rand:{0} \t {1}", firstList.Get(i).Random.Data, secondList.Get(i).Random.Data);
            }

            Console.ReadLine();
        }

    }
}
