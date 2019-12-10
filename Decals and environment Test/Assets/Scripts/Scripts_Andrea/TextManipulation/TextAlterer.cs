using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAlterer : MonoBehaviour
{
    [SerializeField] TextMeshPro tmpProScript = null;
    [SerializeField] AudioClip[] staticClips;
    [SerializeField] AudioClip[] scarySounds;

    AudioSource aud;
    string startingText;
    public string alteredText;
    static readonly string spookyCharacters = "!£$%&/=?___&&&/_____??##########[][][[][][°ç#F§°E27_";

    IEnumerator Start()
    {
        yield return new WaitForSeconds(5.5f);
        aud = GetComponent<AudioSource>();

        startingText = tmpProScript.text;
        StartCoroutine(CorruptCharacters(tmpProScript.text, 0.6f, 6, 90));
        StartCoroutine(CompressDecompressText(4, 2.7f));
        StartCoroutine(CorruptText(startingText, 0.45f));
    }

    //Overrides the text with spooky symbols
    IEnumerator CorruptText(string textToAlter, float stepDelay, int rateOfConversion = 15)
    {
        yield return new WaitForSeconds(1.1f);
       
        yield return new WaitForSeconds(4.5f); //temporary, this should be part of the function signature

        AudioSource audi = gameObject.AddComponent<AudioSource>();
        audi.PlayOneShot(scarySounds[1]);

        int lapsed = 1;
        bool distortingText = true;
        Color startColor = tmpProScript.color;

        while (distortingText)
        {
            //STRING MANIPULATION PART
            string chunkToModify = textToAlter.Substring(0, lapsed); //how many characters to alter, can't be less than 1 or more than total
            string chunkToModifyBottom = textToAlter.Substring(textToAlter.Length - lapsed); //same as above, but starting from the end of the string

            //the part of the text that's still normal
            //we take away a part from the beginning of the string and an equal part from the end of the string, if those two parts combined are bigger than the whole text
            //we simply replace it all in one go
            string remainingOriginalText = tmpProScript.text.Substring(chunkToModify.Length - 1, chunkToModify.Length * 2 > textToAlter.Length ? (textToAlter.Length - chunkToModify.Length) : (textToAlter.Length - chunkToModify.Length - chunkToModifyBottom.Length));

            chunkToModify = chunkToModify.Replace(chunkToModify, GetSpookyString(chunkToModify.Length));
            chunkToModifyBottom = chunkToModifyBottom.Replace(chunkToModifyBottom, GetSpookyString(chunkToModifyBottom.Length));

            tmpProScript.text = chunkToModify + remainingOriginalText + chunkToModifyBottom;

            //COLOR MANIPULATION
            float f = ((float)lapsed / (float)textToAlter.Length);
            Color mutatedColor = Color.Lerp(startColor, Color.red, f);
            tmpProScript.color = mutatedColor;

            tmpProScript.richText = enabled;

            StartCoroutine(CheckBreak());
            yield return new WaitForSeconds(stepDelay);
            stepDelay *= 0.9f;
        }
        tmpProScript.fontStyle = FontStyles.Bold;
        StartCoroutine(InsertAlteredMessage());
        IEnumerator CheckBreak()
        {
            if (lapsed == textToAlter.Length)
                distortingText = false;
            else if (lapsed + rateOfConversion > textToAlter.Length)
                lapsed = textToAlter.Length;
            else
                lapsed += rateOfConversion;
            yield return null;
        }
    }

    //lerps spacing between words and lines
    IEnumerator CompressDecompressText(float compressionTime, float decompressionTime)
    {
        yield return new WaitForSeconds(1.5f);

        AudioSource audi = gameObject.AddComponent<AudioSource>();
        audi.PlayOneShot(scarySounds[0]);

        float lapsed = 0f;

        float originalWordSpacing = tmpProScript.wordSpacing, originalLineSpacing = tmpProScript.lineSpacing;

        while (lapsed < compressionTime)
        {
            tmpProScript.wordSpacing = Mathf.Lerp(originalWordSpacing, -70, Mathf.SmoothStep(0, 1, lapsed / compressionTime));
            tmpProScript.lineSpacing = Mathf.Lerp(originalLineSpacing, -70, Mathf.SmoothStep(0, 1, lapsed / compressionTime));

            lapsed += Time.deltaTime;
            yield return null;
        }

        lapsed = 0f;

        while (lapsed < compressionTime)
        {
            tmpProScript.wordSpacing = Mathf.Lerp(-70, originalWordSpacing, Mathf.SmoothStep(0, 1, lapsed / decompressionTime));
            tmpProScript.lineSpacing = Mathf.Lerp(-70, originalLineSpacing, Mathf.SmoothStep(0, 1, lapsed / decompressionTime));

            lapsed += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator InsertAlteredMessage()
    {
        //INSERTING THE ACTUAL NEW MESSAGE
        //We simply split the original message in half and subtract enough characters so that the new message won't make the final message longer
        int midpoint = tmpProScript.text.Length / 2;
        string firstHalf = tmpProScript.text.Substring(0, alteredText.Length > tmpProScript.text.Length ? 0 : midpoint - alteredText.Length / 2);
        string secondHalf = tmpProScript.text.Substring(alteredText.Length > tmpProScript.text.Length ? tmpProScript.text.Length - 1 : midpoint + alteredText.Length / 2);

        string middle = "";
        int i = 0;
        while (middle != alteredText) //add letters in the middle message one by one
        {
            middle += alteredText[i];
            i++;
            tmpProScript.text = firstHalf + middle + secondHalf;
            aud.PlayOneShot(staticClips[Random.Range(0, staticClips.Length)]);
            yield return null;
        }
    }

    IEnumerator CorruptCharacters(string textToAlter, float frequency, int numCorruptedChars, int targetCorruptedCharacters)
    {
        int corrupted = 0;
        float originalSpacing = tmpProScript.characterSpacing;
        while (corrupted < targetCorruptedCharacters)
        {
            for (int i = 0; i < numCorruptedChars; i++)
            {
                corrupted++;
                int characterToCorrupt = Random.Range(0, textToAlter.Length); //we pick a random char to mess up in the original text

                char[] newText = tmpProScript.text.ToCharArray(); //we turn the original text into char array
                newText[characterToCorrupt] = GetSpookyChar(); //alter one character in the copy

                string newString = new string(newText); //turn into a string via this method - else it won't work
                tmpProScript.text = newString; //make the original equal to the copy

                aud.PlayOneShot(staticClips[Random.Range(0, staticClips.Length)]);
                TryDisplacement(corrupted);
                yield return new WaitForSeconds(frequency / 3);
            }

            tmpProScript.characterSpacing = originalSpacing;
            if (corrupted < targetCorruptedCharacters / 2)
                frequency *= 0.6f;
            else
                frequency *= 1.27f;
            yield return new WaitForSeconds(frequency);
        }
    }

    #region UTILITIES
    void TryDisplacement(int num) //alters character spacing
    {
        int key = Random.Range(9, 28);
        if (num % key == 0 && num != 0)
            tmpProScript.characterSpacing = Random.Range(55, 127);
    }

    string GetSpookyString(int howMany) //creates a string of varying length made of from the spooky characters list
    {
        string spookyString = "";

        for (int i = 0; i < howMany; i++)
            spookyString += spookyCharacters[Random.Range(0, spookyCharacters.Length)].ToString();

        return spookyString;
    }

    char GetSpookyChar()
    {
        return spookyCharacters[Random.Range(0, spookyCharacters.Length)];
    }

    #endregion
}
