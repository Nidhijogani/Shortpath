using edu.ufl.cise.bsmock.graph.util;
using System;
using System.Collections.Generic;

namespace edu.ufl.cise.bsmock.graph.ksp {
    /**
     * Yen's algorithm for computing the K shortest loopless paths between two nodes in a graph.
     *
     * Copyright (C) 2015  Brandon Smock (dr.brandon.smock@gmail.com, GitHub: bsmock)
     *
     * This program is free software: you can redistribute it and/or modify
     * it under the terms of the GNU General Public License as published by
     * the Free Software Foundation, either version 3 of the License, or
     * (at your option) any later version.
     *
     * This program is distributed in the hope that it will be useful,
     * but WITHOUT ANY WARRANTY; without even the implied warranty of
     * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     * GNU General Public License for more details.
     *
     * You should have received a copy of the GNU General Public License
     * along with this program.  If not, see <http://www.gnu.org/licenses/>.
     *
     * Created by Brandon Smock on September 23, 2015.
     * Last updated by Brandon Smock on December 24, 2015.
     */
    public sealed class Yen : KSPAlgorithm {

        public Yen() {}

        public bool IsLoopless() {
            return true;
        }

        /**
         * Computes the K shortest paths in a graph from node s to node t using Yen's algorithm
         *
         * @param graph         the graph on which to compute the K shortest paths from s to t
         * @param sourceLabel   the starting node for all of the paths
         * @param targetLabel   the ending node for all of the paths
         * @param K             the number of shortest paths to compute
         * @return              a list of the K shortest paths from s to t, ordered from shortest to longest
         */
        public IList<Path> Ksp(Graph graph, String sourceLabel, String targetLabel, int K) {
            // Initialize containers for candidate paths and k shortest paths
            List<Path> ksp = new List<Path>();
            java.util.PriorityQueue<Path> candidates = new java.util.PriorityQueue<Path>();

            //try {
                /* Compute and add the shortest path */
                Path kthPath = Dijkstra.ShortestPath(graph, sourceLabel, targetLabel);
                ksp.Add(kthPath);

                /* Iteratively compute each of the k shortest paths */
                for (int k = 1; k < K; k++) {
                    // Get the (k-1)st shortest path
                    Path previousPath = ksp[k-1];

                    /* Iterate over all of the nodes in the (k-1)st shortest path except for the target node; for each node,
                       (up to) one new candidate path is generated by temporarily modifying the graph and then running
                       Dijkstra's algorithm to find the shortest path between the node and the target in the modified
                       graph */
                    for (int i = 0; i < previousPath.Size(); i++) {
                        // Initialize a container to store the modified (removed) edges for this node/iteration
                        java.util.LinkedList<Edge> removedEdges = new java.util.LinkedList<Edge>();

                        // Spur node = currently visited node in the (k-1)st shortest path
                        String spurNode = previousPath.GetEdges().get(i).GetFromNode();

                        // Root path = prefix portion of the (k-1)st path up to the spur node
                        Path rootPath = previousPath.CloneTo(i);

                        /* Iterate over all of the (k-1) shortest paths */
                        foreach(Path p in ksp) {
                            Path stub = p.CloneTo(i);
                            // Check to see if this path has the same prefix/root as the (k-1)st shortest path
                            if (rootPath.Equals((object)stub)) {
                            /* If so, eliminate the next edge in the path from the graph (later on, this forces the spur
                               node to connect the root path with an un-found suffix path) */
                            Edge re = p.GetEdges().get(i);
                                graph.RemoveEdge(re.GetFromNode(), re.GetToNode());
                                removedEdges.add(re);
                            }
                        }

                        /* Temporarily remove all of the nodes in the root path, other than the spur node, from the graph */
                        foreach(Edge rootPathEdge in rootPath.GetEdges()) {
                            String rn = rootPathEdge.GetFromNode();
                            if (!rn.Equals(spurNode)) {
                                removedEdges.addAll(graph.RemoveNode(rn));
                            }
                        }

                        // Spur path = shortest path from spur node to target node in the reduced graph
                        Path spurPath = Dijkstra.ShortestPath(graph, spurNode, targetLabel);

                        // If a new spur path was identified...
                        if (spurPath != null) {
                            // Concatenate the root and spur paths to form the new candidate path
                            Path totalPath = rootPath.Clone();
                            totalPath.AddPath(spurPath);

                            // If candidate path has not been generated previously, add it
                            if (!candidates.contains(totalPath))
                                candidates.add(totalPath, totalPath.GetTotalCost());
                        }

                        // Restore all of the edges that were removed during this iteration
                        graph.AddEdges(removedEdges);
                    }

                    /* Identify the candidate path with the shortest cost */
                    bool isNewPath;
                    do {
                        kthPath = candidates.poll();
                        isNewPath = true;
                        if (kthPath != null) {
                            foreach (Path p in ksp) {
                                // Check to see if this candidate path duplicates a previously found path
                                if (p.Equals(kthPath)) {
                                    isNewPath = false;
                                    break;
                                }
                            }
                        }
                    } while(!isNewPath);

                    // If there were not any more candidates, stop
                    if (kthPath == null)
                        break;

                    // Add the best, non-duplicate candidate identified as the k shortest path
                    ksp.Add(kthPath);
                }
            //} catch (Exception e) {
            //    SystemOut.println(e);
            //    e.printStackTrace();
            //}

            // Return the set of k shortest paths
            return ksp;
        }

        /**
         * An in-progress alternative implementation of Yen's algorithm.
         *
         * @param graph         the graph on which to compute the K shortest paths from s to t
         * @param sourceLabel   the starting node for all of the paths
         * @param targetLabel   the ending node for all of the paths
         * @param K             the number of shortest paths to compute
         * @return              a list of the K shortest paths from s to t, ordered from shortest to longest
         */
        public static  List<Path> Ksp_v2(Graph graph, String sourceLabel, String targetLabel, int K) {
            // Initialize containers for candidate paths and k shortest paths
            List<Path> ksp = new List<Path>();
            java.util.PriorityQueue<Path> candidates = new java.util.PriorityQueue<Path>();

            try {
                /* Compute and add the shortest path */
                Path kthPath = Dijkstra.ShortestPath(graph, sourceLabel, targetLabel);
                ksp.Add(kthPath);

                /* Iteratively compute each of the k shortest paths */
                for (int k = 1; k < K; k++) {
                    // Get the (k-1)st shortest path
                    Path previousPath = ksp[k-1];

                    /* Iterate over all of the nodes in the (k-1)st shortest path except for the target node; for each node,
                       (up to) one new candidate path is generated by temporarily modifying the graph and then running
                       Dijkstra's algorithm to find the shortest path between the node and the target in the modified
                       graph */
                    List<Edge> rootPathEdges = new List<Edge>();
                    var it = previousPath.GetEdges().GetEnumerator();
                    for (int i = 0; i < previousPath.Size(); i++) {
                        if (i > 0) {
                            it.MoveNext();
                            rootPathEdges.Add(it.Current);
                        }

                        // Initialize container to store the edited (removed) edges
                        java.util.LinkedList<Edge> removedEdges = new java.util.LinkedList<Edge>();

                        // Spur node = currently visited node in the (k-1)st shortest path
                        String spurNode = previousPath.GetEdges().get(i).GetFromNode();

                        // Root path = prefix portion of the (k-1)st path up to the spur node
                        // REFACTOR THIS
                        Path rootPath = previousPath.CloneTo(i);

                        /* Iterate over all of the (k-1) shortest paths */
                        foreach(Path p in ksp) {
                            int pSize = p.Size();
                            if (pSize < i)
                                continue;
                            bool rootMatch = true;
                            for (int rootPos = 0; rootPos < i; rootPos++) {
                                if (!p.GetEdges().get(rootPos).Equals(rootPathEdges[rootPos])) {
                                    rootMatch = false;
                                    break;
                                }
                            }
                            // Check to see if this path has the same prefix/root as the (k-1)st shortest path
                            if (rootMatch) {
                                /* If so, eliminate the next edge in the path from the graph (later on, this forces the spur
                                   node to connect the root path with an un-found suffix path) */
                                Edge re = p.GetEdges().get(i);
                                graph.RemoveEdge(re.GetFromNode(),re.GetToNode());
                                removedEdges.add(re);
                            }
                        }

                        /* Temporarily remove all of the nodes in the root path, other than the spur node, from the graph */
                        foreach(Edge rootPathEdge in rootPathEdges) {
                            String rn = rootPathEdge.GetFromNode();
                            if (!rn.Equals(spurNode)) {
                                removedEdges.addAll(graph.RemoveNode(rn));
                            }
                        }

                        // Spur path = shortest path from spur node to target node in the reduced graph
                        Path spurPath = Dijkstra.ShortestPath(graph, spurNode, targetLabel);

                        // If a new spur path was identified...
                        if (spurPath != null) {
                            // Concatenate the root and spur paths to form the new candidate path
                            // REFACTOR THIS?
                            Path totalPath = rootPath.Clone();
                            //Path totalPath2 = new Path(rootPathEdges);
                            totalPath.AddPath(spurPath);

                            // If candidate path has not been generated previously, add it
                            //if (!candidates.contains(totalPath))
                            candidates.add(totalPath, totalPath.GetTotalCost());
                        }

                        // Restore removed edges
                        graph.AddEdges(removedEdges);
                    }

                    /* Identify the candidate path with the shortest cost */
                    bool isNewPath;
                    do {
                        kthPath = candidates.poll();
                        isNewPath = true;
                        if (kthPath != null) {
                            foreach (Path p in ksp) {
                                // Check to see if this candidate path duplicates a previously found path
                                if (p.Equals(kthPath)) {
                                    isNewPath = false;
                                    break;
                                }
                            }
                        }
                    } while(!isNewPath);

                    // If there were not any more candidates, stop
                    if (kthPath == null)
                        break;

                    // Add the best, non-duplicate candidate identified as the k shortest path
                    ksp.Add(kthPath);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                //e.printStackTrace();
            }

            // Return the set of k shortest paths
            return ksp;
        }
    }
}