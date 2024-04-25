using AspNetCore.Reporting;
using AttendenceSystem.IRepo;
using Microsoft.EntityFrameworkCore;

namespace AttendenceSystem.Repo
{
    public class ReportService: IReportService
    {
        private readonly IHostEnvironment _env;
        public ReportService(IHostEnvironment environment)
        {
            _env = environment;
        }
        public async Task<ReportResult> DownloadReport<T>(T dto, string ReportName, RenderType ReportrType, string DataSetName = "DataSet1", Dictionary<string, string> Params = null)
        {
            string Path = _env.ContentRootPath + "/wwwroot/Reports/" + ReportName + ".rdlc";
            LocalReport report = new LocalReport(Path);
            report.AddDataSource(DataSetName, dto);

            
            var reportResult = report.Execute(ReportrType, 1);
            return reportResult;
        }

    }
}
