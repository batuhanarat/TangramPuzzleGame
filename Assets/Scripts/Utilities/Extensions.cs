
public static class Extensions
{
        public static void Shuffle<T>(this T[] array)
        {
            if (array == null) return;

            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T temp = array[k];
                array[k] = array[n];
                array[n] = temp;
            }
        }

}