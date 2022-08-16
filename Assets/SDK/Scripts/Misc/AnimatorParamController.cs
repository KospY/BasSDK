using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    [HelpURL("https://kospy.github.io/BasSDK/Components/ThunderRoad/AnimatorParamController")]
	[AddComponentMenu("ThunderRoad/Animator Param Controller")]
    [RequireComponent(typeof(Animator))]
    public class AnimatorParamController : MonoBehaviour
    {
        [Tooltip("Only works if this controller is on an item!")]
        public bool saveValuesOnStore = false;
        [Tooltip("Only works if this controller is on an item!")]
        public bool loadValuesOnLoad = false;
        public Animator animator { get; protected set; }
        public Item item { get; protected set; }
        // Avoid parsing the same operations again when possible
        private Dictionary<string, ParsedOperation> preParsedOperations = new Dictionary<string, ParsedOperation>();
        private Dictionary<string, AnimatorControllerParameterType> animatorParams = new Dictionary<string, AnimatorControllerParameterType>();

        public class ParsedOperation
        {
            public int paramHash = 0;
            public string lhs = "";
            public char oper = ' ';
            public string rhs = "";
            public string originalOperation = "";
        }

        protected void Start()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"AnimatorParamController is on GameObject \"{gameObject.name}\" and has no animator on it. This shouldn't happen. The script will not function.");
                return;
            }
        }

        private void StoreParamValues(Container _)
        {
        }

        public void SetTrigger(string triggerName) => animator.SetTrigger(Animator.StringToHash(triggerName));

        [Button]
        public void BoolOperation(string input)
        {
            if (animator == null) return;

            if (ParseInput(input, out ParsedOperation parsedOperation))
            {
                if (" &|%".Contains(parsedOperation.oper.ToString()))
                {
                    bool lhs;
                    if (!StringToBool(parsedOperation.lhs, out lhs))
                    {
                        // Perform a coin flip
                        if (parsedOperation.lhs == "~")
                        {
                            lhs = Random.Range(0, 2) == 0;
                        }
                        else
                        {
                            Debug.LogError("Type error: First value in operation is not a valid boolean\nCould not process input bool operation"
                                + "\nError from: " + parsedOperation.originalOperation);
                            return;
                        }
                    }
                    if (parsedOperation.oper == ' ')
                    {
                        animator.SetBool(parsedOperation.paramHash, lhs);
                        return;
                    }
                    bool rhs;
                    if (!StringToBool(parsedOperation.rhs, out rhs))
                    {
                        Debug.LogError("Type error: Second value in operation is not a valid boolean\nCould not process input bool operation"
                            + "\nError from: " + parsedOperation.originalOperation);
                        return;
                    }
                    switch (parsedOperation.oper)
                    {
                        case '&':
                            animator.SetBool(parsedOperation.paramHash, lhs && rhs);
                            break;
                        case '|':
                            animator.SetBool(parsedOperation.paramHash, lhs || rhs);
                            break;
                        case '%':
                            animator.SetBool(parsedOperation.paramHash, (lhs || rhs) && !(lhs && rhs));
                            break;
                    }
                }
                else
                {
                    float lhs;
                    if (!StringToFloat(parsedOperation.lhs, out lhs))
                    {
                        Debug.LogError("Type error: First value in comparison is not a valid number\nCould not process input bool operation"
                            + "\nError from: " + parsedOperation.originalOperation);
                        return;
                    }
                    float rhs;
                    if (!StringToFloat(parsedOperation.rhs, out rhs))
                    {
                        Debug.LogError("Type error: Second value in comparison is not a valid number\nCould not process input bool operation"
                            + "\nError from: " + parsedOperation.originalOperation);
                        return;
                    }
                    switch (parsedOperation.oper)
                    {
                        case '=':
                            animator.SetBool(parsedOperation.paramHash, Mathf.Approximately(lhs, rhs));
                            break;
                        case '<':
                            animator.SetBool(parsedOperation.paramHash, lhs < rhs);
                            break;
                        case '>':
                            animator.SetBool(parsedOperation.paramHash, lhs > rhs);
                            break;
                    }
                }
            }
            else
            {
                Debug.LogError("Could not process input bool operation"
                            + "\nError from: " + parsedOperation.originalOperation);
            }
        }

        private bool StringToBool(string input, out bool value)
        {
            bool invertOutput = input.Contains("!");
            string cleaned = input.Replace("!", "");
            if (cleaned.ToLower() == "true" || cleaned.ToLower() == "false")
            {
                value = cleaned.ToLower() == "true";
                if (invertOutput) value = !value;
                return true;
            }
            else if (CheckForParam(cleaned) == 1)
            {
                value = animator.GetBool(Animator.StringToHash(cleaned));
                if (invertOutput) value = !value;
                return true;
            }
            else
            {
                value = false;
                return false;
            }
        }

        [Button]
        public void IntegerOperation(string input)
        {
            if (animator == null) return;

            if (ParseInput(input, out ParsedOperation parsedOperation))
            {
                int lhs;
                if (!StringToInt(parsedOperation.lhs, out lhs))
                {
                    Debug.LogError("Type error: First value in operation is not a valid integer\nCould not process input integer operation"
                            + "\nError from: " + parsedOperation.originalOperation);
                    return;
                }
                if (parsedOperation.oper == ' ')
                {
                    animator.SetInteger(parsedOperation.paramHash, lhs);
                    return;
                }
                int rhs;
                if (!StringToInt(parsedOperation.rhs, out rhs))
                {
                    Debug.LogError("Type error: Second value in operation is not a valid integer\nCould not process input integer operation"
                            + "\nError from: " + parsedOperation.originalOperation);
                    return;
                }
                switch (parsedOperation.oper)
                {
                    case '+':
                        animator.SetInteger(parsedOperation.paramHash, lhs + rhs);
                        break;
                    case '-':
                        animator.SetInteger(parsedOperation.paramHash, lhs - rhs);
                        break;
                    case '*':
                        animator.SetInteger(parsedOperation.paramHash, lhs * rhs);
                        break;
                    case '/':
                        animator.SetInteger(parsedOperation.paramHash, lhs / rhs);
                        break;
                    case '^':
                        animator.SetInteger(parsedOperation.paramHash, (int)Mathf.Pow(lhs, rhs));
                        break;
                    case '[':
                        animator.SetInteger(parsedOperation.paramHash, Mathf.Max(lhs, rhs));
                        break;
                    case ']':
                        animator.SetInteger(parsedOperation.paramHash, Mathf.Min(lhs, rhs));
                        break;
                    case '?':
                        animator.SetInteger(parsedOperation.paramHash, Random.Range(lhs, rhs));
                        break;
                }
            }
            else
            {
                Debug.LogError("Could not process input integer operation"
                            + "\nError from: " + parsedOperation.originalOperation);
            }
        }

        private bool StringToInt(string input, out int value)
        {
            if (int.TryParse(input, out value))
            {
                return true;
            }
            if (CheckForParam(input) == 2)
            {
                value = animator.GetInteger(Animator.StringToHash(input));
                return true;
            }
            value = 0;
            return false;
        }

        [Button]
        public void FloatOperation(string input)
        {
            if (animator == null) return;

            if (ParseInput(input, out ParsedOperation parsedOperation))
            {
                float lhs;
                if (!StringToFloat(parsedOperation.lhs, out lhs))
                {
                    Debug.LogError("Type error: First value in operation is not a valid float\nCould not process input float operation"
                            + "\nError from: " + parsedOperation.originalOperation);
                    return;
                }
                if (parsedOperation.oper == ' ')
                {
                    animator.SetFloat(parsedOperation.paramHash, lhs);
                    return;
                }
                float rhs;
                if (!StringToFloat(parsedOperation.rhs, out rhs))
                {
                    Debug.LogError("Type error: Second value in operation is not a valid float\nCould not process input float operation"
                            + "\nError from: " + parsedOperation.originalOperation);
                    return;
                }
                switch (parsedOperation.oper)
                {
                    case '+':
                        animator.SetFloat(parsedOperation.paramHash, lhs + rhs);
                        break;
                    case '-':
                        animator.SetFloat(parsedOperation.paramHash, lhs - rhs);
                        break;
                    case '*':
                        animator.SetFloat(parsedOperation.paramHash, lhs * rhs);
                        break;
                    case '/':
                        animator.SetFloat(parsedOperation.paramHash, lhs / rhs);
                        break;
                    case '^':
                        animator.SetFloat(parsedOperation.paramHash, Mathf.Pow(lhs, rhs));
                        break;
                    case '[':
                        animator.SetFloat(parsedOperation.paramHash, Mathf.Max(lhs, rhs));
                        break;
                    case ']':
                        animator.SetFloat(parsedOperation.paramHash, Mathf.Min(lhs, rhs));
                        break;
                    case '?':
                        animator.SetFloat(parsedOperation.paramHash, Random.Range(lhs, rhs));
                        break;
                }
            }
            else
            {
                Debug.LogError("Could not process input float operation"
                            + "\nError from: " + parsedOperation.originalOperation);
            }
        }

        private bool StringToFloat(string input, out float value)
        {
            if (float.TryParse(input, out value))
            {
                return true;
            }
            int type = CheckForParam(input);
            if (type > 1)
            {
                if (type == 2) value = animator.GetInteger(Animator.StringToHash(input));
                if (type == 3) value = animator.GetFloat(Animator.StringToHash(input));
                return true;
            }
            value = 0f;
            return false;
        }

        public bool ParseInput(string input, out ParsedOperation parsedOperation)
        {
            parsedOperation = new ParsedOperation();
            if (animator == null) return false;

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
            int type = CheckForParam(splitAtEquals[0]);
            if (type == 0)
            {
                Debug.LogError("Synax error in operation: Right hand side is not a valid bool, int, or float animator parameter name");
                return false;
            }
            string boolOperators = "&|%<>=";
            string numOperators = "+-*/^[]?";
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
                    if (boolOp && type != 1)
                    {
                        Debug.LogError("Synax error in operation: Bool operator in int or float assignment");
                        return false;
                    }
                    if (numOp && type == 1)
                    {
                        Debug.LogError("Synax error in operation: Number operator in bool assignment");
                        return false;
                    }
                    oper = splitAtEquals[1][i];
                }
            }
            parsedOperation.paramHash = Animator.StringToHash(splitAtEquals[0]);
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

        public int CheckForParam(string name)
        {
            if (animator == null) return 0;

            if (animatorParams.Count == 0)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    if (!animatorParams.ContainsKey(param.name))
                    {
                        animatorParams.Add(param.name, param.type);
                    }
                }
            }
            string cleaned = name.Replace("!", "");
            if (animatorParams.TryGetValue(cleaned, out AnimatorControllerParameterType type))
            {
                switch (type)
                {
                    case AnimatorControllerParameterType.Bool: return 1;
                    case AnimatorControllerParameterType.Int: return 2;
                    case AnimatorControllerParameterType.Float: return 3;
                    default: return 0;
                }
            }
            return 0;
        }
    }
}
