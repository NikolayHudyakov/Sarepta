namespace ProductLabeling.RegistryEdit
{
    public static class RegPassword
    {
        private const string Key = "PassValue";

        private static readonly RegEdit _regEdit = new();

        public static bool CheckHash(string value) => _regEdit.CheckHash(Key, value);

        public static void CreateHash(string value) => _regEdit.CreateHash(Key, value);
        
    }
}
