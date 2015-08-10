﻿using System.Web.Mvc;

namespace Libvirt_WebManager.Areas.Storage_Pool.Controllers
{
    public class ListingController : Libvirt_WebManager.Controllers.CommonController
    {
        private Service.Storage_Service _Storage_Service;
        public ListingController()
        {
            _Storage_Service = new Service.Storage_Service(new Libvirt_WebManager.Models.Validator(ModelState));
        }


        public ActionResult _Partial_DetailsMainContent(string host, string pool)
        {
            var vm = new Libvirt_WebManager.Areas.Storage_Pool.Models.Storage_Pool_Down();
            vm.Pool = GetHost(host).virStoragePoolLookupByName(pool);
            Libvirt.CS_Objects.Storage_Volume[] vols;
            AddToAutomaticDisposal(vm.Pool).virStoragePoolListAllVolumes(out vols);
            AddToAutomaticDisposal(vols);
            vm.Volumes = vols;
            vm.Host = host;
            vm.Parent = pool;
            return PartialView(vm);
        }

        public ActionResult _Partial_MainContent(string host)
        {
            var vm = new Libvirt_WebManager.Areas.Storage_Pool.Models.Storage_Pools();
            var h = GetHost(host);
            Libvirt.CS_Objects.Storage_Pool[] p;
            h.virConnectListAllStoragePools(out p, Libvirt.virConnectListAllStoragePoolsFlags.VIR_CONNECT_LIST_STORAGE_POOLS_DEFAULT);
            AddToAutomaticDisposal(p);
            vm.Pools = p;
            vm.Host = host;
            return PartialView(vm);
        }


        public ActionResult _Partial_Delete_Volume(string Host, string Pool, string Volume)
        {
            using (var p = GetHost(Host).virStoragePoolLookupByName(Pool))
            {
                var v = p.virStorageVolLookupByName(Volume);
                AddToAutomaticDisposal(v);
                if (v.virStorageVolDelete() == -1) return PartialView("_Partial_Volume_Details_Listing", new Libvirt_WebManager.Areas.Storage_Pool.Models.Storage_Volume_Details { Host = Host, Parent = Pool, Volume = v });
                else return Content("");//return an empty string so it will delete the node
            }
        }


        public ActionResult _Partial_Refresh_Pool(string Host, string Pool)
        {
            
            var p = GetHost(Host).virStoragePoolLookupByName(Pool);
            AddToAutomaticDisposal(p);
            p.virStoragePoolRefresh();
            return PartialView("_Partial_Pool_TableRowDetails", new Models.Storage_PoolBase { Host = Host, Parent = Host, Pool = p });
        }
        public ActionResult _Partial_Delete_Pool(string Host, string Pool)
        {
            using (var p = GetHost(Host).virStoragePoolLookupByName(Pool))
            {
                p.virStoragePoolDelete(Libvirt.virStoragePoolDeleteFlags.VIR_STORAGE_POOL_DELETE_NORMAL);
                p.virStoragePoolUndefine();
            }
            return Content("");
        }
        public ActionResult _Partial_Start_Pool(string Host, string Pool)
        {
            var p = GetHost(Host).virStoragePoolLookupByName(Pool);
            AddToAutomaticDisposal(p);
            p.virStoragePoolCreate();
            return PartialView("_Partial_Pool_TableRowDetails", new Models.Storage_PoolBase { Host = Host, Parent = Host, Pool = p });
        }
        public ActionResult _Partial_Stop_Pool(string Host, string Pool)
        {
            var p = GetHost(Host).virStoragePoolLookupByName(Pool);
            AddToAutomaticDisposal(p);
            p.virStoragePoolDestroy();
            return PartialView("_Partial_Pool_TableRowDetails", new Models.Storage_PoolBase { Host = Host, Parent = Host, Pool = p });
        }
 
    }
}
