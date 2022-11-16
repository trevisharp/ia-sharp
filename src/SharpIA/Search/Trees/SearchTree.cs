namespace SharpIA.Search.Trees;

public abstract class SearchTree : SearchAlgorithm
{
    public abstract ITreeState Root { get; }
}