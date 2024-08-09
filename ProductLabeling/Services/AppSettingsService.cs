using ProductLabeling.Services.Dto;
using System;
using System.IO;

namespace ProductLabeling.Services
{
    internal class AppSettingsService : Serializer<AppSettingsDto>
    {
        private const string FileName = "appsettings.json";
        protected override string FilePath => Path.Combine(AppContext.BaseDirectory, FileName);
    }
}
