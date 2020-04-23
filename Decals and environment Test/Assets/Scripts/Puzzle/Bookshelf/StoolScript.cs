using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StoolScript : MonoBehaviour
{
    /*===========
     * TO BE USED WITHIN A BOOKSHELF PUZZLE
    =============*/

    [Tooltip("All the places on the side of the box where unallocated fuses appear")]
    public Transform[] slots;
    private GameObject[] restingBooks; //equivalent of embeddedFuses in Fuse Tray Script
    private BookshelfScript bookshelf;

    private void Start()
    {
        bookshelf = GetComponentInParent<BookshelfScript>();

        restingBooks = new GameObject[slots.Length];
        enabled = false;
    }

    public void AssignFilledSlot(GameObject g) //this way the tray remembers how many slots have ever been filled (it avoids spawning things twice)
    {
        for (int i = 0; i < restingBooks.Length; i++)
            if (restingBooks[i] == null) // if there is no embedded fuse
            {
                g.transform.position = slots[i].position;
                restingBooks[i] = g;
                return;
            }
    }

    public void HandBook(Vector3 point) //equips a book to the cursor, selecting the nearest one
    {
        GameObject nearest = null; //default values, never used
        float nearestDist = 9999;
        int bookNum = -1;

        for (int i = 0; i < restingBooks.Length; i++)
            if (restingBooks[i] != null)
            {
                //Check the distance between the point the player clicked and each available book
                Vector3 bookPos = restingBooks[i].transform.position;
                float dist = Vector3.Distance(bookPos, point);

                if (dist < nearestDist) //Save the nearest book and then hand it to the player
                {
                    nearest = restingBooks[i];
                    nearestDist = dist;
                    bookNum = i;
                }
            }

        if (bookNum == -1) { return; } //means there are no books available

        bookshelf.currentlyHeldBook = nearest;
        nearest.transform.localRotation = Quaternion.Euler(0, 180, 0); //will work with right values
        restingBooks[bookNum] = null;
    }

    public void StoreBook(GameObject book) //accept a fuse from the cursor and stores it
    {
        for (int i = 0; i < restingBooks.Length; i++)
            if (restingBooks[i] == null) //if there is no embedded fuse
            {
                restingBooks[i] = book;
                bookshelf.currentlyHeldBook = null;
                book.transform.localRotation = Quaternion.Euler(0, 180, 0);

                StartCoroutine(PlaceBookOnStool(book.transform, slots[i].position));
                return;
            }
    }

    public static IEnumerator PlaceBookOnStool(Transform book, Vector3 endPos) //lerps the fuse from cursor to tray slot
    {
        float lapsed = 0.0f;
        Vector3 startPos = book.position;
        Vector3 startRot = book.transform.localEulerAngles;

        while (lapsed <= 1.0f)
        {
            lapsed += Time.deltaTime * 4;
            book.position = Vector3.Lerp(startPos, endPos, lapsed * lapsed);
            book.transform.localEulerAngles = Vector3.Lerp(startRot, new Vector3(0, startRot.y, startRot.z), lapsed);
            yield return null;
        }
    }
}
