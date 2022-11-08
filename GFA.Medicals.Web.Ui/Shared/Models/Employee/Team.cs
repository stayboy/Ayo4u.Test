﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GFA.Medicals.Web.Ui.Shared.Models.Employee
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamColor { get; set; }

        [JsonIgnore]
        // ^^^^^^^
        // System.Text.Json does not identify circular references.
        // We need to ignore the employees reference otherwise it will loop.
        public List<Employee> Employees { get; set; }
    }
}
