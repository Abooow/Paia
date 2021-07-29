# Paia
Paia is a CLI framework that enables you to easily render and change between different views in a Console application.

</br>

***NOTE:** This is a **very** early version, a lot of code breaking changes will occur in later versions*
___

**Table of Contents**
- [Usage](#usage)
  - [Installing the library](#installing-the-library)
- [Getting Started](#getting-started) 
  - [Initialize the App](#initialize-the-app)
- [Create, Render and Change View](#create-render-and-change-view)
  - [Creating a View](#creating-a-view)
  - [Change between Views](#change-between-views)
- [Passing data between Views](#passing-data-between-views)
  - [Passing data to the starting View](#passing-data-to-the-starting-view)
- [Dependency Injection](#dependency-injection)
  - [Adding Dependency Injection](#adding-dependency-injection)
  - [Injecting a service](#injecting-a-service)
- [More](#more)
  - [Exit() and exitcode](#exit-and-exitcode)

___

## Usage
See the [sample projects](./samples/) for a basic usage of the framework.

The full project from the tutorial below can be found [here](./samples/TutorialSample). </br>

### Installing the library
This project is not available as a NuGet package yet :(
```sh
PM> Install-Package ...
```
*-- or --*  
Clone the repository
```sh
# if you are using ssh
$ git clone git@github.com:Abooow/Paia.git

# if you are using https
$ git clone https://github.com/Abooow/Paia.git
```

</br>

## Getting Started
Start by creating a new **Console Application**. </br>
```sh
# .NET CLI command for creating a new console app
$ dotnet new console -o "APP_NAME"
```
***NOTE:** Paia is using **.NET 6*** </br>

### Initialize the App
Add the following lines of code in `Program.cs`
```csharp
using Paia;

static void Main(string[] args)
{
  new AppBuilder()
    .Build()
    .Run<MyFirstView>();
}
```
`MyFirstView` will become the **starting View** for the application, we'll create that View in the next section.

</br>

## Create, Render and Change View
### Creating a View
To create a View, start by creating a new class and inherit the `ViewBase` class.
```csharp
using Paia.Views;

class MyFirstView : ViewBase
{
  public override ViewResult Render()
  {
    Console.Clear();
    
    Console.WriteLine("Hello World!");
    Console.WriteLine();
    Console.WriteLine("Press 1 to exit");
    char input = Console.ReadKey().KeyChar;
    
    return input switch
    {
      '1' => Exit(),
       _  => ReRenderView()
    };
  }
}
```
The `Render()` method is where you place your main/rendering logic. The method returns a `ViewResult`, which basically holds information about what to do next after this View finishes rendering. </br>
</br>
`Exit()` will return a ViewResult telling the framework to exit the application with exitcode 0. (Read more about Exit() and exitcode [here](#exit-and-exitcode)) </br>
`ReRenderView()` tells the framework to stay on, but re-render the same View. The state of the View will **NOT** change, the ViewManager will keep the same instance of the View and only call the Render method again. If you want to re-render the same View but not keeping the state of the object, use the `RefreshView()` method instead. </br>
</br>

### Change between Views
To change view, simply use the `ChangeView<TView>()` method provided by the ViewBase class.
```csharp
using Paia.Views;

class MyFirstView : ViewBase
{
  public override ViewResult Render()
  {
    Console.Clear();
    
    Console.WriteLine("Hello World!");
    Console.WriteLine();
    Console.WriteLine("Press 1 to change View, 2 to exit");
    char input = Console.ReadKey().KeyChar;
    
    return input switch
    {
      '1' => ChangeView<MySecondView>(),
      '2' => Exit(),
       _  => ReRenderView()
    };
  }
}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~

class MySecondView : ViewBase
{
  public override ViewResult Render()
  {
    Console.Clear();
    Console.WriteLine("Hello World from my Second View!");
    Console.WriteLine();
    Console.WriteLine("Press a key to go back...");
    Console.ReadKey();
    
    return GoBack();
  }
}
```
When changing to a new View, the old View (MyFirstView) will be pushed to a stack (BackStack) by the ViewManager, making it possible to return to it later if needed. The `GoBack()` method pops the stack and renders the **previous View** (MyFirstView). The current View (MySecondView) will pushed to yet another stack, the FrontStack, so it is possible to **go forward** as well by calling the `GoForward()` method.

</br>

## Passing data between Views
Let's add a public `Name` property to `MySecondView` in order for `MyFirstView` to be able to assign a value to it before changing View.
```csharp
class MySecondView : ViewBase
{
  public string Name { get; set; }

  public override ViewResult Render()
  {
    Console.Clear();
    Console.WriteLine("Hello World from my Second View!");
    Console.WriteLine($"And hello to you, {Name}!");
    
    Console.WriteLine();
    Console.WriteLine("Press a key to go back...");
    Console.ReadKey();
    
    return GoBack();
  }
}
```
</br>

```csharp
class MyFirstView : ViewBase
{
  public override ViewResult Render()
  {
    Console.Clear();
  
    Console.WriteLine("Hello World!");
    Console.WriteLine();
    Console.WriteLine("Press 1 to change View, 2 to exit");
    char input = Console.ReadKey().KeyChar;
  
    return input switch
    {
      '1' => ChangeView<MySecondView>(context => context.Name = "Monkey Paia"),
      '2' => Exit(),
       _  => ReRenderView()
    };
  }
}
```
Going back to `MyFirstView`; the `ChangeView<MySecondView>()` method has a overload allowing an `Action\<MySecondView\>` to be passed as a argument. </br>
The example is showing an lambda where `context` is a reference to a **MySecondView** instance, which will be created by the framework. This is how we can access the `Name` property in order to assign a value to it. </br>
</br>

### Passing data to the starting View
Let's say we want to be able to change the welcome message in the starting View from `Main()`. </br>
Start by adding a public property called `Message` in `MyFirstView`.
```csharp
class MyFirstView : ViewBase
{
  public string Message { get; set; }

  public override void OnInitialized()
  {
    Message ??= "Hello World!";
  }

  public override ViewResult Render()
  {
    Console.Clear();
  
    Console.WriteLine(Message);
    Console.WriteLine();
    Console.WriteLine("Press 1 to change View, 2 to exit");
    char input = Console.ReadKey().KeyChar;
  
    return input switch
    {
      '1' => ChangeView<MySecondView>(context => context.Name = "Monkey Paia"),
      '2' => Exit(),
       _  => ReRenderView()
    };
  }
}
```
The `OnInitialized()` method will be called only once per View instance, immediately after the constructor and right before the `Render()` method. Place your initialization code in the `OnInitialized()` method, avoid placing any "rendering" code here. </br>
***NOTE:** The example is using the `??=` operator, which is known as the [null-coalescing assignment operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator) (if **Message** is null, the string "Hello World!" will be assigned to it)* </br>
</br>
Passing data to the starting View is done in a similar manner as the previous section, by passing a `Action\<MyFirstView\>` as an argument, but to the `.Run<MyFirstView>()` method.
```csharp
static void Main(string[] args)
{
  new AppBuilder()
    .Build()
    .Run<MyFirstView>(context => context.Message = "Yo World!");
}
```

</br>

## Dependency Injection
### Adding Dependency Injection
Add services to the app by calling the `.ConfigureServices(Action<IServiceCollection>)` method on `AppBuilder` in **Program.cs**
```csharp
static void Main(string[] args)
{
  new AppBuilder()
    .ConfigureServices(services => services.AddSingleton<IMyService, MyServiceImplementation>())
    .Build()
    .Run<MyFirstView>(context => context.Message = "Yo World!");
}
```

### Injecting a service
To inject a service to a view, use the `[Inject]` attribute on a public property with a setter.
```csharp
using Paia.Attributes;

class MyFirstView : ViewBase
{
  [Inject]
  public IMyService MyService { get; set; }

  public override ViewResult Render()
  {
    ...
  }
}
```

</br>

## More
### Exit() and exitcode
To exit the application with a exitcode other than 0, you have to change the `Main()` method to the following:
```csharp
static int Main(string[] args)
{
  return new AppBuilder()
      .Build()
      .Run<MyFirstView>();
}
```
The `.Run<>()` method returns an int, which is the exit code for the application. </br>
`Exit()` (called from Views) will return 0 by default, but has a overload allowing you to pass an integer as a argument for a custom exitcode. </br>
</br>

```csharp
class MyFirstView : ViewBase
{
  public override ViewResult Render()
  {
    Console.WriteLine("Press a key for a nice exit...");
    Console.ReadKey();
    
    return 69;
  }
}
```
It's possible to also return an integer directly from a View, there is an implicit conversion from int to ViewResult. This is telling the View to exit with exitcode 69.
