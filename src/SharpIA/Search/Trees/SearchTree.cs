namespace SharpIA.Search.Trees;

public abstract class SearchTree
{
    public abstract ITreeState Root { get; }

    public abstract void ChooseNext();
}