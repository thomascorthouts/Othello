using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Model.ArtificialIntelligence;
using System.Collections;

namespace ModelTests
{
    [TestClass]
    public class MinimaxTests
    {
        private void Check(Node node, string expected, int ply = 2)
        {
            CheckMinimax(node, expected, ply);
            CheckMinimaxWithPruning(node, expected, ply);
        }

        private void CheckMinimax(Node node, string expected, int ply = 2)
        {
            var ai = new Minimax<string>(ply);
            Assert.AreEqual(expected, ai.GetBestMove(node), "Minimax failed");
        }

        private void CheckMinimaxWithPruning(Node node, string expected, int ply = 2)
        {
            var ai = new MinimaxWithAlphaBetaPruning<string>(ply);
            Assert.AreEqual(expected, ai.GetBestMove(node), "Minimax with pruning failed");
        }

        [TestMethod]
        public void Minimax1()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) }
            };

            Check(tree, "a");
        }

        [TestMethod]
        public void Minimax2()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) },
                { "b", new Node(3, false) }
            };

            Check(tree, "a");
        }

        [TestMethod]
        public void Minimax3()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) },
                { "b", new Node(7, false) }
            };

            Check(tree, "b");
        }

        [TestMethod]
        public void Minimax4()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) },
                { "b", new Node(7, false)
                    {
                        { "a", new Node(1, true) },
                        { "b", new Node(4, true) },
                    }
                }
            };

            Check(tree, "a");
        }

        [TestMethod]
        public void Minimax5()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(2, false) },
                { "b", new Node(7, false)
                    {
                        { "a", new Node(1, true) },
                        { "b", new Node(4, true) },
                    }
                }
            };

            Check(tree, "a");
        }

        [TestMethod]
        public void Minimax6()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(2, false) },
                { "b", new Node(7, false)
                    {
                        { "a", new Node(3, true) },
                        { "b", new Node(4, true) },
                    }
                }
            };

            Check(tree, "b");
        }

        [TestMethod]
        public void Minimax7()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) },
                { "b", new Node(7, true)
                    {
                        { "a", new Node(3, true) },
                        { "b", new Node(4, true) },
                    }
                }
            };

            Check(tree, "a");
        }

        [TestMethod]
        public void Minimax8()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) },
                { "b", new Node(7, true)
                    {
                        { "a", new Node(6, false) },
                        { "b", new Node(4, true) },
                    }
                }
            };

            Check(tree, "b");
        }

        [TestMethod]
        public void Minimax9()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) },
                { "b", new Node(7, false)
                    {
                        { "a", new Node(1, true) },
                        { "b", new Node(8, true) },
                    }
                }
            };

            Check(tree, "a");
        }

        [TestMethod]
        public void PruneTest1()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) },
                { "b", new Node(7, false)
                    {
                        { "a", new Node(1, true) },
                        { "b", new ENode() },
                    }
                }
            };

            CheckMinimaxWithPruning(tree, "a");
        }

        [TestMethod]
        [ExpectedException(typeof(ENodeException))]
        public void PruneTest2()
        {
            var tree = new Node(10, true)
            {
                { "a", new Node(5, false) },
                { "b", new Node(7, false)
                    {
                        { "a", new Node(6, true) },
                        { "b", new ENode() },
                    }
                }
            };

            var ai = new MinimaxWithAlphaBetaPruning<string>(2);
            ai.GetBestMove(tree);
        }

        class Node : IGameTree<string>, IEnumerable<string>
        {
            private readonly Dictionary<string, IGameTree<string>> _children;

            public Node(double score, bool aiTurn)
            {
                this.Score = score;
                this._children = new Dictionary<string, IGameTree<string>>();
                this.IsAITurn = aiTurn;
            }

            public bool IsAITurn { get; set; }

            public double Score { get; }

            public IList<string> ValidMoves
            {
                get
                {
                    return new List<string>(_children.Keys);
                }
            }

            public void Add(string move, IGameTree<string> child)
            {
                _children.Add(move, child);
            }

            public IEnumerator<string> GetEnumerator()
            {
                return _children.Keys.GetEnumerator();
            }

            public IGameTree<string> MakeMove(string move)
            {
                return _children[move];
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        class ENodeException : Exception
        {

        }

        class ENode : IGameTree<string>
        {
            public bool IsAITurn
            {
                get
                {
                    throw new ENodeException();
                }
            }

            public double Score
            {
                get
                {
                    throw new ENodeException();
                }
            }

            public IList<string> ValidMoves
            {
                get
                {
                    throw new ENodeException();
                }
            }

            public IGameTree<string> MakeMove(string move)
            {
                throw new ENodeException();
            }
        }
    }
}
