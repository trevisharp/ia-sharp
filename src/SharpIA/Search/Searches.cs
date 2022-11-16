using System;
using System.Collections.Generic;

namespace SharpIA.Search;

using Trees;

public static class Searches
{
    public static IEnumerable<IState> GetFullPath(this SearchAlgorithm algorithm)
    {
        var crr = algorithm.ChooseNext();
        yield return crr;

        var next = algorithm.ChooseNext();
        while (crr != next && next != null)
        {
            yield return next;
            crr = next;
            next = algorithm.ChooseNext();
        }
    }

    public static SearchAlgorithm DepthFirstSearch(this IState state, bool findMax = true)
    {
        if (state is ITreeState treeState)
            return new DepthFirstTree(treeState, findMax);
        
        throw new InvalidOperationException(
            $"The state of type {state.GetType().Name} needs inherit ITreeState."
        );
    }

    public static SearchAlgorithm BreadthFirstSearch(this IState state, bool findMax = true)
    {
        if (state is ITreeState treeState)
            return new BreadthFirstTree(treeState, findMax);
        
        throw new InvalidOperationException(
            $"The state of type {state.GetType().Name} needs inherit ITreeState."
        );
    }

    public static SearchAlgorithm MinMaxSearch(this IState state, bool findMax = true)
    {
        if (state is ITreeState treeState)
            return new MinMaxTree(treeState, findMax);
        
        throw new InvalidOperationException(
            $"The state of type {state.GetType().Name} needs inherit ITreeState."
        );
    }

    public static SearchAlgorithm AlphaBetaPrunningSearch(this IState state, bool findMax = true)
    {
        if (state is ITreeState treeState)
            return new AlphaBetaPrunningTree(treeState, findMax);
        
        throw new InvalidOperationException(
            $"The state of type {state.GetType().Name} needs inherit ITreeState."
        );
    }
}