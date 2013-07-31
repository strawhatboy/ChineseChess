using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel.Composition.Hosting;

namespace Strawhat.Games._2DModelEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static CompositionContainer compositionContainer;

        public static CompositionContainer CompositionContainer
        {
            get { return compositionContainer; }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly));
            compositionContainer = new CompositionContainer(catalog);
        }
    }
}
