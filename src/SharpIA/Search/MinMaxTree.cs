namespace SharpIA.Search;

public class MinMaxTree : SearchTree
{
    private MinMaxNode root;

    public override IState Root => root.State;

    public MinMaxTree(IState initial, bool max = true)
        => this.root = new MinMaxNode(initial, max);

    public override void Expand(int depth)
        => this.root.Expand(depth);

    public override void PlayBest()
    {
        var newState = this.root.ChooseBest();
        if (newState == null)
            return;
        
        this.root = newState;
    }
}