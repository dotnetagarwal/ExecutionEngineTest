namespace ESOC.CLTPull.ExecutionEngine.Core.Constants
{
    public static class ControlConstants
    {

        public static class Status
        {
            public const string INIT = "INIT";
            public const string INPROGRESS = "INPROGRESS";
            public const string ALERTCOMPLETED = "COMPLETED";

        }

        public static class Connectionstrings
        {
            public const string Oracle = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = {2})));User ID = {3}; Password={4}";
        }
    }
    public static class DateFormats
    {
        public const string DateFormatddMMMyyyy = "dd-MMM-yyyy";
    }
    public static class AttachmentDetails
    {
        public const string AttachmentFolder = @"\\NAS01402pn\Data\SOC India Share\Arunima\Attachments";
        public const string AttachmentName = "OOBAttachment";
        public const string ArchiveFolder = "Archive";
        public const string ExcelExtension = ".xlsx";
    }
}
