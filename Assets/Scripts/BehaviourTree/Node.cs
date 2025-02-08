using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bTree
{
    public enum State
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected State state;
        public Node parent;
        protected List<Node> children = new();

        private Dictionary<string, object> dataCtx = new();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach(Node child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual State Evaluate() => State.FAILURE;

        public void SetData(string str, object obj)
        {
            dataCtx[str] = obj;
        }

        public object GetData(string str)
        {
            if (dataCtx.TryGetValue(str, out object obj))
                return obj;

            Node node = parent;
            while(node != null)
            {
                obj = node.GetData(str);
                if(obj != null) return obj;
                node = node.parent;
            }
            return null;
        }
        public bool ClearData(string str)
        {
            if (dataCtx.ContainsKey(str))
            {
                if (dataCtx.Remove(str))
                {
                    Debug.Log("Has Removed Coin");
                }
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool clear = node.ClearData(str);
                if (clear)
                {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }

}

