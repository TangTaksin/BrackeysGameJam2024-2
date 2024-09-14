using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum elements
{
    None,
    Wind,
    Rain,
    Hail,
    Thunder
}

public class Elements
{
    public static elements IntToElement(int _num)
    {
        var element = elements.None;

        switch (_num)
        {
            case 0: element = elements.None; break;
            case 1: element = elements.Wind; break;
            case 2: element = elements.Rain; break;
            case 3: element = elements.Hail; break;
            case 4: element = elements.Thunder; break;
        }

        return element;
    }
}
