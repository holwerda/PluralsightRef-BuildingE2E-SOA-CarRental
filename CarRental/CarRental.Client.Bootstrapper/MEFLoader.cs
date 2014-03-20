using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarRental.Client.Proxies;

namespace CarRental.Client.Bootstrapper
{
    public static class MEFLoader
    {
        public static CompositionContainer Init()
        {
            return Init(null);
        }

        public static CompositionContainer Init(ICollection<ComposablePartCatalog> catalogParts)
        {
            AggregateCatalog catalog = new AggregateCatalog();

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(InventoryClient).Assembly));

            CompositionContainer container = new CompositionContainer(catalog);

            if (catalogParts != null)
            {
                foreach (var part in catalogParts)
                {
                    catalog.Catalogs.Add(part);
                }
            }

            return container;
        }
    }
}
