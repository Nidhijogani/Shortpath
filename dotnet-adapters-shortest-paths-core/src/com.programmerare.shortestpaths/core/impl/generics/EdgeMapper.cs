/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code is made available under the terms of the MIT License.
* https://github.com/TomasJohansson/adapters-shortest-paths/blob/master/adapters-shortest-paths-core/License.txt
*/
using com.programmerare.shortestpaths.core.api;
using com.programmerare.shortestpaths.core.api.generics;
using System.Collections.Generic;

namespace com.programmerare.shortestpaths.core.impl.generics
{
    /**
     * Edge is an interface which the implementations will not know of.
     * Instances are passed as parameter into a construction of an implementation specific graph
     * which will return its own kind of edges.
     * Then those edges will be converted back to the common Edge interface, but in such case 
     * it will be a new instance which may not be desirable if the instances are not 
     * the default implementations provided but this project, but they may be classes with more data methods,
     * and therefore it is desirable to map them back to the original instances, which is the purpose of this class.
     * @author Tomas Johansson
     */
    public sealed class EdgeMapper<E, V, W> 
        where E : EdgeGenerics<V, W>
        where V : Vertex
        where W : Weight
    {
	    private readonly IDictionary<string, E> edgeMapWithVertexIdsAsKey = new Dictionary<string, E>();

	    /**
	     * Precondition: the edges must already be validated. Use GraphEdgesValidator before createEdgeMapper.
	     * It has package level access to reduce the risk of misusing it with precondition violation.   
	     * @param edges a list of edges to be used for constructing a graph. Note that they are assumed to be validated as a precondition.
	     * @return
	     */
	    public static EdgeMapper<E, V, W> CreateEdgeMapper<E, V, W>(IList<E> edges)
            where E : EdgeGenerics<V, W>
            where V : Vertex
            where W : Weight
        {
		    return new EdgeMapper<E, V, W>(edges);
	    }
	
	    private EdgeMapper(IList<E> edges) {
		    foreach (E edge in edges) {
			    string idForMapping = GetIdForMapping(edge);
			    edgeMapWithVertexIdsAsKey.Add(idForMapping, edge);
		    }
	    }

	    public IList<E> GetOriginalObjectInstancesOfTheEdges(IList<E> edges) {
		    IList<E> originalObjectInstancesOfTheEdges = new List<E>();
		    foreach (E edge in edges) {
			    originalObjectInstancesOfTheEdges.Add(edgeMapWithVertexIdsAsKey[GetIdForMapping(edge)]);
		    }		
		    return originalObjectInstancesOfTheEdges;
	    }

	    public E GetOriginalEdgeInstance(string startVertexId, string endVertexId) {
		    return edgeMapWithVertexIdsAsKey[GetIdForMapping(startVertexId, endVertexId)];
	    }
	
	    private string GetIdForMapping(E edge) {
		    return GetIdForMapping(edge.StartVertex, edge.EndVertex);
	    }
	
	    private string GetIdForMapping(V startVertex, V endVertex) {
		    return GetIdForMapping(startVertex.VertexId, endVertex.VertexId);
	    }
	
	    private string GetIdForMapping(string startVertexId, string endVertexId) {
		    return EdgeGenericsImpl<V, W>.CreateEdgeIdValue(startVertexId, endVertexId);
	    }	
    }
}