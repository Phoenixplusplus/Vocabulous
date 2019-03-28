//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

// Referencing: https://stackoverflow.com/a/6073004 
// https://www.geeksforgeeks.org/trie-insert-and-search/?fbclid=IwAR2aBUOojJJfqTwLO6dn8Svvrzr8rMgRdo9PIOl-q1Bdu50xNVFyHy8hXrM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrieTest : MonoBehaviour
{
    public struct Letter
    {
        // conversion operator, allowing this struct to be converted to and from a char,
        // while also setting the index value of the new struct to the where 'c' is the numerical value within 'letters'
        public static implicit operator Letter(char c)
        {
            return new Letter()
            {
                index = letters.IndexOf(c)
            };
        }

        public const string letters = "abcdefghijklmnopqrstuvwxyz";
        public int index;
        
        public char ToChar() { return letters[index]; }
        public override string ToString() { return letters[index].ToString(); }
    }

    public class Trie
    {
        // records the letter, isLeaf node, and the 'group' of Dictionary<Letter, Node> it has
        public class Node
        {
            public Dictionary<Letter, Node> group = new Dictionary<Letter, Node>();
            public string word;
            public bool isLeaf { get { return word != null; } }
        }

        // return a root when needed
        public Node root = new Node();

        // constructor
        public Trie(string[] arrayOfWords)
        {
            int wl = arrayOfWords.Length;
            for (int i = 0; i < wl; i++)
            {
                string letter = arrayOfWords[i];
                Node node = root;

                for (int a = 1; a <= letter.Length; a++)
                {
                    char character = letter[a - 1];
                    Node next;

                    if (!node.group.TryGetValue(character, out next))
                    {
                        next = new Node();
                        node.group.Add(character, next);
                    }

                    if (a == letter.Length)
                    {
                        next.word = letter;
                    }

                    node = next;
                }
            }
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
            foreach (KeyValuePair<Letter, Trie.Node> group in node.group)
            {
                if (setsOfLetters[currentArrayIndex].Contains(group.Key))
                {
                    if (group.Value.isLeaf)
                    {
                        wordsFound.Add(group.Value.word);
                    }
                    SearchWords(group.Value, setsOfLetters, currentArrayIndex + 1, wordsFound);
                }
            }
        }
    }

    // public lists to be accessed outside the class
    public List<string> allWordsList;
    public List<string> lastStoredWords = new List<string>();
    string[] wordsArray;
    List<string> wordsFound = new List<string>();
    List<char> lettersToSearch = new List<char>();
    Trie trie;

    // as 'start' function
    public void Initialise()
    {
        // the Trie takes in an array of strings, convert the list and chops the null reference off at the end
        wordsArray = new string[allWordsList.Count];
        allWordsList.RemoveAt(allWordsList.Count - 1);
        wordsArray = allWordsList.ToArray();

        // populate the trie with the pulled dictionary
        trie = new Trie(wordsArray);
    }

    // main search function
    public bool TrieSearch(bool anagram, bool exactCompare, bool storeWords, int lengthOfStoredWords, bool debug)
    {
        // clear list of any stored words first
        lastStoredWords.Clear();

        // create a new HashSet Letter array to the length of lettersToSearch
        // each element of the HashSet Letter array must be a new Letter array,
        // populate it with the char in lettersToSearch
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

        // now prepared, search trie
        double t = 0;
        if (debug)
        {
            t += Time.deltaTime;
        }
        SearchWords(trie.root, sets, 0, wordsFound);

        // debug other words found within the search string -- also store last searched words for convenience
        foreach (string word in wordsFound)
        {
            if (debug) Debug.Log("'" + word + "' was found -- " + System.DateTime.Now);
            if (storeWords)
            {
                if (word.Length == lengthOfStoredWords)
                {
                    lastStoredWords.Add(word);
                }
            }
        }

        // no words found
        if (debug)
        {
            if (wordsFound.Count == 0)
            {
                Debug.Log("No words found -- " + System.DateTime.Now);
            }
            else
            {
                Debug.Log("Found " + wordsFound.Count + " words and took " + t + " seconds");
            }
        }

        if (debug) t = 0;

        // how many words were found if exactCompare false
        // else check each word in the returned words, if the length of any word in the list == lettersToSearch length, then we have an exact match */
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

        // clean up
        lettersToSearch.Clear();
        wordsFound.Clear();
        return numWords > 0;
    }

    // convert the string to chars, add them in order to the 'lettersToSearch' array then do the search without anagram
    public bool SearchString(string str, bool anagram, bool exactCompare, bool storeWords, int lengthOfStoredWords, bool debug)
    {
        str.ToLower();
        for (int i = 0; i < str.Length; i++)
        {
            lettersToSearch.Add(str[i]);
        }
        return TrieSearch(anagram, exactCompare, storeWords, lengthOfStoredWords, debug);
    }
}
