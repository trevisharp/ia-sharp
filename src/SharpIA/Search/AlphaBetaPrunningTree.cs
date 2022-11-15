namespace SharpIA.Search;

public class AlphaBetaPrunningTree
{
    private AlphaBetaPrunningNode root;

    public IState Current => root.State;

    public AlphaBetaPrunningTree(IState initial, bool max = true)
    {
        this.root = new AlphaBetaPrunningNode(initial, max);
    }

    public void Expand(int depth)
        => this.root.Expand(depth);

    public void PlayBest()
    {
        var newState = this.root.ChooseBest();
        if (newState == null)
            return;
        
        this.root = newState;
    }
}