namespace SharpIA.Search;

public class AlphaBetaPrunningTree : SearchTree
{
    private AlphaBetaPrunningNode root;

    public override IState Root => root.State;

    public AlphaBetaPrunningTree(IState initial, bool max = true)
    {
        this.root = new AlphaBetaPrunningNode(initial, max);
    }

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