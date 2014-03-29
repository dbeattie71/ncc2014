namespace DemoApp.Core.Rest
{
    public class FileParameter
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
        public string Name { get; set; }

        public FileParameter(string name, byte[] bytes, string fileName)
        {
            FileName = fileName;
            Data = bytes;
            Name = name;
        }
    }
}
