using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FuseTrayScript : MonoBehaviour
{
    [Tooltip("All the places on the side of the box where unallocated fuses appear")]
    public Transform[] slots;
    class TraySlot 
    {
        public GameObject embeddedFuse = null;
    }

    TraySlot[] traySlots;
    private FuseBoxScript fuseBox;

    private void Start()
    {
        fuseBox = GetComponentInParent<FuseBoxScript>();

        traySlots = new TraySlot[slots.Length];
        for (int i = 0; i < traySlots.Length; i++)
            traySlots[i] = new TraySlot();
    }

    public void AssignFilledSlot(GameObject g)
    {
        for (int i = 0; i < traySlots.Length; i++)
            if (traySlots[i].embeddedFuse == null) // if there is no embedded fuse
            {
                g.transform.position = slots[i].position;
                traySlots[i].embeddedFuse = g;
                return;
            }
    }

    public void HandFuse()
    {
        for (int i = 0; i < traySlots.Length; i++)
            if (traySlots[i].embeddedFuse != null) //if there is embedded fuse
            {
                fuseBox.currentlyHeldFuse = traySlots[i].embeddedFuse;
                traySlots[i].embeddedFuse = null;
                return;
            }
    }

    public void StoreFuse(GameObject fuse)
    {
        for (int i = 0; i < traySlots.Length; i++)
            if (traySlots[i].embeddedFuse == null) //if there is no embedded fuse
            {
                traySlots[i].embeddedFuse = fuse;
                fuse.transform.position = slots[i].position;
                fuseBox.currentlyHeldFuse = null;
                return;
            }
    }
}
