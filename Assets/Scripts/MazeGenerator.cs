using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public int mazeRows;
    public int mazeCols;
    [SerializeField] private GameObject mazeNodePrefab;
    public bool disableNodeSprite;      // bool for disabling the "background"/floor part of the maze node

    private int centerSize = 2;         // the size of the center / starting room 
    private Dictionary<Vector2, MazeNode> allNodes = new Dictionary<Vector2, MazeNode>();
    private List<MazeNode> unvisited = new List<MazeNode>();
    private List<MazeNode> stack = new List<MazeNode>();

    // this array holds 4 center room nodes
    private MazeNode[] centerNodes = new MazeNode[4];
    
    // Node vars to hold current nodes and nodes being checked
    private MazeNode currentNode;
    private MazeNode checkNode;

    // array of all possible neighbor positions
    private Vector2[] neighborPositions = new Vector2[] {new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, -1) };

    [SerializeField] private float nodeSize;
    private GameObject mazeParent;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMaze(mazeRows, mazeCols);
    }

    private void GenerateMaze(int rows, int cols){
        if(mazeParent != null){
            DeleteMaze();
        }
        mazeRows = rows;
        mazeCols = cols;
        CreateLayout();
    }

    // Creates the grid of nodes that will become our maze
    public void CreateLayout(){
        InitValues();

        // set starting point
        Vector2 startPos = new Vector2(-(nodeSize * (mazeCols/2)) + (nodeSize/2), -(nodeSize * (mazeRows / 2)) + (nodeSize / 2));
        Vector2 spawnPos = startPos;

        for(int x = 1; x <= mazeCols; x++){
            for(int y = 1; y <= mazeRows; y++){
                GenerateNode(spawnPos, new Vector2(x, y));
                // increase spawnPos y value
                spawnPos.y += nodeSize;
            }
            // reset spawnPos.y and increase spawnPos.x 
            spawnPos.y = startPos.y;
            spawnPos.x += nodeSize;
        }
        CreateCenter();
        RunAlgorithm();
        MakeExit();
    }

    public void RunAlgorithm(){
        // Get start node, remove from the unvisited list
        unvisited.Remove(currentNode);

        // while we have unvisited nodes
        while(unvisited.Count > 0){
            List<MazeNode> unvisitedNeighbors = GetUnvisitedNeighbors(currentNode);
            // if we have any unvisitied neighbors
            if(unvisitedNeighbors.Count > 0){
                checkNode = unvisitedNeighbors[Random.Range(0,unvisitedNeighbors.Count)];   // get random unvisited neighbor
                // add current node to the stack
                stack.Add(currentNode);
                // Compare and remove walls
                CompareWalls(currentNode, checkNode);
                // set current node to neighbor node
                currentNode = checkNode;
                // mark new current node as visited
                unvisited.Remove(currentNode);
            }
            else if(stack.Count > 0){
                // make current node the most recently added node from the stack
                currentNode = stack[stack.Count - 1];
                // remove it from the stack
                stack.Remove(currentNode);
            }
        }
    }

    private void MakeExit(){
        // create and populate list of all possible edge nodes
        List<MazeNode> edgeNodes = new List<MazeNode>();

        foreach(KeyValuePair<Vector2, MazeNode> node in allNodes){
            if(node.Key.x == 0 || node.Key.x == mazeCols || node.Key.y == 0 || node.Key.y == mazeRows){
                edgeNodes.Add(node.Value);
            }
        }

        // get a random edge node
        MazeNode newNode = edgeNodes[Random.Range(0,edgeNodes.Count)];

        // remove appropriate wall for chose edge node
        if(newNode.gridPos.x == 0){
            RemoveWall(newNode.nScript, 1);
        }
        else if(newNode.gridPos.x == mazeCols){
            RemoveWall(newNode.nScript, 2);
        }
        else if(newNode.gridPos.y == mazeRows){
            RemoveWall(newNode.nScript, 3);
        }
        else{
            RemoveWall(newNode.nScript, 4);
        }

    }

    public List<MazeNode> GetUnvisitedNeighbors(MazeNode curNode){
        // create list to return
        List<MazeNode> neighbors = new List<MazeNode>();
        // create a node object equal to the current node
        MazeNode aNode = curNode; 
        // store current node's grid position
        Vector2 curPos = curNode.gridPos;

        foreach(Vector2 p in neighborPositions){
            // Find pos of neighbor on grid relative to current node
            Vector2 aPos = curPos + p;
            // if a node exists 
            if(allNodes.ContainsKey(aPos)){
                aNode = allNodes[aPos];
            }
            // if node is unvisited 
            if(unvisited.Contains(aNode)){
                neighbors.Add(aNode);
            }
        }
        return neighbors;
    }

    public void CompareWalls(MazeNode cNode, MazeNode nNode){
        // if neighbor is left of current node 
        if(nNode.gridPos.x < cNode.gridPos.x){
            RemoveWall(nNode.nScript, 2);   // remove neighbor's right wall
            RemoveWall(cNode.nScript, 1);   // remove cur's left wall
        }
        // else if neighbor is right of current
        else if(nNode.gridPos.x > cNode.gridPos.x){
            RemoveWall(nNode.nScript, 1);   // remove neighbor's left wall
            RemoveWall(cNode.nScript, 2);   // remove cur's right wall
        }
        // else if neighbor is above current
        else if(nNode.gridPos.y > cNode.gridPos.y){
            RemoveWall(nNode.nScript, 4);   // remove neighbor's lower wall
            RemoveWall(cNode.nScript, 3);   // remove cur's upper wall
        }
        else if(nNode.gridPos.y < cNode.gridPos.y){
            RemoveWall(nNode.nScript, 3);   // remove neighbor's upper wall
            RemoveWall(cNode.nScript, 4);   // remove cur's lower wall            
        }
    }

    public void RemoveWall(NodeScript nScript, int wallID){
        if(wallID == 1){
            nScript.wallL.SetActive(false);
        }
        else if(wallID == 2){
            nScript.wallR.SetActive(false);
        }
        else if(wallID == 3){
            nScript.wallU.SetActive(false);
        }
        else if(wallID == 4){
            nScript.wallD.SetActive(false);
        }
    }

    public void CreateCenter(){
        // get the 4 center nodes using the row/col variables
        centerNodes[0] = allNodes[new Vector2((mazeCols / 2), (mazeRows / 2) + 1)];
        RemoveWall(centerNodes[0].nScript, 4);
        RemoveWall(centerNodes[0].nScript, 2);
        centerNodes[1] = allNodes[new Vector2((mazeCols / 2) + 1, (mazeRows / 2) + 1)];
        RemoveWall(centerNodes[1].nScript, 4);
        RemoveWall(centerNodes[1].nScript, 1);
        centerNodes[2] = allNodes[new Vector2((mazeCols / 2), (mazeRows / 2))];
        RemoveWall(centerNodes[2].nScript, 3);
        RemoveWall(centerNodes[2].nScript, 2);
        centerNodes[3] = allNodes[new Vector2((mazeCols / 2) + 1, (mazeRows / 2))];
        RemoveWall(centerNodes[3].nScript, 3);
        RemoveWall(centerNodes[3].nScript, 1);

        // Create a list of ints, select one randomly and remove it
        // use remaining ints to remove the other 3 center nodes from the unvisited list
        // this will make the center room have only 1 entrance/exit
        List <int> rndList = new List<int> { 0, 1, 2, 3,};
        int startNode = rndList[Random.Range(0, rndList.Count)];

        rndList.Remove(startNode);
        currentNode = centerNodes[startNode];
        foreach(int c in rndList){
            unvisited.Remove(centerNodes[c]);
        }
    }

    public void GenerateNode(Vector2 pos, Vector2 keyPos){
        // create new node object 
        MazeNode newNode = new MazeNode();

        // store its position in grid
        newNode.gridPos = keyPos;
        // set & instantiate Node GameObject
        newNode.nodeObject = Instantiate(mazeNodePrefab, pos, mazeNodePrefab.transform.rotation);
        // make the new node's parent the mazeParent
        if(mazeParent != null){
            newNode.nodeObject.transform.parent = mazeParent.transform;
        }
        // set name of Node object
        newNode.nodeObject.name = "Node - X:" + keyPos.x + " Y:" + keyPos.y;
        // get reference to attached node script
        newNode.nScript = newNode.nodeObject.GetComponent<NodeScript>();
        // disable node sprite (if disableNodeSprite == true)
        if(disableNodeSprite){
            newNode.nodeObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        // add to lists
        allNodes[keyPos] = newNode;
        unvisited.Add(newNode);
    }

    public void DeleteMaze(){
        if(mazeParent != null){
            Destroy(mazeParent);
        }
    }

    public void InitValues(){
        // check generation values to prevent generation from failing
        if(IsOdd(mazeRows)){
            mazeRows--;
        }
        if(IsOdd(mazeCols)){
            mazeCols--;
        }

        // enforce a minimum number of rows/cols
        if(mazeRows <= 3){
            mazeRows = 4;
        }
        if (mazeCols <= 3){
            mazeCols = 4;
        }

        // determine size of node using localScale
        nodeSize = mazeNodePrefab.transform.localScale.x;

        // create empty parent object to hold the maze
        mazeParent = new GameObject();
        mazeParent.transform.position = Vector2.zero;
        mazeParent.name = "Maze";
    }

    public bool IsOdd(int val){
        return val % 2 != 0;
    }

    public class MazeNode{
        public Vector2 gridPos;
        public GameObject nodeObject;
        public NodeScript nScript;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
