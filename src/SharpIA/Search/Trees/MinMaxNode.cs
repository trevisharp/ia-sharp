using System.Collections.Generic;

namespace SharpIA.Search.Trees;

public record MinMaxNode
{
    private bool expanded;
    private MinMaxNode parent;
    private float avaliation;
    private List<MinMaxNode> children;

    public float Avaliation => avaliation;
    public ITreeState State { get; init; }
    public bool IsMax { get; init; }

    public MinMaxNode(ITreeState state, bool max)
    {
        this.State = state;
        this.IsMax = max;
        this.children = new List<MinMaxNode>();
        this.parent = null;
        this.expanded = false;
        this.avaliation = State.Avaliation;
    }

    public void Expand(int depth = 1)
    {
        expand(depth);
        compute();
    }

    public MinMaxNode ChooseBest()
        => IsMax ? getMaxNode() : getMinNode();

    private MinMaxNode getMaxNode()
    {
        MinMaxNode best = null;
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

    private MinMaxNode getMinNode()
    {
        MinMaxNode best = null;
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
            new MinMaxNode(state, !this.IsMax)
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

    private bool compute()
    {
        var newValue = computeNewValue();

        bool hasChanged = this.avaliation != newValue;
        this.avaliation = newValue;

        return hasChanged;
    }

    private float computeNewValue()
    {
        if (children.Count == 0 || !this.expanded)
            return this.avaliation;
        
        return IsMax ? computeMaxNewValue() :
            computeMinNewValue();
    }

    private float computeMaxNewValue()
    {
        float newValue = float.NegativeInfinity;

        foreach (var child in children)
        {
            bool changed = child.compute();
            if (!changed)
                continue;
            
            float value = child.avaliation;
            newValue = value > newValue
                ? value : newValue;
        }

        return newValue;
    }

    private float computeMinNewValue()
    {
        float newValue = float.PositiveInfinity;

        foreach (var child in children)
        {
            bool changed = child.compute();
            if (!changed)
                continue;
            
            float value = child.avaliation;
            newValue = value < newValue
                ? value : newValue;
        }

        return newValue;
    }
}