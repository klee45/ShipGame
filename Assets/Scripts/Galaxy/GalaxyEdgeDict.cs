using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GalaxyEdgeDict : MonoBehaviour
{
    [SerializeField]
    private GalaxyMapEdge edgePrefab;

    [Header("Constellations in order")]
    [Space(15)]
    [SerializeField]
    private GameObject[] constellations;

    [Header("Intraconstellation edges")]
    [Space(4)]
    [SerializeField]
    private Vector2Int[] orionEdges;
    [Space(4)]
    [SerializeField]
    private Vector2Int[] libraEdges;
    [Space(15)]
    [SerializeField]
    private Vector2Int[] cetusEdges;

    [Header("Connections between constellations")]
    [SerializeField]
    private StringPair[] constellationConnectEdges;

    private Dictionary<GalaxyMapVertex, List<GalaxyMapVertex>> edges;

    private void Awake()
    {
        edges = new Dictionary<GalaxyMapVertex, List<GalaxyMapVertex>>();
    }

    private void Start()
    {
        List<Vector2Int[]> allEdges = new List<Vector2Int[]> { orionEdges, libraEdges, cetusEdges };
        List<GalaxyMapVertex[]> allVertices = new List<GalaxyMapVertex[]>();
        List<string> names = new List<string>();

        GameObject constellationEdgeContainer = AddContainer("Intra-constellation edges", transform);
        foreach (GameObject constellation in constellations)
        {
            allVertices.Add(constellation.GetComponentsInChildren<GalaxyMapVertex>());
            names.Add(constellation.name);
        }
        
        for (int i = 0; i < constellations.Length; i++)
        {
            SetupConstellationEdges(allVertices[i], allEdges[i], names[i], constellationEdgeContainer);
        }

        ConnectConstellations(allVertices.SelectMany(i => i));
    }

    public IReadOnlyDictionary<GalaxyMapVertex, List<GalaxyMapVertex>> GetEdges()
    {
        return edges;
    }

    private void SetupConstellationEdges(GalaxyMapVertex[] vertices, Vector2Int[] edges, string name, GameObject overallContainer)
    {
        GameObject container = AddContainer(name, overallContainer.transform);
        foreach (Vector2Int edge in edges)
        {
            GalaxyMapVertex first = vertices[edge.x - 1];
            GalaxyMapVertex second = vertices[edge.y - 1];
            AddDoubleConnections(first, second, container);
        }
    }

    private void ConnectConstellations(IEnumerable<GalaxyMapVertex> vertices)
    {
        GameObject container = AddContainer("Inter-constellation edges", transform);
        Dictionary<string, GalaxyMapVertex> nameDict = new Dictionary<string, GalaxyMapVertex>();
        foreach (GalaxyMapVertex vertex in vertices)
        {
            string name = vertex.GetSectorID();
            nameDict.Add(name, vertex);
        }

        foreach (StringPair pair in constellationConnectEdges)
        {
            AddDoubleConnections(nameDict[pair.start], nameDict[pair.end], container);
        }
    }

    private void AddDoubleConnections(GalaxyMapVertex first, GalaxyMapVertex second, GameObject container)
    {
        AddConnection(first, second);
        AddConnection(second, first);
        AddEdgeObj(first, second, container);
    }

    private GameObject AddContainer(string name, Transform parent)
    {
        GameObject container = new GameObject(name);
        container.transform.SetParent(parent);
        container.transform.localScale = Vector3.one;
        container.transform.localPosition = Vector3.zero;
        return container;
    }

    private void AddEdgeObj(GalaxyMapVertex first, GalaxyMapVertex second, GameObject container)
    {
        GalaxyMapEdge edge = Instantiate(edgePrefab);
        edge.transform.SetParent(container.transform);
        edge.Setup(first, second);
    }

    private void AddConnection(GalaxyMapVertex start, GalaxyMapVertex end)
    {
        if (edges.TryGetValue(start, out List<GalaxyMapVertex> connections))
        {
            connections.Add(end);
        }
        else
        {
            edges[start] = new List<GalaxyMapVertex>() { end };
        }
    }


    [System.Serializable]
    private struct StringPair
    {
        public string start;
        public string end;
    }
}
