//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ELabel.QC
//{
//    class Junk
//    {
//    }
//}


//        private IEnumerable<Alerts> ProcessAlertDefinition(IEnumerable<Label> labelCollection, AlertDefinition alertDefinition)
//        {
//            //this method compares the values with the conditions from the alert definition

//            // testing
//            alertDefinition = new AlertDefinition
//            {
//                DefinitionName = "K Level",
//                compareField = "Ag_MEICP41s_ppm",
//                Active = true,
//                LastUpdated = DateTime.Now,
//                Minvalue = 55,
//                MaxValue = 59,
//                MineralPrefix = "Ag",
//                TolerancePercentage = 10,
//                Valueoperator = "FIND_BEST_METHOD",
//            };

//            // maintain a second list with only duplicates (for efficiency)
//            var duplicateCollection = labelCollection.Where(x => x.PCHECKID != "");

//            List<Alerts> alertList = new List<Alerts>();
//            List<Label> failedDuplicates = new List<Label>();

//            // track how many failures per testing method
//            // [0] is number of failed duplicates / [1] total number of records using that test
//            IDictionary<string, int[]> methodErrorFreq = new Dictionary<string, int[]>();

//            //todo add rules to compare actual with duplicate sample
//            foreach (var label in labelCollection)
//            {
//                // optimisation stuff
//                // try to get the property specified for comparison
//                var value = GetValueFromField(alertDefinition.compareField, label);
//                if (alertDefinition.Valueoperator.ToUpper() != "FIND_BEST_METHOD")
//                {
//                    if (value == null)
//                    {
//                        continue;
//                    }
//                }

//                if (alertDefinition.Valueoperator.ToUpper() == "OVER")
//                {
//                    if (value > alertDefinition.MaxValue)
//                    {
//                        alertList.Add(new Alerts
//                        {
//                            AlertDefinition = alertDefinition.DefinitionName,
//                            LabelID = label.SAMPLEID,
//                            Date = DateTime.Now,
//                            FieldName = alertDefinition.compareField,
//                            Description = "Maximum allowed value: " + alertDefinition.MaxValue + ", actual value: " + value
//                        });
//                    }
//                }
//                else if (alertDefinition.Valueoperator.ToUpper() == "UNDER")
//                {
//                    if (value < alertDefinition.Minvalue)
//                    {
//                        alertList.Add(new Alerts
//                        {
//                            AlertDefinition = alertDefinition.DefinitionName,
//                            LabelID = label.SAMPLEID,
//                            Date = DateTime.Now,
//                            FieldName = alertDefinition.compareField,
//                            Description = "Minimum allowed value: " + alertDefinition.Minvalue + ", actual value: " + value
//                        });
//                    }
//                }
//                else if (alertDefinition.Valueoperator.ToUpper() == "BETWEEN")
//                {
//                    if (value >= alertDefinition.Minvalue && value <= alertDefinition.MaxValue)
//                    {
//                        alertList.Add(new Alerts
//                        {
//                            AlertDefinition = alertDefinition.DefinitionName,
//                            LabelID = label.SAMPLEID,
//                            Date = DateTime.Now,
//                            FieldName = alertDefinition.compareField,
//                            Description = "Must be between " + alertDefinition.Minvalue + " and " + alertDefinition.MaxValue + ", actual value: " + value
//                        });
//                    }
//                }
//                else if (alertDefinition.Valueoperator.ToUpper() == "FIND_BEST_METHOD")
//                {
//                    var duplicate = duplicateCollection.FirstOrDefault<Label>(x => x.PCHECKID == label.SAMPLEID);

//                    if (duplicate != null)
//                    {
//                        // find the test that was used and use it as a basis for comparison
//                        var fieldName = FindTestUsed(alertDefinition.MineralPrefix, label);

//                        // try to get duplicate value for field
//                        var dupeVal = GetValueFromField(fieldName, duplicate);

//                        // create a new dictionary record if necessary
//                        if (!methodErrorFreq.Any(x => x.Key == fieldName))
//                        {
//                            methodErrorFreq[fieldName] = new int[] { 0, 0 };
//                        }

//                        // increment total duplicates tested with this method
//                        methodErrorFreq[fieldName][1]++;

//                        if (dupeVal != null)
//                        {
//                            // get bounds
//                            var upper = value * (1 + (alertDefinition.TolerancePercentage / 100));
//                            var lower = value * (1 - (alertDefinition.TolerancePercentage / 100));

//                            // check if duplicate is outside bounds
//                            if (dupeVal > upper || dupeVal < lower)
//                            {
//                                // increment frequency of errors for that method
//                                methodErrorFreq[fieldName][0]++;
//                            }
//                        }
//                    }
//                }
//                else if (alertDefinition.Valueoperator.ToUpper() == "COMPARE_TO_DUPLICATE")
//                {
//                    var duplicate = duplicateCollection.FirstOrDefault<Label>(x => x.PCHECKID == label.SAMPLEID);

//                    if (duplicate != null)
//                    {
//                        // try to get duplicate value for field
//                        var dupeVal = GetValueFromField(alertDefinition.compareField, duplicate);

//                        if (dupeVal != null)
//                        {
//                            // get bounds
//                            var upper = value * (1 + (alertDefinition.TolerancePercentage / 100));
//                            var lower = value * (1 - (alertDefinition.TolerancePercentage / 100));

//                            // check if duplicate is outside bounds
//                            if (dupeVal > upper || dupeVal < lower)
//                            {
//                                alertList.Add(new Alerts
//                                {
//                                    AlertDefinition = alertDefinition.DefinitionName,
//                                    LabelID = label.SAMPLEID,
//                                    Date = DateTime.Now,
//                                    FieldName = alertDefinition.compareField,
//                                    Description = "Value is outside " + alertDefinition.TolerancePercentage + "% tolerance range of its duplicate"
//                                });
//                            }
//                        }
//                    }
//                }
//            }

//            // final analysis to find the best method
//            if (alertDefinition.Valueoperator.ToUpper() == "FIND_BEST_METHOD")
//            {
//                var best = new KeyValuePair<string, int[]>();
//                foreach (var key in methodErrorFreq.Keys)
//                {
//                    if (best.Value == null || ((double)methodErrorFreq[key][0] / (double)methodErrorFreq[key][1]) < best.Value[0])
//                    {
//                        best = new KeyValuePair<string, int[]>(key, methodErrorFreq[key]);
//                    }
//                }

//                alertList.Add(new Alerts
//                {
//                    AlertDefinition = alertDefinition.DefinitionName,
//                    LabelID = "N/A",
//                    Date = DateTime.Now,
//                    FieldName = alertDefinition.MineralPrefix + "*",
//                    Description = "Best test method: " + best.Key + " with " + best.Value[0] + "/" + best.Value[1] + " failures (" + ((double)best.Value[0] / (double)best.Value[1]) + "% failure rate)"
//                });
//            }

//            return alertList;
//        }