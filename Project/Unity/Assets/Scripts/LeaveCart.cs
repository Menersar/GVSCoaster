using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCart : MonoBehaviour
{
    public ConnectionManager conMan;

    public void leaveCart()
    {
        conMan.leaveCart();
    }
}