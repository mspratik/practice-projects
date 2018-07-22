using System;
using System.IO;

namespace AquaAPI.Models
{
    public class ReportTemplate
    {
        #region Member Properties

        public int ReportID { get; set; }
        public string ReportName { get; set; }
        public string ReportDescription { get; set; }
        public string Tags { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FullImageUrl { get; set; }
        public string PdfUrl { get; set; }
        public string ZipFileUrl { get; set; }

        #endregion

        #region Public Methods

        public void ProcessReportInformation(ReportInfo info)
        {
            if (info != null)
            {
                this.ReportID = info.ReportID;
                this.ReportName = info.ReportName;
                this.ReportDescription = info.ReportDescription;
                this.Tags = info.Tags;
                this.CreationDate = Convert.ToDateTime(info.CreationDate);
                this.LastModifiedDate = Convert.ToDateTime(info.LastModifiedDate);

                // File path generation                
                string path = Directory.GetCurrentDirectory() + info.FilePath;  
                if (Directory.Exists(path))
                {
                    // Zip File
                    this.ZipFileUrl = ProcessFile(path, "*.zip");
                    
                    // Pdf File
                    this.PdfUrl = ProcessFile(path, "*.pdf");

                    // Image Files
                    this.ThumbnailUrl = ProcessFile(path, "*.png");
                    this.FullImageUrl = ProcessFile(path, "*.png");                    
                }
                else
                {
                    this.ZipFileUrl = "No file path / file available!";
                    this.PdfUrl = "No file path / file available!";
                    this.ThumbnailUrl = "No file path / file available!";
                    this.FullImageUrl = "No file path / file available!";
                }                
            }            
        }

        #endregion

        #region Private Methods

        private string ProcessFile(string path, string filter)
        {
            string url = string.Empty;

            string[] fileEntries = Directory.GetFiles(path, filter);
            if (fileEntries != null && fileEntries.Length > 0)
            {
                var fileUri = new System.Uri(fileEntries[0]);
                url = fileUri.AbsoluteUri.ToString();
            }

            return url;
        }

        #endregion
    }
}