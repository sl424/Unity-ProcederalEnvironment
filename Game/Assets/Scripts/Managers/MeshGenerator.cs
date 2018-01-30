using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour {

    public SquareGrid sq;
    public MeshFilter walls;
    public MeshFilter cave;

    List<Vector3> vertices;
    List<int> triangles;
    
    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedList = new HashSet<int>();
    Dictionary<int,List<Triangle>> tD = new Dictionary<int,List<Triangle>>();

   // int newMesh= 0;
    MeshCollider wallCollider;
    int newWallMesh= 0;
    MeshCollider wallCollider2;

    public void GenerateMesh(int[,] map, float squareSize){
        outlines.Clear();
        checkedList.Clear();
        tD.Clear();

        sq =  new SquareGrid(map, squareSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for(int x = 0; x < sq.squares.GetLength(0); x++){
            for(int y=0; y < sq.squares.GetLength(1); y++){
                TrigulateSquare(sq.squares[x,y]);
            }
        }
        Mesh mesh = new Mesh();
//        GetComponent<MeshFilter>().mesh = mesh;
        cave.mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        createWallMesh();


        /*
        if (newMesh==0){
            //MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider>();
            wallCollider = walls.gameObject.AddComponent<MeshCollider>();
            newMesh=1;
        }
        wallCollider.sharedMesh = mesh;
        */
    }


    void createWallMesh(){
        calculateMesh();

        List<Vector3> wallVertices = new List<Vector3>();
        List<int> wallTriangles = new List<int>();
        Mesh wallMesh = new Mesh();
        float wallHeight = 5;

        // using the outline to create a square vertices and
        // 2 triangles 
        foreach(List<int> outline in outlines){
            for(int i =0; i < outline.Count-1; i++){
                int start = wallVertices.Count;

                // 4 vertices and 2 triangles for each square
                wallVertices.Add(vertices[outline[i]]);//left
                wallVertices.Add(vertices[outline[i+1]]);//right
                wallVertices.Add(vertices[outline[i]]-Vector3.up*wallHeight);//left-top
                wallVertices.Add(vertices[outline[i+1]]-Vector3.up*wallHeight);//right-top

                wallTriangles.Add(start+0);
                wallTriangles.Add(start+2);
                wallTriangles.Add(start+3);

                wallTriangles.Add(start+3);
                wallTriangles.Add(start+1);
                wallTriangles.Add(start+0);
            }
        }

        wallMesh.vertices = wallVertices.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        walls.mesh = wallMesh;

        // create MeshCollider
        if (newWallMesh==0){
            //MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider>();
            wallCollider2 = walls.gameObject.AddComponent<MeshCollider>();
            newWallMesh=1;
        }
        //wallCollider = walls.gameObject.AddComponent<MeshCollider>();
        wallCollider2.sharedMesh=wallMesh;
    }


    // These are all the possible combinations of Triangle for each
    // configuration value
    void TrigulateSquare(Square s){
        switch(s.config){
            case 0: 
                break;
            /* 1point cases*/
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

            /* 2points cases*/
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
            /* 3points cases*/
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

            /* 4points cases*/
            case 15:
                MeshFromPoints(s.topLeft, s.topRight, s.bottomRight, s.bottomLeft);
                checkedList.Add(s.topLeft.index);
                checkedList.Add(s.topRight.index);
                checkedList.Add(s.bottomRight.index);
                checkedList.Add(s.bottomLeft.index);
                break;

        }
    }

    // create all the triangle meshes from a list of points 
    // in clockwise direction.
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

    void assignVertices(Node[] points){
        for(int i = 0; i < points.Length; i++){
            if (points[i].index == -1){
                points[i].index = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void createTriangle(Node a, Node b, Node c){
        triangles.Add(a.index);
        triangles.Add(b.index);
        triangles.Add(c.index);

        Triangle triangle = new Triangle (a.index, b.index, c.index);
        addToDict(triangle.indexA, triangle);
        addToDict(triangle.indexB, triangle);
        addToDict(triangle.indexC, triangle);

    }

    //get all neighboring triangles that uses the vertex and return 
    //a valid outline points to follow.
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

    // go through each vertices and create an array of List 
    // each containing the points that forms an closed loop outline
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

    // a recursive call that search and add valid outline points
    // to the array until it returns back to itself.
    void followNext(int index, int outlineIndex){
        outlines[outlineIndex].Add(index);
        checkedList.Add(index);
        int nextindex = getNext(index);
        if (nextindex != -1 ){
            followNext(nextindex, outlineIndex);
        }
    }

    // search through the dictionary book and get
    // all the triangles vertex A belong to and check if 
    // vertex B formed an edge line
    // return true if they shared an edge only 1 time.
    bool isOutline(int vertexA, int vertexB){
        List<Triangle> containsA = tD[vertexA];
        int shared = 0;
        for (int i = 0; i < containsA.Count; i++){
            if (containsA[i].Contains(vertexB)) {
                shared++;
                //if (shared > 1) {break;}
            }
        }
        bool isTrue = shared==1;
        //return shared == 1;
        return isTrue;
    }

    //data structure for triangle and references its 3 points
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

    void addToDict(int indexKey, Triangle triangle){
        if (tD.ContainsKey(indexKey)){
            tD[indexKey].Add(triangle);
        } else {
            List<Triangle> triList = new List<Triangle>();
            triList.Add(triangle);
            tD.Add(indexKey, triList);
        }
    }



    // Take the point map array and create square 2d array of control nodes
    public class SquareGrid{
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize){
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

    // set up nodes to its respective control Node and assign the 
    //  numerical value to each configuration.
    public class Square{
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node midTop, midRight, midLeft, midBottom;
        public int config;

        public Square (ControlNode _topLeft, 
                       ControlNode _topRight, 
                       ControlNode _bottomRight, 
                       ControlNode _bottomLeft){
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

    /* Node and Control Node
      _________________

      + - +
      -   -
      + - + 
      _________________
      
      + > control node, references the  4 points of each square and contains 2 additional nodes
      - > node, auxillary nodes used to form triangles
    */

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
