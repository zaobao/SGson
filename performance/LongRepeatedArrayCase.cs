using System;
using System.Collections.Generic;
using System.Diagnostics;

using SGson;

namespace SGson.Performance
{
	public class LongRepeatedArrayCase()
	{
		public static void Main()
		{
			Employee scott = new Employee()
			{
				EmpNo = 7788,
				Name = "SCOTT",
				Job = Job.Analyst,
				HireDate = DateTime.Parse("1982-12-09"),
				Salary = 3000
			};
			List<Employee> list = new List<Employee>();
			for (int i = 0; i < 200000; i++)
			{
				list.Add(scott);
			}
			Gson gson = new GsonBuilder().SetVisitedObjectCountLimit(int.MaxValue).Create();
			Stopwatch stopWatch = new Stopwatch();
			stopWatch.Start();
			gson.ToJson(list);
			stopWatch.Stop();
			Console.WriteLine(stopWatch.Elapsed.TotalMilliseconds);
		}
	}

	class Employee
	{
		public int EmpNo { get; set; }
		public string Name { get; set; }
		public Job Job { get; set; }
		public DateTime? HireDate { get; set; }
		public double Salary { get; set; }
	}

	enum Job
	{
		Clerk,
		Salesman,
		Manager,
		Black,
		Analyst,
		President
	}
}