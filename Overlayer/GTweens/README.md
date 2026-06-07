[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/Guillemsc/GTweens/blob/main/CONTRIBUTING.md)
[![Release](https://img.shields.io/github/release/Guillemsc/GTweens.svg)](https://github.com/Guillemsc/GTweens/releases/latest)
[![NuGet](https://img.shields.io/nuget/v/GTweens.svg?label=nuget)](https://www.nuget.org/packages/GTweens)
[![Downloads](https://img.shields.io/nuget/dt/GTweens)](https://www.nuget.org/packages/GTweens)
[![Tests](https://github.com/Guillemsc/GTweens/actions/workflows/tests.yml/badge.svg)](https://github.com/Guillemsc/GTweens/actions/workflows/tests.yml)
[![Twitter Follow](https://img.shields.io/badge/twitter-%406uillem-blue.svg?style=flat&label=Follow)](https://twitter.com/6uillem)

![LogoWide-2](https://github.com/Guillemsc/GTweens/assets/17142208/ffeede41-5f4a-456f-8cd1-b74e2a26710a)

GTweens is a lightweight and versatile tweening library for C#. 
This library simplifies the process of creating animations and transitions in your projects, allowing you to bring your game elements to life with ease.

## 🤜 Features
- **Sequencing**: Easily chain multiple tweens together to create complex sequences of animations.
- **Versatile Easing Functions**: Choose from a variety of easing functions to achieve different animation effects, including linear, ease-in, ease-out, etc.  
- **Looping**: Create looping animations with a single line of code, and control loop count and behavior.
- **Delays**: Specify delays, allowing precise timing of your animations.
- **Callbacks**: Attach callbacks to tweens for event handling at various points in the animation timeline.

## 📦 Installation
1. [Download the latest release](https://github.com/Guillemsc/GTweens/releases/latest).
2. Unpack the `GTweens` folder into the project.

## 📚 Getting started
### Nomenclature
- Tween: a generic word that indicates some or multiple values being animated.
- Sequence: an combination of tweens that get animated as a group.

### Prefixes
Prefixes are important to use the most out of IntelliSense, so try to remember these:
- **Set**: prefix for all settings that can be chained to a tween.
    ```csharp
    myTween.SetLoops(4).SetEasing(Easing.InOutCubic);
    ```
- **On**: prefix for all callbacks that can be chained to a tween.
    ```csharp
    myTween.OnStart(myStartFunction).OnComplete(myCompleteFunction);
    ```
 
### Generic tweening
This is the most flexible way of tweening and allows you to tween almost any value.
```csharp
// For default C# values (int, float, etc)
GTweenExtensions.Tween(getter, setter, to, duration)
```
- **Getter**: a delegate that returns the value of the property to tween. Can be written as a lambda like this: `() => myValue`
where `myValue` is the name of the property to tween.
- **Setter**: a delegate that sets the value of the property to tween. Can be written as a lambda like this: `x => myValue = x`
where `myValue` is the name of the property to tween.
- **To**: the end value to reach.
- **Duration**: the duration of the tween in seconds.
- **Validation** (optional): a delegate that every time the tween updates, checks if it should be running. Can be written as a lambda like this: `() => shouldKeepRunning`
where `shouldKeepRunning` is a boolean.
  
```csharp
// For default C# values
GTween tween = GTweenExtensions.Tween(
    () => Target.SomeFloat, // Getter
    x => Target.SomeFloat = x, // Setter
    100f, // To
    1 // Duration
);
```

### Tweens context
Tweens require a system that updates them every frame. In GTweens, this system is referred to as a `GTweensContext`. 

Essentially, `GTweensContext` is a class that maintains a list of active tweens and advances their progress collectively when the Tick method is called.
To set a GTween in motion, it needs to be initiated through the `Play` method provided by the `GTweensContext`. 
Here's a practical example of how to implement this concept within an application:
```csharp
class MyApplication
{
    // We need a single instance of a GTweensContext
    readonly GTweensContext _gTweensContext = new();

    void UpdateApplication(float frameDeltaTime)
    {
        // With out aplication update, we tick the context with the frame delta time
        _gTweensContext.Tick(frameDeltaTime)
    }

    void PlaySomeTween()
    {
        // We create a tween
        GTween tween = GTweenExtensions.Tween(
            () => Target.SomeFloat, // Getter
            x => Target.SomeFloat = x, // Setter
            100f, // To
            1 // Duration
        );

        // We play the tween with the context
        _gTweensContext.Play(tween);
    }
}
```
In this example:
- We establish a single instance of GTweensContext, _gTweensContext, within the MyApplication class.
- The UpdateApplication function is responsible for advancing the tweens within the context using the provided frameDeltaTime as the time increment.
- To initiate a new tween, we use the GTweenExtensions.Tween method, specifying the getter, setter, target value, and duration.
- Finally, we play the tween by adding it to the context using _gTweensContext.Play(tween).
This approach allows for the management and synchronization of tweens within the application using a GTweensContext.

### Sequences
Sequences are a combination of tweens that get animated as a group. 
Sequences can be contained inside other sequences without any limit to the depth of the hierarchy.
To create sequences, you need to use the helper `GTweenSequenceBuilder`.
- First you call to start creating a new sequence `New()`.
- Next you `Append()` or `Join()` any tweens to the sequence.
	- **Append**: Adds the given tween to the end of the Sequence. This tween will play after all the previous tweens have finished.
	- **Join**: Inserts the given tween at the same time position of the last tween added to the Sequence. This tween will play at the same time as the previous tween.
- Finally you call `Build()` to get the generated sequence Tween.
```csharp
 GTween tween = GTweenSequenceBuilder.New()
    .Append(SomeTween)
        .Join(SomeOtherTween)
    .Append(SomeOtherOtherTween)
    .AppendTime(0.5f)
    .Append(AnotherTween)
    .AppendCallback(() => Console.WriteLine("I'm finished!"))
    .Build();
        
tween.SetEasing(Easing.InOutCubic);
tween.Play();
```

### Tween controls
- **Kill**: kills the tween. This means that the tween will stop playing.
- **Complete**: instantly reaches the final state of the tween, and stops playing.
- **Reset**: sets the tween to its initial state, and stops playing.
- **SetLoops**: sets the amount of times the tween should loop.
- **SetEasing**: sets the easing used by the tween. If the tween is a sequence, the easing will be applied to all child tweens. Set to linear by default.
- **SetTimeScale**: sets the time scale that will be used to tick the tween. Set to 1 by default.

### Tasks
- **AwaitCompleteOrKill**: returns a Task that waits until the tween is killed or completed.
