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
        for (int i = 0; i < pieces.Length; i++)
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

    // Update is called once per frame
    void Update()
    {
        //Test();
    }

    private void Test() {
        if ( Counter( ref timeRemaining)) { // Update every 1 seconds
            
            
            timeRemaining = 0.3f;
            //Debug.Log(tempName + " to " + tempPebble.self.name);
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
            if (piece != null) {
                Debug.Log(piece.gameObject.name);
            }
            startPebble = GetPebbleByPiece(piece);
            Debug.Log(startPebble.self.name);
            //Debug.Log("MUH-"+ GetIndexOfPebble(startPebble));
            pathway = ShortestPebblePath(startPebble, endPebble);
            //getShortestDistance(adj, source, dest, v);
            //pathway = ShortestPebblePath(startPebble, endPebble, prevlist, isVisited);

            pebblePathMade = true;
            animationPlaying = true;
            return false;
        }
        else if (pathway == null && !animationPlaying) {
            Debug.Log("true returned");
            pebblePathMade = false;
            hopPrev = null;
            hopTarget = null;
            return true;
        }
        else {
            Debug.Log("DEQUEUING");
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
        Debug.Log("path length = " + pathLength);
        Queue<Pebble> shortestPath = new Queue<Pebble>();
        for ( int i = 0; i < pathLength; i++) {
            Debug.Log("NNNN " + (path.Peek() + 1));
            shortestPath.Enqueue( boardPlacement[path.Dequeue()]);
        }
        Debug.Log("path length2 = " + shortestPath.Count);
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




 
private Queue<int> getShortestDistance(List<List<int>> adj,
                                          int s, int dest, int v)
{
    Queue<int> result = new Queue<int>();
  int []pred = new int[v];
  int []dist = new int[v];
  if (BFS(adj, s, dest,
          v, pred, dist) == false)
  {
    Debug.Log("Given source and destination" +
                      "are not connected");
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
  Debug.Log("Shortest path length is: " +
                     dist[dest]);
  Debug.Log("Path is ::");
   
  for (int i = path.Count - 1;
           i >= 0; i--)
  {
    Debug.Log(path[i] + 1 + " named path ");
    result.Enqueue( path[i]);
  }
  Debug.Log("length is: " + result.Count);
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
      if (visited[adj[u][i]] == false && boardPlacement[adj[u][i]].piece == null)
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
    return adj;
}
private static void addEdge(List<List<int>> adj,
                            int i, int j)
{
  adj[i-1].Add(j-1);
  adj[j-1].Add(i-1);
}
    /*
    


        //bool isCompleted = false;
        foreach (Pebble neighbor in startPebble.PebblesLinked)
        {
            Queue<int> currentPath = null;
                            int index = GetIndexOfPebble( neighbor); 
                            if (!isVisited[index]) {
                                Queue<int> currentList = Clone(prevList);
                                bool[] currentVisited = Clone(isVisited);
                                currentVisited[index] = true;
                                currentList.Enqueue(index);
                                currentPath = ShortestPebblePath(neighbor, endPebble, currentList, currentVisited);
                            }
        }
    
            int index = GetIndexOfPebble( neighbor); 
            if ( !isVisited[index]) {
                if ( neighbor.Equals( endPebble)) { // dunno if works
                    Debug.Log("SAMEEEE  " + neighbor.gameObject.name );
                    Queue<int> prevListUpdated = Clone(prevList);
                    bool[] isVisitedUpdated = Clone(isVisited);
                    isVisitedUpdated[index] = true;
                    prevListUpdated.Enqueue( index);
                    if ( prevListUpdated != null) {
                        if ( !isCompleted) {
                            shortestPath = prevListUpdated;
                            isCompleted = true;
                        }
                        else if (prevListUpdated.Count + depth < shortestPath.Count) {
                            shortestPath = prevListUpdated;
                        }
                    }
                }
                else {
                    Debug.Log( index + 1 + "        v");
                    Queue<int> prevListUpdated = Clone(prevList);
                    bool[] isVisitedUpdated = Clone(isVisited);
                    isVisitedUpdated[index] = true;
                    prevListUpdated.Enqueue( index);
                    Queue<int> neighborPath = ShortestPebblePath(neighbor, endPebble, prevListUpdated, isVisitedUpdated, depth + 1);
                    if (neighborPath != null && neighborPath.Peek().Equals(endPebble)) {
                        if (isCompleted && (neighborPath.Count + depth < shortestPath.Count || shortestPath == null)) {
                            shortestPath = neighborPath;
                        }
                        else if ( !isCompleted) {
                            shortestPath = neighborPath;
                        }
                        isCompleted = true;
                    }
                    if ( !isCompleted && neighborPath != null && ( shortestPath == null || neighborPath.Count + depth < shortestPath.Count)) {
                        Debug.Log( neighborPath.Count + "       neighbor Path");
                        shortestPath = neighborPath;
                    }
                }
            }



    private Queue<int> ShortestPebblePath(Pebble startPebble, Pebble endPebble, Queue<int> prevList, bool[] isVisited) 
    {
        Queue<int> shortestPath = null;
        foreach (Pebble neighbor in startPebble.PebblesLinked)
        {
            int index = GetIndexOfPebble( neighbor); 
            bool isCompleted = false;
            if ( !isVisited[index]) {
                if ( neighbor.Equals( endPebble)) {
                    if (isCompleted && shortestPath.Count > prevList.Count + 1) {
                        
                    }
                    
                    
                    isCompleted = true;
                }
                else {
                    
                }
            }
        }
        return shortestPath;
        

    }


    private Queue<int> ShortestPebblePath2(Pebble startPebble, Pebble endPebble, Queue<int> prevList)
    {
        Queue<int> shortestPath = null;
        foreach (Pebble neighbor in startPebble.PebblesLinked)
        {
            if (neighbor.visited) {continue;}
            if ( neighbor == endPebble) {
                neighbor.visited = true;
                prevList.Enqueue(GetIndexOfPebble(neighbor));
                return prevList;
            }
            else if (!neighbor.visited ) {
                neighbor.visited = true;
                Queue<int> anotherPath = new Queue<int>();
                foreach (int index in prevList)
                {
                    anotherPath.Enqueue(index);
                }
                anotherPath.Enqueue(GetIndexOfPebble(neighbor));
                Queue<int> pathres = ShortestPebblePath( startPebble, endPebble ,anotherPath);
                if( pathres != null && shortestPath == null) {
                    shortestPath = pathres;
                }
                else if (shortestPath.Count > pathres.Count) {
                    shortestPath = pathres;
                }
            }
        }
        return shortestPath;
    }*/

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
            Debug.Log("pebble " + pebble.gameObject.name);
            if (pebble.piece == piece) {
                return pebble;
            }
        }
        foreach (Pebble pebble in waitPebbles)
        {
            Debug.Log("pebblew " + pebble.gameObject.name);
            if (pebble.piece == piece) {
                return pebble;
            }
        }
        return null; // this shouldn't happen! Means the pebble was not in the boardPlacement array
    }











    // Dijkstra algorithms:


}
