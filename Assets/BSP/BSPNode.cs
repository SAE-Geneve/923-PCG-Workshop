
    using UnityEngine;

    public struct BSPNode
    {
        public Bounds Bounds;
        public BSPGenerator.CutDirection CutDirection;

        public BSPNode(Bounds bounds, BSPGenerator.CutDirection cutDirection)
        {
            Bounds = bounds;
            CutDirection = cutDirection;
        }
        
    }
