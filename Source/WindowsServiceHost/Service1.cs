using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using HostingNarrator;
using Topshelf;

namespace WindowsServiceHost
{
    public partial class TestBcService : ServiceBase, ServiceControl
    {
        private Narrator narrator;

        public TestBcService()
        {
            InitializeComponent();
        }

        public void start()
        {
            OnStart(new string[0]);
        }

        protected override void OnStart(string[] args)
        {
            LoadRefs(ConfigurationManager.AppSettings["RefsPath"]);

            var logger = new NotepadLogger(ConfigurationManager.AppSettings["LogPath"]);

            var bcPath = new DirectoryInfo(ConfigurationManager.AppSettings["BcDir"]);

            narrator = new Narrator(bcPath, logger);

            narrator.Start();
        }

        protected override void OnStop()
        {
            narrator.Dispose();

            narrator = null;
        }

        private void LoadRefs(string refsPath)
        {
            var filesPath = Directory.GetFiles(refsPath, "*.dll").ToList();

            var loadedFiles = new List<string>();

            var loops = 0;

            while (filesPath.Count > 0)
            {
                foreach (var file in filesPath)
                {
                    try
                    {
                        Assembly.LoadFrom(file);

                        loadedFiles.Add(file);
                    }
                    catch
                    {
                    }
                }

                foreach (var loadedFile in loadedFiles)
                {
                    filesPath.Remove(loadedFile);
                }

                loadedFiles.Clear();

                loops++;

                if (loops == 100)
                {
                    throw new Exception($"These assemblies not loaded: {string.Join(",", filesPath)}");
                }
            }
        }

        public bool Start(HostControl hostControl)
        {
            OnStart(new string[0]);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            OnStop();

            return true;
        }
    }
}
