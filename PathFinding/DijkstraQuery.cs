using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestryGameGeneral.PathFinding
{
    public class DijkstraMap<T>
    {

        #region Inner Structures

        class Node
        {
            public T nodeObject;
            public float distance = float.MaxValue;
            public Dictionary<T, float> connectedNodes = new Dictionary<T, float>();
            public Node priorNode = null;
            public bool visited = false;
        }

        public struct NodeLink
        {
            public T endObject;
            public float distance;
        }

        #endregion

        #region Parameters and Constructors

        private List<Node> nodes = new List<Node>();
        private Dictionary<T, Node> nodeMap = new Dictionary<T, Node>();

        public DijkstraMap()
        {

        }

        public DijkstraMap(List<T> nodeObjects, List<List<NodeLink>> objectsConnectedTo)
        {
            AddNodes(nodeObjects, objectsConnectedTo);
        }

        #endregion

        #region Operations

        #region Addition

        /// <summary>
        /// Adds a node and its connections to the map. O(n) where n= size of connectedTo. Also creates nodes for nodes that are already connected to. If a distance is different then changes distance.
        /// </summary>
        /// <param name="connectedTo"> Can be left as empty if you want simply the node to be added, and for example, will add the links later.</param>
        public void AddNode(T nodeObject, List<NodeLink> connectedTo)
        {
            if (nodeMap.ContainsKey(nodeObject))
                goto connection;
            Node n = new Node();
            n.nodeObject = nodeObject;
            nodes.Add(n);
            nodeMap.Add(nodeObject, n);
            connection:
            if (connectedTo == null)
                return;
            n = nodeMap[nodeObject];
            foreach (NodeLink l in connectedTo)
            {
                if (!n.connectedNodes.ContainsKey(l.endObject))
                {
                    n.connectedNodes.Add(l.endObject, l.distance);
                    if (!nodeMap.ContainsKey(l.endObject))
                    {
                        List<NodeLink> toBeAdded = new List<NodeLink>();
                        toBeAdded.Add(new NodeLink() { endObject = nodeObject, distance = l.distance });
                        AddNode(l.endObject, toBeAdded);
                    }
                    else
                    {
                        if (!nodeMap[l.endObject].connectedNodes.ContainsKey(nodeObject))
                            nodeMap[l.endObject].connectedNodes.Add(nodeObject, l.distance);
                    }
                }
                else if (n.connectedNodes[l.endObject] != l.distance)
                {
                    AddModifyNodeLink(nodeObject, l.endObject, l.distance);
                }
            }

        }

        /// <summary>
        /// A node Object and its list of connections have to be the same index in both lists input. 
        /// </summary>
        public void AddNodes(List<T> nodeObjects, List<List<NodeLink>> objectsConnectedTo)
        {
            for (int i = 0; i < nodeObjects.Count; i++)
            {
                AddNode(nodeObjects[i], objectsConnectedTo[i]);
            }
        }

        #endregion

        #region Node Removal

        /// <summary>
        /// Removes the node, along with the connections that it harbored.
        /// </summary>
        public void RemoveNode(T nodeObject)
        {
            nodes.Remove(nodeMap[nodeObject]);
            nodeMap.Remove(nodeObject);
            foreach (Node n in nodes)
            {
                n.connectedNodes.Remove(nodeObject);
            }
        }

        #endregion

        #region Node Manipulation

        /// <summary>
        /// Adds a link from a node to another or resets distance. If such a link already exists at the same distance or one of the nodes do not exist in the map the operation will fail and will return false.
        /// if the isModification variable is set to true without being sure that the two nodes exist and already have a connection between them, this will produce an error. 
        /// </summary>
        public bool AddModifyNodeLink(T node1, T node2, float distance, bool isModification = false)
        {
            if (isModification)
                goto modify;
            if (!nodeMap.ContainsKey(node1) || !nodeMap.ContainsKey(node2))
                return false;
            else if ((nodeMap[node1].connectedNodes.ContainsKey(node2) && nodeMap[node1].connectedNodes[node2] == distance) ||
                (nodeMap[node2].connectedNodes.ContainsKey(node1) && nodeMap[node2].connectedNodes[node1] == distance))
                return false;
            else
            {
                if (nodeMap[node2].connectedNodes.ContainsKey(node1))
                {
                    nodeMap[node2].connectedNodes[node1] = distance;
                    nodeMap[node1].connectedNodes[node2] = distance;
                }
                else
                {
                    nodeMap[node1].connectedNodes.Add(node2, distance);
                    nodeMap[node2].connectedNodes.Add(node1, distance);
                }
                return true;
            }
            modify:
            nodeMap[node2].connectedNodes[node1] = distance;
            nodeMap[node1].connectedNodes[node2] = distance;
            return true;
        }

        /// <summary>
        /// removes the link between the two specified nodes. if the nodes do not exist returns false, otherwise returns true, even if the connection did not exist in the first place.
        /// </summary>
        public bool RemoveNodeLink(T node1, T node2)
        {
            if (!nodeMap.ContainsKey(node1) || !nodeMap.ContainsKey(node2))
                return false;
            //if (nodeMap[node1].connectedNodes.ContainsKey(node2))
            //{
            nodeMap[node1].connectedNodes.Remove(node2);
            nodeMap[node2].connectedNodes.Remove(node1);
            //}
            return true;
        }

        #endregion

        #endregion

        #region Queries

        private SortedDictionary<float, Queue<T>> priorityStack = new SortedDictionary<float, Queue<T>>();

        /// <summary>
        /// if one of the two input objects are not in the map or there is no valid path, will return a null path. Otherwise, will return a path that starts from the first destination to the end goal.
        /// </summary>
        public Stack<T> FindPath(T origin, T goal)
        {
            if (!nodeMap.ContainsKey(origin) || !nodeMap.ContainsKey(goal) || nodeMap[origin] == nodeMap[goal])
                return null;
            ResetNodeInfos();
            priorityStack.Clear();
            Stack<T> path = new Stack<T>();
            nodeMap[origin].distance = 0;
            nodeMap[origin].visited = true;
            Node current = nodeMap[origin];
            do
            {
                if (current == nodeMap[goal])
                {
                    //construct path.
                    while (current != nodeMap[origin])
                    {
                        path.Push(current.nodeObject);
                        current = current.priorNode;
                    }
                    return path;
                }
                foreach (T t in current.connectedNodes.Keys)
                {
                    if (nodeMap[t].visited)
                        continue;
                    if (nodeMap[t].distance > current.distance + current.connectedNodes[t])
                    {
                        Node n = nodeMap[t];
                        n.distance = current.distance + current.connectedNodes[t];
                        n.priorNode = current;
                        if (!priorityStack.ContainsKey(n.distance))
                            priorityStack[n.distance] = new Queue<T>();
                        priorityStack[n.distance].Enqueue(t);
                    }
                }
                if (priorityStack.Count == 0)
                    break;
                var enumerator = priorityStack.Keys.GetEnumerator();
                enumerator.MoveNext();
                float first = enumerator.Current;
                current = nodeMap[priorityStack[first].Dequeue()];
                if (priorityStack[first].Count == 0)
                    priorityStack.Remove(first);
            } while (true);
            return null;
        }

        private void ResetNodeInfos()
        {
            foreach (Node n in nodes)
            {
                n.distance = float.MaxValue;
                n.priorNode = null;
                n.visited = false;
            }
        }

        #endregion
    }
}
