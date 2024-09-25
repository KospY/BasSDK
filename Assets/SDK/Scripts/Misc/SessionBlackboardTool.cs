using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using TriInspector;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/SessionBlackboardTool")]
    public class SessionBlackboardTool : ThunderBehaviour
    {
        [System.Serializable]
        public class BlackboardEvaluateEvent
        {
            public string evaluator;
            public UnityEvent onTrue;
            public UnityEvent onFalse;
            public bool evaluateOnStart = false;
            public bool evaluateOnBBUpdate = false;

            [System.Flags]
            private enum ComparisonFlags
            {
                Null = 0,
                LessThan = 1,
                LessThanOrEqual = 2,
                EqualTo = 4,
                GreaterThanOrEqual = 8,
                GreaterThan = 16,
            }

            public void Evaluate()
            {
            }
        }

        public static event Action onBlackboardUpdate;

        public List<BlackboardEvaluateEvent> evaluators = new List<BlackboardEvaluateEvent>();
        public List<string> startingBoolOperations = new List<string>();
        public List<string> startingIntegerOperations = new List<string>();
        public List<string> startingFloatOperations = new List<string>();
        private List<BlackboardEvaluateEvent> updateEvaluators = new List<BlackboardEvaluateEvent>();

        private Dictionary<string, ParsedOperation> preParsedOperations = new Dictionary<string, ParsedOperation>();

        public class ParsedOperation
        {
            public string valueName = "";
            public string lhs = "";
            public char oper = ' ';
            public string rhs = "";
            public string originalOperation = "";
            public bool onlyIfMissing = false;
        }
        
        public void BoolOperation(string input)
        {
        }

        public static bool StringToBool(string input, out bool value)
        {
            value = false;
            return false;
        }

        public void IntegerOperation(string input)
        {
        }

        public static bool StringToInt(string input, out int value)
        {
            value = 0;
            return false;
        }

        public void FloatOperation(string input)
        {
        }

        public static bool StringToFloat(string input, out float value)
        {
            value = 0;
            return false;
        }

        public bool ParseInput(string input, out ParsedOperation parsedOperation, int targetType)
        {
            if (preParsedOperations.TryGetValue(input, out parsedOperation))
            {
                return true;
            }
            parsedOperation = new ParsedOperation();
            string operationString = input;
            operationString = operationString.Replace(" ", "");
            parsedOperation.originalOperation = input;
            if (!operationString.Contains("="))
            {
                Debug.LogError("Synax error in operation: Missing assignment (=)");
                return false;
            }
            string[] splitAtEquals = operationString.Split(new char[] { '=' }, count: 2);
            if (splitAtEquals.Length < 2)
            {
                Debug.LogError("Synax error in operation: Assignment is missing left or right side");
                return false;
            }
            if (string.IsNullOrEmpty(splitAtEquals[0]) || string.IsNullOrEmpty(splitAtEquals[1]))
            {
                Debug.LogError("Synax error in operation: Left or right side of operation is empty");
                return false;
            }
            if (splitAtEquals[0].Contains("?"))
            {
                splitAtEquals[0] = splitAtEquals[0].Replace("?", "");
                parsedOperation.onlyIfMissing = true;
            }
            bool correctType = false;
            switch (targetType)
            {
                case 1:
                    correctType = StringToBool(splitAtEquals[0], out _);
                    break;
                case 2:
                    correctType = StringToInt(splitAtEquals[0], out _);
                    break;
                case 3:
                    correctType = StringToFloat(splitAtEquals[0], out _);
                    break;
            }
            if (!correctType)
            {
                Debug.LogError("Synax error in operation: Setter name in operation is stored in the blackboard as a different type");
                return false;
            }
            string boolOperators = "&|#<>=";
            string numOperators = "+-*/%^[]?";
            char oper = ' ';
            for (int i = 0; i < splitAtEquals[1].Length; i++)
            {
                bool boolOp = boolOperators.Contains(splitAtEquals[1][i].ToString());
                bool numOp = numOperators.Contains(splitAtEquals[1][i].ToString());
                if (boolOp || numOp)
                {
                    if (oper != ' ')
                    {
                        Debug.LogError("Synax error in operation: Too many operators");
                        return false;
                    }
                    if (boolOp && targetType != 1)
                    {
                        Debug.LogError("Synax error in operation: Bool operator in int or float assignment");
                        return false;
                    }
                    if (numOp && targetType == 1)
                    {
                        Debug.LogError("Synax error in operation: Number operator in bool assignment");
                        return false;
                    }
                    oper = splitAtEquals[1][i];
                }
            }
            parsedOperation.valueName = splitAtEquals[0];
            parsedOperation.oper = oper;
            if (oper != ' ')
            {
                string[] splitOperation = splitAtEquals[1].Split(oper);
                parsedOperation.lhs = splitOperation[0];
                parsedOperation.rhs = splitOperation[1];
            }
            else
            {
                parsedOperation.lhs = splitAtEquals[1];
            }
            preParsedOperations.Add(input, parsedOperation);
            return true;
        }

        public static int CheckValueTypeOfName(string name)
        {
            return 0;
        }

        public static bool GetFloatOrIntValue(string input, bool setIfUnused, out float value)
        {
            value = 0;
            return false;
        }

        public void CheckEvaluatorAtIndex(int index)
        {
            if (index >= evaluators.Count)
            {
                Debug.LogError($"Input index [{index}] is out of bounds for the list! Index range: [0-{evaluators.Count - 1}] (Inclusive)");
                return;
            }
            evaluators[index].Evaluate();
        }

        public void CheckAllEvaluators()
        {
        }

    }
}
