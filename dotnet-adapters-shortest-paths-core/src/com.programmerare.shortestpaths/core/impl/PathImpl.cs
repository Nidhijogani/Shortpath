/*
* Copyright (c) Tomas Johansson , http://www.programmerare.com
* The code in this "core" project is licensed with MIT.
* Other projects within this Visual Studio solution may be released with other licenses e.g. Apache.
* Please find more information in the files "License.txt" and "NOTICE.txt" 
* in the project root directory and/or in the solution root directory.
* It should also be possible to find more license information at this URL:
* https://github.com/TomasJohansson/adapters-shortest-paths-dotnet/
*/
using com.programmerare.shortestpaths.core.api;
using com.programmerare.shortestpaths.core.impl.generics;
using System.Collections.Generic;

namespace com.programmerare.shortestpaths.core.impl
{
    public sealed class PathImpl : PathGenericsImpl<Edge, Vertex, Weight> , Path 
    {

	    private PathImpl(Weight totalWeight, IList<Edge> edges): base(totalWeight, edges) {
	    }

	    public static Path CreatePath(Weight totalWeight, IList<Edge> edges) {
		    return new PathImpl(totalWeight, edges);
	    }

    }
}