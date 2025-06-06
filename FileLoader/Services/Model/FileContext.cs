namespace FileLoader.Services.Model
{
    public class FileContext
    {
        public string Key { get; set; }

        public string CountryCodePrefix { get; set; }

        public string ProcessType { get; set; }

        public string AffiliateId { get; set; }

        public string ProcessId { get; set; }

        public string FileExtension { get; set; }

        public string Message { get; set; }

        public FileContext(string key)
        {
            Key = key;
            SplitKey(key);
        }

        public void SplitKey(string filePath)
        {
            var parts = filePath.Split('/');
            if (parts.Length < 3)
            {
                throw new ArgumentException("Invalid file key format.");
            }

            CountryCodePrefix = parts[0];
            ProcessType = parts[1];

            var fileName = parts[2];
            var fileNameParts = fileName.Split('_');

            if (fileNameParts.Length < 3)
            {
                throw new ArgumentException("Invalid file name format.");
            }

            AffiliateId = fileNameParts[0];

            var idAndExt = fileNameParts[2].Split('.');
            ProcessId = idAndExt[0];
            FileExtension = idAndExt.Length > 1 ? idAndExt[1] : string.Empty;

            if (!Guid.TryParse(ProcessId, out _))
                throw new ArgumentException("Invalid ProcessId format. Must be a GUID.");

        }
    }
}
