using AspNetCore.Reporting;

namespace AttendenceSystem.IRepo
{
    public interface IReportService
    {
        Task<ReportResult> DownloadReport<T>(T dto, string ReportName, RenderType ReportrType, string DataSetName = "DataSet1", Dictionary<string, string> Params = null);
    }
}
