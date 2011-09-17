using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Raven.Database;
using Raven.Database.Server;

namespace Storage
{
    public class WorkerRole : RoleEntryPoint
    {
        // The Endpoint that is exposed to the Internet via the load balancer
        IPEndPoint _endPoint;

        private CloudDrive _ravenDataDrive;
        private string _ravenDrivePath;
        private HttpServer _ravenHttpServer;
        private DocumentDatabase _documentDatabase;

        public override void Run()
        {
            Trace.WriteLine("RavenRole entry point called", "Information");

            while (true)
            {
                Thread.Sleep(60000);
                Log.Info("I'm still standing, yeah, yeah, yeah.");
            }
        }

        public override bool OnStart()
        {

            RoleEnvironment.Changing += RoleEnvironmentChanging;

            DiagnosticMonitorConfiguration dmc = DiagnosticMonitor.GetDefaultInitialConfiguration();

            dmc.Logs.ScheduledTransferPeriod = TimeSpan.FromSeconds(10);
            dmc.Logs.ScheduledTransferLogLevelFilter = LogLevel.Undefined;

            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", dmc);

            Log.Info("Worker Role OnStart Entered");

            try
            {
                _endPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Raven"].IPEndpoint;

                MountCloudDrive();
                StartTheServer();

                Log.Info("Worker Role OnStart Exited");
            }
            catch (Exception ex)
            {
                Log.Error("Worker Role OnStart error: " + ex.Message);
            }

            return base.OnStart();
        }

        public override void OnStop()
        {
            _ravenHttpServer.StopTcp();

            if (_ravenDataDrive != null)
                _ravenDataDrive.Unmount();

            base.OnStop();
        }

        private void MountCloudDrive()
        {
            LocalResource localCache;
            try
            {
                Log.Info("Mounting CloudDrive...");
                localCache = RoleEnvironment.GetLocalResource("RavenCache");
                Log.Info("Local cache: RootPath = {0}, Size = {1}", localCache.RootPath, localCache.MaximumSizeInMegabytes);

                CloudDrive.InitializeCache(localCache.RootPath.TrimEnd('\\'), localCache.MaximumSizeInMegabytes);
                Log.Info("Local cache initialized.");

                //var ravenDataStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
                var ravenDataStorageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageAccount"));
                var blobClient = ravenDataStorageAccount.CreateCloudBlobClient();
                var ravenDrives = blobClient.GetContainerReference("ravendrives");
                ravenDrives.CreateIfNotExist();
                var vhdUrl =
                    blobClient.GetContainerReference("ravendrives").GetPageBlobReference("RavenData.vhd").Uri.ToString();

                Log.Info("CloudDrive Blob URL: {0}", vhdUrl);

                BlobContainerPermissions permissions = ravenDrives.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                ravenDrives.SetPermissions(permissions);

                Log.Info("Permissions set");

                _ravenDataDrive = ravenDataStorageAccount.CreateCloudDrive(vhdUrl);
            }
            catch (Exception ex)
            {
                Log.Error("{0}: {1}", ex.GetType().Name, ex.Message);
                throw;
            }

            try
            {
                _ravenDataDrive.Create(localCache.MaximumSizeInMegabytes);
                Log.Info("CloudDrive instance created");
            }
            catch (CloudDriveException ex)
            {
                Log.Error("ravenDataDrive.Create threw exception: " + ex.Message);
            }

            _ravenDrivePath = _ravenDataDrive.Mount(localCache.MaximumSizeInMegabytes, DriveMountOptions.Force);

            Log.Info("Drive mounted as {0}", _ravenDrivePath);

            if (!Directory.Exists(Path.Combine(_ravenDrivePath, "Raven")))
            {
                Directory.CreateDirectory(Path.Combine(_ravenDrivePath, "Raven"));
            }
        }

        private static void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        private void StartTheServer()
        {
            try
            {
                var ravenConfiguration = new RavenConfiguration
                {
                    AnonymousUserAccessMode = AnonymousUserAccessMode.All,
                    Port = _endPoint.Port,
                    ListenerProtocol = ListenerProtocol.Tcp,
                    DataDirectory = _ravenDrivePath
                };

                _documentDatabase = new DocumentDatabase(ravenConfiguration);
                _documentDatabase.SpinBackgroundWorkers();

                _ravenHttpServer = new HttpServer(ravenConfiguration, _documentDatabase);
                _ravenHttpServer.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
