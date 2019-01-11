/* Referencing: https://stackoverflow.com/a/6073004 
 * https://www.geeksforgeeks.org/trie-insert-and-search/?fbclid=IwAR2aBUOojJJfqTwLO6dn8Svvrzr8rMgRdo9PIOl-q1Bdu50xNVFyHy8hXrM */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrieTest : MonoBehaviour
{
    public struct Letter
    {
        /* conversion operator, allowing this struct to be converted to and from a char,
         * while also setting the index value of the new struct to the where 'c' is the numerical value within 'letters' */
        public static implicit operator Letter(char c)
        {
            return new Letter()
            {
                index = letters.IndexOf(c)
            };
        }

        /* define the available letters that can be indexed */
        public const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /* the index of the key */
        public int index;
        
        /* convert the index within the 'letters' string into a char */
        public char ToChar() { return letters[index]; }
        /* we will only want to ToString() the actual char */
        public override string ToString() { return letters[index].ToString(); }
    }

    public class Trie
    {
        /* within an Trie, we have nodes */
        public class Node
        {
            /* a dictionary to pair a Letter to a Node, our 'group' */
            public Dictionary<Letter, Node> group = new Dictionary<Letter, Node>();
            /* the letter at this node */
            public string letter;
            /* are we a leaf node? end of searching */
            public bool isLeaf { get { return group.Count == 0 && letter != null; } }
        }

        /* declare a root */
        public Node root = new Node();

        /* CONSTRUCTOR */
        public Trie(string[] arrayOfWords)
        {
            /* precalculate length of fed in array */
            int wl = arrayOfWords.Length;
            /* START looping through array of words */
            for (int i = 0; i < wl; i++)
            {
                string letter = arrayOfWords[i];
                Node node = root;

                /* as a saftey measure(?) make sure we only get 1 character */
                for (int a = 1; a <= letter.Length; a++)
                {
                    char character = letter[a - 1];
                    Node next;

                    /* searching for the node that corresponds to the character we have, if it doesn't exist.. */
                    if (!node.group.TryGetValue(character, out next))
                    {
                        /* assign the character to a new node in the 'group' dictionary 
                         * notice that the 'Letter' to 'char' conversion comes in handy here */
                        next = new Node();
                        node.group.Add(character, next);
                    }

                    /* when finishing up, set this nodes 'letter' to the one we have */
                    if (a == letter.Length)
                    {
                        next.letter = letter;
                    }

                    /* go to next node */
                    node = next;
                }
            }
            /* END looping through array of words */
        }
    }
}
