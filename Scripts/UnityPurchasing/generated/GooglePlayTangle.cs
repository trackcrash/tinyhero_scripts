// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("MIvypKTe2fucMLSP6ayUDDRvYSS9UYEYVGECVomF61nFhQi0+PwPDXGBDlGh0qMIEzEbSQEUPmPVHNCFuAOc9ZSJvY1uQbzNv3/eW1lu2WYygAMgMg8ECyiESoT1DwMDAwcCAZDup7DmwFHiksYKRdsO36zEN7digAMNAjKAAwgAgAMDAriyYr/J6IdCSvDtMbl/v3meOw19RqBCnHdl+OWUi6C5Qv6vQPzrlhfMd0mn1io4ReBeIwzlcqog2nfZ/5pC5bdVITOzypyad0o5aqv3SSkr3yf5GIHXOXhRY+4KdE3Rf6zi6nXX1LLEA78cRusKOUfMF+NX12vGWrjQ/md3BQWf4CzoF4aUZomKS4O5/7KAODR29hrR/fauH9n1gwABAwID");
        private static int[] order = new int[] { 7,3,13,11,7,10,11,8,13,11,12,12,13,13,14 };
        private static int key = 2;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
