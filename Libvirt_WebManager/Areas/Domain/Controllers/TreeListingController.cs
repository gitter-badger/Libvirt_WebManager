﻿using System.Diagnostics;
using System.Web.Mvc;

namespace Libvirt_WebManager.Areas.Domain.Controllers
{
    public class TreeListingController : Libvirt_WebManager.Controllers.CommonController
    {
        private Service.Domain_Service _Domain_Service;
        public TreeListingController()
        {
            _Domain_Service = new Service.Domain_Service(new Libvirt_WebManager.Models.Validator(ModelState));
        }
        public ActionResult _Partial_Index(string host)
        {
            var vm = new Models.Domain_List_Down();
            var  ds= GetHost(host).virConnectListAllDomains(Libvirt.virConnectListAllDomainsFlags.VIR_DEFAULT);
            AddToAutomaticDisposal(ds);
            vm.Host = host;
            vm.Parent = host;
            vm.Domains = ds;
            return PartialView(vm);
        }
        public ActionResult _Partial_Hardware(string host, string domain)
        {
            var vm = new Models.Domain_Hardware_Down();
            using(var d = GetHost(host).virDomainLookupByName(domain))
            {
                vm.Machine = d.virDomainGetXMLDesc(Libvirt.virDomainXMLFlags.VIR_DEFAULT);
                vm.Host = host;
                vm.Parent = domain;
            }
            return PartialView(vm);
        }
    }
}
