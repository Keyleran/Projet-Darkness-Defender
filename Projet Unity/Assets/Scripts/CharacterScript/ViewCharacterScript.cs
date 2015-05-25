// --------------------------------------------------
// Project: Darkness Defender
// Script: ViewCharacterScript.cs
// Author: Kevin Rey 3A 3DJV
// --------------------------------------------------

// Library
using UnityEngine;
using System.Collections;

// --------------------------------------------------
// 
// Script de Vues
// 
// --------------------------------------------------
public class ViewCharacterScript : MonoBehaviour
{
    [SerializeField]
    Camera _cameraPlayer = new Camera();

    [SerializeField]
    NetworkView _network;

    // Angle minimum et maximum de rotation sur l'axe Y (tourne sur Y pour changer la vue sur X)    // PAS NECESSAIRE
    // -360 -> 360, possibilite de faire un tour complet (ou plusieurs) sur l'axe Y
    //private float minimumX = -360.0f;
    //private float maximumX = 360.0f;

    // Angle minimum et maximum de rotation sur l'axe X (tourne sur X pour changer la vue sur Y)
    // -45 -> 45, angle de vue optimal pour le jeu
    private float minimumY = -60.0f;
    private float maximumY = 60.0f;

    // Angle Y initial
    private float rotationY = 0.0f;

    // Paramètres modifiables par le joueur
    public float sensitivity = 5.0f; // 5 par défaut

    private Data_keeper _data;

    void Start()
    {
        GameObject Network_Data = GameObject.Find("Network_Data");
        _data = (Data_keeper)Network_Data.GetComponent("Data_keeper");

        if ((_network.isMine) || (_data.gameMode == "Solo"))
        {
            _cameraPlayer.enabled = true;
        }
        else
        {
            _cameraPlayer.enabled = false;
        }
    }
     
    void FixedUpdate()
    {
        if ((_network.isMine) || (_data.gameMode == "Solo"))
        {
            // Si le joueur déplace la souris sur l'axe Horizontal
            if (Input.GetAxis("Mouse X") != 0)
            {
                // Rotation sur l'axe Y
                this.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
            }

            // Si le joueur déplace la souris sur l'axe Vertical
            if (Input.GetAxis("Mouse Y") != 0)
            {
                rotationY -= Input.GetAxis("Mouse Y") * sensitivity;

                // Permet de contenir les valeurs entre -360 et 360
                if (rotationY < -360)
                    rotationY += 360;
                if (rotationY > 360)
                    rotationY -= 360;

                // Mathf.Clamp permet de "borné" la valeur entre de limite, ex: Mathf.Clamp(160,-50,42), la valeur de sortie devient 42. 
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                // Applique le nouvel angle. !!! Penser à ajouter l'angle sur l'axe Y, sinon la caméra sera bloquée !!!
                _cameraPlayer.transform.rotation = Quaternion.Euler(rotationY, transform.localEulerAngles.y, 0);
            }
        }
    } 
}
