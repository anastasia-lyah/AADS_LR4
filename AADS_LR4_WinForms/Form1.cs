using System;
using System.Windows.Forms;

namespace AADS_LR4_WinForms{
    
    public partial class Form1 : Form
    {
        private BinaryTree binaryTree;
        public Form1()
        {
            InitializeComponent();
            binaryTree = new BinaryTree();
        }
        private void Form1_Load(object sender, EventArgs e)
        {}
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNodeAndUpdateTreeView();
        }
        private void AddNodeAndUpdateTreeView()
        {
            if (int.TryParse(txtInput.Text, out int value))
            {
                bool wasDuplicate = binaryTree.ContainsDuplicate(binaryTree.Root, value);
                binaryTree.Insert(value);
                UpdateTreeView();

                if (!wasDuplicate && binaryTree.ContainsDuplicate(binaryTree.Root, value))
                {
                    MessageBox.Show($"Значення {value} зустрічається двічі в дереві.");
                }
                txtInput.Clear();
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректне ціле число.");
            }
        }
        private void UpdateTreeView()
        {
            treeView.Nodes.Clear();
            if (binaryTree.Root != null)
            {
                AddNodeToTreeView(binaryTree.Root, null);
            }
            treeView.ExpandAll();
        }
        private void AddNodeToTreeView(BinaryTreeNode node, TreeNode parent)
        {
            TreeNode newNode = new TreeNode(node.Value.ToString());

            if (parent == null)
            {
                treeView.Nodes.Add(newNode);
            }
            else
            {
                parent.Nodes.Add(newNode);
            }

            if (node.Left != null)
            {
                AddNodeToTreeView(node.Left, newNode);
            }
            if (node.Right != null)
            {
                AddNodeToTreeView(node.Right, newNode);
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (TryGetInputValue(out int value))
            {
                bool found = binaryTree.Search(binaryTree.Root, value);
                MessageBox.Show(found ? "Значення знайдено." : "Значення не знайдено.");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (TryGetInputValue(out int value))
            {
                binaryTree.Root = binaryTree.Delete(binaryTree.Root, value);
                UpdateTreeView();
            }
        }
        private void btnCheckDuplicates_Click(object sender, EventArgs e)
        {
            if (TryGetInputValue(out int value))
            {
                bool hasDuplicate = binaryTree.ContainsDuplicate(binaryTree.Root, value);
                MessageBox.Show(hasDuplicate ? "Значення зустрічається двічі." : "Значення не зустрічається двічі.");
            }
        }
        private bool TryGetInputValue(out int value)
        {
            bool success = int.TryParse(txtInput.Text, out value);
            if (!success)
            {
                MessageBox.Show("Будь ласка, введіть коректне ціле число.");
                txtInput.Clear();
            }
            return success;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            PostOrderTraversal(binaryTree.Root);
        }
        private void PostOrderTraversal(BinaryTreeNode node)
        {
            if (node != null)
            {
                PostOrderTraversal(node.Left);
                PostOrderTraversal(node.Right);
                listBox.Items.Add(node.Value);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
    public class BinaryTreeNode
    {
        public int Value { get; set; }
        public BinaryTreeNode Left { get; set; }
        public BinaryTreeNode Right { get; set; }

        public BinaryTreeNode(int value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }
    public class BinaryTree
    {
        public BinaryTreeNode Root { get; set; }

        public void Insert(int value)
        {
            Root = Insert(Root, value);
        }

        private BinaryTreeNode Insert(BinaryTreeNode node, int value)
        {
            if (node == null)
                return new BinaryTreeNode(value);

            if (value <= node.Value)
                node.Left = Insert(node.Left, value);
            else
                node.Right = Insert(node.Right, value);

            return node;
        }
        public bool ContainsDuplicate(BinaryTreeNode node, int targetValue)
        {
            return CountOccurrences(node, targetValue) > 1;
        }
        private int CountOccurrences(BinaryTreeNode node, int targetValue)
        {
            if (node == null)
                return 0;

            int count = node.Value == targetValue ? 1 : 0;
            return count + CountOccurrences(node.Left, targetValue) + CountOccurrences(node.Right, targetValue);
        }
        public bool Search(BinaryTreeNode node, int value)
        {
            if (node == null) return false;
            if (node.Value == value) return true;

            return value < node.Value ? Search(node.Left, value) : Search(node.Right, value);
        }
        public BinaryTreeNode Delete(BinaryTreeNode node, int value)
        {
            if (node == null) return null;

            if (value < node.Value)
                node.Left = Delete(node.Left, value);
            else if (value > node.Value)
                node.Right = Delete(node.Right, value);
            else
            {
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                BinaryTreeNode minNode = GetMinValueNode(node.Right);
                node.Value = minNode.Value;
                node.Right = Delete(node.Right, minNode.Value);
            }
            return node;
        }
        private BinaryTreeNode GetMinValueNode(BinaryTreeNode node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }
    }
}
