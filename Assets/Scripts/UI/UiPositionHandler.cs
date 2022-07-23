using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 08/05/2022

 * Brief    : The power of positionning UI stuff at runtime
 * **********************************************************************
*/

/// <summary>
/// For an example usecase, see <see cref="CodeExamples.BoxRepositionExample"/> 
/// </summary>
[Serializable]
public class UIPositionHandler
{
    public enum Pivots
    {
        RIGHT_TOP,
        CENTER,
        LEFT_TOP,
        LEFT_BOTTOM,
        RIGHT_BOTTOM,
    }
    public enum Quadrant
    {
        LEFT_TOP,
        LEFT_BOTTOM,
        RIGHT_TOP,
        RIGHT_BOTTOM,
        CENTER,
    }
    public enum DynamicPivots
    {
        TOP_CORNERS,
        BOTTOM_CORNERS,
        LEFT_CORNERS,
        RIGHT_CORNERS,
        ANY_CORNERS,
    }

    public enum DynamicPosition
    {
        OUTWARD,
        INWARD,
        NONE
    }

    /// <summary>
    /// Don't forget to set this up in the editor
    /// </summary>
    [SerializeField,Header("Starting point from where the destination will be calculated")]
    Transform _origin;

    [SerializeField,Tooltip("Tip: The origin point will be the center of the screen instead")]
    bool _doNotUseOrigin;

    [SerializeField, Tooltip("Tip: X: Left -> right, Y: Down -> Up"),Header("Where you want to reposition your UI element")]
    Vector3 _destination;

    [Header("Will try to center(inward or outward) the object based on the origin's position on the screen")]
    [SerializeField] DynamicPosition _dynamicRepostionningX = DynamicPosition.NONE;
    [SerializeField] DynamicPosition _dynamicRepostionningY = DynamicPosition.NONE;

    [SerializeField, Header("Changes pivot to one of these when Dynamic (X or Y) are used")]
    DynamicPivots _dynamicPivot = DynamicPivots.ANY_CORNERS;

    [SerializeField, Header("Sets the default pviot if no dynamic pivot is used")]
    Pivots _defaultPivot = Pivots.CENTER;

    /// <summary>
    /// Retourne une position identique à l'origine
    /// </summary>
    public Vector3 GetOriginPosition()
    {
        return new Vector3(_origin.transform.position.x, _origin.transform.position.y, _origin.transform.position.z);
    }

    /// <summary>
    /// Change la position d'origine
    /// </summary>
    /// <param name="newPosition"></param>
    /// <param name="UIElement">L'élément du ui à repositionner. Si null, ne reposisitionne rien</param>
    public void SetNewOrigin(Transform newOrigin, RectTransform UIElement = null)
    {
        _origin = newOrigin;

        if (UIElement)
            RepositionUIElement(UIElement);
    }

    /// <summary>
    /// Gère la position d'un UI element 
    /// </summary>
    public void RepositionUIElement(RectTransform UIElement)
    {
        var originAsScreenPoint = FindOriginPositionOnScreen();

        // Modifie le pivot de l'objet 
        SetPivot(UIElement, FindBestPivotFromOrigin(originAsScreenPoint));

        // Modifie la position selon les paramètres dynamic positionning (XY)
        var newPosition = SetPositionDynamically(originAsScreenPoint);

        UIElement.position = newPosition;
    }

    /// <summary>
    /// Find the position of the origin point on screen.
    /// WARNING: if the origin is a UI element, it must be on an overlay Canvas, otherwise
    /// unpredictable stuff may happen
    /// </summary>
    Vector2 FindOriginPositionOnScreen()
    {
        if(_origin == null && _doNotUseOrigin == false)
        {
            Debug.LogError("Origin is not set. If you don't want to use origin, please toggle the 'Do No Use Origin' paramater.");
            new Vector2(Screen.width / 2, Screen.height / 2);
        }

        // The origin is in the center of the screen by default
        if (_doNotUseOrigin)
        {
            return new Vector2(Screen.width / 2, Screen.height / 2);
        }

        var rect = _origin.GetComponent<RectTransform>();

        // The origin point is a UI element
        if (rect)
        {
            // Gets the center of the originTransform as a screen point
            Vector3[] objectCorners = new Vector3[4];
            rect.GetWorldCorners(objectCorners);

            // [2] = Bottom-Right corner, [0] = Top Left corner
            var originInScreenSpace = new Vector2(
                objectCorners[0].x + (objectCorners[2].x - objectCorners[0].x) / 2,
                objectCorners[0].y + (objectCorners[2].y - objectCorners[0].y) / 2);

            return originInScreenSpace;
        }

        // If the origin's Transform is a non UI Element
        var point = DynamicUiManager.Instance.MainCamera.WorldToScreenPoint(_origin.position);
        return point;
    }
    /// <summary>
    ///  Modifie la position selon le quadrant dans lequel se trouve le point d'origine <br/><br/>
    /// Si _dynamicRepostionning(XY)= INWARD:    l'objet sera positionné vers le centre de l'écran en X et/ou Y<br/>
    /// Si _dynamicRepostionning(XY)= OUTWARD:  l'objet sera positionné à l'opposé du centre de l'écran en X et/ou Y<br/>
    /// Si _dynamicRepostionning(XY)= NONE: Aucune repositionnement n'aura lieu en X et/ou Y <br/><br/>
    /// </summary>
    Vector2 SetPositionDynamically(Vector2 originAsScreenPoint)
    {
        var originX = originAsScreenPoint.x;
        var originY = originAsScreenPoint.y;

        float centerScreenX = Screen.width / 2;
        float centerScreenY = Screen.height / 2;

        // This little thing here makes the UI responsive on different resolutions
        var destinationX = _destination.x * ((Screen.width / DynamicUiManager.CANVAS_SCALE_RATIO.x) );
        var destinationY = _destination.y * ((Screen.height / DynamicUiManager.CANVAS_SCALE_RATIO.y));

        // safety assignement
        var newX = originX + destinationX;
        var newY = originY + destinationY;

        // Traitement de la position en X
        var originXIsPositive = originX > centerScreenX;
        var destinationXIsPositive = destinationX > 0;

        if (_dynamicRepostionningX != DynamicPosition.NONE)
        {
            // Si se trouve à droite
            if (originXIsPositive)
            {
                // La direction du point est vers la droite
                if (destinationXIsPositive)
                {
                    // Change la direction vers l'intérieur(gauche)
                    if (_dynamicRepostionningX == DynamicPosition.INWARD)
                        newX = originX - destinationX;
                }
                else
                {
                    // Change la direction vers l'intérieur(droite)
                    if (_dynamicRepostionningX == DynamicPosition.OUTWARD)
                        newX = originX - destinationX;
                }
            }
            else
            {
                // La direction du point est vers la gauche
                if (!destinationXIsPositive)
                {
                    // Change la direction vers l'intérieur(droite)
                    if (_dynamicRepostionningX == DynamicPosition.INWARD)
                        newX = originX - destinationX;
                }
                else
                {
                    // Change la direction vers l'intérieur(gauche)
                    if (_dynamicRepostionningX == DynamicPosition.OUTWARD)
                        newX = originX - destinationX;
                }
            }
        }

        // Traitement de la position en Y
        var originYIsPositive = originY > centerScreenY;
        var destinationYIsPositive = destinationY > 0;

        if (_dynamicRepostionningY != DynamicPosition.NONE)
        {
            // Si se trouve en haut
            if (originYIsPositive)
            {
                // La direction du point est en haut
                if (destinationYIsPositive)
                {
                    // Change la direction vers l'intérieur(bas)
                    if (_dynamicRepostionningY == DynamicPosition.INWARD)
                        newY = originY - destinationY;
                }
                else
                {
                    // Change la direction vers l'intérieur(haut)
                    if (_dynamicRepostionningY == DynamicPosition.OUTWARD)
                        newY = originY - destinationY;
                }
            }
            else
            {
                // La direction du point est vers le bas
                if (!destinationYIsPositive)
                {
                    // Change la direction vers l'intérieur(haut)
                    if (_dynamicRepostionningY == DynamicPosition.INWARD)
                        newY = originY - destinationY;
                }
                else
                {
                    // Change la direction vers l'intérieur(bas)
                    if (_dynamicRepostionningY == DynamicPosition.OUTWARD)
                        newY = originY - destinationY;
                }
            }
        }

        return new Vector3(newX, newY, 0);
    }

    /// <summary>
    /// Détermine dans quel quadrant se trouve un point
    /// WARNING: seul le point compte. Il se peut très bien que le point soit associés
    /// à un objet qui touche à plusieurs quadrants
    /// </summary>
    Quadrant FindQuadrant(Vector2 screenPoint)
    {
        // The Screen origin point is bottom left. To find the quadrant, more easily, we
        // add a half the width/height to the points so that the origin is at the center of the screen
        var x = screenPoint.x - Screen.width / 2;
        var y = screenPoint.y - Screen.height / 2;
        var isPositiveX = x > 0;
        var isPositiveY = y > 0;

        if (x == 0 && y == 0)
            return Quadrant.CENTER;

        // Détermine les positions "non-centrés"
        // (c'est techniquement possible mais une perte de temps de gérer ça)
        if (isPositiveX)
        {
            if (isPositiveY)
                return Quadrant.RIGHT_TOP;

            return Quadrant.RIGHT_BOTTOM;
        }
        else
        {
            if (isPositiveY)
                return Quadrant.LEFT_TOP;

            return Quadrant.LEFT_BOTTOM;
        }
    }

    /// <summary>
    /// Trouve le pivot qui permet de centrer le plus possible le UIElement
    /// selon le Quadrant du point d'origine. 
    /// </summary>
    Pivots FindBestPivotFromOrigin(Vector2 originAsScreenPoint)
    {
        if (_dynamicRepostionningX == DynamicPosition.NONE && _dynamicRepostionningY == DynamicPosition.NONE)
        {
            return _defaultPivot;
        }

        var quadrant = FindQuadrant(originAsScreenPoint);
        switch (quadrant)
        {
            case Quadrant.LEFT_TOP:
                return ClosestAnchorLEFT_TOP();
            case Quadrant.RIGHT_TOP:
                return ClosestAnchorRIGHT_TOP();
            case Quadrant.RIGHT_BOTTOM:
                return ClosestAnchorRIGHT_BOTTOM();
            case Quadrant.LEFT_BOTTOM:
                return ClosestAnchorLEFT_BOTTOM();
            case Quadrant.CENTER:
                return Pivots.CENTER;
            default:
                Debug.LogError("Enum missing member");
                return _defaultPivot;
        }
    }

    /// <summary>
    /// Set le point de rotation d'un rect transform 
    /// </summary>
    void SetPivot(RectTransform UIElement, Pivots pivot)
    {
        switch (pivot)
        {
            case Pivots.LEFT_TOP:
                UIElement.pivot = new Vector2(0, 1);
                break;
            case Pivots.LEFT_BOTTOM:
                UIElement.pivot = new Vector2(0, 0);
                break;
            case Pivots.CENTER:
                UIElement.pivot = new Vector2(0.5f, 0.5f);

                break;
            case Pivots.RIGHT_TOP:
                UIElement.pivot = new Vector2(1, 1);
                break;
            case Pivots.RIGHT_BOTTOM:
                UIElement.pivot = new Vector2(1, 0);
                break;
            default:
                Debug.LogError("Only one anchor point can be set");
                UIElement.pivot = new Vector2(0.5f, 0.5f);
                break;
        }
    }

    /*
        WARNING: j'ai pas trouvé de façon intelligente d'automatiser tout ça.
        Ne voulant pas faire des switchs imbriqué j'ai donc fait les méthodes suivantes
        en espérant au moins que ce sera plus facile à modifier en débuggant tout ça
     */
    // --------------------------------------------------------------------------------------------
    Pivots ClosestAnchorLEFT_TOP()
    {
        switch (_dynamicPivot)
        {
            case DynamicPivots.TOP_CORNERS: return Pivots.LEFT_TOP;
            case DynamicPivots.BOTTOM_CORNERS: return Pivots.LEFT_BOTTOM;
            case DynamicPivots.LEFT_CORNERS: return Pivots.LEFT_TOP;
            case DynamicPivots.RIGHT_CORNERS: return Pivots.RIGHT_TOP;
            case DynamicPivots.ANY_CORNERS: return Pivots.LEFT_TOP;
            default:
                Debug.LogError("Enum missing member");
                return Pivots.LEFT_TOP;
        }
    }

    Pivots ClosestAnchorRIGHT_TOP()
    {
        switch (_dynamicPivot)
        {
            case DynamicPivots.TOP_CORNERS: return Pivots.RIGHT_TOP;
            case DynamicPivots.RIGHT_CORNERS: return Pivots.RIGHT_TOP;
            case DynamicPivots.ANY_CORNERS: return Pivots.RIGHT_TOP;
            case DynamicPivots.BOTTOM_CORNERS: return Pivots.RIGHT_BOTTOM;
            case DynamicPivots.LEFT_CORNERS: return Pivots.LEFT_TOP;
            default:
                Debug.LogError("Enum missing member");
                return Pivots.RIGHT_TOP;
        }
    }

    Pivots ClosestAnchorRIGHT_BOTTOM()
    {
        switch (_dynamicPivot)
        {
            case DynamicPivots.RIGHT_CORNERS: return Pivots.RIGHT_BOTTOM;
            case DynamicPivots.ANY_CORNERS: return Pivots.RIGHT_BOTTOM;
            case DynamicPivots.BOTTOM_CORNERS: return Pivots.RIGHT_BOTTOM;
            case DynamicPivots.TOP_CORNERS: return Pivots.RIGHT_TOP;
            case DynamicPivots.LEFT_CORNERS: return Pivots.LEFT_BOTTOM;
            default:
                Debug.LogError("Enum missing member");
                return Pivots.RIGHT_BOTTOM;
        }
    }

    Pivots ClosestAnchorLEFT_BOTTOM()
    {
        switch (_dynamicPivot)
        {
            case DynamicPivots.LEFT_CORNERS: return Pivots.LEFT_BOTTOM;
            case DynamicPivots.ANY_CORNERS: return Pivots.LEFT_BOTTOM;
            case DynamicPivots.BOTTOM_CORNERS: return Pivots.LEFT_BOTTOM;
            case DynamicPivots.RIGHT_CORNERS: return Pivots.RIGHT_BOTTOM;
            case DynamicPivots.TOP_CORNERS: return Pivots.LEFT_TOP;
            default:
                Debug.LogError("Enum missing member");
                return Pivots.LEFT_BOTTOM;
        }
    }


}