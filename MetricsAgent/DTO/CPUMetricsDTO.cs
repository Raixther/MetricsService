﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.DTO
{
	public class CPUMetricsDTO
	{	
			public int Id { get; set; }

			public int Value { get; set; }

			public TimeSpan Time { get; set; }
	}	
}
