using System.Collections.Generic;

namespace SharpIA.Search.Trees;

internal record AlphaBetaPrunningNode
{
    private bool expanded;
    private AlphaBetaPrunningNode parent;
    private float avaliation;
    private List<AlphaBetaPrunningNode> children;

    public float Avaliation => avaliation;
    public ITreeState State { get; init; }
    public bool IsMax { get; init; }

    public AlphaBetaPrunningNode(ITreeState state, bool max)
    {
        this.State = state;
        this.IsMax = max;
        this.children = new List<AlphaBetaPrunningNode>();
        this.parent = null;
        this.expanded = false;
        this.avaliation = float.NaN;
    }

    public void Expand(int depth = 1)
    {
        expand(depth);
        compute();
    }

    public AlphaBetaPrunningNode ChooseBest()
        => IsMax ? getMaxNode() : getMinNode();

    private AlphaBetaPrunningNode getMaxNode()
    {
        AlphaBetaPrunningNode best = null;
        float bestAvaliation = float.NegativeInfinity;

        foreach (var child in children)
        {
            if (child.avaliation > bestAvaliation)
            {
                bestAvaliation = child.avaliation;
                best = child;
            }
        }

        return best;
    }

    private AlphaBetaPrunningNode getMinNode()
    {
        AlphaBetaPrunningNode best = null;
        float bestAvaliation = float.PositiveInfinity;

        foreach (var child in children)
        {
            if (child.avaliation < bestAvaliation)
            {
                bestAvaliation = child.avaliation;
                best = child;
            }
        }

        return best;
    }

    private void addChild(ITreeState state)
        => this.children.Add(
            new AlphaBetaPrunningNode(state, !this.IsMax)
            {
                parent = this
            }
        );

    private void expand(int depth)
    {
        if (depth < 1)
            return;
        
        if (!this.expanded)
        {
            foreach (var next in State.NextMoves())
                addChild(next);
        }
        
        foreach (var child in children)
            child.Expand(depth - 1);
        
        this.expanded = true;
    }

    private void compute(
        float alfa = float.NegativeInfinity, 
        float beta = float.PositiveInfinity)
            => this.avaliation = computeNewValue(alfa, beta);

    private float computeNewValue(float alfa, float beta)
    {
        if (children.Count == 0 || !this.expanded)
            return State.Avaliation;
        
        return IsMax ? computeMaxNewValue(alfa, beta) :
            computeMinNewValue(alfa, beta);
    }

    private float computeMaxNewValue(float alfa, float beta)
    {
        float newValue = float.NegativeInfinity;

        foreach (var child in children)
        {
            child.compute(alfa, beta);
            
            float value = child.avaliation;
            newValue = value > newValue
                ? value : newValue;
            if (value > beta)
                break;
            
            alfa = alfa > newValue 
                ? alfa : newValue;
        }

        return newValue;
    }

    private float computeMinNewValue(float alfa, float beta)
    {
        float newValue = float.PositiveInfinity;

        foreach (var child in children)
        {
            child.compute(alfa, beta);
            
            float value = child.avaliation;
            newValue = value < newValue
                ? value : newValue;
            if (value < alfa)
                break;
            
            beta = beta < newValue 
                ? beta : newValue;
        }

        return newValue;
    }
}