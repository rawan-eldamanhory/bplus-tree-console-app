using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        public class Node
        {
            public bool IS_LEAF;
            public int[] key;
            public int size;
            public Node[] ptr;
            public int MAX = 3;

            public Node()
            {
                key = new int[MAX];
                ptr = new Node[MAX + 1];
            }
        }
        public class BPTree : Node
        {
            Node root;

            public BPTree()
            {
                root = null;
            }

            //search operation
            public void search(int x)
            {
                if (root == null)
                {
                    Console.WriteLine("Tree is Empty.");
                }
                else
                {
                    Node cursor = root;
                    while (!cursor.IS_LEAF)
                    {
                        for (int i = 0; i < cursor.size; i++)
                        {
                            if (x < cursor.key[i])
                            {
                                cursor = cursor.ptr[i];
                                break;
                            }
                            if (i == cursor.size - 1)
                            {
                                cursor = cursor.ptr[i + 1];
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < cursor.size; i++)
                    {
                        if (cursor.key[i] == x)
                        {
                            Console.WriteLine("Found.");
                            return;
                        }
                    }
                    Console.WriteLine("Not Found!");
                }
            }

            //insert operation
            public void insert(int x)
            {
                if (root == null)
                {
                    root = new Node();
                    root.key[0] = x;
                    root.IS_LEAF = true;
                    root.size = 1;
                }
                else
                {
                    Node cursor = root;
                    Node parent;
                    while (!cursor.IS_LEAF)
                    {
                        parent = cursor;
                        for (int i = 0; i < cursor.size; i++)
                        {
                            if (x < cursor.key[i])
                            {
                                cursor = cursor.ptr[i];
                                break;
                            }
                            if (i == cursor.size - 1)
                            {
                                cursor = cursor.ptr[i + 1];
                                break;
                            }
                        }
                    }
                    if (cursor.size < MAX)
                    {
                        int i = 0;
                        while (x > cursor.key[i] && i < cursor.size)
                            i++;
                        for (int j = cursor.size; j > i; j--)
                        {
                            cursor.key[j] = cursor.key[j - 1];
                        }
                        cursor.key[i] = x;
                        cursor.size++;
                        cursor.ptr[cursor.size] = cursor.ptr[cursor.size - 1];
                        cursor.ptr[cursor.size - 1] = null;
                    }
                    else
                    {
                        Node newLeaf = new Node();
                        int[] virtualNode = new int[MAX + 1];
                        int i = 0;
                        for (i = 0; i < MAX; i++)
                        {
                            virtualNode[i] = cursor.key[i];
                        }
                        int j;
                        while (x > virtualNode[i] && i < MAX)
                            i++;
                       
                        virtualNode[i] = x;
                        newLeaf.IS_LEAF = true;
                        cursor.size = (MAX + 1) / 2;
                        newLeaf.size = MAX + 1 - (MAX + 1) / 2;
                        cursor.ptr[cursor.size] = newLeaf;
                        newLeaf.ptr[newLeaf.size] = cursor.ptr[MAX];
                        cursor.ptr[MAX] = null;
                        for (i = 0; i < cursor.size; i++)
                        {
                            cursor.key[i] = virtualNode[i];
                        }
                        for (i = 0, j = cursor.size; i < newLeaf.size; i++, j++)
                        {
                            newLeaf.key[i] = virtualNode[j];
                        }
                        if (cursor == root)
                        {
                            Node newRoot = new Node();
                            newRoot.key[0] = newLeaf.key[0];
                            newRoot.ptr[0] = cursor;
                            newRoot.ptr[1] = newLeaf;
                            newRoot.IS_LEAF = false;
                            newRoot.size = 1;
                            root = newRoot;
                        }
                        else
                        {
                            parent = cursor;
                            insertInternal(newLeaf.key[0], parent, newLeaf);
                        }
                    }
                }
            }

            //insert operation
            public void insertInternal(int x, Node cursor, Node child)
            {
                if (cursor.size < MAX)
                {
                    int i = 0;
                    while (x > cursor.key[i] && i < cursor.size)
                        i++;
                    for (int j = cursor.size; j > i; j--)
                    {
                        cursor.key[j] = cursor.key[j - 1];
                    }
                    for (int j = cursor.size + 1; j > i + 1; j--)
                    {
                        cursor.ptr[j] = cursor.ptr[j - 1];
                    }
                    cursor.key[i] = x;
                    cursor.size++;
                    cursor.ptr[i + 1] = child;
                }
                else
                {
                    Node newInternal = new Node();
                    int[] virtualKey = new int[MAX + 1];
                    Node[] virtualPtr = new Node[MAX + 2];
                    int i = 0;
                    for (i = 0; i < MAX; i++)
                    {
                        virtualKey[i] = cursor.key[i];
                    }
                    for (i = 0; i < MAX + 1; i++)
                    {
                        virtualPtr[i] = cursor.ptr[i];
                    }
                    int j;
                    while (x > virtualKey[i] && i < MAX)
                        i++;
                    for (j = MAX + 1; j > i; j--)
                    {
                        virtualKey[j] = virtualKey[j - 1];
                    }
                    virtualKey[i] = x;
                    for (j = MAX + 2; j > i + 1; j--)
                    {
                        virtualPtr[j] = virtualPtr[j - 1];
                    }
                    virtualPtr[i + 1] = child;
                    newInternal.IS_LEAF = false;
                    cursor.size = (MAX + 1) / 2;
                    newInternal.size = MAX - (MAX + 1) / 2;
                    for (i = 0, j = cursor.size + 1; i < newInternal.size; i++, j++)
                    {
                        newInternal.key[i] = virtualKey[j];
                    }
                    for (i = 0, j = cursor.size + 1; i < newInternal.size + 1; i++, j++)
                    {
                        newInternal.ptr[i] = virtualPtr[j];
                    }
                    if (cursor == root)
                    {
                        Node newRoot = new Node();
                        newRoot.key[0] = cursor.key[cursor.size];
                        newRoot.ptr[0] = cursor;
                        newRoot.ptr[1] = newInternal;
                        newRoot.IS_LEAF = false;
                        newRoot.size = 1;
                        root = newRoot;
                    }
                    else
                    {
                        insertInternal(cursor.key[cursor.size], FindParent(root, cursor), newInternal);
                    }
                }
            }

            // Find the parent
            Node FindParent(Node cursor, Node child)
            {
                Node parent;
                if (cursor.IS_LEAF || (cursor.ptr[0]).IS_LEAF)
                {
                    return null;
                }
                for (int i = 0; i < cursor.size + 1; i++)
                {
                    if (cursor.ptr[i] == child)
                    {
                        parent = cursor;
                        return parent;
                    }
                    else
                    {
                        parent = FindParent(cursor.ptr[i], child);
                        if (parent != null)
                            return parent;
                    }
                    return parent;
                }
                return null;
            }

            public void Remove(int x)
            {
                if (root == null)
                {
                    Console.WriteLine("Tree empty");
                }
                else
                {
                    Node cursor = root;
                    Node parent = null;
                    int leftSibling = 0, rightSibling = 0;
                    while (!cursor.IS_LEAF)
                    {
                        for (int i = 0; i < cursor.size; i++)
                        {
                            parent = cursor;
                            leftSibling = i - 1;
                            rightSibling = i + 1;
                            if (x < cursor.key[i])
                            {
                                cursor = cursor.ptr[i];
                                break;
                            }
                            if (i == cursor.size - 1)
                            {
                                leftSibling = i;
                                rightSibling = i + 2;
                                cursor = cursor.ptr[i + 1];
                                break;
                            }
                        }
                    }
                    bool found = false;
                    int pos;
                    for (pos = 0; pos < cursor.size; pos++)
                    {
                        if (cursor.key[pos] == x)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Console.WriteLine("Not found");
                        return;
                    }
                    for (int i = pos; i < cursor.size; i++)
                    {
                        cursor.key[i] = cursor.key[i + 1];
                    }
                    cursor.size--;
                    if (cursor == root)
                    {
                        for (int i = 0; i < MAX + 1; i++)
                        {
                            cursor.ptr[i] = null;
                        }
                        if (cursor.size == 0)
                        {
                            Console.WriteLine("Tree died");
                            cursor.key = null;
                            cursor.ptr = null;
                            cursor = null;
                            root = null;
                        }
                        return;
                    }
                    cursor.ptr[cursor.size] = cursor.ptr[cursor.size + 1];
                    cursor.ptr[cursor.size + 1] = null;
                    if (cursor.size >= (MAX + 1) / 2)
                    {
                        return;
                    }
                    if (leftSibling >= 0)
                    {
                        Node leftNode = parent.ptr[leftSibling];
                        if (leftNode.size >= (MAX + 1) / 2 + 1)
                        {
                            for (int i = cursor.size; i > 0; i--)
                            {
                                cursor.key[i] = cursor.key[i - 1];
                            }
                            cursor.size++;
                            cursor.ptr[cursor.size] = cursor.ptr[cursor.size - 1];
                            cursor.ptr[cursor.size - 1] = null;
                            cursor.key[0] = leftNode.key[leftNode.size - 1];
                            leftNode.size--;
                            leftNode.ptr[leftNode.size] = cursor;
                            leftNode.ptr[leftNode.size + 1] = null;
                            parent.key[leftSibling] = cursor.key[0];
                            return;
                        }
                    }
                    if (rightSibling <= parent.size)
                    {
                        Node rightNode = parent.ptr[rightSibling];
                        if (rightNode.size >= (MAX + 1) / 2 + 1)
                        {
                            cursor.size++;
                            cursor.ptr[cursor.size] = cursor.ptr[cursor.size - 1];
                            cursor.ptr[cursor.size - 1] = null;
                            cursor.key[cursor.size - 1] = rightNode.key[0];
                            rightNode.size--;
                            rightNode.ptr[rightNode.size] = rightNode.ptr[rightNode.size + 1];
                            rightNode.ptr[rightNode.size + 1] = null;
                            for (int i = 0; i < rightNode.size; i++)
                            {
                                rightNode.key[i] = rightNode.key[i + 1];
                            }
                            parent.key[rightSibling - 1] = rightNode.key[0];
                            return;
                        }
                    }
                    if (leftSibling >= 0)
                    {
                        Node leftNode = parent.ptr[leftSibling];
                        for (int i = leftNode.size, j = 0; j < cursor.size; i++, j++)
                        {
                            leftNode.key[i] = cursor.key[j];
                        }
                        leftNode.ptr[leftNode.size] = null;
                        leftNode.size += cursor.size;
                        leftNode.ptr[leftNode.size] = cursor.ptr[cursor.size];
                        removeInternal(parent.key[leftSibling], parent, cursor);
                        cursor.key = null;
                        cursor.ptr = null;
                        cursor = null;
                    }
                    else if (rightSibling <= parent.size)
                    {
                        Node rightNode = parent.ptr[rightSibling];
                        for (int i = cursor.size, j = 0; j < rightNode.size; i++, j++)
                        {
                            cursor.key[i] = rightNode.key[j];
                        }
                        cursor.ptr[cursor.size] = null;
                        cursor.size += rightNode.size;
                        cursor.ptr[cursor.size] = rightNode.ptr[rightNode.size];
                        Console.WriteLine("Merging two leaf nodes");
                        removeInternal(parent.key[rightSibling - 1], parent, rightNode);
                        rightNode.key = null;
                        rightNode.ptr = null;
                        rightNode = null;
                    }
                }
            }
            void removeInternal(int x, Node cursor, Node child)
            {
                if (cursor == root)
                {
                    if (cursor.size == 1)
                    {
                        if (cursor.ptr[1] == child)
                        {
                            child.key = null;
                            child.ptr = null;
                            child = null;
                            root = cursor.ptr[0];
                            cursor.key = null;
                            cursor.ptr = null;
                            cursor = null;
                            Console.WriteLine("Changed root node");
                            return;
                        }
                        else if (cursor.ptr[0] == child)
                        {
                            child.key = null;
                            child.ptr = null;
                            child = null;
                            root = cursor.ptr[1];
                            cursor.key = null;
                            cursor.ptr = null;
                            cursor = null;
                            Console.WriteLine("Changed root node");
                            return;
                        }
                    }
                }
                int pos;
                for (pos = 0; pos < cursor.size; pos++)
                {
                    if (cursor.key[pos] == x)
                    {
                        break;
                    }
                }
                for (int i = pos; i < cursor.size; i++)
                {
                    cursor.key[i] = cursor.key[i + 1];
                }
                for (pos = 0; pos < cursor.size + 1; pos++)
                {
                    if (cursor.ptr[pos] == child)
                    {
                        break;
                    }
                }
                for (int i = pos; i < cursor.size + 1; i++)
                {
                    cursor.ptr[i] = cursor.ptr[i + 1];
                }
                cursor.size--;
                if (cursor.size >= (MAX + 1) / 2 - 1)
                {
                    return;
                }
                if (cursor == root)
                    return;
                Node parent = FindParent(root, cursor);
                int leftSibling, rightSibling;
                for (pos = 0; pos < parent.size + 1; pos++)
                {
                    if (parent.ptr[pos] == cursor)
                    {
                        leftSibling = pos - 1;
                        rightSibling = pos + 1;
                        break;
                    }
                }
                leftSibling = 0;
                if (leftSibling >= 0)
                {
                    Node leftNode = parent.ptr[leftSibling];
                    if (leftNode.size >= (MAX + 1) / 2)
                    {
                        for (int i = cursor.size; i > 0; i--)
                        {
                            cursor.key[i] = cursor.key[i - 1];
                        }
                        cursor.key[0] = parent.key[leftSibling];
                        parent.key[leftSibling] = leftNode.key[leftNode.size - 1];
                        for (int i = cursor.size + 1; i > 0; i--)
                        {
                            cursor.ptr[i] = cursor.ptr[i - 1];
                        }
                        cursor.ptr[0] = leftNode.ptr[leftNode.size];
                        cursor.size++;
                        leftNode.size--;
                        return;
                    }
                }
                rightSibling = 0;
                if (rightSibling <= parent.size)
                {
                    Node rightNode = parent.ptr[rightSibling];
                    if (rightNode.size >= (MAX + 1) / 2)
                    {
                        cursor.key[cursor.size] = parent.key[pos];
                        parent.key[pos] = rightNode.key[0];
                        for (int i = 0; i < rightNode.size - 1; i++)
                        {
                            rightNode.key[i] = rightNode.key[i + 1];
                        }
                        cursor.ptr[cursor.size + 1] = rightNode.ptr[0];
                        for (int i = 0; i < rightNode.size; ++i)
                        {
                            rightNode.ptr[i] = rightNode.ptr[i + 1];
                        }
                        cursor.size++;
                        rightNode.size--;
                        return;
                    }
                }
                if (leftSibling >= 0)
                {
                    Node leftNode = parent.ptr[leftSibling];
                    leftNode.key[leftNode.size] = parent.key[leftSibling];
                    for (int i = leftNode.size + 1, j = 0; j < cursor.size; j++)
                    {
                        leftNode.key[i] = cursor.key[j];
                    }
                    for (int i = leftNode.size + 1, j = 0; j < cursor.size + 1; j++)
                    {
                        leftNode.ptr[i] = cursor.ptr[j];
                        cursor.ptr[j] = null;
                    }
                    leftNode.size += cursor.size + 1;
                    cursor.size = 0;
                    removeInternal(parent.key[leftSibling], parent, cursor);
                }
                else if (rightSibling <= parent.size)
                {
                    Node rightNode = parent.ptr[rightSibling];
                    cursor.key[cursor.size] = parent.key[rightSibling - 1];
                    for (int i = cursor.size + 1, j = 0; j < rightNode.size; j++)
                    {
                        cursor.key[i] = rightNode.key[j];
                    }
                    for (int i = cursor.size + 1, j = 0; j < rightNode.size + 1; j++)
                    {
                        cursor.ptr[i] = rightNode.ptr[j];
                        rightNode.ptr[j] = null;
                    }
                    cursor.size += rightNode.size + 1;
                    rightNode.size = 0;
                    removeInternal(parent.key[rightSibling - 1], parent, rightNode);
                }
            }

            // Print the tree
            void display(Node cursor)
            {
                if (cursor != null)
                {
                    for (int i = 0; i < cursor.size; i++)
                    {
                        Console.WriteLine(cursor.key[i]);
                    }
                    if (cursor.IS_LEAF != true)
                    {
                        for (int i = 0; i < cursor.size + 1; i++)
                        {
                            display(cursor.ptr[i]);
                        }
                    }
                }
            }

            // Get the root
            Node getRoot()
            {
                return root;
            }
            static void Main(string[] args)
            {
                BPTree node = new BPTree();
                node.insert(5);
                node.insert(15);
                node.insert(25);
                node.insert(35);
                node.insert(45);

                node.display(node.getRoot());

                Console.WriteLine(".........................");

                node.Remove(15);

                node.display(node.getRoot());

                Console.WriteLine("35 is Found.");

                Console.ReadKey();
            }
        }
    }
}
