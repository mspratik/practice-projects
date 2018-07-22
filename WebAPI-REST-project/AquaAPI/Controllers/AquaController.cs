using AquaAPI.DAL;
using AquaAPI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AquaAPI.Controllers
{
    public class AquaController : ApiController
    {
        // GET api/aqua
        [HttpGet]
        public HttpResponseMessage GetAllReports()
        {
            HttpResponseMessage message = null;
            
            try 
            {
                List<ReportTemplate> reports = null;
                List<ReportInfo> reportInfoList = DbManager.GetAllReports();
                if (reportInfoList != null && reportInfoList.Count > 0)
                {
                    reports = new List<ReportTemplate>();
                    foreach(ReportInfo info in reportInfoList)
                    {
                        ReportTemplate template = new ReportTemplate();
                        template.ProcessReportInformation(info);
                        reports.Add(template);
                    }
                }

                if (reports != null && reports.Count > 0)
                {
                    message = Request.CreateResponse(HttpStatusCode.OK, reports);
                }
                else
                {
                    message = Request.CreateResponse(HttpStatusCode.NotFound, "{ Not Found }");
                }
            }
            catch(Exception ex)
            {
                message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                message.ReasonPhrase = ex.Message.ToString();
            }

            return message;
        }

        // GET api/aqua/5
        [HttpGet]
        public HttpResponseMessage GetReportById(int id)
        {            
            HttpResponseMessage message = null;

            try 
            {
                ReportTemplate report = null;
                ReportInfo info = DbManager.GetReportByID(id);
                if (info != null)
                {
                    report = new ReportTemplate();
                    report.ProcessReportInformation(info);
                } 
                
                if (report != null)
                {
                    message = Request.CreateResponse(HttpStatusCode.OK, report);
                }
                else
                {
                    message = Request.CreateResponse(HttpStatusCode.NotFound, "{ Not Found }");
                }
            }
            catch (Exception ex)
            {
                message = new HttpResponseMessage(HttpStatusCode.BadRequest);                
                message.ReasonPhrase = ex.Message.ToString();                
            }

            return message;
        }

        // POST api/aqua
        [HttpPost]
        [Authorize]
        public HttpResponseMessage CreateReport(ReportTemplate report)
        {
            HttpResponseMessage message = null;

            // Check Input
            if (report != null)
            {
                ReportInfo info = new ReportInfo();
                info.ProcessReport(report, "Create");
                try 
                {
                    if(DbManager.AddReport(info))
                    {
                        message = Request.CreateResponse(HttpStatusCode.OK, "{ Record Created }");
                    }
                    else
                    {
                        message = Request.CreateResponse(HttpStatusCode.ExpectationFailed, "{ Record failed to be created }");
                    }
                }
                catch(Exception ex)
                {
                    message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    message.ReasonPhrase = ex.Message.ToString();
                }
            }
            else
            {
                message = Request.CreateResponse(HttpStatusCode.NotAcceptable, "{ Record information not provided }");
            }

            return message;
        }

        // PUT api/aqua/5
        [HttpPut]
        [Authorize]
        public HttpResponseMessage UpdateReport(int id, ReportTemplate report)
        {
            HttpResponseMessage message = null;

            // Check Input
            if (report != null && id != 0)
            {
                ReportInfo info = new ReportInfo();
                info.ReportID = id;
                report.ReportID = id;                
                try
                {
                    info.ProcessReport(report, "Update");
                    if(DbManager.UpdateReportByID(info))
                    {
                        message = Request.CreateResponse(HttpStatusCode.OK, "{ Record Updated }");
                    }
                    else 
                    {
                        message = Request.CreateResponse(HttpStatusCode.ExpectationFailed, "{ Record failed to be updated }");
                    }
                }
                catch (Exception ex)
                {
                    message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    message.ReasonPhrase = ex.Message.ToString();
                }
            }
            else
            {
                message = Request.CreateResponse(HttpStatusCode.NotAcceptable, "{ Record information not provided }");
            }

            return message;
        }

        // DELETE api/aqua/5
        [HttpDelete]
        [Authorize]
        public HttpResponseMessage DeleteReport(int id)
        {
            HttpResponseMessage message = null;

            if (id != 0)
            {
                try
                {
                    if (DbManager.DeleteReportByID(id))
                    {
                        message = Request.CreateResponse(HttpStatusCode.OK, "{ Record Deleted }");
                    }
                    else
                    {
                        message = Request.CreateResponse(HttpStatusCode.ExpectationFailed, "{ Record failed to be deleted }");
                    }
                }
                catch (Exception ex)
                {
                    message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    message.ReasonPhrase = ex.Message.ToString();
                }
            }
            else
            {
                message = Request.CreateResponse(HttpStatusCode.NotAcceptable, "{ Record ID not provided }");
            }            

            return message;
        }
    }
}