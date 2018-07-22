using AquaAPI.DAL;
using System;

namespace AquaAPI.Models
{
    public class ReportInfo
    {
        #region Member Properties

        public int ReportID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Tags { get; set; }
        public string CreationDate { get; set; }
        public string LastModifiedDate { get; set; }
        public string FilePath { get; set; }

        #endregion

        #region Public Methods

        public void ProcessReport(ReportTemplate template, string filter)
        {     
            if (filter.Equals("Create"))
            {
                this.ReportName = (template.ReportName != null) ? template.ReportName : string.Empty;
                this.ReportDescription = (template.ReportDescription != null) ? template.ReportDescription : string.Empty;
                this.Tags = (template.Tags != null) ? template.Tags : string.Empty;

                // Currently path given here.. can be moved to web.config
                this.FilePath = string.Format("\\AquaDB\\{0}\\", this.ReportName);

                this.CreationDate = DateTime.Now.ToString("yyyy-MM-dd");
                this.LastModifiedDate = this.CreationDate;
            }
            else if (filter.Equals("Update"))
            {
                // Get previous stored record
                ReportInfo previous = null;
                try
                {
                    previous = DbManager.GetReportByID(template.ReportID);
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                if (previous != null)
                {
                    this.ReportName = (!string.IsNullOrEmpty(template.ReportName)) ? template.ReportName : previous.ReportName;
                    this.ReportDescription = (!string.IsNullOrEmpty(template.ReportDescription)) ? template.ReportDescription : previous.ReportDescription;
                    this.Tags = (!string.IsNullOrEmpty(template.Tags)) ? template.Tags : previous.Tags;

                    // Currently path given here.. can be moved to web.config
                    this.FilePath = string.Format("\\AquaDB\\{0}\\", this.ReportName);

                    this.LastModifiedDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
        }

        #endregion
    }
}