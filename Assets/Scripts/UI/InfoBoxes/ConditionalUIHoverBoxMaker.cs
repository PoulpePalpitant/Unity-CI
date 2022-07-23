using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 24/05/2022

 * Brief    : Fait et détruit une box selon une condition
 * **********************************************************************
*/

public abstract class ConditionalUIHoverBoxMaker<BoxType, BoxContent> : UIHoverBoxMaker<BoxType, BoxContent>
    where BoxType : InfoBox
    where BoxContent : InfoBoxContent
{
    protected override void OnHoverEnd(UiHoverObject.OnHoverArgs args)
    {
        if(CanDeleteCondition(args))
            base.OnHoverEnd(args);
    }

    protected override void OnHoverStart(UiHoverObject.OnHoverArgs args)
    {
        if (CanMakeCondition(args))
            base.OnHoverStart(args);
    }

    /// <summary>
    /// On hover start appel cette méthode pour savoir si on peut construire une box
    /// </summary>
    protected virtual bool CanMakeCondition(UiHoverObject.OnHoverArgs args)
    {
        return true;
    }

    /// <summary>
    /// On hover end appel cette méthode pour savoir si on peut delete une box
    /// </summary>
    protected virtual bool CanDeleteCondition(UiHoverObject.OnHoverArgs args)
    {
        return true;
    }
}