namespace Libs.BinaryTrees
{
    public class BinaryTree
    {
        public BinaryTree(int element)
        {
            Element = element;
        }
        public BinaryTree()
        {

        }
        public int Element { get; set; }

        public BinaryTree Left { get; set; }
        public BinaryTree Right { get; set; }
        public BinaryTree Parent { get; set; }



    }
}
