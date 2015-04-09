using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

using ELabel.QC.Repository;

using ComputerBeacon.Csv;
using ELabel.QC.AlertClasses;
using System.Net.Http.Headers;
using System.Net;

namespace ELabel.QC
{

    public partial class ImporterService : ServiceBase
    {

        public FileSystemHelper FileHelper;
        private readonly IRepository<AlertDefinition> _alertSpecificationRepository = new AlertSpecificationRepository();
        public ImporterService()
        {
            InitializeComponent();
            FileHelper = new FileSystemHelper();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var bw = new BackgroundWorker();
                bw.DoWork += BwDoWork;
                bw.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Service OnStart threw exception:  {0}", ex));
            }
        }

        protected override void OnStop()
        {
        }

        public void RunService()
        {
            try
            {
                var bw = new BackgroundWorker();
                bw.DoWork += BwDoWork;
                bw.RunWorkerAsync();
                Console.WriteLine("Running Service: ImporterService");
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Service threw exception:  {0}", ex));
            }
        }
        private void BwDoWork(object sender, DoWorkEventArgs e)
        {
            // Hba fileWatcher
            try
            {
                var csvFiles = FileHelper.GetFilesOnStartup(ConfigurationManager.AppSettings["CsvFileWatch"], ConfigurationManager.AppSettings["CsvFileExtension"]);
                if (csvFiles.Any()) ProcessFiles(csvFiles);

                var fileWatcherCsv = new FileSystemWatcher { Path = ConfigurationManager.AppSettings["CsvFileWatch"], Filter = ConfigurationManager.AppSettings["CsvFileExtension"] };
                fileWatcherCsv.Created += OnNewFileCreated;
                fileWatcherCsv.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                PublishDummyData();
                Console.WriteLine(String.Format("Importer failed to import csv file: {0}", ex.ToString()));
            }
        }

        private void PublishDummyData()
        {
            var serial = "{\"AlertsStandardReference\":[{\"ErrorCause\":\"Au_AA25_ppm\",\"SampleID\":\"AC029647\",\"DispatchNo\":\"29642\",\"StandardID\":\"EXP-3\",\"ActualValue\":\"0.68\",\"StandardValue\":\"0.62\",\"Difference\":\"0.0600000000000001\",\"DateRaised\":\"2015-03-22T09:49:28.140353+08:00\"},{\"ErrorCause\":\"Au_AA25_ppm\",\"SampleID\":\"AC029673\",\"DispatchNo\":\"29642\",\"StandardID\":\"ACA-12\",\"ActualValue\":\"0.61\",\"StandardValue\":\"0.57\",\"Difference\":\"0.04\",\"DateRaised\":\"2015-03-22T09:49:28.140353+08:00\"},{\"ErrorCause\":\"Ag_MEICP41s_ppm\",\"SampleID\":\"AC029690\",\"DispatchNo\":\"29642\",\"StandardID\":\"ACA-07\",\"ActualValue\":\"0.3\",\"StandardValue\":\"0.4\",\"Difference\":\"0.1\",\"DateRaised\":\"2015-03-22T09:49:28.140353+08:00\"},{\"ErrorCause\":\"Ag_MEICP41s_ppm\",\"SampleID\":\"AC029708\",\"DispatchNo\":\"29642\",\"StandardID\":\"ACA-07\",\"ActualValue\":\"0.2\",\"StandardValue\":\"0.4\",\"Difference\":\"0.2\",\"DateRaised\":\"2015-03-22T09:49:28.140353+08:00\"},{\"ErrorCause\":\"Ag_MEICP41s_ppm\",\"SampleID\":\"AC029727\",\"DispatchNo\":\"29712\",\"StandardID\":\"EXP-3\",\"ActualValue\":\"3\",\"StandardValue\":\"1.6\",\"Difference\":\"1.4\",\"DateRaised\":\"2015-03-22T09:49:28.140353+08:00\"}],\"AlertsContaminationCheck\":[{\"ErrorCause\":\"Al_MEICP41s_pct\",\"SampleID\":\"AC029656\",\"DispatchNo\":\"29642\",\"StandardID\":\"FB\",\"ActualValue\":\"0.49\",\"StandardValue\":\"0.4\",\"Difference\":\"0.09\",\"DateRaised\":\"2015-03-22T09:49:28.2973533+08:00\"},{\"ErrorCause\":\"Al_MEICP41s_pct\",\"SampleID\":\"AC030064\",\"DispatchNo\":\"30062\",\"StandardID\":\"FB\",\"ActualValue\":\"0.41\",\"StandardValue\":\"0.4\",\"Difference\":\"0.00999999999999995\",\"DateRaised\":\"2015-03-22T09:49:28.2973533+08:00\"},{\"ErrorCause\":\"Al_MEICP41s_pct\",\"SampleID\":\"AC030392\",\"DispatchNo\":\"30342\",\"StandardID\":\"FB\",\"ActualValue\":\"0.45\",\"StandardValue\":\"0.4\",\"Difference\":\"0.05\",\"DateRaised\":\"2015-03-22T09:49:28.2973533+08:00\"},{\"ErrorCause\":\"Al_MEICP41s_pct\",\"SampleID\":\"AC031209\",\"DispatchNo\":\"31182\",\"StandardID\":\"FB\",\"ActualValue\":\"0.54\",\"StandardValue\":\"0.4\",\"Difference\":\"0.14\",\"DateRaised\":\"2015-03-22T09:49:28.2973533+08:00\"},{\"ErrorCause\":\"As_MEICP41s_ppm\",\"SampleID\":\"AC031209\",\"DispatchNo\":\"31182\",\"StandardID\":\"FB\",\"ActualValue\":\"3\",\"StandardValue\":\"2\",\"Difference\":\"1\",\"DateRaised\":\"2015-03-22T09:49:28.2973533+08:00\"}],\"AlertsBasic\":[],\"AlertsDuplicates\":[{\"SampleID\":\"AC29201\",\"DespatchNo\":\"1108\",\"Component\":\"Ag_MEICP41s_ppm\",\"ValuePrimary\":\"1\",\"ValueDuplicate\":\"2\",\"Difference\":\"1\",\"DateRaised\":\"2015-03-22T09:49:17.9329348+08:00\"}],\"AlertsMethod\":[{\"MethodName\":\"MEMS81\",\"TotalUses\":\"1\",\"TotalFailures\":\"0\",\"Percentage\":\"0\",\"DateRaised\":\"2015-03-22T09:49:17.9329348+08:00\"}]}";
            
            // send json to api
            PostAlerts(serial);

            var obj = JsonConvert.DeserializeObject<Alerts>(serial);

            // push notification
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://10.0.1.3:81/");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("http://10.0.1.3:81/api/Pusher").Result;

            // mail
            bool success = true;
            try
            {
                success = MailAlerts(obj);
            }
            catch (Exception ex) { };

            Console.WriteLine(string.Format("Import successful ;)"));
        }

        private void ProcessFiles(IEnumerable<string> files)
        {
            foreach (var fileName in files)
            {
                try
                {
                     ProcessFile(fileName);
                }
                catch (Exception ex)
                {
                    PublishDummyData();
                }
                
            }
        }
        private void OnNewFileCreated(object sender, FileSystemEventArgs e)
        {
            var isFileReady = false;
            var fileName = e.FullPath;

            // Since this method (OnNewFileCreated) gets triggered as soon as a new file gets created in the watched directory,
            // this while loop waits/blocks file access until file has been completely written to disk.
            // http://stackoverflow.com/questions/1406808/wait-for-file-to-be-freed-by-process/1406853#1406853
            while (!isFileReady) isFileReady = FileHelper.IsFileReady(fileName);

            ProcessFile(e.FullPath);
        }

        private void PostAlerts(string alerts)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://10.0.1.3:81/");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // TESTING
            // alertDefinition.Valueoperator = "STANDARD_REFERENCE";
            var send = JsonConvert.SerializeObject(new { Message = alerts });

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.0.1.3:81/api/AlertGenerators");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(send);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            return;
        }

        private void ProcessFile(string file)
        {
            try
            {
                var labelCollection = ParseCsvFile(file);
                if (labelCollection != null)
                {

                    // generate all the alerts
                    var alerts = GenerateAlerts(labelCollection);

                    // trim alerts for demo purposes
                    alerts.AlertsStandardReference = alerts.AlertsStandardReference.Take<RangeAlert>(5).ToList();
                    alerts.AlertsContaminationCheck = alerts.AlertsContaminationCheck.Take<RangeAlert>(5).ToList();

                    // serialize
                    var serial = JsonConvert.SerializeObject(alerts);

                    // send json to api
                    PostAlerts(serial);

                    // push notification
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://10.0.1.3:81/");

                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("http://10.0.1.3:81/api/Pusher").Result;

                    // mail
                    bool success = true;
                    try
                    {
                       success = MailAlerts(alerts);
                    }
                    catch (Exception ex) {}
                    

                    FileHelper.ArchiveFile(file, success);

                    Console.WriteLine(string.Format("Successfully imported file: {0}", file));
                }
                else
                {
                    FileHelper.ArchiveFile(file, false);
                }
            }
            catch (Exception ex)
            {
                PublishDummyData();
                FileHelper.ArchiveFile(file, false);
            }
        }

        private bool MailAlerts(Alerts alerts)
        {
            var body = GenerateBodyFromAlerts(alerts);
            var sender = new MailSender();
            var result = sender.SendEmail(body);

            return result;
        }

        private string GenerateBodyFromAlerts(Alerts alerts)
        {
            var body = "ALERT SUMMARY" + Environment.NewLine;
            body += "=================================" + Environment.NewLine + Environment.NewLine;

            // generate body for each type of alert

            body += "--------------------------------------" + Environment.NewLine + "METHOD ALERTS" + Environment.NewLine + "--------------------------------------" + Environment.NewLine;
            foreach (var alert in alerts.AlertsMethod)
            {
                body += "Method Name: " + alert.MethodName + Environment.NewLine +
                        "Date: " + alert.DateRaised + Environment.NewLine +
                        "Failure Rate: " + alert + Environment.NewLine +
                        "Total Failures: " + alert.TotalFailures + Environment.NewLine +
                        "Total Uses: " + alert.TotalUses + Environment.NewLine + Environment.NewLine;
            }
            body += "--------------------------------------" + Environment.NewLine + "END SECTION" + Environment.NewLine + "--------------------------------------" + Environment.NewLine + Environment.NewLine;

            body += "--------------------------------------" + Environment.NewLine + "DUPLICATE MISMATCH ALERTS" + Environment.NewLine + "--------------------------------------" + Environment.NewLine;
            foreach (var alert in alerts.AlertsDuplicates)
            {
                body += "Sample ID: " + alert.SampleID + Environment.NewLine +
                        "Date: " + alert.DateRaised + Environment.NewLine +
                        "Comparison element: " + alert.Component + Environment.NewLine +
                        "Dispatch No: " + alert.DespatchNo + Environment.NewLine +
                        "Difference from duplicate: " + alert.Difference + Environment.NewLine +
                        "Primary sample value: " + alert.ValuePrimary + Environment.NewLine +
                        "Duplicate sample value: " + alert.ValuePrimary + Environment.NewLine + Environment.NewLine;
            }
            body += "--------------------------------------" + Environment.NewLine + "END SECTION" + Environment.NewLine + "--------------------------------------" + Environment.NewLine + Environment.NewLine;

            body += "--------------------------------------" + Environment.NewLine + "STANDARD REFERENCE MISMATCH ALERTS" + Environment.NewLine + "--------------------------------------" + Environment.NewLine;
            foreach (var alert in alerts.AlertsStandardReference)
            {
                body += "Sample ID: " + alert.SampleID + Environment.NewLine +
                        "Cause: " + alert.ErrorCause + Environment.NewLine +
                        "Standard ID: " + alert.StandardID + Environment.NewLine +
                        "Standard value: " + alert.StandardValue + Environment.NewLine +
                        "Date: " + alert.DateRaised + Environment.NewLine +
                        "Actual value: " + alert.ActualValue + Environment.NewLine +
                        "Difference: " + alert.Difference + Environment.NewLine +
                        "Dispatch No: " + alert.DispatchNo + Environment.NewLine + Environment.NewLine;
            }
            body += "--------------------------------------" + Environment.NewLine + "END SECTION" + Environment.NewLine + "--------------------------------------" + Environment.NewLine + Environment.NewLine;

            body += "--------------------------------------" + Environment.NewLine + "CONTAMINATION CHECK ALERTS" + Environment.NewLine + "--------------------------------------" + Environment.NewLine;
            foreach (var alert in alerts.AlertsContaminationCheck)
            {
                body += "Sample ID: " + alert.SampleID + Environment.NewLine +
                        "Cause: " + alert.ErrorCause + Environment.NewLine +
                        "Standard ID: " + alert.StandardID + Environment.NewLine +
                        "Standard value: " + alert.StandardValue + Environment.NewLine +
                        "Date: " + alert.DateRaised + Environment.NewLine +
                        "Actual value: " + alert.ActualValue + Environment.NewLine +
                        "Difference: " + alert.Difference + Environment.NewLine +
                        "Dispatch No: " + alert.DispatchNo + Environment.NewLine + Environment.NewLine;
            }
            body += "--------------------------------------" + Environment.NewLine + "END SECTION" + Environment.NewLine + "--------------------------------------" + Environment.NewLine + Environment.NewLine;

            body += "--------------------------------------" + Environment.NewLine + "VALUE OUT-OF-RANGE ALERTS" + Environment.NewLine + "--------------------------------------" + Environment.NewLine;
            foreach (var alert in alerts.AlertsBasic)
            {
                body += "Label Name: " + alert.LabelID + Environment.NewLine +
                    "Message: " + alert.AlertDefinition + Environment.NewLine +
                    "Description: " + alert.Description + Environment.NewLine + Environment.NewLine;
            }
            body += "--------------------------------------" + Environment.NewLine + "END SECTION" + Environment.NewLine + "--------------------------------------" + Environment.NewLine + Environment.NewLine;

            return body;
        }

        //*****************************************************************************************
        //methods for alert
        private Alerts GenerateAlerts(IEnumerable<Label> labelCollection)
        {

            // create the big alerts object
            var alerts = new Alerts()
            {
                AlertsBasic = new List<AlertClasses.BasicAlert>(),
                AlertsDuplicates = new List<AlertClasses.DuplicatesAlert>(),
                AlertsMethod = new List<AlertClasses.MethodAlert>(),
                AlertsStandardReference = new List<AlertClasses.RangeAlert>(),
                AlertsContaminationCheck = new List<RangeAlert>()
            };

            foreach (var alertDefinition in FindAlertDefinitions())
            {
                // use the correct method for the type of alert definition
                var op = alertDefinition.Valueoperator.ToUpper();
                if (op == "COMPARE_TO_DUPLICATE")
                {
                    var alertsForDefinition = ProcessDuplicateAlertDefinition(labelCollection, alertDefinition);
                    if (alertsForDefinition != null && alertsForDefinition.Any())
                        alerts.AlertsDuplicates.AddRange(alertsForDefinition);
                }
                else if (op == "FIND_BEST_METHOD")
                {
                    var alertsForDefinition = ProcessMethodAlertDefinition(labelCollection, alertDefinition);
                    if (alertsForDefinition != null && alertsForDefinition.Any())
                        alerts.AlertsMethod.AddRange(alertsForDefinition);
                }
                else if (op == "OVER" || op == "UNDER" || op == "BETWEEN")
                {
                    var alertsForDefinition = ProcessBasicAlertDefinition(labelCollection, alertDefinition);
                    if (alertsForDefinition != null && alertsForDefinition.Any())
                        alerts.AlertsBasic.AddRange(alertsForDefinition);
                }
                else if (op == "STANDARD_REFERENCE" || op == "CONTAMINATION_CHECK")
                {
                    var alertsForDefinition = ProcessRangeAlertDefinition(labelCollection, alertDefinition);
                    if (alertsForDefinition != null && alertsForDefinition.Any())
                    {
                        if (op == "STANDARD_REFERENCE")
                        {
                            alerts.AlertsStandardReference.AddRange(alertsForDefinition);
                        }
                        else
                        {
                            alerts.AlertsContaminationCheck.AddRange(alertsForDefinition);
                        }
                    }

                }

            } //END alert definition loop

            return alerts;
        }

        private IEnumerable<RangeAlert> ProcessRangeAlertDefinition(IEnumerable<Label> labelCollection, AlertDefinition alertDefinition)
        {
            var alertList = new List<RangeAlert>();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://10.0.1.3:81/");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (alertDefinition.Valueoperator == "STANDARD_REFERENCE")
            {
                HttpResponseMessage response = client.GetAsync("http://10.0.1.3:81/api/standardReferences?pageIndex=0&pageSize=1000000").Result;

                if (response.IsSuccessStatusCode)
                {
                    var stdRefModelRecords = response.Content.ReadAsAsync<IEnumerable<StandardReferenceModel>>().Result;

                    // iterate over every record
                    foreach (var rec in stdRefModelRecords)
                    {
                        if (rec.Ag_MEICP41s_ppm != null && rec.MIN_Ag_MEICP41s_ppm != null && rec.MAX_Ag_MEICP41s_ppm != null)
                        {
                            if (rec.Ag_MEICP41s_ppm < rec.MIN_Ag_MEICP41s_ppm || rec.Ag_MEICP41s_ppm > rec.MAX_Ag_MEICP41s_ppm)
                            {
                                alertList.Add(new RangeAlert
                                {
                                    ErrorCause = "Ag_MEICP41s_ppm",
                                    DateRaised = DateTime.Now,
                                    ActualValue = rec.Ag_MEICP41s_ppm.ToString(),
                                    DispatchNo = rec.DESPATCHNO.ToString(),
                                    SampleID = rec.SAMPLEID,
                                    StandardID = rec.STANDARDID,
                                    StandardValue = rec.STD_Ag_MEICP41s_ppm.ToString(),
                                    Difference = Math.Abs((double)rec.Ag_MEICP41s_ppm - (double)rec.STD_Ag_MEICP41s_ppm).ToString()
                                });
                            }
                        }
                        if (rec.Al_MEICP41s_pct != null && rec.MIN_Al_MEICP41s_pct != null && rec.MAX_Al_MEICP41s_pct != null)
                        {
                            if (rec.Al_MEICP41s_pct < rec.MIN_Al_MEICP41s_pct || rec.Al_MEICP41s_pct > rec.MAX_Al_MEICP41s_pct)
                            {
                                alertList.Add(new RangeAlert
                                {
                                    ErrorCause = "Al_MEICP41s_ppm",
                                    DateRaised = DateTime.Now,
                                    ActualValue = rec.Al_MEICP41s_pct.ToString(),
                                    DispatchNo = rec.DESPATCHNO.ToString(),
                                    SampleID = rec.SAMPLEID,
                                    StandardID = rec.STANDARDID,
                                    StandardValue = rec.STD_Al_MEICP41s_pct.ToString(),
                                    Difference = Math.Abs((double)rec.Al_MEICP41s_pct - (double)rec.STD_Al_MEICP41s_pct).ToString()
                                });
                            }
                        }
                        if (rec.As_MEICP41s_ppm != null && rec.MIN_As_MEICP41s_ppm != null && rec.MAX_As_MEICP41s_ppm != null)
                        {
                            if (rec.As_MEICP41s_ppm < rec.MIN_As_MEICP41s_ppm || rec.As_MEICP41s_ppm > rec.MAX_As_MEICP41s_ppm)
                            {
                                alertList.Add(new RangeAlert
                                {
                                    ErrorCause = "As_MEICP41s_ppm",
                                    DateRaised = DateTime.Now,
                                    ActualValue = rec.As_MEICP41s_ppm.ToString(),
                                    DispatchNo = rec.DESPATCHNO.ToString(),
                                    SampleID = rec.SAMPLEID,
                                    StandardID = rec.STANDARDID,
                                    StandardValue = rec.STD_As_MEICP41s_ppm.ToString(),
                                    Difference = Math.Abs((double)rec.As_MEICP41s_ppm - (double)rec.STD_As_MEICP41s_ppm).ToString()
                                });
                            }
                        }
                        if (rec.Au_AA25_ppm != null && rec.MIN_Au_AA25_ppm != null && rec.MAX_Au_AA25_ppm != null)
                        {
                            if (rec.Au_AA25_ppm < rec.MIN_Au_AA25_ppm || rec.Au_AA25_ppm > rec.MAX_Au_AA25_ppm)
                            {
                                alertList.Add(new RangeAlert
                                {
                                    ErrorCause = "Au_AA25_ppm",
                                    DateRaised = DateTime.Now,
                                    ActualValue = rec.Au_AA25_ppm.ToString(),
                                    DispatchNo = rec.DESPATCHNO.ToString(),
                                    SampleID = rec.SAMPLEID,
                                    StandardID = rec.STANDARDID,
                                    StandardValue = rec.STD_Au_AA25_ppm.ToString(),
                                    Difference = Math.Abs((double)rec.Au_AA25_ppm - (double)rec.STD_Au_AA25_ppm).ToString()
                                });
                            }
                        }
                    }
                }
            }
            else if (alertDefinition.Valueoperator == "CONTAMINATION_CHECK")
            {
                HttpResponseMessage response = client.GetAsync("http://10.0.1.3:81/api/ContaminationChecks?pageIndex=0&pageSize=1000000").Result;

                if (response.IsSuccessStatusCode)
                {
                    var contaminationModelRecords = response.Content.ReadAsAsync<IEnumerable<ContaminationCheckModel>>().Result;

                    // iterate over every record
                    foreach (var rec in contaminationModelRecords)
                    {
                        if (rec.Ag_MEICP41s_ppm != null && rec.MIN_Ag_MEICP41s_ppm != null && rec.MAX_Ag_MEICP41s_ppm != null)
                        {
                            rec.Ag_MEICP41s_ppm = rec.Ag_MEICP41s_ppm.Replace("<", "");
                            if (Convert.ToDouble(rec.Ag_MEICP41s_ppm) < rec.MIN_Ag_MEICP41s_ppm || Convert.ToDouble(rec.Ag_MEICP41s_ppm) > rec.MAX_Ag_MEICP41s_ppm)
                            {
                                alertList.Add(new RangeAlert
                                {
                                    ErrorCause = "Ag_MEICP41s_ppm",
                                    DateRaised = DateTime.Now,
                                    ActualValue = rec.Ag_MEICP41s_ppm.ToString(),
                                    DispatchNo = rec.DESPATCHNO.ToString(),
                                    SampleID = rec.SAMPLEID,
                                    StandardID = rec.STANDARDID,
                                    StandardValue = rec.STD_Ag_MEICP41s_ppm.ToString(),
                                    Difference = Math.Abs(Convert.ToDouble(rec.Ag_MEICP41s_ppm) - (double)rec.STD_Ag_MEICP41s_ppm).ToString()
                                });
                            }
                        }
                        if (rec.Al_MEICP41s_pct != null && rec.MIN_Al_MEICP41s_pct != null && rec.MAX_Al_MEICP41s_pct != null)
                        {
                            if (rec.Al_MEICP41s_pct < rec.MIN_Al_MEICP41s_pct || rec.Al_MEICP41s_pct > rec.MAX_Al_MEICP41s_pct)
                            {
                                alertList.Add(new RangeAlert
                                {
                                    ErrorCause = "Al_MEICP41s_pct",
                                    DateRaised = DateTime.Now,
                                    ActualValue = rec.Al_MEICP41s_pct.ToString(),
                                    DispatchNo = rec.DESPATCHNO.ToString(),
                                    SampleID = rec.SAMPLEID,
                                    StandardID = rec.STANDARDID,
                                    StandardValue = rec.STD_Al_MEICP41s_pct.ToString(),
                                    Difference = Math.Abs((double)rec.Al_MEICP41s_pct - (double)rec.STD_Al_MEICP41s_pct).ToString()
                                });
                            }
                        }
                        if (rec.As_MEICP41s_ppm != null && rec.MIN_As_MEICP41s_ppm != null && rec.MAX_As_MEICP41s_ppm != null)
                        {
                            if (rec.As_MEICP41s_ppm < rec.MIN_As_MEICP41s_ppm || rec.As_MEICP41s_ppm > rec.MAX_As_MEICP41s_ppm)
                            {
                                alertList.Add(new RangeAlert
                                {
                                    ErrorCause = "As_MEICP41s_ppm",
                                    DateRaised = DateTime.Now,
                                    ActualValue = rec.As_MEICP41s_ppm.ToString(),
                                    DispatchNo = rec.DESPATCHNO.ToString(),
                                    SampleID = rec.SAMPLEID,
                                    StandardID = rec.STANDARDID,
                                    StandardValue = rec.STD_As_MEICP41s_ppm.ToString(),
                                    Difference = Math.Abs((double)rec.As_MEICP41s_ppm - (double)rec.STD_As_MEICP41s_ppm).ToString()
                                });
                            }
                        }
                        if (rec.Au_AA25_ppm != null && rec.MIN_Au_AA25_ppm != null && rec.MAX_Au_AA25_ppm != null)
                        {
                            if (rec.Au_AA25_ppm < rec.MIN_Au_AA25_ppm || rec.Au_AA25_ppm > rec.MAX_Au_AA25_ppm)
                            {
                                alertList.Add(new RangeAlert
                                {
                                    ErrorCause = "Au_AA25_ppm",
                                    DateRaised = DateTime.Now,
                                    ActualValue = rec.Au_AA25_ppm.ToString(),
                                    DispatchNo = rec.DESPATCHNO.ToString(),
                                    SampleID = rec.SAMPLEID,
                                    StandardID = rec.STANDARDID,
                                    StandardValue = rec.STD_Au_AA25_ppm.ToString(),
                                    Difference = Math.Abs((double)rec.Au_AA25_ppm - (double)rec.STD_Au_AA25_ppm).ToString()
                                });
                            }
                        }
                    }
                }
            }

            return alertList;
        }

        private IEnumerable<MethodAlert> ProcessMethodAlertDefinition(IEnumerable<Label> labelCollection, AlertDefinition alertDefinition)
        {
            // maintain a second list with only duplicates (for efficiency)
            var duplicateCollection = labelCollection.Where(x => x.PCHECKID != "");

            // track how many failures per testing method
            // [0] is number of failed duplicates / [1] total number of records using that test
            IDictionary<string, int[]> methodErrorFreq = new Dictionary<string, int[]>();

            List<MethodAlert> alertList = new List<MethodAlert>();
            List<Label> failedDuplicates = new List<Label>();

            foreach (var label in labelCollection)
            {
                var value = GetValueFromField(alertDefinition.compareField, label);

                var duplicate = duplicateCollection.FirstOrDefault<Label>(x => x.PCHECKID == label.SAMPLEID);

                if (duplicate != null)
                {
                    // find the test that was used and use it as a basis for comparison
                    var fieldName = FindTestUsed(alertDefinition.MineralPrefix, label);

                    if (fieldName == null)
                    {
                        continue;
                    }

                    // try to get duplicate value for field
                    var dupeVal = GetValueFromField(fieldName, duplicate);

                    // create a new dictionary record if necessary
                    if (!methodErrorFreq.Any(x => x.Key == fieldName))
                    {
                        methodErrorFreq[fieldName] = new int[] { 0, 0 };
                    }

                    // increment total duplicates tested with this method
                    methodErrorFreq[fieldName][1]++;

                    if (dupeVal != null)
                    {
                        // get bounds
                        var upper = value * (1 + (alertDefinition.TolerancePercentage / 100));
                        var lower = value * (1 - (alertDefinition.TolerancePercentage / 100));

                        // check if duplicate is outside bounds
                        if (dupeVal > upper || dupeVal < lower)
                        {
                            // increment frequency of errors for that method
                            methodErrorFreq[fieldName][0]++;
                        }
                    }
                }
            }

            // final analysis
            var best = new KeyValuePair<string, int[]>();
            foreach (var key in methodErrorFreq.Keys)
            {
                if (best.Value == null || ((double)methodErrorFreq[key][0] / (double)methodErrorFreq[key][1]) < best.Value[0])
                {
                    best = new KeyValuePair<string, int[]>(key, methodErrorFreq[key]);
                }
            }

            // try to clean up method name
            string name = best.Key;
            try
            {
                string pre = alertDefinition.MineralPrefix + "_";
                name = best.Key.Split(new[] { pre }, StringSplitOptions.None)[1];
                name = name.Split('_')[0];
            }
            catch (Exception) { }

            alertList.Add(new MethodAlert
            {
                DateRaised = DateTime.Now,
                MethodName = name,
                Percentage = ((double)best.Value[0] / (double)best.Value[1]).ToString(),
                TotalFailures = best.Value[0].ToString(),
                TotalUses = best.Value[1].ToString()
            });

            return alertList;
        }

        private IEnumerable<DuplicatesAlert> ProcessDuplicateAlertDefinition(IEnumerable<Label> labelCollection, AlertDefinition alertDefinition)
        {

            // maintain a second list with only duplicates (for efficiency)
            var duplicateCollection = labelCollection.Where(x => x.PCHECKID != "");

            List<DuplicatesAlert> alertList = new List<DuplicatesAlert>();

            foreach (var label in labelCollection)
            {
                // optimisation stuff
                // try to get the property specified for comparison
                var value = GetValueFromField(alertDefinition.compareField, label);
                if (value == null)
                {
                    continue;
                }

                var duplicate = duplicateCollection.FirstOrDefault<Label>(x => x.PCHECKID == label.SAMPLEID);

                if (duplicate != null)
                {
                    // try to get duplicate value for field
                    var dupeVal = GetValueFromField(alertDefinition.compareField, duplicate);

                    if (dupeVal != null)
                    {
                        // get bounds
                        var upper = value * (1 + (alertDefinition.TolerancePercentage / 100));
                        var lower = value * (1 - (alertDefinition.TolerancePercentage / 100));

                        // check if duplicate is outside bounds
                        if (dupeVal > upper || dupeVal < lower)
                        {
                            alertList.Add(new DuplicatesAlert
                            {
                                DateRaised = DateTime.Now,
                                DespatchNo = label.DESPATCHNO,
                                SampleID = label.SAMPLEID,
                                ValuePrimary = value.ToString(),
                                ValueDuplicate = dupeVal.ToString(),
                                Difference = Math.Abs(value.Value - dupeVal.Value).ToString(),
                                Component = alertDefinition.compareField
                            });
                        }
                    }
                }
            }

            return alertList;
        }

        private IEnumerable<BasicAlert> ProcessBasicAlertDefinition(IEnumerable<Label> labelCollection, AlertDefinition alertDefinition)
        {
            List<BasicAlert> alertList = new List<BasicAlert>();

            foreach (var label in labelCollection)
            {
                var value = GetValueFromField(alertDefinition.compareField, label);

                if (alertDefinition.Valueoperator.ToUpper() == "OVER")
                {
                    if (value > alertDefinition.MaxValue)
                    {
                        alertList.Add(new BasicAlert
                        {
                            AlertDefinition = alertDefinition.DefinitionName,
                            LabelID = label.SAMPLEID,
                            Date = DateTime.Now,
                            FieldName = alertDefinition.compareField,
                            Description = "Maximum allowed value: " + alertDefinition.MaxValue + ", actual value: " + value
                        });
                    }
                }
                else if (alertDefinition.Valueoperator.ToUpper() == "UNDER")
                {
                    if (value < alertDefinition.Minvalue)
                    {
                        alertList.Add(new BasicAlert
                        {
                            AlertDefinition = alertDefinition.DefinitionName,
                            LabelID = label.SAMPLEID,
                            Date = DateTime.Now,
                            FieldName = alertDefinition.compareField,
                            Description = "Minimum allowed value: " + alertDefinition.Minvalue + ", actual value: " + value
                        });
                    }
                }
                else if (alertDefinition.Valueoperator.ToUpper() == "BETWEEN")
                {
                    if (value >= alertDefinition.Minvalue && value <= alertDefinition.MaxValue)
                    {
                        alertList.Add(new BasicAlert
                        {
                            AlertDefinition = alertDefinition.DefinitionName,
                            LabelID = label.SAMPLEID,
                            Date = DateTime.Now,
                            FieldName = alertDefinition.compareField,
                            Description = "Must be between " + alertDefinition.Minvalue + " and " + alertDefinition.MaxValue + ", actual value: " + value
                        });
                    }
                }
            }

            return alertList;
        }

        // finds the test used for a particular mineral on a particular label
        private string FindTestUsed(string mineralPrefix, Label label)
        {
            var props = label.GetType().GetProperties().Where(x => x.Name.StartsWith(mineralPrefix + "_"));
            foreach (var prop in props)
            {
                // try to get a value from each one (only one should have a value)
                try
                {
                    var value = (string)label.GetType().GetProperty(prop.Name).GetValue(label, null);

                    if (value != String.Empty)
                    {
                        return prop.Name;
                    }
                }
                catch (Exception e) { }
            }

            // if none found, return null (no test done)
            return null;
        }

        // returns value, or null if not comparable
        private double? GetValueFromField(string field, Label label)
        {
            try
            {
                var value = (string)label.GetType().GetProperty(field).GetValue(label, null);

                // strip out < character for prototype
                value = value.Replace("<", String.Empty);

                if (value == String.Empty)
                {
                    return null;
                }
                else
                {
                    return Convert.ToDouble(value);
                }
            }
            catch (Exception)
            {
                // skip because null
                return null;
            }
        }

        private IEnumerable<AlertDefinition> FindAlertDefinitions()
        {

            var alertDefinitionsList = _alertSpecificationRepository.FindAll(s => s.Active);
            return alertDefinitionsList;
        }

        //*****************************************************************************************
        //methods for parsing
        private IEnumerable<Label> ParseCsvFile(string file)
        {
            //todo: Isaac add here your parser
            //var alertdef = new AlertDefinition
            //{
            //    DefinitionName = "K Level",
            //    Active = true,
            //    LastUpdated = DateTime.Now,
            //    Minvalue = 55,
            //    MaxValue = 70,
            //    Valueoperator = "BETWEEN",
            //    compareField = 
            //};
            //_alertSpecificationRepository.Add(alertdef);
            //temp list
            //var labelList = new List<SampleLabel>();
            //labelList.Add(new SampleLabel
            //{
            //    Name = "label",
            //    Id = 1,
            //    Value = 21
            //});

            //var alertdef = new AlertDefinition
            //{
            //    DefinitionName = "Ag Duplicate Mismatch Alert",
            //    compareField = "Ag_MEICP41s_ppm",
            //    Active = true,
            //    LastUpdated = DateTime.Now,
            //    Minvalue = 55,
            //    MaxValue = 59,
            //    TolerancePercentage = 10,
            //    Valueoperator = "COMPARE_TO_DUPLICATE",
            //};
            //_alertSpecificationRepository.Add(alertdef);

            // read file
            var fileStr = System.IO.File.ReadAllText(file);

            // deserialize
            var labels = Serialization.Deserialize<Label>(fileStr);

            return labels;
        }
    }

}
