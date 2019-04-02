using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textControl : MonoBehaviour
{
    public OurAssets assets;
    public SoundMan SM;
    public Camera cam;
    public Vector3 camPos;
    public Vector3 camLookAt;
    public Vector3 AnswerListOffset;
    public string[] myStrings;
    public float delay_start;
    public float delay_word;
    public float delay_letter;
    public GameObject myWords;
    public float AnswersListWidth;
    public float AnswersListPitch;
    private List<ConAnagramWord> ToGets;




    // Start is called before the first frame update
    void Start()
    {
        ToGets = new List<ConAnagramWord>();
        //Sounds.PlayLobbyMusic();
        //Sounds.PlayMiscSFX(MiscSFX.TimeUp);
        StartCoroutine("DoMagic");
    }

    IEnumerator DoMagic()
    {
        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "vocabulous" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count -1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "Enjoy","word","games" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "vocabulous", "from", "chumbawumba", "games" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "fancy", "a", "challenge"};
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "love", "word", "games" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "hi", "darrel", "and", "hope" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "this", "gotta", "be", "an", "a" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "worddice" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "anagrams" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "wordsearch" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "freeword" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

        yield return new WaitForSeconds(delay_start);
        myStrings = new string[] { "and","We","are","done" };
        DisplayStrings();
        yield return new WaitForSeconds(delay_word);
        //SM.PlayRandomTrack();
        for (int i = ToGets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(delay_word);
            RollString(i);
        }
        yield return new WaitForSeconds(5);
        KillWords();

    }



    void DisplayStrings()
    {
        float[] offsets = new float[myStrings.Length];
        for (int i = myStrings.Length -1; i >= 0; i--)
        {
            offsets[i] = (((float)AnswersListWidth - (float)myStrings[i].Length) / 2f);
        }
        for (int i = myStrings.Length - 1; i >= 0; i--)
        {
            GameObject ToGet = assets.MakeWordFromTiles(myStrings[i], Vector3.zero, 1f, true, false, false);
            ToGet.transform.parent = myWords.transform;
            ToGet.transform.localRotation = myWords.transform.localRotation;
            ToGet.transform.localPosition = AnswerListOffset + new Vector3(offsets[i], 0, (myStrings.Length - i) * AnswersListPitch);
            ToGet.AddComponent<ConAnagramWord>();
            ToGet.GetComponent<ConAnagramWord>().myWord = myStrings[i];
            ToGets.Add(ToGet.GetComponent<ConAnagramWord>());
        }
    }

    void KillWords()
    {
        foreach (Transform child in myWords.transform)
        {
            Destroy(child.gameObject);
        }
        ToGets.Clear();
    }


    void RollString(int index)
    {
            ToGets[index].Roll(delay_letter);
        for (int i = 1; i < ToGets[index].myWord.Length +1; i++)
        {
            SM.PlayTileSFX((TileSFX)Random.Range(2, 6), i * delay_letter);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //cam.transform.position = camPos;
        //cam.transform.LookAt(camLookAt);
    }
}
