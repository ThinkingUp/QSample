using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ELabel.QC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
#if !DEBUG
            var servicesToRun = new ServiceBase[]
            {
                new HbwdImporterService()
            };
            ServiceBase.Run(servicesToRun);
#else
            // Debug code: this allows the process to run as a non-service.
            // It will kick off the service start point, but never kill it.
            // Shut down the debugger to exit
            var svc = new ImporterService();
            svc.RunService();
            // Put a breakpoint on the following line to always catch
            // your service when it has finished its work
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#endif
        }
    }
}
