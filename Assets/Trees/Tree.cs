using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trees
{
    public class Tree : MonoBehaviour
    {
        [SerializeField] private Transform _unityRoot;

        private TreeNode _tree;

        private void Start()
        {
            _tree = new TreeNode("Tree");
            _tree.FillTree(_unityRoot);
            //_tree.Print();
            Debug.Log("BFS Order __________________________");
            _tree.BFSOrder();
            
            Debug.Log("DFS Order ----------------------------");
            _tree.DFSOrder();
            
        }


    }
}
