using System.Collections.Generic;
using UnityEngine;
using Event;

public class AStarAlgorithmNew : MonoBehaviour {
    private CreateField grid;
    private Node startNode, targetNode;
    public List<Node> openList = new List<Node>();
    public HashSet<Node> closedList = new HashSet<Node>();
    private Statistics2 statistics;
    
    public Dictionary<Node, Node> cameFrom
        = new Dictionary<Node, Node>();
    public Dictionary<Node, int> costSoFar
        = new Dictionary<Node, int>();

    void Awake() {
        statistics = GetComponent<Statistics2>();
        grid = GetComponent<CreateField>();
    }
    private void visualFeedback(IAction action) {
        GetComponent<AnimationQueue>().enqueueAction(action);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !grid.paused) {
            Execute();
        }
    }
    public void Execute() {


        print("new AStar");
        foreach (Node node in grid.GetArray()) {
            if (node.start == true) {
                startNode = node;
            }

            if (node.target == true) {
                targetNode = node;
            }
        }
        AStarAlgo();
    }

    private void AStarAlgo() {
        print("AstarNew");
        openList.Clear();
        closedList.Clear();
        openList.Add(startNode);
        startNode.gCost = 0;
        startNode.hCost = GetManhattenDistance(startNode, targetNode);
        Node currentNode;
        
        // new Stuff
        var frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startNode, 0);
        cameFrom[startNode] = startNode;
        costSoFar[startNode] = 0;

        openList.Add(startNode);

        while (frontier.Count > 0)
        {
            currentNode = frontier.Dequeue();

            if (currentNode != startNode) {
                visualFeedback(new ColorizeAction(Color.magenta, currentNode.fieldCell));
            }

            if (currentNode != targetNode) {
                visualFeedback(new ColorizeAction(Color.magenta, currentNode.fieldCell));
            }
            
            if (currentNode == targetNode)
            {
                GetPath(startNode, targetNode);
                statistics.setVisited(closedList.Count);
                break;
            }

            foreach (var next in grid.GetNeighboringNodes(currentNode))
            {
                int newCost = costSoFar[currentNode] + GetManhattenDistance(currentNode, next);

                if (newCost < costSoFar[next] || !costSoFar.ContainsKey(next))
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + GetManhattenDistance(next, targetNode);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = currentNode;
                }
            }
        }
        
        
    }


    private void GetPath(Node startingNode, Node endNode) {
        List<Node> finalPath = new List<Node>();
        Node currentNode = endNode;
        int count = 0;

        while (currentNode != startingNode) {
            count++;
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
            visualFeedback(new ColorizeAction(Color.blue, currentNode.fieldCell));
        }
        statistics.setPathLength(count);
        finalPath.Reverse();
        grid.path = finalPath;
        visualFeedback(new ColorizeAction(Color.green, startNode.fieldCell));
        visualFeedback(new ColorizeAction(Color.red, targetNode.fieldCell));
    }

    private int GetManhattenDistance(Node nodeA, Node nodeB) {
        int disX = Mathf.Abs(nodeA.cordX - nodeB.cordX);
        int disY = Mathf.Abs(nodeA.cordY - nodeB.cordY);

        return disX + disY;
    }
}