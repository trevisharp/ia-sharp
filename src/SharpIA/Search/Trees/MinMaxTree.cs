namespace SharpIA.Search.Trees;

public class MinMaxTree : SearchTree
{
    private MinMaxNode root;

    public override ITreeState Root => root.State;

    public MinMaxTree(ITreeState initial, bool max = true)
        => this.root = new MinMaxNode(initial, max);

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