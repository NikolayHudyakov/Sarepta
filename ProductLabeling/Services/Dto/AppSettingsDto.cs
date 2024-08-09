using ProductLabeling.DataBase.Dto;
using ProductLabeling.Models.Dto.FirstStage;

namespace ProductLabeling.Services.Dto
{
    internal class AppSettingsDto
    {
        public ModbusTcpIoModuleDto IoModule { get; set; } = new ();
        public DataBaseDto DataBase { get; set; } = new();
        public FirstStageDto FirstStage { get; set; } = new ();
        
    }
}
