using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPopUp : PopUp
{

    void Update()
    {
        if (templatePath == null)
        {
            this.Close();
        }
    }

    
}
