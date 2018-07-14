using com.programmerare.shortestpaths.core.api;
using com.programmerare.shortestpaths.core.api.generics;
using com.programmerare.shortestpaths.core.impl.generics;

namespace com.programmerare.shortestpaths.adapter.bsmock.generics
{
    /**
     * @author Tomas Johansson
     */
    public class PathFinderFactoryBsmockGenerics<F, P, E, V, W>
        : PathFinderFactoryGenericsBase<F, P, E, V, W>
        , PathFinderFactoryGenerics<F, P, E, V, W>
        where F : PathFinderGenerics<P, E, V, W>
        where P : PathGenerics<E, V, W>
        where E : EdgeGenerics<V, W>
        where V : Vertex
        where W : Weight
    {
        public override F CreatePathFinder(GraphGenerics<E, V, W> graph)
        {
            PathFinderGenerics<P, E, V, W> pathFinder =  new PathFinderBsmockGenerics<P, E, V, W>(graph);
            // TODO: try to get rid of the casting below ( warning: "Type safety: Unchecked cast from PathFinderYanQi<P,E,V,W> to F" )
            return (F) pathFinder;
        }
    }
}