/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code is made available under the terms of the MIT License.
* https://github.com/TomasJohansson/adapters-shortest-paths/blob/master/adapters-shortest-paths-core/License.txt
*/
using System.Collections.Generic;

namespace com.programmerare.shortestpaths.utils
{
    /**
     * The purpose of this class is to provide mapping between strings and integer ids
     * for those kind of implementations which requires integers as ids.
     * 
     * For example if you want to provide data to an implementation you may want to specify the 
     * two vertices and the weight for the edge between them like below:
     * A B 12.4
     * B C 13.7
     * A C 11.9
     * 
     * However, some implementations (e.g. the current implementation of https://github.com/yan-qi/k-shortest-paths-java-version
     * require that the vertices are specified with integers like this:
     * 0 1 12.4
     * 1 2 13.7
     * 0 2 11.9 
     * 
     * To be able to use such an implementation, this mapper class was created.
     * 
     * @author Tomas Johansson 
     */
    public sealed class MapperForIntegerIdsAndGeneralStringIds {
	
	    private int integerCounterForVertices;
	
	    private readonly IDictionary<string, int> mapFromGeneralStringToIncreasingInteger = new Dictionary<string, int>();
	    private readonly IDictionary<int, string> mapFromIncreasingIntegerToGeneralString = new Dictionary<int, string>();
	
	    /**
	     * @param integerIdForFirstVertex probably either zero or one
	     */	
	    public static MapperForIntegerIdsAndGeneralStringIds createIdMapper(int integerIdForFirstVertex) {
		    return new MapperForIntegerIdsAndGeneralStringIds(integerIdForFirstVertex);
	    }
	
	    private MapperForIntegerIdsAndGeneralStringIds(int integerIdForFirstVertex) {
		    this.integerCounterForVertices = integerIdForFirstVertex - 1; // minus one because of increasing just before each usage
	    }

	    // --------------------------------------------------------------------------------
	    // --------------------------------------------------------------------------------
	    // The two methods below are the main methods of the class and they are named with the purpose of illustrating the flow,
	    // i.e. you first create (or retrieve if previously created) internally autogenerated integers which are mapped with 
	    // the string parameter, and then you can later get back the string with that integer by invoking the other method below.
	
	    public int createOrRetrieveIntegerId(string id) {
		    if(mapFromGeneralStringToIncreasingInteger.ContainsKey(id)) {
			    return mapFromGeneralStringToIncreasingInteger[id];
		    }
		    else {
			    integerCounterForVertices++;
			    mapFromGeneralStringToIncreasingInteger.Add(id,  integerCounterForVertices);
			    mapFromIncreasingIntegerToGeneralString.Add(integerCounterForVertices, id);
			    return integerCounterForVertices;
		    }
	    }
	    // The above and the below method belong together. See comment further above about how to use them.  
	    public string getBackThePreviouslyStoredGeneralStringIdForInteger(int id) {
		    return mapFromIncreasingIntegerToGeneralString[id];
	    }
	    // --------------------------------------------------------------------------------
	    // --------------------------------------------------------------------------------	
	
	
	    public int getNumberOfVertices() {
		    return mapFromGeneralStringToIncreasingInteger.Count;
	    }
    }
}