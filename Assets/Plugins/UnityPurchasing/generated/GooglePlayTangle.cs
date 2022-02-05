#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("MnNxRcBiDZo348tWsH/7ChR4KK2JO+iSdYZ81VDbTEBOe0YIzC+hdmJ/lpdBdsJjUivXZzAXyRjwGP98/X5wf0/9fnV9/X5+f/o9PFEsdUT+C/2xxuzoxbmp+LYP/Zi0Bwo85bZuFNQOU9CmklmOsIOfGT3dpkvLQpnNIIabPgE/e8rDu9iw72EAZ9bv+eNST9gP9/usYr6IZZA6orFjZQ56DDlol9x6uIy4t/gtLP7YcIToNNzsegtHiYKP3eDkm88Rr0yAp2tP/X5dT3J5dlX5N/mIcn5+fnp/fLA3EUpXSUujxCpT62UV5H3CoKgk0aNCoI0xir6SFlXB9vFGdGfBybr/37Ol/kf3nPfiiFkbfDz7RkYro/uRr4+TT2V6gn18fn9+");
        private static int[] order = new int[] { 12,10,3,10,11,7,9,13,9,13,12,13,13,13,14 };
        private static int key = 127;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
