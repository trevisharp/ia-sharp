namespace SharpIA.Search.Trees;

public class AlphaBetaPrunningTree : SearchTree
{
    private AlphaBetaPrunningNode root;

    public override ITreeState Root => root.State;

    public AlphaBetaPrunningTree(ITreeState initial, bool max = true)
        => this.root = new AlphaBetaPrunningNode(initial, max);

    public void Expand(int depth)
        => this.root.Expand(depth);

    public override void ChooseNext()
    {
        var newState = this.root.ChooseBest();
        if (newState == null)
            return;
        
        this.root = newState;
    }
}