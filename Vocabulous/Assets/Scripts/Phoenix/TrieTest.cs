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
        public const string letters = "abcdefghijklmnopqrstuvwxyz";
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
            /* a dictionary to pair a Letter to a Node, our 'group' 
             * Letter (Key), Node (Value) */
            public Dictionary<Letter, Node> group = new Dictionary<Letter, Node>();
            /* the letter at this node */
            public string word;
            /* are we a leaf node? end of searching */
            public bool isLeaf { get { return word != null; } }
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

                /* loop through word element in array */
                for (int a = 1; a <= letter.Length; a++)
                {
                    char character = letter[a - 1];
                    Node next;

                    /* searching for the node that corresponds to the character we have, if it doesn't exist.. */
                    if (!node.group.TryGetValue(character, out next))
                    {
                        /* assign the character to a new node in the 'group' dictionary */
                        next = new Node();
                        node.group.Add(character, next);
                    }

                    /* at the last letter of array, set it's letter, thereby making it a leaf node 'isLeaf == true' */
                    if (a == letter.Length)
                    {
                        next.word = letter;
                    }

                    /* go to next node */
                    node = next;
                }
            }
            /* END looping through array of words */
        }
    }

    /* recursive function for searching words in nodes, takes:
     * a Trie.Node, ie. the root
     * a HashSet of Letter arrays, basically a half of a dictionary where all we care about it 'does this set contain element x', it is not linked to any other value 
     * an index to iterate through 'setsOfLetters'
     * a list of strings to hold 'wordsFound' */
    public void SearchWords(Trie.Node node, HashSet<Letter>[] setsOfLetters, int currentArrayIndex, List<string> wordsFound)
    {
        if (currentArrayIndex < setsOfLetters.Length)
        {
            /* for each node branching off from this current node */
            foreach (KeyValuePair<Letter, Trie.Node> group in node.group)
            {
                /* if the letter (Key) present in any of the branching nodes.. */
                if (setsOfLetters[currentArrayIndex].Contains(group.Key))
                {
                    /* check if it is a leaf node, if so, we've found a word - add it to the list */
                    if (group.Value.isLeaf)
                    {
                        wordsFound.Add(group.Value.word);
                    }
                    /* if we've found a word or not AND we've detected a node with our letter,
                     * search again with our found node (Value), and increment our index for the next letter */
                    SearchWords(group.Value, setsOfLetters, currentArrayIndex + 1, wordsFound);
                }
            }
        }
        // return wordsFound;
    }

    /* used by Text_Transfer1 to add lines to the list */
    public List<string> allWordsList;
    /* 'allWordsList' converted into an array to be fed into the Trie */
    string[] wordsArray;
    /* the words found from 'SearchWords' function */
    List<string> wordsFound = new List<string>();
    /* letters to search the Trie, with a maximum number of letters (will need to precalculate the longest word in the dictionary soon) */
    //char[] lettersToSearch = new char[26];
    List<char> lettersToSearch = new List<char>();
    /* the Trie */
    Trie trie;

    /* called by Text_Transfer1 once it has read the dictionary being used */
    public void Initialise()
    {
        Debug.Log("-- Using: XLGameDictUK --");

        /* the Trie takes in an array of strings, convert the list and chops the null reference off at the end */
        wordsArray = new string[allWordsList.Count];
        allWordsList.RemoveAt(allWordsList.Count - 1);
        wordsArray = allWordsList.ToArray();

        /* populate Trie */
        double t = 0;
        t += Time.deltaTime;
        trie = new Trie(wordsArray);
        Debug.Log("Constructed Trie in " + t + " seconds -- " + System.DateTime.Now);
        t = 0;
        Debug.Log("Trie consists of " + wordsArray.Length + " leaf nodes");

        ///* using an array of HashSet Letters to search for in order */
        //HashSet<Letter>[] sets = new HashSet<Letter>[] {
        //  new HashSet<Letter>(new Letter[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' }),

        //t += Time.deltaTime;
        //SearchWords(trie.root, sets, 0, wordsFound);
        //Debug.Log("Found " + wordsFound.Count + " words and took " + t + " seconds");
        //t = 0;

        //foreach (string word in wordsFound)
        //{
        //    Debug.Log(word + " could also carry on to be ");
        //    //for (int i = 0; i < wordsArray.Length; i++)
        //    //{
        //    //    if (wordsArray[i].StartsWith(word))
        //    //    {
        //    //        Debug.Log(wordsArray[i]);
        //    //    }
        //    //}
        //}
    }

    /* main search function */
    public bool TrieSearch(bool anagram, bool exactCompare, bool debug)
    {
        /* create a new HashSet Letter array to the length of lettersToSearch
         * each element of the HashSet Letter array must be a new Letter array,
         * populate it with the char in lettersToSearch */
        HashSet<Letter>[] sets = new HashSet<Letter>[lettersToSearch.Count];
        if (anagram)
        {
            HashSet<Letter> selectedLetters = new HashSet<Letter>();
            for (int i = 0; i < sets.Length; i++)
            {
                for (int j = 0; j < sets.Length; j++)
                {
                    selectedLetters.Add(lettersToSearch[j]);
                }
                sets[i] = selectedLetters;
            }
        }
        else
        {
            for (int i = 0; i < sets.Length; i++)
            {
                sets[i] = new HashSet<Letter>(new Letter[] { lettersToSearch[i] });
            }
        }

        /* search trie */
        double t = 0;
        t += Time.deltaTime;
        SearchWords(trie.root, sets, 0, wordsFound);

        /* debug other words found within the search string */
        if (debug)
        {
            foreach (string word in wordsFound)
            {
                Debug.Log("'" + word + "' was found -- " + System.DateTime.Now);
            }
            /* no words found */
            if (wordsFound.Count == 0) { Debug.Log("No words found -- " + System.DateTime.Now); }
            else { Debug.Log("Found " + wordsFound.Count + " words and took " + t + " seconds"); }
        }
        t = 0;

        /* how many words were found if exactCompare false
         * else check each word in the returned words, if the length of any word in the list == lettersToSearch length, then we have an exact match */
        int numWords = 0;
        if (!exactCompare) numWords = wordsFound.Count;
        else
        {
            foreach (string word in wordsFound)
            {
                if (word.Length == lettersToSearch.Count)
                {
                    if (debug) Debug.Log("Exact match found");
                    numWords++;
                }
            }
        }

        /* clear lettersToSearch and wordsFound after search */
        lettersToSearch.Clear();
        wordsFound.Clear();
        /* return true or false based on how many words found */
        return numWords > 0;
    }

    /* convert the string to chars, add them in order to the 'lettersToSearch' array then do the search without anagram */
    public bool SearchString(string str, bool anagram, bool exactCompare, bool debug)
    {
        for (int i = 0; i < str.Length; i++)
        {
            lettersToSearch.Add(str[i]);
        }
        return TrieSearch(anagram, exactCompare, debug);
    }





    /* DEBUG */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < lettersToSearch.Count; i++)
            {
                Debug.Log(lettersToSearch[i] + " - ");
            }
        }
    }
}
