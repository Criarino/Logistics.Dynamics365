using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Gerenciadores
{
    public interface IGerenciadorPlugin
    {
        IOrganizationService Service { get; set; }
        ITracingService Trace { get; set; }

        void OnCreate(Entity entity);
        void OnDelete(Entity entity);
        void OnUpdate(Entity entity);
    }
}
