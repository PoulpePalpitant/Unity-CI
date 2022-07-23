
/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 16/05/2022

 * Brief    : Création d'enum automatiquement selon les properties
 * cosnt de n'importe quelle classe
 * 
 * In Depth : Créé des enums à partir de tout les noms de constantes 
 * d'un type.
 * 
 * Ref      : https://answers.unity.com/questions/1170350/editorscript-generate-enum-from-string.html
 * **********************************************************************
*/

#if UNITY_EDITOR

using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class ClassConstPropertiesEnumGenerator
{
    /// <summary>
    /// Ce path doit exister
    /// </summary>
    const string TXT_READ_ONLY_ENUMS_PATH = "Assets/Scripts/Data/TextReadOnly/TxtReadOnlyAutoEnums/";
    const string SUFFIX = "Enum";
    const string CS = ".cs";

    const string WARNING_MESSAGE = "///<summary>\n" +
    "///This class has been created automatically. Please refer to 'ClassConstPropertiesEnumGenerator' for more details\n" +
    "///</summary>\n";

    /// <summary>
    /// Ajoutez les types que vous désirez ici, puis cliquez sur le bouton Tools -> Generate all enums dans l'editeur
    /// Si vous ajouter des constantes au types ci-bas, vous devez regénérer de nouveau
    /// </summary>
    static readonly Type[] typesToGenerate = new Type[] {
        typeof(SceneNames),
        typeof(SortingLayers),
    };

    /// <summary>
    /// Génère des enums pour tout les types figurant dans cette 
    /// </summary>
    [MenuItem("Tools/Generate all Enums")]
    public static void Go()
    {
        for (int i = 0; i < typesToGenerate.Length; i++)
        {
            GenerateEnumForType(typesToGenerate[i]);
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Sorcery. Returns all property names of a given type
    /// https://stackoverflow.com/questions/10261824/how-can-i-get-all-constants-of-a-type-by-reflection
    /// https://stackoverflow.com/questions/13781468/get-list-of-properties-from-list-of-objects
    /// </summary>
    static List<string> GetConstantNamesInClass(Type type)
    {
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).Select(o => o.Name).ToList();
    }

    static void GenerateEnumForType(Type type)
    {
        string enumName = type.ToString() + SUFFIX;
        var enumEntries = GetConstantNamesInClass(type);
        string filePathAndName = TXT_READ_ONLY_ENUMS_PATH + enumName + CS; //The folder is expected to exist

        using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
        {
            streamWriter.Write(WARNING_MESSAGE);
            streamWriter.WriteLine("public enum " + enumName);
            streamWriter.WriteLine("{");
            for (int i = 0; i < enumEntries.Count; i++)
            {
                streamWriter.WriteLine("\t" + enumEntries[i] + ",");
            }
            streamWriter.WriteLine("}");
        }
    }
}
#endif