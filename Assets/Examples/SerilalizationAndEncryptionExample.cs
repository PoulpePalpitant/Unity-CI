using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Matthew Ventures
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 24/04/2022

 * Brief    : Example de sérialization et Encryption
 * **********************************************************************
*/


namespace CodeExamples
{
    [System.Serializable]
    public struct WeaponInfo
    {
        public string weaponID;
        public int durability;
    }
    public class SerilalizationAndEncryptionExample : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI text;
        [SerializeField] bool serialize;
        [SerializeField] bool usingXML;
        [SerializeField] bool encrypt;

        void Start()
        {
            WeaponInfo createdWeaponInfo = new WeaponInfo();
            createdWeaponInfo.weaponID = "Dirty Knife";
            createdWeaponInfo.durability = 5;

            text.text = createdWeaponInfo.ToString();
            Debug.Log("Weapon ID: " + createdWeaponInfo.weaponID);

            //////////////////////////////////////////////////////////////
            // Let's first serialize and encrypt....
            //////////////////////////////////////////////////////////////

            if (serialize)
            {
                if (usingXML)
                    text.text = Utils.Data.SerializeXML<WeaponInfo>(createdWeaponInfo);
                else
                    text.text = JsonUtility.ToJson(createdWeaponInfo);

                string serialized = text.text;
                Debug.Log("Serialized: " + serialized);
            }

            if (encrypt)
            {
                text.text = Utils.Data.EncryptAES(text.text);

                string encrypted = text.text;
                Debug.Log("Encrypted: " + encrypted);
            }

            //////////////////////////////////////////////////////////////
            // Now let's de-serialize and de-encrypt....
            //////////////////////////////////////////////////////////////

            string stringData = text.text;
            if (encrypt)
            {
                stringData = Utils.Data.DecryptAES(stringData);
                Debug.Log("Decrypted: " + stringData);
            }

            WeaponInfo derivedWeaponInfo = new WeaponInfo();
            if (serialize)
            {
                if (usingXML)
                    derivedWeaponInfo = Utils.Data.DeserializeXML<WeaponInfo>(stringData);
                else
                    derivedWeaponInfo = JsonUtility.FromJson<WeaponInfo>(stringData);

                Debug.Log("Deserialized: " + derivedWeaponInfo.weaponID);
            }
        }
    }
}