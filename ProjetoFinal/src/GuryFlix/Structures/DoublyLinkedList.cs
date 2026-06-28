namespace Guryflix.Structures
{
    class DoublyLinkedList
    {

        public string[] str_name;
        
        public class Node
        {
            public string name;
            public Node next;
            public Node prev;
        };
        Node start;
        

        
        public void insertEnd(string Name)
        {
            Node new_node;

            
            
            if (start == null)
            {
                new_node = new Node();
                new_node.name = Name;
                new_node.next = new_node.prev = new_node;
                start = new_node;
                return;
            }

            

            
            Node last = (start).prev;

            
            new_node = new Node();
            new_node.name = Name;
            
            new_node.next = start;

            
            (start).prev = new_node;

            
            new_node.prev = last;

            
            last.next = new_node;
        }

        public void storeData(int n)
        {
            int i = 0;
            str_name = new string[n];

            Node temp = start;
            do
            {
                str_name[i] = temp.name;
                i++;
                temp = temp.next;

            } while (temp.next != start);
            str_name[i] = temp.name;
        }
    }
}
