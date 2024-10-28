using System;
using System.Collections.Generic;


namespace ThunderRoad
{
    [Serializable]
    public class LorePackCondition
    {
        #region internalClass
        public enum Visibility
        {
            Hidden = 0,
            PartiallyHidden = 1,
            Visibile = 2,
            VeryVisibile = 3
        }

        [Serializable]
        public class LoreLevelOptionCondition
        {
            public enum ComparisonType
            {
                StringEquals,
                StringNotEquals,
                StringContains,
                IntEqual,
                IntNotEqual,
                IntGreater,
                IntLesser,
                IntGreaterOrEqual,
                IntLesserOrEqual,
            }

            public string key;
            public ComparisonType comparison;
            public string value;

            public bool CheckValue(string levelOptionValue)
            {
                // Comparison string
                switch (comparison)
                {
                    case ComparisonType.StringEquals:
                        return string.Equals(value, levelOptionValue);
                    case ComparisonType.StringNotEquals:
                        return !string.Equals(value, levelOptionValue);
                    case ComparisonType.StringContains:
                        return levelOptionValue != null ? levelOptionValue.Contains(value) : false;
                    default:
                        break;
                }

                // comparison int
                if(!int.TryParse(value, out int intValue))
                {
                    return false;
                }

                if(!int.TryParse(levelOptionValue, out int levelOptionIntValue))
                {
                    return false;
                }

                switch (comparison)
                {
                    case ComparisonType.IntEqual:
                        return levelOptionIntValue == intValue;
                    case ComparisonType.IntNotEqual:
                        return levelOptionIntValue != intValue;
                    case ComparisonType.IntGreater:
                        return levelOptionIntValue > intValue;
                    case ComparisonType.IntLesser:
                        return levelOptionIntValue < intValue;
                    case ComparisonType.IntGreaterOrEqual:
                        return levelOptionIntValue >= intValue;
                    case ComparisonType.IntLesserOrEqual:
                        return levelOptionIntValue <= intValue;
                    default:
                        return false;
                }
            }
        }
        #endregion internalClass

        public List<Visibility> visibilityRequired;
        public LoreLevelOptionCondition[] levelOptions;
        public string[] requiredParameters;

        public LorePackCondition(LorePackCondition condition)
        {
            if (condition.visibilityRequired != null)
            {
                int count = condition.visibilityRequired.Count;
                visibilityRequired = new List<Visibility>(count);
                for (int i = 0; i < count; i++)
                {
                    visibilityRequired.Add(condition.visibilityRequired[i]);
                }
            }
            else
            {
                visibilityRequired = null;
            }

            if (condition.levelOptions != null)
            {
                levelOptions = new LoreLevelOptionCondition[condition.levelOptions.Length];
                for (int i = 0; i < levelOptions.Length; i++)
                {
                    levelOptions[i] = condition.levelOptions[i];
                }
            }
            else
            {
                levelOptions = null;
            }

            if (condition.requiredParameters != null)
            {
                requiredParameters = new string[condition.requiredParameters.Length];
                for (int i = 0; i < requiredParameters.Length; i++)
                {
                    requiredParameters[i] = condition.requiredParameters[i];
                }
            }
            else
            {
                requiredParameters = null;
            }
        }

        public bool IsValid(Visibility visibility, string[] validationRequiredParameters, string[] validationOptionalParameters)
        {
            return true;
        }
    }
}