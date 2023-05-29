using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pumpkin.Database;

//This is a multidimentional recursive mess... help me

public class UserAutomations 
{
	public List<Automation> Automations { get; set; }
}

public class Automation 
{
	public string Name { get; set; }
	public Condition Condition { get; set; }
}

public class Condition : ICondition
{
	public string Variable { get; set; }
	public dynamic Value { get; set; }
	public Operator Operator { get; set; }
	public AutomationType Type { get; set; }

	public bool Evaluate(Dataframe dataframe) //dataframe = structure packed with all meassured data :)
	{
		//eval condition based on val, var, type and operator using dataframe
		switch (Type)
		{
			case AutomationType.DateTime:
				if (Value is not DateTime valueDate) return false;
				DateTime today = DateTime.Today;
				switch (Operator)
				{
					case Operator.Greater:
						return valueDate < today;
					case Operator.GreaterEqual:
						return valueDate <= today;
					case Operator.Less:
						return valueDate > today;
					case Operator.LessEqual:
						return valueDate >= today;
					case Operator.Equal:
						return valueDate == today;
					case Operator.NotEqual:
						return valueDate != today;
					default:
						return false;
				}
			case AutomationType.String:
				string valueStr = Value.ToString();
				string varStr = dataframe.GetString(Variable);
				switch (Operator)
				{
					case Operator.Equal:
						return valueStr == varStr;
					case Operator.NotEqual:
						return valueStr != varStr;
					default:
						return false;
				}
			case AutomationType.Color:
				Color valColor = ColorTranslator.FromHtml(Value.ToString());
				Color varColor = dataframe.GetColor(Variable);
				switch (Operator)
				{
					case Operator.Equal:
						return valColor == varColor;
					case Operator.NotEqual:
						return valColor != varColor;
					default:
						return false;
				}
			case AutomationType.Number:
				decimal valNumber = (decimal)Value;
				decimal varNumber = dataframe.GetDecimal(Variable);
				switch (Operator)
				{
					case Operator.Greater:
						return varNumber > valNumber;
					case Operator.GreaterEqual:
						return varNumber >= valNumber;
					case Operator.Less:
						return varNumber < valNumber;
					case Operator.LessEqual:
						return varNumber <= valNumber;
					case Operator.Equal:
						return varNumber == valNumber;
					case Operator.NotEqual:
						return varNumber != valNumber;
					default:
						return false;
				}
			default:
				return false;
		}
	}
}

public class AndCondition : INestedCondition 
{
	public List<Condition> Conditions { get; set; }
	public List<AndCondition> AndConditions { get; set; }
	public List<OrCondition> OrConditions { get; set; }

	public bool Evaluate(Dataframe dataframe) //dataframe = structure packed with all meassured data :)
	{
		int condCount = Conditions.Count, andCount = AndConditions.Count, orCount = OrConditions.Count;
		bool condResult = true, andResult = true, orResult = true;

		if (Conditions is not null && condCount > 0) for (int i = 0; i < condCount; i++) condResult &= Conditions[i].Evaluate(dataframe);
		if (AndConditions is not null && andCount > 0) for (int i = 0; i < andCount; i++) andResult &= AndConditions[i].Evaluate(dataframe);
		if (OrConditions is not null && orCount > 0) for (int i = 0; i < orCount; i++) orResult &= OrConditions[i].Evaluate(dataframe);

		return condResult && andResult && orResult;
	}
}

public class OrCondition : INestedCondition 
{
	public List<Condition> Conditions { get; set; }
	public List<AndCondition> AndConditions { get; set; }
	public List<OrCondition> OrConditions { get; set; }

	public bool Evaluate(Dataframe dataframe) //dataframe = structure packed with all meassured data :)
	{
		int condCount = Conditions.Count, andCount = AndConditions.Count, orCount = OrConditions.Count;
		bool condResult = false, andResult = false, orResult = false;

		if (Conditions is not null && condCount > 0) for (int i = 0; i < condCount; i++) condResult |= Conditions[i].Evaluate(dataframe);
		if (AndConditions is not null && andCount > 0) for (int i = 0; i < andCount; i++) andResult |= AndConditions[i].Evaluate(dataframe);
		if (OrConditions is not null && orCount > 0) for (int i = 0; i < orCount; i++) orResult |= OrConditions[i].Evaluate(dataframe);

		return condResult || andResult || orResult;
	}
}

public class Action
{
	//data to hold action
}

//Interfaces needed to be able to treat AND and OR conditions like normal conditions and evaluate them recursively

public interface IRootCondition
{
	public bool Evaluate(Dataframe dataframe);
}

public interface ICondition : IRootCondition
{
	public string Variable { get; set; }
	public dynamic Value { get; set; }
	public Operator Operator { get; set; }
	public AutomationType Type { get; set; }
}

public interface INestedCondition : IRootCondition
{
	public List<Condition> Conditions { get; set; }
	public List<AndCondition> AndConditions { get; set; }
	public List<OrCondition> OrConditions { get; set; }
}