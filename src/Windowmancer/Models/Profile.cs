﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windowmancer.Models
{
  public class Profile
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public BindingList<WindowInfo> Windows { get; set; }
  }
}
