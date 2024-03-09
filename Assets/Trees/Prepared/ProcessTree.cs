using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProcessTree : MonoBehaviour
{
    [SerializeField] Transform _rootTransform;
    TreeNode _root = new TreeNode("nope");
    
    // Start is called before the first frame update
    void Start()
    {
        _root = new TreeNode(_rootTransform.name);
        _root.Process(_rootTransform);
        
        Debug.Log("BFS Order ++++++++++++++++++++++++++++++++++++++++++");
        BFSOrder();

        Debug.Log("DFS Order ------------------------------------------");
        DFSOrder();
        
    }

    public void BFSOrder()
    {

        Queue<TreeNode> tempQueue = new Queue<TreeNode>();
            
        tempQueue.Enqueue(_root);

        do
        {
            TreeNode tn = tempQueue.Dequeue();
                
            // Here is process -------------------------------------
            Debug.Log(tn.Name);
                
            foreach (var child in tn.Childrens)
                tempQueue.Enqueue(child);

        } while (tempQueue.Count > 0);

        

    }
        
    public void DFSOrder()
    {

        Stack<TreeNode> _tempQueue = new Stack<TreeNode>();
        List<TreeNode> _visited = new List<TreeNode>();
        
        String treeTrace = "";
            
        _tempQueue.Push(_root);

        do
        {
            TreeNode tn = _tempQueue.Pop();
            _visited.Add(tn);
                
            // Here is process -------------------------------------
            treeTrace += tn.Name + "\n";

            /*
            List<TreeNode> r_childrens = tn.Childrens;
            r_childrens.Reverse();
            
            foreach (var child in r_childrens)*/
            foreach (var child in tn.Childrens)
            {
                if (!_visited.Contains(child))
                {
                    _tempQueue.Push(child);    
                }
            }

        } while (_tempQueue.Count > 0);

        Debug.Log(treeTrace);

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
