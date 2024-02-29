using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trees
{
    public class TreeNode
    {
        private string _name;

        private List<TreeNode> _children = new List<TreeNode>();

        public TreeNode(string name)
        {
            _name = name;
        }

        public void FillTree(Transform _unityRoot)
        {
            AddNode(_unityRoot);
        }
        
        public void AddNode(Transform node)
        {
            TreeNode parent = new TreeNode(node.name);
            _children.Add(parent);
        
            var children = node.GetComponentsInChildren<Transform>();
            foreach (Transform c in children)
            {
                if(c != node && c.parent == node)
                {
                    parent.AddNode(c);
                }
            }
        }

        public void Print()
        {
            Debug.Log(_name);
            foreach (TreeNode child in _children)
            {
                child.Print();
            }
        }
        
        public void BFSOrder()
        {

            Queue<TreeNode> tempQueue = new Queue<TreeNode>();
            string treeTrace = "";
            
            tempQueue.Enqueue(this);

            do
            {
                TreeNode tn = tempQueue.Dequeue();
                
                // Here is process -------------------------------------
                treeTrace += tn._name + "-";
                
                foreach (var child in tn._children)
                    tempQueue.Enqueue(child);

            } while (tempQueue.Count > 0);

            Debug.Log(treeTrace);

        }

        public void DFSOrder()
        {

            Stack<TreeNode> tempStack = new Stack<TreeNode>();
        
            string treeTrace = "";
            
            tempStack.Push(this);

            do
            {
                TreeNode tn = tempStack.Pop();
                
                // Here is process -------------------------------------
                treeTrace += tn._name + " - ";

                for (int i = 0; i < _children.Count; i++)
                {
                        tempStack.Push(_children[i]);    
                }
                
                for (int i = _children.Count; i < 0; i--)
                {
                        tempStack.Push(_children[i]);    
                }

            } while (tempStack.Count > 0);

            Debug.Log(treeTrace);

        }
        
    }
}
