﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Libvirt_WebManager.Areas.Network.Models
{
    public class Network_List_Down : ViewModels.BaseViewModel
    {
        public Libvirt.CS_Objects.Network[] Networks { get; set; }
    }
}