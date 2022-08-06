# Animator Param Controller
The Animator param controller is used to manipulate parameters you create on Animators; using a combination of an Animator and the controller, you can create very complex state machines where button presses, ungrabbing, regrabbing, etc. all manipulate the item/object state. The Animator param controller doesn't do a whole lot on its own, and will generally need to be paired with event linkers or other Unity events. Animator param controllers have to be added onto GameObjects with Animators on them.

**Please note that this is one of the more advanced SDK scripts we've added. In order to utilize it to its greatest extent, you may need to understand the very basics of programming.** All of your inputs into the Animator param controller are going to mimic lines of code. In order to increase a variable by one, for example, you'd invoke one of the methods using `YourParameter = YourParameter + 1`. This script's learning curve is a little steeper than the rest.

## Methods
Below are all of the methods you can invoke using the animator param controller. These all take a string input, and the type of input you can give is dependent upon the method type.

### SetTrigger(string)
This is a simple convenience method. You can already do this exact same thing by targeting the Animator itself (`Animator.SetTrigger(string)`), but for ease of access it's also been added to this component. This sets a trigger "on" so that it activates in the Animator controller as soon as possible.

### BoolOperation(string)
Invoking this method allows you to change the value of a boolean (true/false) parameter in your animator. You can set it directly (using either `true` or `false`, or the name of another boolean parameter in the animator) or set it using a logic gate or a numerical comparison.

| Operator | Behaviour |
| --- | --- |
| ! | Invert |
| & | AND |
| \| | OR |
| % | XOR |
| < | LESS |
| = | EQUAL |
| > | GREATER |

### IntegerOperation(string)

### FloatOperation(string)
