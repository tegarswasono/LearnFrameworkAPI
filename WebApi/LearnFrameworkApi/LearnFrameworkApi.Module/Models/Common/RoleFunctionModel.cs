﻿using LearnFrameworkApi.Module.Datas.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnFrameworkApi.Module.Models.Common
{
    public class RoleFunctionModel
    {
        public string ModuleId { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
        public List<RoleFunctionModelItem> Items { get; set; } = [];
    }
    public class RoleFunctionModelItem
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
    }
}
