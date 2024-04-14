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

            //Get Logo from AppearancSettings Table 

            //var config = _dbcontext.Configs.AsNoTracking().FirstOrDefault();
            //var logoAttachment = _dbcontext.Attachments.FirstOrDefault(x => x.Id == config.LogoAttachmentId);

            //Dictionary<string, string> param = new Dictionary<string, string>();
            //if (logoAttachment != null)
            //{
            //    //Convert Logo() to Base 64 

            //    using MemoryStream stream = new MemoryStream(logoAttachment.FileData);
            //    var logoImage = Convert.ToBase64String(stream.ToArray());
            //    param.Add("logo", logoImage);
            //}
            //else
            //{
            //    param.Add("logo", "");
            //}

            //if (Params != null)
            //{

            //    foreach (var par in Params)
            //    {
            //        param.Add(par.Key, par.Value);
            //    }
            //}

            //var reportResult = report.Execute(ReportrType, 1, param);
            var reportResult = report.Execute(ReportrType, 1);
            return reportResult;
        }

    }
}
