using System.Collections.Generic;

namespace SharpIA.Search;

internal record MinMaxNode
{
    private bool expanded;
    private MinMaxNode parent;
    private List<MinMaxNode> children;
    private float alfa;
    private float beta;
    private float deepAvaliation;

    public IState State { get; init; }
    public bool Max { get; init; }

    public MinMaxNode(IState state, bool max)
    {
        this.State = state;
        this.Max = max;
        this.children = new List<MinMaxNode>();
        this.parent = null;
        this.expanded = false;
        this.alfa = max ? float.NegativeInfinity : float.PositiveInfinity;
        this.beta = max ? float.PositiveInfinity : float.NegativeInfinity;
    }

    public void Expand(int depth = 1)
    {
        if (depth == 0)
            return;
        
        if (this.expanded)
        {
            update(depth);
            return;
        }
        
        foreach (var next in State.NextMoves())
        {
            MinMaxNode newNode = new MinMaxNode(next, !Max);
            newNode.Expand(depth - 1);
            this.children.Add(newNode);
        }
        this.deepAvaliation = expand(depth);
    }

    private float update(int depth)
    {
        throw new System.NotImplementedException();
    }

    private float expand(int depth)
    {
        this.expanded = true;

        var avaliation = this.State.Avaliation;
        if (depth == 0 || float.IsFinite(avaliation))
            return avaliation;

        float value = float.NaN;
        if (this.Max)
        {
            value = float.NegativeInfinity;
            foreach (var node in this.children)
            {
                if (node.deepAvaliation > value)
                    value = node.deepAvaliation;
                
                if (value >= beta)
                    return value;
                
                if (value > alfa)
                    alfa = value;
            }
        }
        else
        {
            value = float.PositiveInfinity;
            foreach (var node in this.children)
            {
                if (node.deepAvaliation < value)
                    value = node.deepAvaliation;
                
                if (value <= alfa)
                    return value;
                
                if (value < beta)
                    beta = value;
            }
        }
        return value;
    }
}