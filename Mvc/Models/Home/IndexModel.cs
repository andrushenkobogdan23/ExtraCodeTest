using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TodoServices.Shared.Dto;

namespace MVC.Models.Home
{
    public class IndexModel : BaseFilter
    {
        [JsonIgnore]
        public IEnumerable<TodoInfo> Items { get; set; } = new List<TodoInfo>();

        [JsonIgnore]
        public IEnumerable<SelectListItem> Users { get; set; }
        
    }
}