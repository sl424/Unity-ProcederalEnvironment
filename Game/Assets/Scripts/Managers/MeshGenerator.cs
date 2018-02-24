using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour {

    public SquareMap sq;
    public MeshFilter walls;
    public MeshFilter floor;
    public float wallHeight = 5;

    /* vertices and triangles needed for mesh generation */
    List<Vector3> vertices;
    List<int> triangles;
    
    /* 
     * Mesh  helper data structures for checking outline */
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedList = new HashSet<int>();
    Dictionary<int,List<Triangle>> tD = new Dictionary<int,List<Triangle>>();

    MeshCollider wallCollider;
    int newWallMesh= 0;
    MeshCollider wallCollider2;

    /*
     * Genereate the floor mesh using the randomly generate map
     * and create the vertical wall
     */
    public void GenerateMesh(int[,] map, float squareSize){
        outlines.Clear();
        checkedList.Clear();
        tD.Clear();

        sq =  new SquareMap(map, squareSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for(int x = 0; x < sq.squares.GetLength(0); x++){
            for(int y=0; y < sq.squares.GetLength(1); y++){
                TrigulateSquare(sq.squares[x,y]);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        floor.mesh = mesh;
        createWallMesh();
    }


    /*
     * For each vertice in the outline, create a square
     * and create two triangles for each square.
     */
    void createWallMesh(){
        calculateMesh(); // generate floor mesh and outlines list

        /* Mesh components
         * wv -- wall vertices
         * wt -- wall triangles
         */
        List<Vector3> wv = new List<Vector3>();
        List<int> wt = new List<int>();


        foreach(List<int> outline in outlines){
            for(int i =0; i < outline.Count-1; i++){
                int start = wv.Count;

                //create 4 vertices for each outline vertex
                wv.Add(vertices[outline[i]]); //vertex i
                wv.Add(vertices[outline[i+1]]);//vertex i +1
                wv.Add(vertices[outline[i]]-Vector3.up*wallHeight);//vertex i at wall height
                wv.Add(vertices[outline[i+1]]-Vector3.up*wallHeight);//vertext i+1 at wall height

                //triangle 1
                wt.Add(start+0);
                wt.Add(start+2);
                wt.Add(start+3);

                //Triangle 2
                wt.Add(start+3);
                wt.Add(start+1);
                wt.Add(start+0);
            }
        }

        Mesh wallMesh = new Mesh();
        wallMesh.vertices = wv.ToArray();
        wallMesh.triangles = wt.ToArray();
        walls.mesh = wallMesh;

        // bool flag used to add component if needed
        if (newWallMesh==0){
            wallCollider2 = walls.gameObject.AddComponent<MeshCollider>();
            newWallMesh=1;
        }
        wallCollider2.sharedMesh=wallMesh;
    }


    /*  Trianglulate the following square configuration
     *    * - * 
     *    -   -
     *    * - *
     * There are 4 main control nodes for a total of 16 different
     * combinations/configrations.
     *
     * all Triangles vertices are selected in clock-wise direction
     *
     * case values corresponde to the 4 states of the control nodes
     *   x - 8
     *   x - 4
     *   x - 2
     *   x - 1
     *
     */
    void TrigulateSquare(Square s){
        switch(s.config){
            case 0: 
                break;
            case 1:
                MeshFromPoints(s.midLeft, s.midBottom, s.bottomLeft);
                break;
            case 2:
                MeshFromPoints(s.bottomRight, s.midBottom, s.midRight);
                break;
            case 4:
                MeshFromPoints(s.topRight, s.midRight, s.midTop);
                break;
            case 8:
                MeshFromPoints(s.topLeft, s.midTop, s.midLeft);
                break;
            case 3:
                MeshFromPoints(s.midRight, s.bottomRight, s.bottomLeft, s.midLeft);
                break;
            case 6:
                MeshFromPoints(s.midTop, s.topRight, s.bottomRight, s.midBottom);
                break;
            case 9:
                MeshFromPoints(s.topLeft, s.midTop, s.midBottom, s.bottomLeft);
                break;
            case 12:
                MeshFromPoints(s.topLeft, s.topRight, s.midRight, s.midLeft);
                break;
            case 5:
                MeshFromPoints(s.midTop, s.topRight, s.midRight, s.midBottom, s.bottomLeft, s.midLeft);
                break;
            case 10:
                MeshFromPoints(s.topLeft, s.midTop, s.midRight, s.bottomRight, s.midBottom, s.midLeft);
                break;
            case 7:
                MeshFromPoints(s.midTop, s.topRight, s.bottomRight, s.bottomLeft, s.midLeft);
                break;
            case 11:
                MeshFromPoints(s.topLeft, s.midTop, s.midRight, s.bottomRight, s.bottomLeft);
                break;
            case 13:
                MeshFromPoints(s.topLeft, s.topRight, s.midRight, s.midBottom, s.bottomLeft);
                break;
            case 14:
                MeshFromPoints(s.topLeft, s.topRight, s.bottomRight, s.midBottom, s.midLeft);
                break;
            case 15:
                MeshFromPoints(s.topLeft, s.topRight, s.bottomRight, s.bottomLeft);
                break;

        }
    }

    /*
     * In a close-wise fashion, create all the triangles from
     * the points array
     */
    void MeshFromPoints(params Node[] points){
        assignVertices(points);
        if (points.Length >= 3)
            createTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            createTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            createTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            createTriangle(points[0], points[4], points[5]);
    }

    /*
     * Add point position to the vertices array
     * and assign an index id to track each point
     */
    void assignVertices(Node[] points){
        for(int i = 0; i < points.Length; i++){
            if (points[i].index == -1){
                points[i].index = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    /*
     * add points to the triangles array
     * create triangle using the 3 points
     * link the triangle to its correspoding points
     * so that its possible to check if they belong to the same triangle.
     */
    void createTriangle(Node a, Node b, Node c){
        triangles.Add(a.index);
        triangles.Add(b.index);
        triangles.Add(c.index);

        Triangle triangle = new Triangle (a.index, b.index, c.index);
        addToDict(triangle.indexA, triangle);
        addToDict(triangle.indexB, triangle);
        addToDict(triangle.indexC, triangle);

    }

    /*
     * Goes through each point and check all the triangles it belong
     * to and return a valid outline vertex if found.
     */
    int getNext(int vertex){
        List<Triangle> container = tD[vertex];

        for (int i = 0; i < container.Count; i++){
            Triangle triangle = container[i];
            for(int j = 0; j < 3; j++){
                int next = triangle[j];
                if (next != vertex && !checkedList.Contains(next)){
                    if (isOutline(vertex, next)){
                        return next;
                    }
                }
            }
        }
        return -1;
    }

    /*
     * Recursively follow each vertice points and 
     * get groups of outline points
     * that forms the outline edges.
     */
    void calculateMesh(){
        for(int index = 0; index < vertices.Count; index++){
            if (!checkedList.Contains(index)){
                int newV = getNext(index);
                if (newV != -1){
                    checkedList.Add(index);
                    List<int> newOutline = new List<int>();
                    newOutline.Add(index);
                    outlines.Add(newOutline);
                    followNext(newV, outlines.Count-1);
                    outlines[outlines.Count-1].Add(index);
                }
            }

        }
    }

    /* Recursive helper function */
    void followNext(int index, int outlineIndex){
        outlines[outlineIndex].Add(index);
        checkedList.Add(index);
        int nextindex = getNext(index);
        if (nextindex != -1 ){
            followNext(nextindex, outlineIndex);
        }
    }

    /* check if two vertices belong to the same triangle group or not */
    bool isOutline(int vertexA, int vertexB){
        List<Triangle> containsA = tD[vertexA];
        int shared = 0;
        for (int i = 0; i < containsA.Count; i++){
            if (containsA[i].Contains(vertexB)) {
                shared++;
                if (shared > 1) {break;}
            }
        }
        return shared == 1;
    }

    /* Triangle struct to track its 3 vertices */
    struct Triangle{
        public int indexA;
        public int indexB;
        public int indexC;
        int[] vertices;
        public Triangle(int a, int b, int c){
            indexA = a;
            indexB = b;
            indexC = c;

            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;

        }

        public int this[int i]{
            get {
                return vertices[i];
            }
        }

        public bool Contains(int index){
            return index==indexA || index==indexB || index ==indexC;
        }
    }

    /* link each triangle to its vertices
     * A vertex can be connected to a list of triangles
     * and must be checked.
     */
    void addToDict(int indexKey, Triangle triangle){
        if (tD.ContainsKey(indexKey)){
            tD[indexKey].Add(triangle);
        } else {
            List<Triangle> triList = new List<Triangle>();
            triList.Add(triangle);
            tD.Add(indexKey, triList);
        }
    }


    /*
     * Use the randomly generated map, 
     * create the square for each point that are true/on
     */
    public class SquareMap{
        public Square[,] squares;

        public SquareMap(int[,] map, float squareSize){
            int nodeCountX  = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX*squareSize;
            float mapHeight = nodeCountY*squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++){
                for(int y =0; y < nodeCountY; y++){
                    Vector3 pos = new Vector3(
                            -mapWidth/2 + x*squareSize + squareSize/2, 
                            0, 
                            -mapHeight/2 + y*squareSize + squareSize/2);
                    controlNodes[x,y] = new ControlNode(pos, map[x,y]==1, squareSize);
                }
            }

            squares = new Square[nodeCountX-1, nodeCountY-1];
            for (int x = 0; x < nodeCountX-1; x++){
                for(int y =0; y < nodeCountY-1; y++){
                    squares[x,y] = new Square(controlNodes[x,y+1], 
                                              controlNodes[x+1,y+1], 
                                              controlNodes[x+1,y], 
                                              controlNodes[x,y] );
                }
            }
        }
    }

    public class Square{
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node midTop, midRight, midLeft, midBottom;
        public int config;

        public Square (ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft){
            topLeft = _topLeft;
            topRight = _topRight;
            bottomLeft = _bottomLeft;
            bottomRight = _bottomRight;

            midTop = topLeft.right;
            midRight = bottomRight.above;
            midBottom = bottomLeft.right;
            midLeft = bottomLeft.above;

            if (topLeft.active)
                config += 8;
            if (topRight.active)
                config += 4;
            if(bottomRight.active)
                config += 2;
            if(bottomLeft.active)
                config += 1;
        }

    }

    public class Node {
        public Vector3 position;
        public int index = -1;

        public Node(Vector3 _pos){
            position = _pos;
        }
    }

    public class ControlNode : Node{
        public bool active; //active=wall
        public Node above, right;
        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos){
            active = _active;
            above = new Node(position + Vector3.forward*squareSize/2f);
            right = new Node(position + Vector3.right*squareSize/2f);

        }
    }

}

        /*
        if (newMesh==0){
            //MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider>();
            wallCollider = walls.gameObject.AddComponent<MeshCollider>();
            newMesh=1;
        }
        wallCollider.sharedMesh = mesh;
        */

/*
                checkedList.Add(s.topLeft.index);
                checkedList.Add(s.topRight.index);
                checkedList.Add(s.bottomRight.index);
                checkedList.Add(s.bottomLeft.index);
                */
