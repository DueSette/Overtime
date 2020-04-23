using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShelfSlotScript : MonoBehaviour
{
    /*===========
     * TO BE USED WITHIN A BOOKSHELF PUZZLE
    =============*/

    [SerializeField] int slotNumber;
    [SerializeField] GameObject containedBook;
    [SerializeField] string correctBookTitle;
    [SerializeField] AudioClip putIntoFuseSlot, retrieveFromFuseSlot;
    bool IsFilled => containedBook != null;

    private BookshelfScript bookshelf;
    private AudioSource aud;

    static int correctBooks = 0;

    private void Start()
    {
        bookshelf = GetComponentInParent<BookshelfScript>();
        aud = GetComponent<AudioSource>();

        if (IsFilled)
            containedBook.GetComponent<Collider>().enabled = false;
    }

    public void Interact()
    {
        if (IsFilled)
        {
            aud.PlayOneShot(retrieveFromFuseSlot);
            RetrieveBook();
        }
        else
        {
            aud.PlayOneShot(putIntoFuseSlot);
            PlaceBook();
        }
    }

    void PlaceBook() //puts the fuse the player is holding in the slot
    {
        if (bookshelf.currentlyHeldBook == null) { return; }

        containedBook = bookshelf.currentlyHeldBook;

        if (CheckFuse())
            correctBooks++;

        StartCoroutine(PlaceBookOnShelfStand(containedBook.transform, transform.position));
        containedBook.GetComponent<Collider>().enabled = false;

        bookshelf.currentlyHeldBook = null;

        if (correctBooks == 4)       
            StartCoroutine(bookshelf.SetSolvedState());
    }

    void RetrieveBook() //extracts the fuse from the slot and hands it to the player
    {
        if (bookshelf.currentlyHeldBook != null) { return; }

        //isFilled = false;

        if (CheckFuse()) { correctBooks--; }

        containedBook.transform.localRotation = Quaternion.Euler(0, 180, 0);
        containedBook.GetComponent<Collider>().enabled = false;
        bookshelf.currentlyHeldBook = containedBook;
        containedBook = null;
    }

    bool CheckFuse()
    {
        return containedBook.name.Contains(correctBookTitle);
    }

    IEnumerator PlaceBookOnShelfStand(Transform book, Vector3 endPos)
    {
        float lapsed = 0.0f;
        Vector3 startPos = book.position;
        Vector3 startRot = book.transform.localEulerAngles;

        while (lapsed <= 1.0f)
        {
            lapsed += Time.deltaTime * 4;
            book.position = Vector3.Lerp(startPos, endPos, lapsed * lapsed);
            book.localEulerAngles = Vector3.Lerp(startRot, new Vector3(18, startRot.y, startRot.z), lapsed);
            yield return null;
        }
    }
}
