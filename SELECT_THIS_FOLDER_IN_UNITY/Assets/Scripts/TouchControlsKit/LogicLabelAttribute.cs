using System;

namespace TouchControlsKit
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class LogicLabelAttribute : Attribute
	{
		public readonly string memberName;

		public readonly string trueLabel;

		public readonly string falseLabel;

		public LogicLabelAttribute(string memberName, string trueLabel, string falseLabel)
		{
			this.memberName = memberName;
			this.trueLabel = trueLabel;
			this.falseLabel = falseLabel;
		}
	}
}
