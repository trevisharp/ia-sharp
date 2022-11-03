namespace SharpIA.Search;

public class MinMaxTree
{
    private MinMaxNode root;

    public IState Current => root.State;

    public MinMaxTree(IState initial, bool max = true)
    {
        this.root = new MinMaxNode(initial, max);
    }

    public void Expand(int depth)
    {
        this.root.Expand(depth);
    }

    public void PlayBest()
    {
        throw new System.NotImplementedException();
    }
}