using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{

    List<TreeNode> _childrens = new List<TreeNode>();
    public List<TreeNode> Childrens => _childrens;

    private string _name;
    public string Name => _name;

    
    public TreeNode(string name)
    {
        _name = name;
    }
    
    // Start is called before the first frame update
    public void Process(Transform root)
    {
        AddNode(root);

        //PrintOut();

    }

    private void AddNode(Transform node)
    {
        TreeNode child = new TreeNode(node.name);
        _childrens.Add(child);
        
        var childrens = node.GetComponentsInChildren<Transform>();
        foreach (Transform c in childrens)
        {
            if(c != node && c.parent == node)
            {
                child.AddNode(c);
            }
        }
    }

    public void PrintOut()
    {
        Debug.Log(_name);
        
        foreach (var c in _childrens)
        {
            c.PrintOut();
        }
    }

    
}
