﻿using System.Collections;
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

        StartCoroutine(StoolScript.PlaceBookOnStool(containedBook.transform, transform.position));
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

        containedBook.transform.rotation = Quaternion.identity; //todo: will work with correct values
        containedBook.GetComponent<Collider>().enabled = false;
        bookshelf.currentlyHeldBook = containedBook;
        containedBook = null;
    }

    bool CheckFuse()
    {
        return containedBook.name.Contains(correctBookTitle);
    }
}
