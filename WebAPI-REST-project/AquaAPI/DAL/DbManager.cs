using AquaAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace AquaAPI.DAL
{
    public class DbManager : DataWorker
    {
        /// <summary>
        /// Get All User Information
        /// </summary>
        /// <returns> List of Users with Information </returns>
        public static List<UserInfo> GetUsers()
        {
            List<UserInfo> userList = null;

            try 
            {
                using (IDbConnection connection = database.CreateOpenConnection())
                {
                    string sqlQuery = string.Format("SELECT * FROM dbo.Users");
                    using (IDbCommand command = database.CreateCommand(sqlQuery, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            userList = new List<UserInfo>();                            
                            while (reader.Read())
                            {
                                userList.Add(new UserInfo()
                                {
                                    FirstName = reader["FirstName"].ToString(),
                                    FamilyName = reader["FamilyName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Password = reader["Password"].ToString()
                                });                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userList;
        }

        /// <summary>
        /// Get All Report Information
        /// </summary>
        /// <returns> List of Report Information </returns>
        public static List<ReportInfo> GetAllReports()
        {
            List<ReportInfo> reportList = null;

            try
            {
                using (IDbConnection connection = database.CreateOpenConnection())
                {
                    string sqlQuery = string.Format("SELECT * FROM dbo.ReportTemplate");
                    using (IDbCommand command = database.CreateCommand(sqlQuery, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {
                            reportList = new List<ReportInfo>();
                            while (reader.Read())
                            {
                                reportList.Add(new ReportInfo()
                                {
                                    ReportID = Int32.Parse(reader["ReportID"].ToString()),
                                    ReportName = reader["ReportName"].ToString(),
                                    ReportDescription = reader["ReportDescription"].ToString(),
                                    Tags = reader["Tags"].ToString(),
                                    CreationDate = reader["CreationDate"].ToString(),
                                    LastModifiedDate = reader["LastModifiedDate"].ToString(),
                                    FilePath = reader["FilePath"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return reportList;
        }

        /// <summary>
        /// Get Report Information by ID
        /// </summary>
        /// <returns> Report Information based on ID </returns>
        public static ReportInfo GetReportByID(int reportID)
        {
            ReportInfo report = null;

            try
            {
                using (IDbConnection connection = database.CreateOpenConnection())
                {
                    string sqlQuery = string.Format("SELECT * FROM dbo.ReportTemplate WHERE ReportID={0}", reportID);
                    using (IDbCommand command = database.CreateCommand(sqlQuery, connection))
                    {
                        using (IDataReader reader = command.ExecuteReader())
                        {                            
                            while (reader.Read())
                            {
                                report = new ReportInfo()
                                {
                                    ReportID = Int32.Parse(reader["ReportID"].ToString()),
                                    ReportName = reader["ReportName"].ToString(),
                                    ReportDescription = reader["ReportDescription"].ToString(),
                                    Tags = reader["Tags"].ToString(),
                                    CreationDate = reader["CreationDate"].ToString(),
                                    LastModifiedDate = reader["LastModifiedDate"].ToString(),
                                    FilePath = reader["FilePath"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return report;
        }

        /// <summary>
        /// Add Report Information
        /// </summary>
        /// <returns> Flag indicating if process was successful </returns>
        public static bool AddReport(ReportInfo info)
        {
            bool isSuccess = true;

            try
            {
                using (IDbConnection connection = database.CreateOpenConnection())
                {
                    string sqlQuery = string.Format("INSERT INTO dbo.ReportTemplate ([ReportName],[ReportDescription],[Tags],[CreationDate],[LastModifiedDate],[FilePath]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
                        info.ReportName, info.ReportDescription, info.Tags, info.CreationDate, info.LastModifiedDate, info.FilePath);
                    using (IDbCommand command = database.CreateCommand(sqlQuery, connection))
                    {
                        int rowAffected = command.ExecuteNonQuery();
                        if (rowAffected == 0)
                            isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        /// Update Report Information by ID
        /// </summary>
        /// <returns> Flag indicating if process was successful </returns>
        public static bool UpdateReportByID(ReportInfo info)
        {
            bool isSuccess = true;

            try
            {
                using (IDbConnection connection = database.CreateOpenConnection())
                {
                    string sqlQuery = string.Format("UPDATE dbo.ReportTemplate SET ReportName='{0}', ReportDescription='{1}', Tags='{2}', LastModifiedDate='{3}', FilePath='{4}' WHERE ReportID='{5}'",
                        info.ReportName, info.ReportDescription, info.Tags, info.LastModifiedDate, info.FilePath, info.ReportID);
                    using (IDbCommand command = database.CreateCommand(sqlQuery, connection))
                    {
                        int rowAffected = command.ExecuteNonQuery();
                        if (rowAffected == 0)
                            isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        /// Delete Report Information by ID
        /// </summary>
        /// <returns> Flag indicating if process was successful </returns>
        public static bool DeleteReportByID(int reportID)
        {
            bool isSuccess = true;

            try
            {
                using (IDbConnection connection = database.CreateOpenConnection())
                {
                    string sqlQuery = string.Format("DELETE FROM dbo.ReportTemplate WHERE ReportID={0}", reportID);
                    using (IDbCommand command = database.CreateCommand(sqlQuery, connection))
                    {
                        int rowDeleteCount = command.ExecuteNonQuery();
                        if (rowDeleteCount == 0)
                            isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }
    }
}