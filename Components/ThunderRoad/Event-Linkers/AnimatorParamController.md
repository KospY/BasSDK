---
parent: Event-Linkers
grand_parent: ThunderRoad
---
# Animator Param Controller
The Animator param controller is used to manipulate parameters you create on Animators; using a combination of an Animator and the controller, you can create very complex state machines where button presses, ungrabbing, regrabbing, etc. all manipulate the item/object state. The Animator param controller doesn't do a whole lot on its own, and will generally need to be paired with event linkers or other Unity events. Animator param controllers have to be added onto GameObjects with Animators on them.

**Please note that this is one of the more advanced SDK scripts we've added. In order to utilize it to its greatest extent, you may need to understand the very basics of programming.** All of your inputs into the Animator param controller are going to mimic lines of code. In order to increase a variable by one, for example, you'd invoke one of the methods using `YourParameter = YourParameter + 1`. This script's learning curve is a little steeper than the rest.

## Methods
Below are all of the methods you can invoke using the animator param controller. These all take a string input, and the type of input you can give is dependent upon the method type.

As a general note, whenever you use the name of a parameter in your inputs (either on the left side of the =, or on the right side), *you have to use the name exactly as it is in the Animator!* **This is case-sensitive!**

### SetTrigger(string)
This is a simple convenience method. You can already do this exact same thing by targeting the Animator itself (`Animator.SetTrigger(string)`), but for ease of access it's also been added to this component. This sets a trigger "on" so that it activates in the Animator controller as soon as possible.

### BoolOperation(string)
Invoking this method allows you to change the value of a boolean (true/false) parameter in your animator. You can set it directly (using either `true` or `false`, or the name of another boolean parameter in the animator) or set it using a logic gate or a numerical comparison. **BoolOperation is where you'll benefit most from understanding basic programming/logic gates!**

**To directly set a boolean**, the syntax is as follows: `BoolParamName = True` or `BoolParamName = AnotherBool`. *For plain boolean values (true/false), you do not have to be case-sensitive! `true`, `True`, `TRUE`, and all variations thereof will all be interpreted as `true`.*

| Operator | Behaviour |
| --- | --- |
| ! | Inverts the value of a boolean (from `true` to `false` or vice versa. *This has to go directly in front of a boolean value that's on the right side of the equals!* You can use this to make something toggle like so: `Active = !Active` |
| & | AND operation, requires a boolean value on both sides. The output of this operation is true if both values are true, and is false otherwise. Using this you can make something true only if two other things are true: `Active = Alive & Moving` |
| \| | OR operation, requires a boolean value on both sides. The output of this operation is true if *either* value is true, and is false if both are false. Using this, you can make something true if either of two possible conditions is true: `Active = Alive \| Moving` |
| \# | XOR operation, requires a boolean value on both sides. The output of this operation is true only if the two values are not the same, and is false if both sides are the same. Example: `Active = Alive # Moving` |
| < | LESS THAN operation, requires numerical values on both sides. The output is true if the left side number is less than the right side number. Example: `Active = Speed < 5.0` |
| = | EQUAL TO operation, requires numerical values on both sides. The output is true if the two values are equal. *Float values may be imprecise and can drift! Using this with integers only is recommended.* Example: `Active = Kills = Hits` |
| > | GREATER THAN operation, requires numerical values on both sides. The output is true if the left side number is greater than the right side number. Example: `Active = Stage > 0` |

### IntegerOperation(string)
IntegerOperation is for setting the values of your integer parameters in your animator. Much like with BoolOperation, you can set your integer values directly (using integers or from another parameter) or you can set them using an operation. **For integer operations, both sides of the operation MUST be integers!**

**To directly set an integer**, the syntax is as follows: `IntegerParamName = 10` or `IntegerParamName = AnotherInteger`.

| Operator | Behaviour |
| --- | --- |
| + | Addition: `Stage = Stage + 1` *(Increases `Stage` by 1)* |
| \- | Subtraction: `Stage - 100 - Kills` |
| \* | Multiplication: `Stage = 2 * Kills` |
| / | Division: `Stage = 64 / Drops` *(This rounds to the nearest whole number!)* |
| % | Modulus (Divide and give the remainder): `Stage = 10 % 9` *(This would set `Stage` to 1)* |
| ^ | Exponent: `Stage = 2 ^ PowerUps` |
| \[ | Clamp floor (Output the larger of the two values, or "don't let the value go below this number"): `Stage = -1 [ 0` *(This would set `Stage` to 0)* |
| \] | Clamp ceiling (Output the smaller of the two values, or "don't let the value be higher than this number"): `Stage = 6 ] 5` *(This would set `Stage` to 5)* |
| ? | Random (Set to a random whole number between the left hand side and the right hand side, **exclusive**): `Speed = 0 ? 10` *(This would give a random number that is 0, 9, or any whole number between those)* |

### FloatOperation(string)
FloatOperation is for setting the values of your float parameters in your animator. With FloatOperation you have access to all the same operation options as you do with IntegerOperation, but with FloatOperation, **you can use floats OR integers in your operations.** You can set a float equal to an integer, or set a float equal to the sum of two integers.

**To directly set a float**, the syntax is as follows: `FloatParamName = 4.2` or `FloatParamName = IntegerOrFloatParam`.

| Operator | Behaviour |
| --- | --- |
| + | Addition: `Speed = Speed + 0.5` *(Increases `Speed` by 0.5)* |
| \- | Subtraction: `Speed = Energy - 1` |
| \* | Multiplication: `Speed = 0.5 * Kills` |
| / | Division: `Speed = Energy / 2.5` |
| % | Modulus (Divide and give the remainder): `Speed = 10.5 % 1` *(This would set `Speed` to 0.5)* |
| ^ | Exponent: `Speed = 1.1 ^ PowerUps` |
| \[ | Clamp floor (Output the larger of the two values, or "don't let the value go below this number"): `Speed = -1 [ 0.0` *(This would set `Speed` to 0)* |
| \] | Clamp ceiling (Output the smaller of the two values, or "don't let the value be higher than this number"): `Speed = 7.5 ] 5` *(This would set `Speed` to 5)* |
| ? | Random (Set to a random number between the left hand side and the right hand side, **inclusive**): `Speed = 1.6 ? 9.8` *(This would give a random number that is 1.6, 9.8, or any value between those)* |

## How to use Animator Param Controllers
With an Animator Param Controller, you'll need to make sure that the Animator Param Controller is on the *same* GameObject that an Animator component is on! **This will not work with an Animation component!** There's a good chance you'll also want for your Animator to be on the same GameObject as your Item component if you're making an item. If you're making a creature, the Animator will be in a different location. The below screenshot shows an example of a full setup for an item with 6 distinct stages:
![image](https://user-images.githubusercontent.com/53928003/189550468-0bee1737-b2ef-4262-a56e-b6a4d5fb3e73.png)
In this image, note the following:
- The input may only have *one* operation. No doing `Stage = (Stage + 1) % 6` unfortunately!
- To guarantee that operations happen in the order you want them (In this case, I want to increase the stage, then use the modulus to set it to 0 whenever it's 6), **use duplicates for the same event** rather than putting them in the same `On Activate ()`. They will execute with no delay, but this guarantees they execute in the order you want!
- Case matters! If the cases aren't the same, you will have issues.

