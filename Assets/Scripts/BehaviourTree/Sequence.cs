using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override State Evaluate()
        {
            bool childRunning = false;

            foreach(Node node in children)
            {
                switch(node.Evaluate())
                {
                    case State.FAILURE: state = State.FAILURE; return state;
                    case State.SUCCESS: continue;
                    case State.RUNNING: childRunning = true; continue;
                    default: state = State.SUCCESS; return state;

                }
            }
            state = childRunning ? State.RUNNING : State.SUCCESS;
            return state;
        }
    }

}
