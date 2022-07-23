using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 21/04/2022

 * Brief    : Exemple d'intégration pour MonobehaviourInitializer 
 * 
 * In depth : Cet initializer permet d'initialiser un 
 * Monobehaviour après sa création. Il est recommandé de l'utiliser avec 
 * une factory pour assurer que l'initialisation soit appelé le plus tôt
 * possible après la création de l'objet.
 * 
 * Pour utiliser l'initializer avec des paramètres custom, il faut faire 
 * l'héritage de 2 classe:
 *   
 *  MonobehaviourInitializer        // Classe contenant la méthode d'initialisation:
 *  MonoBehaviourInitializerArgs   // Classe contenant les params de la méthode d'init:
 * 
 * Voir exemples plus bas...
 * **********************************************************************
*/

namespace CodeExamples
{
    /// <summary>
    /// Example d'application
    /// </summary>
    public class FooBarFactory : MonoBehaviour
    {
        private void Start()
        {
            BuildA();
            /*
             * BuildB();
             * BuildC();
             * ...
            */
        }
        public InitTestA BuildA()
        {
            // Création d'un gameObject bidon
            var gameObject = new GameObject();
            var test = gameObject.AddComponent<InitTestA>();

            // Initialization
            test.Intialize(new InitTestArgsA { A = "OK" });
            return test;
        }
    }





    // *******************************************************************
    // *******************************************************************
    /*
         EN BREF:
         
        Voici un exemple de comment faire l'implémentation.
         Ces classes respectent une structure similaire à l'initializer
    */

    // STEP 1: Héritage des args Generic
    // ---------------------------------
    class argsBase { }
    class args1 : argsBase { }
    class args2 : args1 { }
    class args3 : args2 { }
    class args4 : args3 { }


    // STEP 2: Héritage de la classe qui va contenir la méthode d'Init
    // ---------------------------------------------------------------

    // Base class
    class AYoBase<T> : MonoBehaviour { }

    // Premier parent qui commence l'héritage des Generics    
    class AYoParent<T> : AYoBase<T> where T : argsBase { }

    // Héritage infini (Votre code commence ici ->)
    class AYo1Implementation<T> : AYoParent<T> where T : args1 { }
    class AYo2Implementation<T> : AYo1Implementation<T> where T : args2 { }
    class AYo3Implementation<T> : AYo2Implementation<T> where T : args3 { }
    
    // ...


    // STEP 3: Création de classes qui héritent des implémentation(*)
    // (*) Cette étape est malheureusement nécessaire,
    // car Unity refuse d'attacher les scripts de type
    // Generic(ex: Swag<T>) sur les Gameobject.
    // -----------------------------------------------------------

    class AYo1 : AYo1Implementation<args1>{ }
    class AYo2 : AYo2Implementation<args2> { }
    class AYo3 : AYo2Implementation<args3> { }

    // *******************************************************************
    // *******************************************************************



    // *************************************
    //   EXEMPLE D'IMPLÉMENTATION COMPLET
    // *******************************

    // Args: Chaque initializer peut utiliser ces propres args à travers l'héritage
    public class InitTestArgsA : MonoBehaviourInitializerArgs { public string A; }
    public class InitTestArgsB : InitTestArgsA { public string B; }
    public class InitTestArgsC : InitTestArgsB { public string C; }

    // Initializers:
    // WARNING: Les Generics ne peuvent pas être attaché à des Gameobjects
    // Pour chaque classe qui va implémenter l'initializer, il
    // faut créer une autre class qui va enlever le generic, voici comment...
    
    // D'abord créer implémentation: étant donné que cette classe est Generic,
    // il sera impossible de l'ajouter à un Gameobject
    public abstract class InitTestAImplementation<T> : MonoBehaviourInheritanceInitializer<T> where T : InitTestArgsA 
    {
        /// <summary>
        /// La seule méthode à implémenter. Mettre le code d'initialisation ici
        /// </summary>
        /// <param name="initializerArgs">Les args de l'initializer. Doit hériter de <see cref="MonoBehaviourInitializerArgs"/></param>
        protected override void DoInitialize(T initializerArgs) 
        {
            Debug.Log(initializerArgs.A);
        }
    }

    // Retire la contrainte de class. Cette classe peut être ajouté à un GameObject
    // si celui-ci est placé dans un fichier avec le même nom
    public class InitTestA : InitTestAImplementation<InitTestArgsA> { }

    // Pour continuer l'héritage, il faut hériter de la classe qui est Generic
    public abstract class InitTestBImplementation<T> : InitTestAImplementation<T> where T : InitTestArgsB
    {
        /// <summary>
        /// Héritage de l'initializer du parent
        /// </summary>
        protected override void DoInitialize(T initializerArgs)
        {
            // Step 1: Appelé l'initializer du parent au besoin
            base.DoInitialize(initializerArgs);

            // Step 2: Initialization code
            Debug.Log(initializerArgs.B);

            /*
               ...
            */
        }
    }
    public class InitTestB : InitTestBImplementation<InitTestArgsB> { }


    // Fin de l'héritage infini
    // Le nom du fichier doit être le même que celui-ci pour fonctionner
    public class InitTestC : InitTestBImplementation<InitTestArgsC>
    {
        sealed protected override void DoInitialize(InitTestArgsC initializerArgs)
        {
            base.DoInitialize(initializerArgs);

            Debug.Log(initializerArgs.C);
        }
    }

}