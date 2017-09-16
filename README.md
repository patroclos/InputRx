# InputRx - Fully reactive modular input system for Unity3D

This Plugins aims at providing an extremely flexible and simple system for setting up modular input possibly at runtime.
Behaviour and Input specifics get seperated into InputModes and Constraints, which control when the mode gets "bound"

The InputModes can always do their thing and dont have to worry about keeping track of the users input.
And the constraint system enables you to easily alter input bindings and test around without having to restart your game.

## Constraints
KeyConstraint: supports key press/up/down + keyname combination

## Roadmap
- Interrelated Constraints (eg. or,xor,and for what constraints need to be satisfied)
- Mouse constraint
- KeyCombo constraint with easily editable key sequences (maybe just a child with constraint objects that get put into some list)
- Delay observation and subscription of constraint satisfaction?

## Contribution
If you have some comments or ideas you can open an issue or shoot me an email at jenschjoshua@gmail.com
You can also just fork this repository and shoot me a PR when you did your thing (or dont, I'm not your boss)