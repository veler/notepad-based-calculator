namespace NotepadBasedCalculator.Api
{
    public static class ExtensionOrderer
    {
        public static IEnumerable<Lazy<TExtension, TMetadata>> Order<TExtension, TMetadata>(IEnumerable<Lazy<TExtension, TMetadata>> extensions) where TMetadata : IOrderableMetadata
        {
            var graph = new Graph<TExtension, TMetadata>(extensions);
            return graph.TopologicalSort();
        }

        public static void CheckForCycles<TExtension, TMetadata>(IEnumerable<Lazy<TExtension, TMetadata>> extensions) where TMetadata : IOrderableMetadata
        {
            var graph = new Graph<TExtension, TMetadata>(extensions);
            graph.CheckForCycles();
        }

        private sealed class Node<TExtension, TMetadata> where TMetadata : IOrderableMetadata
        {
            public string Name => Extension.Metadata.Name;

            public HashSet<Node<TExtension, TMetadata>> NodesBefore { get; }

            public Lazy<TExtension, TMetadata> Extension { get; }

            public Node(Lazy<TExtension, TMetadata> extension)
            {
                Extension = extension;
                NodesBefore = new HashSet<Node<TExtension, TMetadata>>();
            }

            public void CheckForCycles()
            {
                CheckForCycles(new HashSet<Node<TExtension, TMetadata>>());
            }

            public void Visit(List<Lazy<TExtension, TMetadata>> result, HashSet<Node<TExtension, TMetadata>> seenNodes)
            {

                if (!seenNodes.Add(this))
                {
                    return;
                }

                foreach (Node<TExtension, TMetadata> before in NodesBefore)
                {
                    before.Visit(result, seenNodes);
                }

                result.Add(Extension);
            }

            private void CheckForCycles(HashSet<Node<TExtension, TMetadata>> seenNodes)
            {

                if (!seenNodes.Add(this))
                {
                    throw new ArgumentException($"Cycle detected in extensions. Extension Name: '{Name}'");
                }

                foreach (Node<TExtension, TMetadata> before in NodesBefore)
                {
                    before.CheckForCycles(seenNodes);
                }

                seenNodes.Remove(this);
            }
        }

        private sealed class Graph<TExtension, TMetadata> where TMetadata : IOrderableMetadata
        {
            private Dictionary<string, Node<TExtension, TMetadata>> Nodes { get; }

            public Graph(IEnumerable<Lazy<TExtension, TMetadata>> extensions)
            {

                Nodes = new Dictionary<string, Node<TExtension, TMetadata>>();

                foreach (Lazy<TExtension, TMetadata> extension in extensions)
                {
                    var node = new Node<TExtension, TMetadata>(extension);
                    Nodes.Add(node.Name, node);
                }

                foreach (Node<TExtension, TMetadata> node in Nodes.Values)
                {

                    foreach (string before in node.Extension.Metadata.Before)
                    {
                        Node<TExtension, TMetadata> nodeAfter = Nodes[before];
                        nodeAfter.NodesBefore.Add(node);
                    }

                    foreach (string after in node.Extension.Metadata.After)
                    {
                        Node<TExtension, TMetadata> nodeBefore = Nodes[after];
                        node.NodesBefore.Add(nodeBefore);
                    }
                }
            }

            public IList<Lazy<TExtension, TMetadata>> TopologicalSort()
            {

                CheckForCycles();

                var result = new List<Lazy<TExtension, TMetadata>>();
                var seenNodes = new HashSet<Node<TExtension, TMetadata>>();

                foreach (Node<TExtension, TMetadata> node in Nodes.Values)
                {
                    node.Visit(result, seenNodes);
                }

                return result;
            }

            public void CheckForCycles()
            {
                foreach (Node<TExtension, TMetadata> node in Nodes.Values)
                {
                    node.CheckForCycles();
                }
            }
        }
    }
}
