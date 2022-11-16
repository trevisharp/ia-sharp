namespace SharpIA.Search;

public abstract class SearchAlgorithm
{
    public abstract IState ChooseNext();
}