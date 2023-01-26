using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestPathFinder : MonoBehaviour
{
    Pebble[] boardPlacement;
    public ShortestPathFinder( Pebble[] boardP) {
        boardPlacement = boardP;
    }
    public Queue<int> getShortestDistance(List<List<int>> adj,
                                            int s, int dest, int v, Pebble[] boardP)
    {
        boardPlacement = boardP;
        Queue<int> result = new Queue<int>();
        int []pred = new int[v];
        int []dist = new int[v];
        if (BFS(adj, s, dest,
                v, pred, dist) == false)
        {
            return null;
        }
        // List to store path
        List<int> path = new List<int>();
        int crawl = dest;
        path.Add(crawl);
        while (pred[crawl] != -1)
        {
            path.Add(pred[crawl]);
            crawl = pred[crawl];
        }
        
        for (int i = path.Count - 1;
                i >= 0; i--)
        {
            result.Enqueue( path[i]);
        }
        return result;
    }
    



    // a modified version of BFS that
    // stores predecessor of each vertex
    // in array pred and its distance
    // from source in array dist
    private bool BFS(List<List<int>> adj,
                            int src, int dest,
                            int v, int []pred,
                            int []dist)
    {
    // a queue to maintain queue of
    // vertices whose adjacency list
    // is to be scanned as per normal
    // BFS algorithm using List of int type
    List<int> queue = new List<int>();
    
    // bool array visited[] which
    // stores the information whether
    // ith vertex is reached at least
    // once in the Breadth first search
    bool []visited = new bool[v];
    
    // initially all vertices are
    // unvisited so v[i] for all i
    // is false and as no path is
    // yet constructed dist[i] for
    // all i set to infinity
    for (int i = 0; i < v; i++)
    {
        visited[i] = false;
        dist[i] = int.MaxValue;
        pred[i] = -1;
    }
    
    // now source is first to be
    // visited and distance from
    // source to itself should be 0
    visited[src] = true;
    dist[src] = 0;
    queue.Add(src);
    
    // bfs Algorithm
    while (queue.Count != 0)
    {
        int u = queue[0];
        queue.RemoveAt(0);
        
        for (int i = 0;
                i < adj[u].Count; i++)
        {
            if (visited[adj[u][i]] == false && boardPlacement[adj[u][i]].piece == null && !boardPlacement[adj[u][i]].isWaitPebble)
            {
                visited[adj[u][i]] = true;
                dist[adj[u][i]] = dist[u] + 1;
                pred[adj[u][i]] = u;
                queue.Add(adj[u][i]);
        
                // stopping condition (when we
                // find our destination)
                if (adj[u][i] == dest)
                return true;
            }
        }
    }
    return false;
    }
    public List<List<int>> getMapConnections( List<List<int>> adj) {
        addEdge(adj, 1, 2);
        addEdge(adj, 3, 2);
        addEdge(adj, 3, 4);
        addEdge(adj, 5, 4);
        addEdge(adj, 5, 6);
        addEdge(adj, 7, 6);
        addEdge(adj, 1, 8);
        addEdge(adj, 13, 8);
        addEdge(adj, 13, 17);
        addEdge(adj, 22, 17);
        addEdge(adj, 12, 7);
        addEdge(adj, 12, 16);
        addEdge(adj, 16, 21);
        addEdge(adj, 21, 28);
        addEdge(adj, 22, 23);
        addEdge(adj, 24, 23);
        addEdge(adj, 24, 25);
        addEdge(adj, 26, 25);
        addEdge(adj, 26, 27);
        addEdge(adj, 26, 27);
        addEdge(adj, 28, 27);
        addEdge(adj, 9, 10);
        addEdge(adj, 11, 10);
        addEdge(adj, 11, 15);
        addEdge(adj, 20, 15);
        addEdge(adj, 20, 19);
        addEdge(adj, 18, 19);
        addEdge(adj, 18, 14);
        addEdge(adj, 9, 14);
        addEdge(adj, 1, 9);
        addEdge(adj, 11, 7);
        addEdge(adj, 3, 10);
        addEdge(adj, 20, 28);
        addEdge(adj, 19, 26);
        addEdge(adj, 18, 22);
        addEdge(adj, 29, 22);
        addEdge(adj, 29, 28);
        addEdge(adj, 30, 22);
        addEdge(adj, 30, 28);
        addEdge(adj, 31, 22);
        addEdge(adj, 31, 28);
        addEdge(adj, 32, 22);
        addEdge(adj, 32, 28);
        addEdge(adj, 33, 22);
        addEdge(adj, 33, 28);
        addEdge(adj, 34, 22);
        addEdge(adj, 34, 28);
        addEdge(adj, 35, 1);
        addEdge(adj, 35, 7);
        addEdge(adj, 36, 1);
        addEdge(adj, 36, 7);
        addEdge(adj, 37, 1);
        addEdge(adj, 37, 7);
        addEdge(adj, 38, 1);
        addEdge(adj, 38, 7);
        addEdge(adj, 39, 1);
        addEdge(adj, 39, 7);
        addEdge(adj, 40, 1);
        addEdge(adj, 40, 7);
        return adj;
    }
    private static void addEdge(List<List<int>> adj,
                                int i, int j)
    {
    adj[i-1].Add(j-1);
    adj[j-1].Add(i-1);
    }
}
