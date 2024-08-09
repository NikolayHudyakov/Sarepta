using Microsoft.Extensions.Logging;
using ProductLabeling.Models.Interfaces;
using ProductLabeling.Services.Dto;
using ProductLabeling.Services.Intefaces;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ProductLabeling.Models
{
    internal abstract class Stage : AppSettingsProvider
    {
        #region IoModule
        protected const ushort DO_0 = 0;
        protected const ushort DO_1 = 1;
        protected const ushort DO_2 = 2;
        protected const ushort DO_3 = 3;
        protected const ushort DO_4 = 4;
        protected const ushort DO_5 = 5;
        protected const ushort DO_6 = 6;
        protected const ushort DO_7 = 7;
        protected const ushort DO_8 = 8;
        protected const ushort DO_9 = 9;
        protected const ushort DO_10 = 10;
        protected const ushort DO_11 = 11;
        protected const ushort DO_12 = 12;
        protected const ushort DO_13 = 13;
        protected const ushort DO_14 = 14;
        protected const ushort DO_15 = 15;

        protected const ushort DI_0 = 0;
        protected const ushort DI_1 = 1;
        protected const ushort DI_2 = 2;
        protected const ushort DI_3 = 3;
        protected const ushort DI_4 = 4;
        protected const ushort DI_5 = 5;
        protected const ushort DI_6 = 6;
        protected const ushort DI_7 = 7;
        protected const ushort DI_8 = 8;
        protected const ushort DI_9 = 9;
        protected const ushort DI_10 = 10;
        protected const ushort DI_11 = 11;
        protected const ushort DI_12 = 12;
        protected const ushort DI_13 = 13;
        protected const ushort DI_14 = 14;
        protected const ushort DI_15 = 15;
        #endregion

        #region DbStatus
        protected const int Status0 = 0;
        protected const int Status1 = 1;
        protected const int Status2 = 2;
        protected const int Status3 = 3;
        protected const int Status4 = 4;
        #endregion

        private readonly IModbusTcpIoModuleService _ioModule;
        protected readonly ILogger logger;

        private NumberFormatInfo _numberFormatInfo = new() { NumberDecimalSeparator = "." };

        protected Stage(ISerializerService<AppSettingsDto> appSettings, IDevices devices, ILogger logger) : base(appSettings)
        {
            _ioModule = devices.IoModule;
            this.logger = logger;
        }


        public Counter Counter { get; init; } = new ();

        protected async void WriteDOPulseAsync(ushort coilAddress, int durationPulse)
        {
            await _ioModule.WriteSingleDOAsync(coilAddress, true);
            await Task.Delay(durationPulse);
            await _ioModule.WriteSingleDOAsync(coilAddress, false);
        }

        protected string GetDateTimeNow() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        protected string GetDateNow() => DateTime.Now.ToString("yy-MM-dd");

        protected string GetGroupCode(string codeReaderData, string codeDelimeter, string pPMDelimeter, double minPPMGroupCode, string noReadString)
        {
            if (codeReaderData == noReadString)
                return noReadString;

            var codesWithPpm = codeReaderData.Split(codeDelimeter);

            foreach (var codeWithPpm in codesWithPpm)
            {
                if (!codeWithPpm.Contains(pPMDelimeter))
                    return noReadString;

                var code = codeWithPpm.Split(pPMDelimeter)[0];
                var ppm = double.Parse(codeWithPpm.Split(pPMDelimeter)[1], _numberFormatInfo);

                if (ppm > minPPMGroupCode)
                    return code;
            }
            return noReadString;
        }

        protected string GetSingleCode(string codeReaderData, string codeDelimeter, string pPMDelimeter, double minPPMGroupCode, string noReadString)
        {
            if (codeReaderData == noReadString)
                return noReadString;

            var codesWithPpm = codeReaderData.Split(codeDelimeter);

            foreach (var codeWithPpm in codesWithPpm)
            {
                if (!codeWithPpm.Contains(pPMDelimeter))
                    return noReadString;

                var code = codeWithPpm.Split(pPMDelimeter)[0];
                var ppm = double.Parse(codeWithPpm.Split(pPMDelimeter)[1], _numberFormatInfo);

                if (ppm < minPPMGroupCode)
                    return code;
            }
            return noReadString;
        }
    }
}
