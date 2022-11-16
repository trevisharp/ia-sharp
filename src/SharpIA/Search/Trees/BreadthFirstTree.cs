using System.Collections.Generic;

namespace SharpIA.Search.Trees;

public class BreadthFirstTree : SearchTree
{
    private ITreeState root;
    public override ITreeState Root => nexts.Peek();
    
    private bool max;
    public bool FindMax => max;

    private int crr = 0;
    private Stack<ITreeState> nexts = new Stack<ITreeState>();

    public BreadthFirstTree(ITreeState root, bool max = true)
    {
        this.max = max;
        var crr = search();

        while (crr != null)
        {
            nexts.Push(crr);
            crr = crr.Parent;
        }
    }

    public override void ChooseNext()
    {
        if (nexts.Count < 2)
            return;
        
        nexts.Pop();
    }
    
    private ITreeState search()
    {
        Queue<ITreeState> queue = new Queue<ITreeState>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            
            bool posInfty = state.Avaliation == float.PositiveInfinity,
                negInfty = state.Avaliation == float.NegativeInfinity;
            if (FindMax && posInfty || !FindMax && negInfty)
                return state;

            foreach (var next in state.NextMoves())
                queue.Enqueue(next);
        }

        return null;
    }
}