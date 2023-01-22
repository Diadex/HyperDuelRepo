using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BoardShadow : Board
{
    public Pebble[] boardPlacement;
    public Pebble[] waitPebbles;
    public Piece[] pieces;
    private float timeRemaining = 1;
    public float hopWaitTime = 0.1f;
    private float hopWaitCounter = 0.1f;
    public float hopDurationTime = 0.7f;
    private float hopDurationCounter = 0f;
    public float hopUpwardsAmount = 0.1f;
    public float inPlaceUpwardsAmount = 1.2f;
    [SerializeField]
    private AnimationCurve hopCurve1;
    [SerializeField]
    private AnimationCurve hopCurve2;
    [SerializeField]
    private AnimationCurve hopCurve3;
    [SerializeField]
    private AnimationCurve hopCurve4;
    public List<List<int>> adj;

    public bool Counter( ref float f) {
        if (f > 0)
        {
            f -= Time.deltaTime;
            return false;
        }
        return true;
    }


    // test variables
    public Pebble tempPebble, previous;
    private readonly Random rnd  = new Random();
    int nextIndex = 0;
    void Start() {
        tempPebble = boardPlacement[0];
        for (int i = 0; i < pieces.Length && i < waitPebbles.Length; i++)
        {
            waitPebbles[i].putPiece(pieces[i]);
            waitPebbles[i].piece.self.transform.position = waitPebbles[i].self.transform.position + new Vector3(0, 0.5f, 0);
        }
        //previous = waitPebbles[0];
        int v = boardPlacement.Length;
        adj = new List<List<int>>(v);
        for (int i = 0; i < v; i++)
        {
            adj.Add(new List<int>());
        }
        adj = getMapConnections(adj);
    }


    private void Test() {
        if ( Counter( ref timeRemaining)) { // Update every 1 seconds
            
            
            timeRemaining = 0.3f;
        }
        //UpwardsJump( pieces[0], (previous.transform.position));
        if ( Hop( pieces[0], (previous.transform.position) ,( tempPebble.self.transform.position))) {
             // hop complete
             do {
                 nextIndex = rnd.Next( 0, tempPebble.PebblesLinked.Length);  // creates a number between 1 and 12
             } while (tempPebble.PebblesLinked[nextIndex] == previous);
             previous = tempPebble;
             tempPebble = tempPebble.PebblesLinked[nextIndex];
        }
    }
    bool pebblePathMade = false;
    bool animationPlaying = false;
    Pebble startPebble = null;
    static Queue<Pebble> pathway = null;// indexes of pebble path
    private Pebble hopTarget = null;
    private Pebble hopPrev = null;
    public bool PathHop( Piece piece, Pebble endPebble) {
        if (!pebblePathMade) {
            startPebble = GetPebbleByPiece(piece);
            pathway = ShortestPebblePath(startPebble, endPebble);
            //getShortestDistance(adj, source, dest, v);
            //pathway = ShortestPebblePath(startPebble, endPebble, prevlist, isVisited);

            pebblePathMade = true;
            animationPlaying = true;
            return false;
        }
        else if (pathway == null && !animationPlaying) {
            pebblePathMade = false;
            hopPrev = null;
            hopTarget = null;
            return true;
        }
        else {
            if (hopPrev == null) {
                hopPrev = pathway.Dequeue();
                hopTarget = pathway.Dequeue();
            }

            if ( Hop( piece, hopPrev.transform.position, hopTarget.transform.position)) {
                if ( pathway.Count > 0) {
                    hopPrev = hopTarget;
                    hopTarget = pathway.Dequeue();
                }
                else { // animation finished...
                    pathway = null;
                    animationPlaying = false;
                }
            }
            return false;
        }
    }


    // for each startPebble's neighbor, 
    //  if the neighbor location is not visited 
    //      if the neighbor is the endPebble, if its length is smaller than the shortestPath or shortestPath is null, shortestPath is that path 
    //      else 
    //      return shortestPath
    //  else return null; // means the search failed 
    private Queue<Pebble> ShortestPebblePath(Pebble startPebble, Pebble endPebble) 
    {
        int v = boardPlacement.Length;
        bool[] isVisited = new bool[v];
        for(int i = 0; i < v; i++) {
            isVisited[i] = false;
        }
        isVisited[GetIndexOfPebble(startPebble)] = true;
        int source = GetIndexOfPebble( startPebble), dest = GetIndexOfPebble( endPebble);
        Queue<int> path = getShortestDistance( adj, source, dest, v);
        int pathLength = path.Count;
        Queue<Pebble> shortestPath = new Queue<Pebble>();
        for ( int i = 0; i < pathLength; i++) {
            shortestPath.Enqueue( boardPlacement[path.Dequeue()]);
        }
        return shortestPath;
    }

     
    private Queue<int> Clone( Queue<int> que) {
        Queue<int> res = new Queue<int>();
        foreach (int item in que)
        {
            res.Enqueue(item);
        }
        return res;
    } 
    private bool[] Clone( bool[] que) {
        bool[] res = new bool[que.Length];
        for (int i = 0; i < que.Length; i++)
        {
            res[i] = que[i];
        }
        return res;
    } 

    public void enableDisableParticlesOf( List<int> pebbleIndexes) {
        foreach (int index in pebbleIndexes)
        {
            Pebble pebble = boardPlacement[index];
            pebble.ParticleSwitch();
        }
    }
    public void disableParticlesOf( List<int> pebbleIndexes) {
        if (pebbleIndexes == null) {
            return;
        }
        foreach (int index in pebbleIndexes)
        {
            Pebble pebble = boardPlacement[index];
            pebble.ParticleDisable();
        }
    }

    public List<int> getAllPebblesInRadius( Piece p) {
        Pebble start = GetPebbleByPiece(p);
        int pebbleStart = GetIndexOfPebble(start);
        int searchLength;
        if (start.isWaitPebble) {
            searchLength = p.startMovementStat;
        }else {
            searchLength = p.movementStat;
        }
        int v = boardPlacement.Length;
        List<int> result = new List<int>();
        List<int> previous = new List<int>();
        previous.Add(pebbleStart);
        List<int> current = new List<int>();
        bool[] isVisited = new bool[v];
        for ( int i = 0; i < v; i++) { isVisited[i] = false;}
        isVisited[pebbleStart] = true;
        int currentDepth = 1;
        while (currentDepth <= searchLength) {
            foreach (int index in previous)
            {
                Pebble pebble = boardPlacement[index];
                Pebble[] neighbors = pebble.PebblesLinked;
                foreach (Pebble neighbor in neighbors)
                {
                    int indexPebble = GetIndexOfPebble(neighbor);
                    if ( !isVisited[indexPebble] && neighbor.piece == null) {
                        current.Add(indexPebble);
                        result.Add(indexPebble);
                        isVisited[indexPebble] = true;
                    }
                }
            }
            previous.Clear();
            int[] currentArray = current.ToArray();
            for (int i = 0; i < currentArray.Length; i++)
            {
                previous.Add(currentArray[i]);
            }
            current.Clear();
            currentDepth ++;
        }
        return result;
    }






 

    private bool Hop( Piece piece, Vector3 startPebble, Vector3 endPebble) {
        Vector3 startPebbleLoc = startPebble + new Vector3(0, 0.5f, 0);
        Vector3 endPebbleLoc = endPebble + new Vector3(0, 0.5f, 0);
        //Vector3 targetLocation = endPebbleLoc;
        Vector3 halfway = (startPebbleLoc + endPebbleLoc) / 2;
        hopDurationCounter += Time.deltaTime;
        float percentageHop = hopDurationCounter * piece.pieceMoveSpeed / hopDurationTime;
        //percentageHop = percentageHop * piece.pieceMoveSpeed;
        if ( percentageHop <= 0.5f) {
            Vector3 vec1 = Vector3.Lerp( startPebbleLoc, halfway, hopCurve1.Evaluate(percentageHop*2));
            Vector3 vec2 = Vector3.Lerp( startPebbleLoc, startPebbleLoc + new Vector3(0, hopUpwardsAmount, 0), hopCurve3.Evaluate(percentageHop*2));
            piece.self.transform.position = vec1 + vec2 - startPebbleLoc;
            return false;
        }
        else if( ( piece.self.transform.position - endPebbleLoc).magnitude >= 0.01) {
            //piece.self.transform.position = Vector3.SmoothDamp( piece.self.transform.position, targetLocation, ref smoothDampCurrentVelocity, piece.pieceMoveSpeed * Time.deltaTime);
            Vector3 vec3 = Vector3.Lerp( halfway, endPebbleLoc, hopCurve2.Evaluate((percentageHop - 0.5f) * 2));
            Vector3 vec4 = Vector3.Lerp( halfway + new Vector3(0, hopUpwardsAmount, 0), halfway, hopCurve4.Evaluate( (percentageHop - 0.5f) * 2));
            piece.self.transform.position = vec3 + vec4 - halfway;

            return false;
        }
        else if ( !Counter( ref hopWaitCounter)) {
            piece.self.transform.position = endPebbleLoc;
            return false;
        }
        else {
            hopWaitCounter = hopWaitTime;
            hopDurationCounter = 0f;
            return true;
        }
    }
    public bool UpwardsJump(Piece piece, Vector3 startPebble) {
        Vector3 startPebbleLoc = startPebble + new Vector3(0, 0.5f, 0);
        Vector3 endPebbleLoc = startPebbleLoc + new Vector3(0, inPlaceUpwardsAmount, 0);
        hopDurationCounter += Time.deltaTime;
        float percentageHop = hopDurationCounter * piece.pieceMoveSpeed / hopDurationTime;
        //percentageHop = percentageHop * piece.pieceMoveSpeed;
        if ( percentageHop <= 0.5f) {
            piece.self.transform.position  = Vector3.Lerp( startPebbleLoc, endPebbleLoc, hopCurve3.Evaluate(percentageHop*2));
            return false;
        }
        else if ( ( piece.self.transform.position - startPebbleLoc).magnitude >= 0.01) {
            piece.self.transform.position = Vector3.Lerp( endPebbleLoc, startPebbleLoc, hopCurve4.Evaluate( (percentageHop - 0.5f) * 2));
            return false;
        }
        else if ( !Counter( ref hopWaitCounter)) {
            piece.self.transform.position = startPebbleLoc;
            return false;
        }
        else {
            hopWaitCounter = hopWaitTime;
            hopDurationCounter = 0f;
            return true;
        }
    }

    public void MovePieceTo(Piece piece, Pebble pebble) {
        piece.self.transform.position = pebble.self.transform.position + new Vector3(0, 0.5f, 0);
    }

    public Piece GetPieceByGameObject(GameObject gameObject) {
        foreach (Piece piece in pieces)
        {
            if (piece.self == gameObject) {
                return piece;
            }
        }
        return null; // this shouldn't happen! Means the piece was not in the pieces array
    }
    public Pebble GetPebbleByGameObject(GameObject gameObject) {
        foreach (Pebble pebble in boardPlacement)
        {
            if (pebble.self == gameObject) {
                return pebble;
            }
        }
        return null; // this shouldn't happen! Means the pebble was not in the boardPlacement array
    }
    public int GetIndexOfPebble(Pebble pebbleSelected) {
        for ( int i = 0; i < boardPlacement.Length; i++) {
            if ( boardPlacement[i] == pebbleSelected) {
                return i; // 0 is boardPlacement
            }
        }
        return -1; // this shouldn't happen! Means the pebble was not in the boardPlacement array
    }
    public Pebble GetPebbleByPiece(Piece piece) {
        foreach (Pebble pebble in boardPlacement)
        {
            if (pebble.piece == piece) {
                return pebble;
            }
        }
        foreach (Pebble pebble in waitPebbles)
        {
            if (pebble.piece == piece) {
                return pebble;
            }
        }
        return null; // this shouldn't happen! Means the pebble was not in the boardPlacement array
    }

































    private Queue<int> getShortestDistance(List<List<int>> adj,
                                            int s, int dest, int v)
    {
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
    private List<List<int>> getMapConnections( List<List<int>> adj) {
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
        addEdge(adj, 34, 1);
        addEdge(adj, 34, 7);
        addEdge(adj, 35, 1);
        addEdge(adj, 35, 7);
        addEdge(adj, 36, 1);
        addEdge(adj, 36, 7);
        addEdge(adj, 37, 1);
        addEdge(adj, 37, 7);
        addEdge(adj, 38, 1);
        addEdge(adj, 38, 7);
        return adj;
    }
    private static void addEdge(List<List<int>> adj,
                                int i, int j)
    {
    adj[i-1].Add(j-1);
    adj[j-1].Add(i-1);
    }
}
