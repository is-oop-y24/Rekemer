namespace IsuExtra
{
    public class Singletone<T>
        where T : class, new()
    {
         private static T _instance;

         public static T Instance
         {
             get
             {
                 // if no instance is found, find the first GameObject of type T
                 if (_instance == null)
                 {
                     _instance = new T();
                 }

                 // return the singleton instance
                 return _instance;
             }
         }
    }
}