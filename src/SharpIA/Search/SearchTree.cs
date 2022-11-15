namespace SharpIA.Search;

public abstract class SearchTree
{
    public abstract IState Root { get; }

    public abstract void Expand(int depth);
    public abstract void PlayBest();
}